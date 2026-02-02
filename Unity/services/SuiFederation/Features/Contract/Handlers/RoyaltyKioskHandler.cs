using System.IO;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.SuiFederation.Features.Common;
using Beamable.SuiFederation.Features.Contract.Models;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Contract.SuiClientWrapper;
using Beamable.SuiFederation.Features.Contract.SuiClientWrapper.Models;
using Beamable.SuiFederation.Features.SuiApi;
using HandlebarsDotNet;

namespace Beamable.SuiFederation.Features.Contract.Handlers;

public class RoyaltyKioskHandler : IService
{
    private readonly ContractService _contractService;
    private readonly SuiClient _suiClient;
    private readonly SuiApiService _suiApiService;
    private readonly Configuration _configuration;

    private const string ModuleName = "royaltyrule";

    public RoyaltyKioskHandler(ContractService contractService, SuiClient suiClient, SuiApiService suiApiService, Configuration configuration)
    {
        _contractService = contractService;
        _suiClient = suiClient;
        _suiApiService = suiApiService;
        _configuration = configuration;
    }

    public async Task<RoyaltyKioskContract> GetContract()
    {
        return (await _contractService.GetByContent<RoyaltyKioskContract>(ModuleName))!;
    }

    public async Task HandleContract()
    {
        if (await _configuration.SuiEnvironment != "devnet")
            return;

        var contract = await _contractService.GetByContent<RoyaltyKioskContract>(ModuleName);
        if (contract != null)
        {
            var objectExists = await _suiApiService.ObjectExists(contract.PackageId);
            if (objectExists)
                return;
        }

        var model = new PersonalKioskContractModel(ModuleName);
        await WriteContractTemplate(model);
        await CompileContract(model);
        await PublishContract(model);
    }

    private async Task WriteContractTemplate(PersonalKioskContractModel model)
    {
        var itemTemplate = await File.ReadAllTextAsync("Features/Contract/Templates/royalty_kiosk.move");
        var template = Handlebars.Compile(itemTemplate);
        var itemResult = template(new KioskContractModel(model.ModuleName));
        var contractPath = $"{SuiFederationConfig.ContractSourcePath}{model.ModuleName}.move";
        await ContractWriter.WriteContract(contractPath, itemResult);
    }

    private async Task CompileContract(PersonalKioskContractModel model)
    {
        await _suiClient.CompilePersonalKiosk(model);
    }

    private async Task<string> PublishContract(PersonalKioskContractModel itemContent)
    {
        var deployOutput = await _suiClient.Publish(itemContent.ModuleName);
        var packageId = deployOutput.GetPackageId();
        await _contractService.UpsertContract(new RoyaltyKioskContract
        {
            PackageId = packageId,
            Module = itemContent.ModuleName,
            ContentId = itemContent.ModuleName
        }, itemContent.ModuleName);
        BeamableLogger.Log($"Created contract for {itemContent.ModuleName}");
        return packageId;
    }

    private record KioskContractModel(string Name);
}