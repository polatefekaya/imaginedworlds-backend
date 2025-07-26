using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Application.Contracts;

public record CommentedTilePatchResponse
(
    TileType TileType,
    int X,
    int Y,
    string Comment
);

public static class CommentedTilePatchResponseExtensions {
    public static TilePatch ToTilePatch(this CommentedTilePatchResponse patch)
    {
        return new(patch.TileType, new(patch.X,patch.Y));
    }
};
