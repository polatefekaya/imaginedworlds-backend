using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;

namespace ImaginedWorlds.Infrastructure;

public class ImaginationArchitect : IArchitect
{
    private readonly Agent _agent;
    private readonly IRequestFactory _requestFactory;
    public ImaginationArchitect(Agent agent, IRequestFactory requestFactory)
    {
        _agent = agent;
        _requestFactory = requestFactory;
    }

    public async Task<ConstructionPlan> GetPlanAsync(string userPrompt)
    {
        HttpRequestMessage request = await _requestFactory.Create(_agent.GetConfiguration(), userPrompt);
        
    }
}
