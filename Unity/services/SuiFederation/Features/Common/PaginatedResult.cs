using System.Collections.Generic;

namespace Beamable.SuiFederation.Features.Common;

public class PaginatedResult<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public long TotalCount { get; set; }
    public List<T> Items { get; set; } = [];
}