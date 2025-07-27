using System;
using System.Text.Json;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Infrastructure.Exceptions;
using ImaginedWorlds.Validation;

namespace ImaginedWorlds.Infrastructure;

public class ImaginationArchitect : IArchitect
{
    private readonly Agent _agent;
    private readonly IRequestFactory _requestFactory;
    private readonly IPromptManager _promptManager;
    private readonly HttpClient _httpClient;
    public ImaginationArchitect(Agent agent, IRequestFactory requestFactory, IPromptManager promptManager, HttpClient httpClient)
    {
        _agent = agent;
        _requestFactory = requestFactory;
        _promptManager = promptManager;
        _httpClient = httpClient;
    }

    public async Task<(ConstructionPlanResponse, Agent)> GetPlanAsync(string prompt, CancellationToken cancellationToken)
    {
        prompt.CheckNullOrWhitespace();

        string systemPrompt = _promptManager.SystemPrompt;
        HttpRequestMessage request = await _requestFactory.Create<ConstructionPlanResponse>(_agent.GetConfiguration(), prompt, systemPrompt);

        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        try {
            ConstructionPlanResponse? planResponse = await response.Content.ReadFromJsonAsync<ConstructionPlanResponse>(cancellationToken: cancellationToken);

            if (planResponse is null)
            {
                string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidAiResponseException("Architect received a null or empty response from AI.", null, rawContent);
            }

            return (planResponse, _agent);
        }
        catch (JsonException jsonEx) {
            string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidAiResponseException("Architect failed to deserialize AI response.", jsonEx, rawContent);
        }
        catch (NotSupportedException ex) {
            string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidAiResponseException("Architect received an unsupported content type from AI.", ex, rawContent);
        }
    }
}
