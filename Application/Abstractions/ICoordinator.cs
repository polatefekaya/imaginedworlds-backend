using System;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Application.Abstractions;

public interface ICoordinator
{
    public IReadOnlyList<CommentedTilePatchResponse> LastPatches { get; }
    public Task Execute(Agent agent, IReadOnlyList<Stage> stages, GridTerrain gridTerrain, string connectionId, CancellationToken cancellationToken);
}
