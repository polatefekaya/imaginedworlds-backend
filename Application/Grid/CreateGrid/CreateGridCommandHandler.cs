using System;
using Mediator;

namespace ImaginedWorlds.Application.Grid.CreateGrid;

public class CreateGridCommandHandler : ICommandHandler<CreateGridCommand, Ulid>
{
    public ValueTask<Ulid> Handle(CreateGridCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
