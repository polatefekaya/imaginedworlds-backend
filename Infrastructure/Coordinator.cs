using System;
using ImaginedWorlds.Domain.Creation;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Infrastructure.Specialists;

public class Coordinator
{
    private int _currentStage;
    private int _totalStages;
    
    public void Execute(IReadOnlyList<Stage> stages, GridTerrain gridTerrain)
    {
        
        foreach (Stage stage in stages)
        {

        }
        
    }
}
