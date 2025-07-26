using System;

namespace ImaginedWorlds.Domain.Agent;

public interface IAgentRepository
{
    Task<Agent?> GetAgentByCodeName(string codeName, CancellationToken cancellationToken);
    Task AddAgent(Agent agent, CancellationToken cancellationToken);
    Task RemoveAgentByCodeName(string codeName, CancellationToken cancellationToken);
}
