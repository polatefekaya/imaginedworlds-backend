using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using Microsoft.AspNetCore.SignalR;

namespace ImaginedWorlds.Infrastructure;

public class SignalRNotifier : ISimulationNotifier
{
    private readonly IHubContext<ImaginedWorldsHub> _hubContext;

    public SignalRNotifier(IHubContext<ImaginedWorldsHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifySimulationStarted(string connectionId) =>
        _hubContext.Clients.Client(connectionId).SendAsync("SimulationStarted");

    public Task NotifySimulationEnded(string connectionId) =>
        _hubContext.Clients.Client(connectionId).SendAsync("SimulationEnded");

    public Task NotifyPlanCreated(string connectionId, ConstructionPlanResponse plan) =>
        _hubContext.Clients.Client(connectionId).SendAsync("PlanCreated", plan);

    public Task NotifyFocusChanged(string connectionId, FocusResponse focus) =>
        _hubContext.Clients.Client(connectionId).SendAsync("FocusChanged", focus);

    public Task NotifyWorldUpdatedBatch(string connectionId, IReadOnlyList<CommentedTilePatchResponse> patches) =>
        _hubContext.Clients.Client(connectionId).SendAsync("WorldUpdatedBatch", patches);
    
    public Task NotifyWorldUpdatedPiece(string connectionId, CommentedTilePatchResponse patch) =>
        _hubContext.Clients.Client(connectionId).SendAsync("WorldUpdatedPiece", patch);

    public Task NotifyStageStarted(string connectionId, Stage stage) =>
        _hubContext.Clients.Client(connectionId).SendAsync("StageStarted", stage);
}
