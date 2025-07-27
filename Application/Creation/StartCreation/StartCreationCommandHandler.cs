using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;
using ImaginedWorlds.Infrastructure;
using Mediator;

namespace ImaginedWorlds.Application.Creation.StartCreation;

public class StartCreationCommandHandler : ICommandHandler<StartCreationCommand, Ulid>
{
    private readonly IArchitectFactory _architectFactory;
    private readonly ICoordinator _coordinator;
    private readonly IPromptBuilder _promptBuilder;
    private readonly IPromptManager _promptManager;
    private readonly ISimulationNotifier _notifier;

    public StartCreationCommandHandler(IArchitectFactory architectFactory, ICoordinator coordinator, IPromptBuilder promptBuilder, IPromptManager promptManager, ISimulationNotifier notifier)
    {
        _architectFactory = architectFactory;
        _coordinator = coordinator;
        _promptBuilder = promptBuilder;
        _promptManager = promptManager;
        _notifier = notifier;
    }

    public async ValueTask<Ulid> Handle(StartCreationCommand command, CancellationToken cancellationToken)
    {
        GridTerrain terrain = GridTerrain.Create(100, 100);
        //call notifier and send emptygrid command
        await _notifier.NotifySimulationStarted(command.ConnectionId);

        IArchitect architect = await _architectFactory.Create(command.AgentCodeName, cancellationToken);
        string prompt = await _promptBuilder.BuildArchitectPrompt(command.UserPrompt);

        (ConstructionPlanResponse plan, Agent agent) = await architect.GetPlanAsync(prompt, cancellationToken);
        //call notifier about stages prepared.
        await _notifier.NotifyPlanCreated(command.ConnectionId, plan);

        _promptManager.SetOverallGoal(plan.OverallPlan);
        _promptManager.SetSystemPrompt("");

        await _coordinator.Execute(agent, plan.Stages, terrain, command.ConnectionId, cancellationToken);
        return terrain.Id;
    }
}
