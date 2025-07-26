using ImaginedWorlds.Domain.Common;

namespace ImaginedWorlds.Domain.Grid;

public record class TilePatch(
    TileType TileType,
    Coordinates Coordinates
);
