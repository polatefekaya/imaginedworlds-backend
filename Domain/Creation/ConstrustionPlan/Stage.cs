using System;
using ImaginedWorlds.Domain.Common.Primitives;
using ImaginedWorlds.Validation;

namespace ImaginedWorlds.Domain.Creation.ConstrustionPlan;

public record Stage
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int TargetStepCount { get; private set; }
    public bool Completed { get; private set; }

    public Stage(string name, string description, int targetStepCount)
    {
        name.CheckNullOrWhitespace();
        description.CheckNullOrWhitespace();

        Name = name;
        Description = description;
        TargetStepCount = targetStepCount;
    }

    public void SetCompleted()
    {
        Completed = true;
    }
}
