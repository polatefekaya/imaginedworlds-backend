using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using Microsoft.AspNetCore.SignalR;

namespace ImaginedWorlds.Infrastructure;

public class SignalRNotifier : ISimulationNotifier
{
    private readonly IHubContext<ImaginedWorldsHub> _hubContext;
    private readonly ILogger<ISimulationNotifier> _logger;

    public SignalRNotifier(IHubContext<ImaginedWorldsHub> hubContext, ILogger<ISimulationNotifier> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotifySimulationStarted(string connectionId) {
        await _hubContext.Clients.Client(connectionId).SendAsync("SimulationStarted");
        _logger.LogDebug("{methodName} notification is sent to client with connectionId: {connectionId}", "SimulationStarted", connectionId);
    }

    public async Task NotifySimulationEnded(string connectionId) {
        await _hubContext.Clients.Client(connectionId).SendAsync("SimulationEnded");
        _logger.LogDebug("{methodName} notification is sent to client with connectionId: {connectionId}", "SimulationEnded", connectionId);
    }

    public async Task NotifyPlanCreated(string connectionId, ConstructionPlanResponse plan)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync("PlanCreated", plan);
        _logger.LogDebug("{methodName} notification is sent to client with connectionId: {connectionId}", "PlanCreated", connectionId);
    }

    public async Task NotifyFocusChanged(string connectionId, FocusResponse focus) {
        await _hubContext.Clients.Client(connectionId).SendAsync("FocusChanged", focus);
        _logger.LogDebug("{methodName} notification is sent to client with connectionId: {connectionId}", "FocusChanged", connectionId);
    }

    public async Task NotifyWorldUpdatedBatch(string connectionId, IReadOnlyList<CommentedTilePatchResponse> patches)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync("WorldUpdatedBatch", patches);
        _logger.LogDebug("{methodName} notification is sent to client with connectionId: {connectionId}", "WorldUpdatedBatch", connectionId);
    }

    public async Task NotifyWorldUpdatedPiece(string connectionId, CommentedTilePatchResponse patch)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync("WorldUpdatedPiece", patch);
        _logger.LogDebug("{methodName} notification is sent to client with connectionId: {connectionId}", "WorldUpdatedPiece", connectionId);
    }

    public async Task NotifyStageStarted(string connectionId, Stage stage) {
        await _hubContext.Clients.Client(connectionId).SendAsync("StageStarted", stage);
        _logger.LogDebug("{methodName} notification is sent to client with connectionId: {connectionId}", "StageStarted", connectionId);
    }

    public async Task NotifyGenerationFailed(string connectionId, string errorMessage) {
        await _hubContext.Clients.Client(connectionId).SendAsync("GenerationFailed", errorMessage);
        _logger.LogDebug("{methodName} notification is sent to client with connectionId: {connectionId}","GenerationFailed", connectionId);
    }
}
