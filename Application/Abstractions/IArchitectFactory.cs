using System;

namespace ImaginedWorlds.Application.Abstractions;

public interface IArchitectFactory
{
    Task<IArchitect> Create(string agentCodeName, CancellationToken cancellationToken);
}
