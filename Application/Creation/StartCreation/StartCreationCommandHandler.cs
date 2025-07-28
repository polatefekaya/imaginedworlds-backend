using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Grid;
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
        try
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
            _promptManager.SetSystemPrompt("You are ImaginedWorlds, a world-building AI. Your sole purpose is to translate abstract human imagination into the structured data required to render a visual world. You are a creative partner, but also a precise, logical engine.");

            await _coordinator.Execute(agent, plan.Stages, terrain, command.ConnectionId, cancellationToken);
            return terrain.Id;
        }
        catch (Exception ex)
        {
            await _notifier.NotifyGenerationFailed(command.ConnectionId, ex.Message);
            return Ulid.Empty;
        }
    }
}
