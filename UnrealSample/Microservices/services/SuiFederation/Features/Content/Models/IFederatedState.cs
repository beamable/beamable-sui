using System.Collections.Generic;
using Beamable.Common;

namespace Beamable.SuiFederation.Features.Content.Models;

public interface IFederatedState { }

public record CurrenciesState : IFederatedState
{
    public Dictionary<string, long> Currencies { get; init; } = new();
}

public record ItemsState : IFederatedState
{
    public Dictionary<string, List<FederatedItemProxy>> Items { get; init; } = new();
}