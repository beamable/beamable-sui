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
using SuiFederationCommon.FederationContent;

namespace Beamable.SuiFederation.Features.Contract.Handlers;

public class PlayerKioskHandler : IService
{
    private readonly ContractService _contractService;
    private readonly SuiClient _suiClient;
    private readonly SuiApiService _suiApiService;
    private readonly Configuration _configuration;

    public const string ModuleName = "playerkiosk";
    public const string PriceSymbol = "sui";

    public PlayerKioskHandler(ContractService contractService, SuiClient suiClient, SuiApiService suiApiService, Configuration configuration)
    {
        _contractService = contractService;
        _suiClient = suiClient;
        _suiApiService = suiApiService;
        _configuration = configuration;
    }

    public async Task HandleContract()
    {
        var contract = await _contractService.GetByContent<PlayerKioskContract>(ModuleName);
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
        var itemTemplate = await File.ReadAllTextAsync("Features/Contract/Templates/player_kiosk.move");
        var template = Handlebars.Compile(itemTemplate);
        var itemResult = template(new KioskContractModel(model.ModuleName));
        var contractPath = $"{SuiFederationConfig.ContractSourcePath}{model.ModuleName}.move";
        await ContractWriter.WriteContract(contractPath, itemResult);
    }

    private async Task CompileContract(PersonalKioskContractModel model)
    {
        await _suiClient.CompilePlayerKiosk(model);
    }

    private async Task<string> PublishContract(PersonalKioskContractModel itemContent)
    {
        var deployOutput = await _suiClient.Publish(itemContent.ModuleName);
        var packageId = deployOutput.GetPackageId();
        await _contractService.UpsertContract(new PlayerKioskContract
        {
            PackageId = deployOutput.GetPackageId(),
            Module = itemContent.ModuleName,
            ContentId = itemContent.ModuleName
        }, itemContent.ModuleName);
        BeamableLogger.Log($"Created contract for {itemContent.ModuleName}");
        return packageId;
    }

    private record KioskContractModel(string Name);
}