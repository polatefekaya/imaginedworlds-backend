using System;
using ImaginedWorlds.Application.Contracts.Agent;
using ImaginedWorlds.Domain.Agent;
using Mediator;

namespace ImaginedWorlds.Application.AiAgent.GetAllAgents;

public sealed class GetAllAgentsQueryHandler : IQueryHandler<GetAllAgentsQuery, IReadOnlyList<GetAgentResponse>>
{
    private readonly IAgentRepository _agentRepository;

    public GetAllAgentsQueryHandler(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    public async ValueTask<IReadOnlyList<GetAgentResponse>> Handle(GetAllAgentsQuery query, CancellationToken cancellationToken)
    {
        // 1. Fetch the raw domain entities from the database using the repository.
        IReadOnlyList<Agent> agents = await _agentRepository.GetAllAsync(cancellationToken);

        // 2. Map the internal domain entities to the public DTOs.
        // This is a crucial step that prevents leaking your domain model.
        // The LINQ 'Select' statement is a highly efficient way to do this.
        IReadOnlyList<GetAgentResponse> agentResponses = agents
            .Select(agent => new GetAgentResponse(
                agent.Id,
                agent.DisplayName,
                agent.CodeName,
                agent.Description,
                agent.IconUrl
            ))
            .ToList();

        return agentResponses;
    }
}