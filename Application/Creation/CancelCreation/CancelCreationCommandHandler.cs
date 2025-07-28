using System;
using ImaginedWorlds.Application.Abstractions;
using Mediator;

namespace ImaginedWorlds.Application.Creation.CancelCreation;

public sealed class CancelCreationCommandHandler : ICommandHandler<CancelCreationCommand>
{
    private readonly ISimulationManager _simulationManager;

    public CancelCreationCommandHandler(ISimulationManager simulationManager) 
        => _simulationManager = simulationManager;

    public async ValueTask<Unit> Handle(CancelCreationCommand command, CancellationToken cancellationToken)
    {
        _simulationManager.Cancel(command.ConnectionId);
        return Unit.Value;
    }
}