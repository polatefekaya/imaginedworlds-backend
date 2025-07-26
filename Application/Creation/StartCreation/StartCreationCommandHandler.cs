using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;
using ImaginedWorlds.Infrastructure;
using Mediator;

namespace ImaginedWorlds.Application.Creation.StartCreation;

public class StartCreationCommandHandler : ICommandHandler<StartCreationCommand>
{
    private readonly IArchitectFactory _architectFactory;

    public StartCreationCommandHandler(IArchitectFactory architectFactory)
    {
        _architectFactory = architectFactory;
    }

    public async ValueTask<Unit> Handle(StartCreationCommand command, CancellationToken cancellationToken)
    {
        IArchitect architect = await _architectFactory.Create(command.AgentCodeName, cancellationToken);
        ConstructionPlan plan = await architect.GetPlanAsync(command.UserPrompt);

        GridTerrain terrain = GridTerrain.Create(100, 100);
        
        Coordinator coordinator = new();

        coordinator.Execute(plan.Stages, terrain.GridView);
    }
}
