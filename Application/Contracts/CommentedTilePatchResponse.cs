using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Application.Contracts;

public record CommentedTilePatchResponse
(
    string Comment,
    int TileType,
    int X,
    int Y
)
{
    public override string ToString()
    {
        return $"x: {X}, y: {Y}, tileType: {TileType} ({(int)TileType}), comment: {Comment}";
    }
};

public static class CommentedTilePatchResponseExtensions {
    public static TilePatch ToTilePatch(this CommentedTilePatchResponse patch)
    {
        return new((TileType)patch.TileType, new(patch.X,patch.Y));
    }
};
