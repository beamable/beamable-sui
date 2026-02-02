using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.SuiFederation.Caching;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Common;
using Beamable.SuiFederation.Features.Content.FunctionMessages;
using Beamable.SuiFederation.Features.Contract.FunctionMesseges;
using Beamable.SuiFederation.Features.Enoki.Models;
using Beamable.SuiFederation.Features.SuiApi.Converters;
using Beamable.SuiFederation.Features.SuiApi.Exceptions;
using Beamable.SuiFederation.Features.SuiApi.Models;
using SuiNodeServicve;

namespace Beamable.SuiFederation.Features.SuiApi;

public class SuiApiService : IService
{
    private readonly Configuration _configuration;
    private readonly AccountsService _accountsService;

    private readonly JsonSerializerOptions _suiTransactionSerializerOptions = new()
    {
        Converters = { new SuiTransactionResultConverter() }
    };

    private readonly JsonSerializerOptions _ignoreNull = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public SuiApiService(Configuration configuration, AccountsService accountsService)
    {
        _configuration = configuration;
        _accountsService = accountsService;
    }

    public async Task<long> GetCurrentEpoch()
    {
        using (new Measure("Sui.GetCurrentEpoch"))
        {
            return await GlobalCache.GetOrCreate<long>(nameof(GetCurrentEpoch), async _ =>
            {
                try
                {
                    var environment = await _configuration.SuiEnvironment;
                    var result = await NodeService.GetCurrentEpoch(environment);
                    return result.ToLong();
                }
                catch (Exception)
                {
                    BeamableLogger.LogWarning("Failed to get current epoch");
                    return 0;
                }
            },TimeSpan.FromHours(1)) ?? 0;
        }
    }

    public async Task<string> GetGasInfo(string digest)
    {
        using (new Measure("Sui.GetGasInfo"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var result = await NodeService.GetGasInfo(digest, environment);
                return result ?? string.Empty;
            }
            catch (Exception ex)
            {
                BeamableLogger.LogError("Can't fetch gas info for transaction {transaction}. Error: {error}", digest, ex.Message);
                return string.Empty;
            }
        }
    }

    public static async Task<CreateWalletResponse> CreateWallet()
    {
        using (new Measure("Sui.createWallet"))
        {
            try
            {
                var jsonString = await NodeService.CreateWallet();
                if (string.IsNullOrWhiteSpace(jsonString))
                    throw new Exception("createWallet output is empty.");

                return JsonSerializer.Deserialize<CreateWalletResponse>(jsonString);
            }
            catch (JsonException ex)
            {
                BeamableLogger.LogError("Can't deserialize createWallet return. Error: {error}", ex.Message);
                throw new SuiApiException($"createWallet: {ex.Message}");
            }
            catch (Exception ex)
            {
                BeamableLogger.LogError("Can't generate new private key. Error: {error}", ex.Message);
                throw new SuiApiException($"createWallet: {ex.Message}");
            }
        }
    }

    public static async Task<CreateWalletResponse> CreateEphemeral()
    {
        using (new Measure("Sui.createEphemeral"))
        {
            try
            {
                var jsonString = await NodeService.CreateEphemeral();
                if (string.IsNullOrWhiteSpace(jsonString))
                    throw new Exception("createWallet output is empty.");

                return JsonSerializer.Deserialize<CreateWalletResponse>(jsonString);
            }
            catch (JsonException ex)
            {
                BeamableLogger.LogError("Can't deserialize createWallet return. Error: {error}", ex.Message);
                throw new SuiApiException($"createWallet: {ex.Message}");
            }
            catch (Exception ex)
            {
                BeamableLogger.LogError("Can't generate new private key. Error: {error}", ex.Message);
                throw new SuiApiException($"createWallet: {ex.Message}");
            }
        }
    }

    public static async Task<CreateWalletResponse> ImportPrivateKey(string privateKey)
    {
        using (new Measure("Sui.importWallet"))
        {
            try
            {
                var jsonString = await NodeService.ImportWallet(privateKey);

                if (string.IsNullOrWhiteSpace(jsonString))
                    throw new Exception("importWallet output is empty.");

                return JsonSerializer.Deserialize<CreateWalletResponse>(jsonString);
            }
            catch (JsonException ex)
            {
                BeamableLogger.LogError("Can't deserialize importWallet return. Error: {error}", ex.Message);
                throw new SuiApiException($"importWallet: {ex.Message}");
            }
            catch (Exception ex)
            {
                BeamableLogger.LogError("Can't import a private key. Error: {error}", ex.Message);
                throw new SuiApiException($"importWallet: {ex.Message}");
            }
        }
    }

    public static async Task<bool> VerifySignature(string token, string challenge, string solution)
    {
        using (new Measure($"Sui.verifySignature: {token}"))
        {
            try
            {
                return await NodeService.VerifySignature(token, challenge, solution);
            }
            catch (Exception ex)
            {
                BeamableLogger.LogError("Can't verify signature for {token}. Error: {error}", token, ex.Message);
                return false;
            }
        }
    }

    public async Task<SuiTransactionResult> MintRegularCurrency(List<RegularCoinMintMessage> mintMessages)
    {
        using (new Measure("Sui.MintRegularCurrency"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var mintRequestJson = JsonSerializer.Serialize(mintMessages);
                var result = await NodeService.MintRegularCoin(mintRequestJson, realmAccount.PrivateKey, environment);
                return JsonSerializer.Deserialize<SuiTransactionResult>(result);
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"MintRegularCurrency: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> BurnRegularCurrency(List<RegularCoinBurnMessage> mintMessages, bool isEnoki = false)
    {
        using (new Measure("Sui.BurnRegularCurrency"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var mintRequestJson = JsonSerializer.Serialize(mintMessages);
                var result = await NodeService.BurnRegularCoin(mintRequestJson, realmAccount.PrivateKey, isEnoki, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"BurnRegularCurrency: {ex.Message}");
            }
        }
    }

    public async Task<string> EnokiSignTransaction(ZkLoginProofData zkLoginProofData, long maxEpoch, string transactionBytes, string ephemeralKey)
    {
        using (new Measure("Sui.EnokiSignTransaction"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var zkLoginProofDataJson = JsonSerializer.Serialize(zkLoginProofData);
                var result = await NodeService.EnokiSignTransaction(zkLoginProofDataJson, maxEpoch, transactionBytes, ephemeralKey, environment);
                return result;
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"EnokiSignTransaction: {ex.Message}");
            }
        }
    }

    public async Task<SuiTransactionResult> MintGameCurrency(List<GameCoinMintMessage> mintMessages)
    {
        using (new Measure("Sui.MintGameCurrency"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var mintRequestJson = JsonSerializer.Serialize(mintMessages);
                var result = await NodeService.MintGameCoin(mintRequestJson, realmAccount.PrivateKey, environment);
                return JsonSerializer.Deserialize<SuiTransactionResult>(result);
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"MintGameCurrency: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> BurnGameCurrency(List<GameCoinBurnMessage> mintMessages, bool isEnoki = false)
    {
        using (new Measure("Sui.BurnGameCurrency"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var mintRequestJson = JsonSerializer.Serialize(mintMessages);
                var result = await NodeService.BurnGameCoin(mintRequestJson, realmAccount.PrivateKey, isEnoki, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"BurnGameCurrency: {ex.Message}");
            }
        }
    }

    public async Task<CoinBalanceResponse> GetCoinBalance(string wallet, CoinBalanceRequest request)
    {
        using (new Measure("Sui.GetCoinBalance"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var requestJson = JsonSerializer.Serialize(request);
                var result = await NodeService.CoinBalance(wallet, requestJson, environment);
                return JsonSerializer.Deserialize<CoinBalanceResponse>(result);
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"GetCoinBalance: {ex.Message}");
            }
        }
    }

    public async Task<GameCoinBalanceResponse> GetGameCoinBalance(string wallet, GameCoinBalanceRequest request)
    {
        using (new Measure("Sui.GetGameCoinBalance"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var requestJson = JsonSerializer.Serialize(request);
                var result = await NodeService.GetGameCoinBalance(wallet, requestJson, environment);
                return JsonSerializer.Deserialize<GameCoinBalanceResponse>(result);
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"GetGameCoinBalance: {ex.Message}");
            }
        }
    }

    public async Task<SuiTransactionResult> MintNfts(List<NftMintMessage> mintMessages)
    {
        using (new Measure("Sui.MintNfts"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var mintRequestJson = JsonSerializer.Serialize(mintMessages);
                var result = await NodeService.MintNfts(mintRequestJson, realmAccount.PrivateKey, environment);
                return JsonSerializer.Deserialize<SuiTransactionResult>(result);
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"MintNfts: {ex.Message}");
            }
        }
    }

    public async Task<IEnumerable<GetOwnedObjectsResponse>> GetOwnedObjects(string wallet, GetOwnedObjectsRequest request)
    {
        using (new Measure("Sui.GetOwnedObjects"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var result = await NodeService.GetOwnedObjects(wallet, request.PackageId, environment);
                return JsonSerializer.Deserialize<IEnumerable<GetOwnedObjectsResponse>>(result)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"GetOwnedObjects: {ex.Message}");
            }
        }
    }

    public async Task<IEnumerable<GetOwnedObjectsResponse>> GetAttachments(string objectId)
    {
        using (new Measure("Sui.GetAttachments"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var result = await NodeService.GetAttachments(objectId, environment);
                return JsonSerializer.Deserialize<IEnumerable<GetOwnedObjectsResponse>>(result)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"GetAttachments: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> DeleteNft(List<NftDeleteMessage> request, bool isEnoki = false)
    {
        using (new Measure("Sui.DeleteNft"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(request);
                var result = await NodeService.DeleteNfts(requestJson, realmAccount.PrivateKey, isEnoki, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"DeleteNft: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> UpdateNft(List<NftUpdateMessage> request, bool isEnoki = false)
    {
        using (new Measure("Sui.UpdateNft"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(request, _ignoreNull);
                var result = await NodeService.UpdateNfts(requestJson, realmAccount.PrivateKey, isEnoki, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"UpdateNft: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> AttachNft(List<NftUpdateMessage> request, bool isEnoki = false)
    {
        using (new Measure("Sui.AttachNft"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(request, _ignoreNull);
                var result = await NodeService.AttachNft(requestJson, realmAccount.PrivateKey, isEnoki, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"AttachNft: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> DetachNft(List<NftUpdateMessage> request, bool isEnoki = false)
    {
        using (new Measure("Sui.DetachNft"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(request, _ignoreNull);
                var result = await NodeService.DetachNft(requestJson, realmAccount.PrivateKey, isEnoki, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"DetachNft: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> NftAttachmentUpdate(List<NftUpdateMessage> request, bool isEnoki = false)
    {
        using (new Measure("Sui.NftAttachmentUpdate"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(request, _ignoreNull);
                var result = await NodeService.NftAttachmentUpdate(requestJson, realmAccount.PrivateKey, isEnoki, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"NftAttachmentUpdate: {ex.Message}");
            }
        }
    }

    public async Task<SuiTransactionResult> SetNftContractOwner(SetOwnerMessage message)
    {
        using (new Measure("Sui.SetNftContractOwner"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(message);
                var result = await NodeService.SetNftContractOwner(requestJson, realmAccount.PrivateKey, environment);
                return JsonSerializer.Deserialize<SuiTransactionResult>(result);
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"SetNftContractOwner: {ex.Message}");
            }
        }
    }

    public async Task<bool> ObjectExists(string objectId)
    {
        using (new Measure("Sui.ObjectExists"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var result = await NodeService.ObjectExists(objectId, environment);
                if (bool.TryParse(result, out var exists))
                    return exists;
                return false;
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"ObjectExists: {ex.Message}");
            }
        }
    }

    public async Task<SuiTransactionResult> WithdrawCurrency(GameCoinTransferMessage transferMessage)
    {
        using (new Measure("Sui.WithdrawCurrency"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(transferMessage);
                var result = await NodeService.WithdrawCurrency(requestJson, realmAccount.PrivateKey, environment);
                return JsonSerializer.Deserialize<SuiTransactionResult>(result);
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"WithdrawCurrency: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> ListForSale(KioskListMessage message)
    {
        using (new Measure("Sui.ListForSale"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(message);
                var result = await NodeService.ListForSale(requestJson, realmAccount.PrivateKey, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"ListForSale: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> DelistFromSale(KioskDelistMessage message)
    {
        using (new Measure("Sui.DelistFromSale"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(message);
                var result = await NodeService.DelistFromSale(requestJson, realmAccount.PrivateKey, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"DelistFromSale: {ex.Message}");
            }
        }
    }

    public async Task<ISuiTransactionResult> KioskPurchase(KioskPurchaseMessage message)
    {
        using (new Measure("Sui.KioskPurchase"))
        {
            try
            {
                var environment = await _configuration.SuiEnvironment;
                var realmAccount = await _accountsService.GetOrCreateRealmAccount();
                var requestJson = JsonSerializer.Serialize(message);
                var result = await NodeService.KioskPurchase(requestJson, realmAccount.PrivateKey, environment);
                return JsonSerializer.Deserialize<ISuiTransactionResult>(result, _suiTransactionSerializerOptions)
                       ?? throw new SuiApiException("Deserialization failed.");
            }
            catch (Exception ex)
            {
                throw new SuiApiException($"KioskPurchase: {ex.Message}");
            }
        }
    }
}