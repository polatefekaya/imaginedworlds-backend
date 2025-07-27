using System;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;

namespace ImaginedWorlds.Application.Abstractions;

public interface ISimulationNotifier
{
    Task NotifySimulationStarted(string connectionId);
    Task NotifySimulationEnded(string connectionId);
    Task NotifyPlanCreated(string connectionId, ConstructionPlanResponse plan);
    Task NotifyFocusChanged(string connectionId, FocusResponse focus);
    Task NotifyWorldUpdatedBatch(string connectionId, IReadOnlyList<CommentedTilePatchResponse> patches);
    Task NotifyWorldUpdatedPiece(string connectionId, CommentedTilePatchResponse patch);
    Task NotifyStageStarted(string connectionId, Stage stage);
    Task NotifyGenerationFailed(string connectionId, string errorMessage);
}
