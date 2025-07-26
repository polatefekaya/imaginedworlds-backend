using System;
using Mediator;

namespace ImaginedWorlds.Application.Grid.SetTile;

public class SetTileCommandHandler : ICommandHandler<SetTileCommand, Ulid>
{
    public ValueTask<Ulid> Handle(SetTileCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
