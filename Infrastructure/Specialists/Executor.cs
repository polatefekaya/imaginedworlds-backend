using System;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Infrastructure.Specialists;

public class Executor
{
    private readonly int _leftSteps;
    private readonly IReadOnlyList<CommentedTilePatchResponse>? _lastPatches;

    public Executor(int leftSteps, IReadOnlyList<CommentedTilePatchResponse>? lastPatches = null)
    {
        _leftSteps = leftSteps;
        
        if(lastPatches is not null)
            _lastPatches = [.. lastPatches];
    }
    public List<CommentedTilePatchResponse> Iterate()
    {
        int randomSteps = Random.Shared.Next(5, 15);
        if (randomSteps > _leftSteps) randomSteps = _leftSteps;


        //build the request, send the request, get the response, parse the response then return.
    }
}
