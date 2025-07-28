using System;
using ImaginedWorlds.Domain.Agent;
using Microsoft.EntityFrameworkCore;

namespace ImaginedWorlds.Infrastructure.Persistence;

public class AgentRepository : IAgentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AgentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Agent?> GetByCodeNameAsync(string codeName, CancellationToken cancellationToken)
    {
        return await _dbContext.Agents
            .FirstOrDefaultAsync(agent => agent.CodeName == codeName, cancellationToken);
    }

    public async void Add(Agent agent, CancellationToken cancellationToken)
    {
        await _dbContext.Agents.AddAsync(agent, cancellationToken);
        //await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Ulid> RemoveByCodeName(string codeName, CancellationToken cancellationToken)
    {
        Agent? agentToRemove = await _dbContext.Agents
            .FirstOrDefaultAsync(agent => agent.CodeName == codeName, cancellationToken);

        if (agentToRemove is not null)
        {
            _dbContext.Agents.Remove(agentToRemove);
            //await _dbContext.SaveChangesAsync(cancellationToken);
            return agentToRemove.Id;
        }

        return Ulid.Empty;
    }

    public async Task<IReadOnlyList<Agent>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Agents
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
