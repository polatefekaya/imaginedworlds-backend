using System;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Creation;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;
using ImaginedWorlds.Infrastructure.Specialists;

namespace ImaginedWorlds.Infrastructure;

public class Coordinator
{
    private Stage _currentStage;
    private int _totalStages;

    private List<CommentedTilePatchResponse> _lastPatches = [];
    public IReadOnlyList<CommentedTilePatchResponse> LastPatches => _lastPatches.AsReadOnly();

    public void Execute(IReadOnlyList<Stage> stages, GridTerrain gridTerrain)
    {
        _totalStages = stages.Count;

        foreach (Stage stage in stages)
        {
            int completedSteps = 0;

            while (completedSteps < stage.TargetStepCount) {
                int leftSteps = stage.TargetStepCount - completedSteps;

                Executor executor = new(leftSteps, LastPatches);
                List<CommentedTilePatchResponse> commentedPatches = executor.Iterate();

                foreach (var commentedPatch in commentedPatches)
                {
                    gridTerrain.SetTile(commentedPatch.ToTilePatch());
                    AddToLastPatch(commentedPatch);
                    completedSteps++;
                }
            }

            stage.SetCompleted();
        }

    }
    

    private void AddToLastPatch(CommentedTilePatchResponse tilePatch)
    {
        ArgumentNullException.ThrowIfNull(tilePatch);

        _lastPatches.Add(tilePatch);

        if (_lastPatches.Count > 20) _lastPatches.RemoveAt(0);
    }
}
