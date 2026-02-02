using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.Common.Content;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Content.FunctionMessages;
using Beamable.SuiFederation.Features.Content.Models;
using Beamable.SuiFederation.Features.Contract;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Enoki;
using Beamable.SuiFederation.Features.Enoki.Models;
using Beamable.SuiFederation.Features.Inventory.Models;
using Beamable.SuiFederation.Features.OAuthProvider;
using Beamable.SuiFederation.Features.SuiApi;
using Beamable.SuiFederation.Features.SuiApi.Models;
using Beamable.SuiFederation.Features.Transactions;
using Beamable.SuiFederation.Features.Transactions.Storage.Models;

namespace Beamable.SuiFederation.Features.Content.Handlers;

public class EnokiRegularCoinHandler : IService, IContentHandler
{
    private readonly RegularCoinHandler _innerHandler;
    private readonly ContractService _contractService;
    private readonly SuiApiService _suiApiService;
    private readonly TransactionManagerFactory _transactionManagerFactory;
    private readonly AccountsService _accountsService;
    private readonly OauthProviderService _oauthProviderService;
    private readonly EnokiService _enokiService;
    private readonly Configuration _configuration;

    public EnokiRegularCoinHandler(RegularCoinHandler innerHandler, ContractService contractService, SuiApiService suiApiService, TransactionManagerFactory transactionManagerFactory, AccountsService accountsService, OauthProviderService oauthProviderService, EnokiService enokiService, Configuration configuration)
    {
        _innerHandler = innerHandler;
        _contractService = contractService;
        _suiApiService = suiApiService;
        _transactionManagerFactory = transactionManagerFactory;
        _accountsService = accountsService;
        _oauthProviderService = oauthProviderService;
        _enokiService = enokiService;
        _configuration = configuration;
    }

    public async Task<BaseMessage?> ConstructMessage(string transaction, string wallet, InventoryRequest inventoryRequest, IContentObject contentObject)
     =>
        inventoryRequest.Amount switch
        {
            > 0 => await _innerHandler.PositiveAmountMessage(transaction, wallet, "mint", inventoryRequest),
            < 0 => await NegativeAmountMessage(transaction, wallet, "burn", inventoryRequest),
            _ => null
        };

    public Task<BaseMessage?> ConstructMessage(string transaction, string wallet, InventoryRequestUpdate inventoryRequest,
        IContentObject contentObject)
    {
        return _innerHandler.ConstructMessage(transaction, wallet, inventoryRequest, contentObject);
    }

    public Task<BaseMessage?> ConstructMessage(string transaction, string wallet, InventoryRequestDelete inventoryRequest, IContentObject contentObject)
    {
        return _innerHandler.ConstructMessage(transaction, wallet, inventoryRequest, contentObject);
    }

    public async Task SendMessages(string transaction, string wallet, List<BaseMessage> messages)
    {
        if (messages.Count == 0) return;

        var lookup = messages.ToLookup(m => m.GetType());

        var mintMessages = lookup[typeof(RegularCoinMintMessage)].ToList();
        if (mintMessages.Count > 0)
        {
            await _innerHandler.SendPositiveAmountMessage(transaction, mintMessages.Cast<RegularCoinMintMessage>().ToList());
        }

        var burnMessages = lookup[typeof(RegularCoinBurnMessage)].ToList();
        if (burnMessages.Count > 0)
        {
            await SendNegativeMessage(transaction, wallet, burnMessages.Cast<RegularCoinBurnMessage>().ToList());
        }
    }

    public Task<IFederatedState> GetState(string wallet, string contentId)
    {
        return _innerHandler.GetState(wallet, contentId);
    }

    private async Task<RegularCoinBurnMessage?> NegativeAmountMessage(string transaction, string wallet, string function, InventoryRequest inventoryRequest)
    {
        var transactionManager = _transactionManagerFactory.Create(transaction);
        var contract = await _contractService.GetByContentId<CoinContract>(inventoryRequest.ContentId);
        var balance = await _suiApiService.GetCoinBalance(wallet, new CoinBalanceRequest(contract.PackageId, contract.Module));
        if (balance.Total >= Math.Abs(inventoryRequest.Amount))
            return new RegularCoinBurnMessage(
                inventoryRequest.ContentId,
                contract.PackageId,
                contract.Module,
                function,
                wallet,
                contract.TreasuryCap,
                Math.Abs(inventoryRequest.Amount),
                ""
            );

        await transactionManager.AddChainTransaction(new ChainTransaction
        {
            Error = $"Insufficient funds for {inventoryRequest.ContentId}, balance is {balance.Total}, requested is {Math.Abs(inventoryRequest.Amount)}",
            Function = $"{nameof(EnokiRegularCoinHandler)}.{nameof(NegativeAmountMessage)}",
            Status = "rejected",
        });
        return null;
    }

    private async Task SendNegativeMessage(string transaction, string wallet, List<RegularCoinBurnMessage> messages)
    {
        var transactionManager = _transactionManagerFactory.Create(transaction);
        try
        {
            var gamerTag = await _accountsService.GetGamerTag(wallet);
            var oauthData = await _oauthProviderService.GetByGamer(gamerTag);

            if (oauthData is null)
                throw new Exception($"No oauth data found for gamertag {gamerTag} and transaction {transaction}");

            oauthData = await _oauthProviderService.TryRecreateAuthData(oauthData);
            var token = EncryptionService.Decrypt(oauthData.Token, _configuration.RealmSecret);

            var burnTransaction = await _suiApiService.BurnRegularCurrency(messages, true);
            var sponsoredTransactionCreate = await _enokiService.CreateSponsoredTransaction(new EnokiSponsoredTransactionCreateRequest
            {
                SuiNetwork = oauthData.Network,
                Jwt = token,
                TransactionBlockKindBytes = (burnTransaction is SuiEnokiTransactionResult result ? result : default).Id,
                PlayerWalletAddress = wallet,
                Functions = messages
                    .DistinctBy(msg => new { msg.PackageId, msg.Module, msg.Function })
                    .Select(msg => new EnokiSponsoredTransactionRequestFunction(
                        msg.PackageId,
                        msg.Module,
                        msg.Function
                    ))
                    .ToArray()
            });

            var zkp = await _enokiService.CreateZkp(new EnokiZkpRequest(oauthData.Network, oauthData.EphemeralPublicKey, oauthData.MaxEpoch, oauthData.Randomness, token));
            var sponsoredTransactionSignature = await _suiApiService.EnokiSignTransaction(zkp.Data, oauthData.MaxEpoch, sponsoredTransactionCreate.CreateData.Bytes, EncryptionService.Decrypt(oauthData.EphemeralKey, _configuration.RealmSecret));
            var sponsoredTransactionResult = await _enokiService
                .ExecuteSponsoredTransaction(new EnokiSponsoredTransactionExecuteRequest(sponsoredTransactionCreate.CreateData.Digest, sponsoredTransactionSignature));

            if (!sponsoredTransactionResult.IsEmpty())
            {
                await transactionManager.AddChainTransaction(new ChainTransaction
                {
                    Digest = sponsoredTransactionResult.CreateData.Digest,
                    Error = string.Empty,
                    Function = $"{nameof(EnokiRegularCoinHandler)}.{nameof(SendNegativeMessage)}",
                    GasUsed = await _suiApiService.GetGasInfo(sponsoredTransactionResult.CreateData.Digest),
                    Data = messages.SerializeSelected(),
                    Status = "success",
                });
            }
            else
            {
                var message = $"{nameof(EnokiRegularCoinHandler)}.{nameof(SendNegativeMessage)} failed ";
                BeamableLogger.LogError(message);
                await transactionManager.TransactionError(transaction, new Exception(message));
            }

        }
        catch (Exception e)
        {
            var message =
                $"{nameof(EnokiRegularCoinHandler)}.{nameof(SendNegativeMessage)} failed with error {e.Message}";
            BeamableLogger.LogError(message);
            await transactionManager.TransactionError(transaction, new Exception(message));
        }
    }
}