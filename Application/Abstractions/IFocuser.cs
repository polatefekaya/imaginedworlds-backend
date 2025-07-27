using System;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Application.Abstractions;

public interface IFocuser
{
    public Task<FocusResponse> Focus(Agent agent, IReadOnlyList<CommentedTilePatchResponse> lastPatches, IReadOnlyList<Stage> stages, Stage currentStage, GridTerrain.Grid2DView gridView, CancellationToken cancellationToken);
}
