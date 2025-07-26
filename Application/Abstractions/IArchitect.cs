using System;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;

namespace ImaginedWorlds.Application.Abstractions;

public interface IArchitect
{
    Task<ConstructionPlan> GetPlanAsync(string userPrompt);
}
