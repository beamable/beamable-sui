using System.Collections.Generic;
using Beamable.Common.Content;
using Beamable.SuiFederation.Features.Contract.Storage.Models;
using SuiFederationCommon.FederationContent;

namespace Beamable.SuiFederation.Features.Contract.Models;

public record ContentContractsModel(IContentObject Parent, IEnumerable<IContentObject> Children);

public record KioskContentContractsModel(KioskItem Kiosk, ContractBase ItemContract, ContractBase CoinContract, IContentObject Item, IContentObject Coin);