using ImaginedWorlds.Domain.Common;
using ImaginedWorlds.Domain.Grid;
using Mediator;

namespace ImaginedWorlds.Application.Grid.SetTile;

public record struct SetTileCommand(
    Coordinates Coordinates,
    TileType TileType
) : ICommand<Ulid>;
