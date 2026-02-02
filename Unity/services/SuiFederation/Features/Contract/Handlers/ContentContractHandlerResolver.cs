using System;
using System.Threading.Tasks;
using Beamable.Common.Content;
using Beamable.Common.Dependencies;
using Beamable.SuiFederation.Features.Contract.Models;
using SuiFederationCommon.FederationContent;
using SuiFederationCommon.Models;

namespace Beamable.SuiFederation.Features.Contract.Handlers;

public class ContentContractHandlerResolver(IDependencyProvider dependencyProvider) : IService
{
    public IContentContractHandler Resolve(ContentContractsModel model)
    {
        ContractTemplateService.Initialize();
        return model.Parent switch
        {
            CoinCurrency => dependencyProvider.GetService<CoinCurrencyHandler>(),
            InGameCurrency => dependencyProvider.GetService<GameCurrencyHandler>(),
            INftBase and IContentObject => dependencyProvider.GetService<NftContractHandler>(),
            _ => throw new InvalidOperationException($"No handler found for content type: {model.Parent.GetType().Name}")
        };
    }

    public async Task HandleContent(KioskContentContractsModel model)
    {
        ContractTemplateService.Initialize();
        switch (model.Kiosk)
        {
            case not null:
                await dependencyProvider.GetService<KioskHandler>().HandleContract(model);
                break;
            default:
                throw new InvalidOperationException("No handler found for kiosk content type:");
        }
    }

    public async Task HandlePersonalKiosk()
    {
        ContractTemplateService.Initialize();
        await dependencyProvider.GetService<PersonalKioskHandler>().HandleContract();
        await dependencyProvider.GetService<RoyaltyKioskHandler>().HandleContract();
    }

    public async Task HandlePlayerKiosk()
    {
        ContractTemplateService.Initialize();
        await dependencyProvider.GetService<PlayerKioskHandler>().HandleContract();
    }
}