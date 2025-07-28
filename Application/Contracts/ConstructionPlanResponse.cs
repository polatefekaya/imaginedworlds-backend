using System.Text;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;

namespace ImaginedWorlds.Application.Contracts;

public record ConstructionPlanResponse
(
    string OverallPlan,
    List<Stage> Stages
)
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"OverallPlan: {OverallPlan}");

        foreach (var stage in Stages)
        {
            stringBuilder.Append(stage.ToString());
        }

        return stringBuilder.ToString();
    }
};
