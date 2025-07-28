using System;
using System.Collections.Concurrent;
using ImaginedWorlds.Application.Abstractions;

namespace ImaginedWorlds.Infrastructure;

public class SimulationManager : ISimulationManager
{
    private readonly ConcurrentDictionary<string, CancellationTokenSource> _simulations = new();

    public CancellationToken Register(string connectionId)
    {
        var cts = new CancellationTokenSource();
        if (_simulations.TryGetValue(connectionId, out var oldCts))
        {
            oldCts.Cancel();
            oldCts.Dispose();
        }
        _simulations[connectionId] = cts;
        return cts.Token;
    }

    public void Cancel(string connectionId)
    {
        if (_simulations.TryGetValue(connectionId, out var cts))
        {
            cts.Cancel();
        }
    }

    public void Unregister(string connectionId)
    {
        if (_simulations.TryRemove(connectionId, out var cts))
        {
            cts.Dispose();
        }
    }
}
