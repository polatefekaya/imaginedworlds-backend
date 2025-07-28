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
    private readonly ILogger _logger;
    public ImaginationArchitect(Agent agent, IRequestFactory requestFactory, IPromptManager promptManager, HttpClient httpClient, ILogger logger)
    {
        _agent = agent;
        _requestFactory = requestFactory;
        _promptManager = promptManager;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<(ConstructionPlanResponse, Agent)> GetPlanAsync(string prompt, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetPlanAsync is starting with prompt: {prompt}", prompt);
        prompt.CheckNullOrWhitespace();

        cancellationToken.ThrowIfCancellationRequested();

        string systemPrompt = _promptManager.SystemPrompt;
        HttpRequestMessage request = await _requestFactory.Create<ConstructionPlanResponse>(_agent.GetConfiguration(), prompt, systemPrompt);


        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        _logger.LogDebug("Response received for ImaginationArtchitect's GetPlanAsync method, response: {reponse}", await response.Content.ReadAsStringAsync(cancellationToken));

        response.EnsureSuccessStatusCode();

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            ResponseWrapper? responseWrapper = await response.Content.ReadFromJsonAsync<ResponseWrapper>(cancellationToken);
            string? innerJsonText = responseWrapper?.Candidates[0]
                                       .Content.Parts[0]
                                       .Text;

            if (string.IsNullOrWhiteSpace(innerJsonText))
            {
                _logger.LogDebug("ResponseWrapper inner json text is null");
                string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidAiResponseException("AI response was valid but contained no text payload.", null, rawContent);
            }

            ConstructionPlanResponse? planResponse = JsonSerializer.Deserialize<ConstructionPlanResponse>(innerJsonText);

            if (planResponse is null)
            {
                _logger.LogDebug("ConstructionPlanResponse is null");
                string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidAiResponseException("Architect received a null or empty response from AI.", null, rawContent);
            }

            _logger.LogDebug("PlanResponse created:\nOverallPLan: {plan}\nStage Count: {stageCount}", planResponse.OverallPlan, planResponse.Stages.Count);

            return (planResponse, _agent);
        }
        catch (JsonException jsonEx)
        {
            string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidAiResponseException("Architect failed to deserialize AI response.", jsonEx, rawContent);
        }
        catch (NotSupportedException ex)
        {
            string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidAiResponseException("Architect received an unsupported content type from AI.", ex, rawContent);
        }
    }
}
