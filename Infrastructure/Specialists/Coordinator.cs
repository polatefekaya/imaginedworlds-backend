using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Common;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Infrastructure;

public class Coordinator : ICoordinator
{
    private List<CommentedTilePatchResponse> _lastPatches = [];
    public IReadOnlyList<CommentedTilePatchResponse> LastPatches => _lastPatches.AsReadOnly();

    private readonly IExecutor _executor;
    private readonly IFocuser _focuser;
    private readonly ISimulationNotifier _notifier;

    private const int LAST_MOVES_MEMORY = 40;

    public Coordinator(IExecutor executor, IFocuser focuser, ISimulationNotifier simulationNotifier)
    {
        _executor = executor;
        _focuser = focuser;
        _notifier = simulationNotifier;
    }

    public async Task Execute(Agent agent, IReadOnlyList<Stage> stages, GridTerrain gridTerrain, string connectionId, CancellationToken cancellationToken)
    {
        foreach (Stage stage in stages)
        {
            cancellationToken.ThrowIfCancellationRequested();

            int completedSteps = 0;
            //call notifier about stage is starting
            await _notifier.NotifyStageStarted(connectionId, stage);

            while (completedSteps < stage.TargetStepCount)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int leftSteps = stage.TargetStepCount - completedSteps;

                FocusResponse focusResponse = await _focuser.Focus(agent, LastPatches, stages, stage, gridTerrain, cancellationToken);
                //call notifier about focus changed
                await _notifier.NotifyFocusChanged(connectionId, focusResponse);

                byte[,] focusedView = GridHelper.GetFocusedView(gridTerrain.GetBytes(), focusResponse);

                bool patchedEver = _lastPatches is not null && _lastPatches.Count > 0;

                List<CommentedTilePatchResponse> commentedPatches = await _executor.Iterate(agent, leftSteps, stages, stage, focusedView, cancellationToken, patchedEver ? LastPatches : null);

                foreach (var commentedPatch in commentedPatches)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    Coordinates globalCoords = GridHelper.ProjectToGlobal(
                        commentedPatch.X,
                        commentedPatch.Y,
                        focusResponse,
                        gridTerrain.Width,
                        gridTerrain.Height
                    );

                    CommentedTilePatchResponse globalPatch = new(commentedPatch.Comment, commentedPatch.TileType, globalCoords.X, globalCoords.Y);

                    gridTerrain.SetTile(globalPatch.ToTilePatch());
                    AddToLastPatch(globalPatch);
                    //call the notifier about tile placed
                    await _notifier.NotifyWorldUpdatedPiece(connectionId, globalPatch);
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

        if (_lastPatches.Count > LAST_MOVES_MEMORY) _lastPatches.RemoveAt(0);
    }
}
