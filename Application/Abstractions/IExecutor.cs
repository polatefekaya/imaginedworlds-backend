using System;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;

namespace ImaginedWorlds.Application.Abstractions;

public interface IExecutor
{
    public Task<List<CommentedTilePatchResponse>> Iterate(Agent agent, int leftSteps, IReadOnlyList<Stage> stages, Stage currentStage, byte[,] focusedGridView, CancellationToken cancellationToken, IReadOnlyList<CommentedTilePatchResponse>? lastPatches = null);
}
