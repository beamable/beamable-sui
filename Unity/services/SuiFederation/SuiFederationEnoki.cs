using System.Collections.Generic;
using Beamable.Common;
using Beamable.Common.Api.Inventory;
using Beamable.Server;
using Beamable.SuiFederation.Endpoints;
using Beamable.SuiFederation.Extensions;
using Beamable.SuiFederation.Features.Accounts;
using Beamable.SuiFederation.Features.OAuthProvider;
using SuiFederationCommon;
using SuiFederationCommon.Models.Oauth;

namespace Beamable.SuiFederation;

public partial class SuiFederation : IFederatedInventory<SuiWeb3EnokiIdentity>
{
    async Promise<FederatedAuthenticationResponse> IFederatedLogin<SuiWeb3EnokiIdentity>.Authenticate(string token, string challenge, string solution)
    {
        return await Provider.GetService<AuthenticateEnokiEndpoint>()
            .Authenticate(token, challenge, solution);
    }

    async Promise<FederatedInventoryProxyState> IFederatedInventory<SuiWeb3EnokiIdentity>.GetInventoryState(string id)
    {
        var microserviceInfo = MicroserviceMetadataExtensions.GetMetadata<SuiFederation, SuiWeb3EnokiIdentity>();
        var existingExternalIdentity = microserviceInfo.ToExternalIdentity();
        return await Provider.GetService<GetInventoryStateEndpoint>()
            .GetInventoryState(id, existingExternalIdentity!);
    }

    async Promise<FederatedInventoryProxyState> IFederatedInventory<SuiWeb3EnokiIdentity>.StartInventoryTransaction(string id, string transaction, Dictionary<string, long> currencies, List<FederatedItemCreateRequest> newItems, List<FederatedItemDeleteRequest> deleteItems,
        List<FederatedItemUpdateRequest> updateItems)
    {
        var gamerTag = await Provider.GetService<AccountsService>().GetGamerTag(id);
        var microserviceInfo = MicroserviceMetadataExtensions.GetMetadata<SuiFederation, SuiWeb3EnokiIdentity>();
        return await Provider.GetService<StartInventoryTransactionEndpoint>()
            .StartInventoryTransaction(id, transaction, currencies, newItems, deleteItems, updateItems, gamerTag, microserviceInfo);
    }

    [Callable]
    public async Promise<OauthCallbackResponse> OAuthCallback()
    {
        return await Provider.GetService<OAuthProviderCallbackService>().ProcessCallback(Context.Body);
    }
}