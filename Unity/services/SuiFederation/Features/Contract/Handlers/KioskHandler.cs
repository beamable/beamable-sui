using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.Common.Inventory;
using Beamable.SuiFederation.Features.Common;
using Beamable.SuiFederation.Features.Contract.Exceptions;
using Beamable.SuiFederation.Features.Contract.Models;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Contract.SuiClientWrapper;
using Beamable.SuiFederation.Features.Contract.SuiClientWrapper.Models;
using Beamable.SuiFederation.Features.Kiosk.Storage;
using Beamable.SuiFederation.Features.SuiApi;
using HandlebarsDotNet;
using SuiFederationCommon.Extensions;
using SuiFederationCommon.FederationContent;

namespace Beamable.SuiFederation.Features.Contract.Handlers;

public class KioskHandler : IService
{
    private readonly ContentContractHandlerResolver _contentContractHandlerResolver;
    private readonly ContractService _contractService;
    private readonly SuiClient _suiClient;
    private readonly SuiApiService _suiApiService;
    private readonly KioskListingCollection _kioskListingCollection;
    private readonly Configuration _configuration;

    public KioskHandler(ContentContractHandlerResolver contentContractHandlerResolver, SuiClient suiClient, SuiApiService suiApiService, ContractService contractService, KioskListingCollection kioskListingCollection, Configuration configuration)
    {
        _contentContractHandlerResolver = contentContractHandlerResolver;
        _suiClient = suiClient;
        _suiApiService = suiApiService;
        _contractService = contractService;
        _kioskListingCollection = kioskListingCollection;
        _configuration = configuration;
    }

    public async Task HandleContract(KioskContentContractsModel model)
    {
        var itemModel = new ContentContractsModel(model.Item, []);
        var itemHandler = _contentContractHandlerResolver.Resolve(itemModel);
        var coinModel = new ContentContractsModel(model.Coin, []);
        var coinHandler = _contentContractHandlerResolver.Resolve(coinModel);

        var contract = await _contractService.GetByContent<KioskContract>(model.Kiosk.Id);
        if (contract != null)
        {
            var objectExists = await _suiApiService.ObjectExists(contract.PackageId);
            if (objectExists)
                return;
        }
        //Delete any existing listing for this kiosk before recreating the contract
        await _kioskListingCollection.Delete(model.Kiosk.Id, await _configuration.SuiEnvironment);

        //Compile dependencies first
        await itemHandler.HandleExistingContract(itemModel);
        await coinHandler.HandleExistingContract(coinModel);

        await WriteContractTemplate(model);
        await CompileContract(model);
        await PublishContract(model.Kiosk.ToModuleName(), model.Kiosk);
    }

    private async Task WriteContractTemplate(KioskContentContractsModel model)
    {
        var itemTemplate = await File.ReadAllTextAsync("Features/Contract/Templates/kiosk.move");
        var template = Handlebars.Compile(itemTemplate);
        var itemResult = template(new KioskContractModel(model.Kiosk.ToModuleName(), model.ItemContract.Module, model.CoinContract.Module, model.Coin is CoinCurrency));
        var contractPath = $"{SuiFederationConfig.ContractSourcePath}{model.Kiosk.ToModuleName()}.move";
        await ContractWriter.WriteContract(contractPath, itemResult);
    }

    private async Task CompileContract(KioskContentContractsModel model)
    {
        await _suiClient.CompileKiosk(model);
    }

    private async Task<string> PublishContract(string moduleName, KioskItem itemContent)
    {
        var deployOutput = await _suiClient.Publish(moduleName);
        var packageId = deployOutput.GetPackageId();
        await _contractService.UpsertContract(new KioskContract
        {
            PackageId = deployOutput.GetPackageId(),
            Module = moduleName,
            ContentId = itemContent.Id,
            MarketPlaceOwnerCap = GetMarketPlaceAdminCap(deployOutput, moduleName),
            MarketPlace = GetMarketplace(deployOutput, moduleName),
            ItemType = itemContent.ItemType,
            PriceContentId = itemContent.CurrencySymbol
        }, itemContent.Id);
        BeamableLogger.Log($"Created contract for {moduleName}");
        await _suiClient.UpdatePackageInfo(moduleName, packageId);
        return packageId;
    }

    private record KioskContractModel(string Name, string NftName, string CoinName, bool IsRegularCoin);

    private string GetMarketPlaceAdminCap(MoveDeployOutput deployOutput, string moduleName)
        => deployOutput.CreatedObjects.FirstOrDefault(obj => obj.ObjectType.EndsWith($"{deployOutput.GetPackageId()}::{moduleName}::{char.ToUpperInvariant(moduleName[0]) + moduleName[1..]}MarketplaceAdminCap"))?.ObjectId
           ?? throw new ContractException("MarketplaceAdminCap not found.");

    private string GetMarketplace(MoveDeployOutput deployOutput, string moduleName)
        => deployOutput.CreatedObjects.FirstOrDefault(obj => obj.ObjectType.EndsWith($"{deployOutput.GetPackageId()}::{moduleName}::{char.ToUpperInvariant(moduleName[0]) + moduleName[1..]}Marketplace"))?.ObjectId
           ?? throw new ContractException("Marketplace not found.");
}