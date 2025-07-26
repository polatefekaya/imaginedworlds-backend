using ImaginedWorlds.Domain.Common;
using ImaginedWorlds.Domain.Common.Primitives;

namespace ImaginedWorlds.Domain.Grid;

public sealed record GridTileSetDomainEvent : AggregatedDomainEvent
{
    public TilePatch Patch { get; private set; }

    public GridTileSetDomainEvent(Ulid aggregateId, TilePatch patch) : base(aggregateId)
    {
        Patch = patch;
    }
}
