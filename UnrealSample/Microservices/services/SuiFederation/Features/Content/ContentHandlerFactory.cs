using System;
using Beamable.Common.Content;
using Beamable.Common.Dependencies;
using Beamable.Common.Inventory;
using Beamable.SuiFederation.Features.Content.Handlers;
using Microsoft.Extensions.DependencyInjection;
using SuiFederationCommon;
using SuiFederationCommon.FederationContent;
using SuiFederationCommon.Models;

namespace Beamable.SuiFederation.Features.Content;

public class ContentHandlerFactory(
    IDependencyProvider serviceProvider) : IService
{
    public IContentHandler GetHandler(IContentObject contentObject)
    {
        return contentObject switch
        {
            CurrencyContent coin => coin switch
            {
                CoinCurrency currency when currency.federation.HasValue &&
                                           currency.federation.Value.Namespace ==
                                           SuiFederationSettings.SuiEnokiIdentityName
                    => serviceProvider.GetRequiredService<EnokiRegularCoinHandler>(),
                CoinCurrency => serviceProvider.GetRequiredService<RegularCoinHandler>(),
                InGameCurrency currency when currency.federation.HasValue &&
                                             currency.federation.Value.Namespace ==
                                             SuiFederationSettings.SuiEnokiIdentityName
                    => serviceProvider.GetRequiredService<EnokiGameCoinHandler>(),
                InGameCurrency => serviceProvider.GetRequiredService<GameCoinHandler>(),
                _ => throw new NotSupportedException($"ContentId '{contentObject.Id}' is not supported.")
            },
            ItemContent item => item switch
            {
                INftBase when item.federation.HasValue &&
                              item.federation.Value.Namespace == SuiFederationSettings.SuiEnokiIdentityName
                    => serviceProvider.GetRequiredService<EnokiNftHandler>(),
                INftBase => serviceProvider.GetRequiredService<NftHandler>(),
                _ => throw new NotSupportedException($"ContentId '{contentObject.Id}' is not supported.")
            },
            _ => throw new NotSupportedException($"ContentId '{contentObject.Id}' is not supported.")
        };
    }
}