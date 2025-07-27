using System;

namespace ImaginedWorlds.Domain.Agent;

public interface IAgentRepository
{
    Task<Agent?> GetByCodeNameAsync(string codeName, CancellationToken cancellationToken);
    void Add(Agent agent, CancellationToken cancellationToken);
    Task<Ulid> RemoveByCodeName(string codeName, CancellationToken cancellationToken);
}
