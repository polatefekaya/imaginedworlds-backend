using ImaginedWorlds.Domain.Common;

namespace ImaginedWorlds.Domain.Grid;

public record class TilePatch(
    TileType TileType,
    Coordinates Coordinates
)
{
    public override string ToString()
    {
        return $"x: {Coordinates.X}, y: {Coordinates.Y}, tileType: {TileType} ({(int)TileType})";
    }
};
