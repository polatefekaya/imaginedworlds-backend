using System;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;

namespace ImaginedWorlds.Application.Abstractions;

public interface IArchitect
{
    Task<(ConstructionPlanResponse, Agent)> GetPlanAsync(string prompt, CancellationToken cancellationToken);
}
