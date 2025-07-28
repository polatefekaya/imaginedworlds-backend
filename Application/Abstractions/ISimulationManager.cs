using System;

namespace ImaginedWorlds.Application.Abstractions;

public interface ISimulationManager
{
    CancellationToken Register(string connectionId);
    void Cancel(string connectionId);
    void Unregister(string connectionId);
}
