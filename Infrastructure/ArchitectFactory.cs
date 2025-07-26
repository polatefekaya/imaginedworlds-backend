using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;

namespace ImaginedWorlds.Infrastructure;

public class ArchitectFactory : IArchitectFactory
{
    private readonly IAgentRepository _agentRepository;
    private readonly IRequestFactory _requestFactory;

    public ArchitectFactory(IAgentRepository agentRepository, IRequestFactory requestFactory)
    {
        _agentRepository = agentRepository;
        _requestFactory = requestFactory;
    }
    public async Task<IArchitect> Create(string agentCodeName, CancellationToken cancellationToken)
    {
        Agent? agent = await _agentRepository.GetAgentByCodeName(agentCodeName, cancellationToken)
            ?? throw new InvalidOperationException("The agent returned from database is null, throwing.");

        return new ImaginationArchitect(agent, _requestFactory);
    }
}
