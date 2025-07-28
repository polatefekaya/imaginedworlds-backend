using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;
using ImaginedWorlds.Infrastructure.Exceptions;

namespace ImaginedWorlds.Infrastructure.Specialists;

public class Executor : IExecutor
{
    private readonly IRequestFactory _requestFactory;
    private readonly IPromptBuilder _promptBuilder;
    private readonly IPromptManager _promptManager;
    private readonly HttpClient _httpClient;
    private readonly ILogger<IExecutor> _logger;

    private record ExecutorAiResponse(List<CommentedTilePatchResponse> Patches);
    
    public Executor(IRequestFactory requestFactory, IPromptBuilder promptBuilder, IPromptManager promptManager, IHttpClientFactory httpClientFactory, ILogger<IExecutor> logger)
    {
        _requestFactory = requestFactory;
        _promptBuilder = promptBuilder;
        _promptManager = promptManager;
        _logger = logger;

        _httpClient = httpClientFactory.CreateClient("llm-client");
    }

    public async Task<List<CommentedTilePatchResponse>> Iterate(Agent agent, int leftSteps, IReadOnlyList<Stage> stages, Stage currentStage, byte[,] focusedGridView, CancellationToken cancellationToken, IReadOnlyList<CommentedTilePatchResponse>? lastPatches = null)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogDebug("Iterate on Executor is started working with agentName: {agentName}, leftSteps: {leftSteps}, currentStage: {currStage}", agent.DisplayName, leftSteps, currentStage.ToString());

        string overallGoal = _promptManager.OverallGoal;
        string systemPrompt = _promptManager.SystemPrompt;

        string prompt = await _promptBuilder.BuildExecutorPrompt(overallGoal, leftSteps, stages, currentStage, lastPatches, focusedGridView);

        HttpRequestMessage request = await _requestFactory.Create<ExecutorAiResponse>(agent.GetConfiguration(), prompt, systemPrompt);

        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        _logger.LogDebug("Response received for Executor's Iterate method, response: {reponse}", await response.Content.ReadAsStringAsync(cancellationToken));
        response.EnsureSuccessStatusCode();

        try {
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

            ExecutorAiResponse? aiResponse = JsonSerializer.Deserialize<ExecutorAiResponse>(innerJsonText);
            if (aiResponse is null)
            {
                _logger.LogDebug("ExecutorAiResponse is null");
                string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidAiResponseException("Executor received a null or empty response from AI.", null, rawContent);
            }

            StringBuilder patchesSb = new();
            foreach (var patch in aiResponse.Patches)
            {
                patchesSb.Append(patch.ToString());
            }
            _logger.LogDebug("ExecutorAiResponse created:\nUpcoming tile patches:\n{patches}", patchesSb.ToString());

            return aiResponse.Patches;
        }
        catch (JsonException jsonEx) {
            string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError(jsonEx, "Executor failed to deserialize AI response: {response}", rawContent);
            throw new InvalidAiResponseException("Executor failed to deserialize AI response.", jsonEx, rawContent);
        }
        catch (NotSupportedException ex) {
            string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidAiResponseException("Executor received an unsupported content type from AI.", ex, rawContent);
        }
    }
}
