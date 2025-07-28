using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;

namespace ImaginedWorlds.Infrastructure;

public class ArchitectFactory : IArchitectFactory
{
    private readonly IAgentRepository _agentRepository;
    private readonly IRequestFactory _requestFactory;
    private readonly IPromptManager _promptManager;
    private readonly IHttpClientFactory _httpClientFactory;
    private ILogger<ArchitectFactory> _logger;

    public ArchitectFactory(IAgentRepository agentRepository, IRequestFactory requestFactory, IPromptManager promptManager, IHttpClientFactory httpClientFactory, ILogger<ArchitectFactory> logger)
    {
        _agentRepository = agentRepository;
        _requestFactory = requestFactory;
        _promptManager = promptManager;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    public async Task<IArchitect> Create(string agentCodeName, CancellationToken cancellationToken)
    {
        Agent? agent = await _agentRepository.GetByCodeNameAsync(agentCodeName, cancellationToken)
            ?? throw new InvalidOperationException("The agent returned from database is null, throwing.");

        return new ImaginationArchitect(agent, _requestFactory, _promptManager, _httpClientFactory.CreateClient("llm-client"), _logger);
    }
}
