using System;
using ImaginedWorlds.Domain.Common.Primitives;

namespace ImaginedWorlds.Domain.Creation.ConstrustionPlan;

public class ConstructionPlan : Entity<Ulid>
{
    private List<Stage> _stages = [];
    public IReadOnlyList<Stage> Stages => _stages.AsReadOnly();

    private ConstructionPlan(Ulid Id, List<Stage> stages) : base(Id)
    {
        ArgumentNullException.ThrowIfNull(stages);
        _stages = [.. stages];
    }

    public static ConstructionPlan Create(List<Stage> stages)
    {
        return new(Ulid.NewUlid(), stages);
    }

    public void AddStage(Stage stage)
    {
        ArgumentNullException.ThrowIfNull(stage);
        if (_stages.Count > 100) throw new InvalidOperationException("Stage count can not be higher than 100");

        _stages.Add(stage);
    }

    public void ClearStages()
    {
        _stages.Clear();
    }
}
