using ImaginedWorlds.Domain.Creation.ConstrustionPlan;

namespace ImaginedWorlds.Application.Contracts;

public record ConstructionPlanResponse
(
    string OverallPlan,
    List<Stage> Stages
);
