using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.Common.Inventory;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.Common;
using Beamable.SuiFederation.Features.Contract.Exceptions;
using Beamable.SuiFederation.Features.Contract.FunctionMesseges;
using Beamable.SuiFederation.Features.Contract.Handlers.Models;
using Beamable.SuiFederation.Features.Contract.Models;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using Beamable.SuiFederation.Features.Contract.SuiClientWrapper;
using Beamable.SuiFederation.Features.Contract.SuiClientWrapper.Models;
using Beamable.SuiFederation.Features.SuiApi;
using HandlebarsDotNet;
using SuiFederationCommon.Extensions;

namespace Beamable.SuiFederation.Features.Contract.Handlers;

public class NftContractHandler : IService, IContentContractHandler
{
    private readonly ContractService _contractService;
    private readonly SuiClient _suiClient;
    private readonly SuiApiService _suiApiService;
    private readonly AccountsService _accountsService;

    public NftContractHandler(ContractService contractService, SuiClient suiClient, SuiApiService suiApiService, AccountsService accountsService)
    {
        _contractService = contractService;
        _suiClient = suiClient;
        _suiApiService = suiApiService;
        _accountsService = accountsService;
    }

    public async Task HandleContract(ContentContractsModel model)
    {
        try
        {
            var clientContentInfo = model.Parent;
            var itemContent = clientContentInfo as ItemContent;
            var moduleName = itemContent!.ToModuleName();
            var hasChildren = model.Children.Any();
            var contract = await _contractService.GetByContent<NftContract>(itemContent!.ContentType);
            var objectExists = false;
            var parentPackageId = "";
            if (contract != null)
            {
                parentPackageId = contract.PackageId;
                objectExists = await _suiApiService.ObjectExists(contract.PackageId);
                if (objectExists && !hasChildren)
                {
                    BeamableLogger.Log($"Contract for {moduleName} already exists.");
                    return;
                }
            }

            var existingPackageId = objectExists ? contract?.PackageId ?? string.Empty : string.Empty;

            await WriteContractTemplate(moduleName);

            if (!objectExists)
            {
                await CompileContract(moduleName, existingPackageId);
                parentPackageId = await PublishContract(moduleName, itemContent);
            }

            if (hasChildren)
            {
                await CreateChildContracts(model, parentPackageId);
            }
        }
        catch (Exception e)
        {
            throw new ContractException($"Error in creating contract for {model.Parent.GetType()}, exception: {e.Message}");
        }
    }

    public async Task HandleExistingContract(ContentContractsModel model)
    {
        try
        {
            var clientContentInfo = model.Parent;
            var itemContent = clientContentInfo as ItemContent;
            var moduleName = itemContent!.ToModuleName();
            var contract = await _contractService.GetByContent<NftContract>(itemContent!.ContentType);
            if (contract is null || clientContentInfo is not ItemContent)
                throw new ContractException($"{clientContentInfo.Id} is not a {nameof(NftContract)}");

            await WriteContractTemplate(moduleName);
            await CompileContract(moduleName, contract.PackageId);
        }
        catch (Exception e)
        {
            throw new ContractException($"Error in creating contract for {model.Parent.Id}, exception: {e.Message}");
        }
    }

    private async Task CreateChildContracts(ContentContractsModel model, string parentPackageId)
    {
        try
        {
            var clientContentInfoParent = model.Parent;
            var itemContentParent = clientContentInfoParent as ItemContent;
            var moduleNameParent = itemContentParent!.ToModuleName();

            foreach (var clientContentInfo in model.Children)
            {
                var itemContent = clientContentInfo as ItemContent;
                var moduleName = itemContent!.ToModuleName();
                var contract = await _contractService.GetByContent<NftContract>(itemContent!.ContentType);
                if (contract != null)
                {
                    var objectExists = await _suiApiService.ObjectExists(contract.PackageId);
                    if (objectExists)
                        continue;
                }

                var itemTemplate = await File.ReadAllTextAsync("Features/Contract/Templates/nft_addon.move");
                var template = Handlebars.Compile(itemTemplate);
                var itemResult = template(new NftTemplate(moduleName, moduleNameParent));
                var contractPath = $"{SuiFederationConfig.ContractSourcePath}{moduleName}.move";
                await ContractWriter.WriteContract(contractPath, itemResult);

                await _suiClient.Compile(moduleName, moduleNameParent, parentPackageId);
                var deployOutput = await _suiClient.Publish(moduleName);

                var ownerObjectId = await CreateContractOwnerObject(deployOutput.GetPackageId(), moduleName, GetAdminCap(deployOutput, moduleName));

                await _contractService.UpsertContract(new NftContract
                {
                    PackageId = deployOutput.GetPackageId(),
                    Module = moduleName,
                    ContentId = itemContent!.ContentType,
                    AdminCap = GetAdminCap(deployOutput, moduleName),
                    OwnerInfo = ownerObjectId,
                    Policy = GetPolicy(deployOutput, moduleName),
                    PolicyCap = GetPolicyCap(deployOutput, moduleName)
                }, itemContent!.ContentType);

                BeamableLogger.Log($"Created child contract for {moduleName}");
            }
        }
        catch (Exception e)
        {
            throw new ContractException($"Error in creating child contracts for {model.Parent.GetType()}, exception: {e.Message}");
        }
    }

    private async Task WriteContractTemplate(string moduleName)
    {
        var itemTemplate = await File.ReadAllTextAsync("Features/Contract/Templates/nft.move");
        var template = Handlebars.Compile(itemTemplate);
        var itemResult = template(new NftTemplate(moduleName, ""));
        var contractPath = $"{SuiFederationConfig.ContractSourcePath}{moduleName}.move";
        await ContractWriter.WriteContract(contractPath, itemResult);
    }

    private async Task CompileContract(string moduleName, string existingPackageId = "")
    {
        await _suiClient.Compile(moduleName, existingPackageId: existingPackageId);
    }

    private async Task<string> PublishContract(string moduleName, ItemContent itemContent)
    {
        var deployOutput = await _suiClient.Publish(moduleName);
        var packageId = deployOutput.GetPackageId();
        var ownerObjectId = await CreateContractOwnerObject(deployOutput.GetPackageId(), moduleName, GetAdminCap(deployOutput, moduleName));
        await _contractService.UpsertContract(new NftContract
        {
            PackageId = deployOutput.GetPackageId(),
            Module = moduleName,
            ContentId = itemContent!.ContentType,
            AdminCap = GetAdminCap(deployOutput, moduleName),
            OwnerInfo = ownerObjectId,
            Policy = GetPolicy(deployOutput, moduleName),
            PolicyCap = GetPolicyCap(deployOutput, moduleName)
        }, itemContent!.ContentType);
        BeamableLogger.Log($"Created contract for {moduleName}");
        await _suiClient.UpdatePackageInfo(moduleName, packageId);
        return packageId;
    }

    private async Task<string> CreateContractOwnerObject(string packageId, string moduleName, string adminCap)
    {
        try
        {
            var realmAccount = await _accountsService.GetOrCreateRealmAccount();
            var result = await _suiApiService.SetNftContractOwner(new SetOwnerMessage(packageId, moduleName, "set_owner", realmAccount.Address, adminCap));
            return result.objectIds[0];
        }
        catch (Exception e)
        {
            throw new ContractException($"Error in creating owner info for {moduleName}, exception: {e.Message}");
        }
    }

    private string GetAdminCap(MoveDeployOutput deployOutput, string moduleName)
        => deployOutput.CreatedObjects.FirstOrDefault(obj => obj.ObjectType.StartsWith($"{deployOutput.GetPackageId()}::{moduleName}::AdminCap"))?.ObjectId
           ?? throw new ContractException("AdminCap not found.");

    private string GetPolicy(MoveDeployOutput deployOutput, string moduleName)
        => deployOutput.CreatedObjects.FirstOrDefault(obj => obj.ObjectType.StartsWith($"0x2::transfer_policy::TransferPolicy<{deployOutput.GetPackageId()}"))?.ObjectId
           ?? throw new ContractException("AdminCap not found.");

    private string GetPolicyCap(MoveDeployOutput deployOutput, string moduleName)
        => deployOutput.CreatedObjects.FirstOrDefault(obj => obj.ObjectType.StartsWith($"0x2::transfer_policy::TransferPolicyCap<{deployOutput.GetPackageId()}"))?.ObjectId
           ?? throw new ContractException("AdminCap not found.");
}