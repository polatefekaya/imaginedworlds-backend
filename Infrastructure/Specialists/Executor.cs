using System;
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

    private record ExecutorAiResponse(List<CommentedTilePatchResponse> Patches);
    
    public Executor(IRequestFactory requestFactory, IPromptBuilder promptBuilder, IPromptManager promptManager, IHttpClientFactory httpClientFactory)
    {
        _requestFactory = requestFactory;
        _promptBuilder = promptBuilder;
        _promptManager = promptManager;

        _httpClient = httpClientFactory.CreateClient("llm-client");
    }

    public async Task<List<CommentedTilePatchResponse>> Iterate(Agent agent, int leftSteps, IReadOnlyList<Stage> stages, Stage currentStage, byte[,] focusedGridView, CancellationToken cancellationToken, IReadOnlyList<CommentedTilePatchResponse>? lastPatches = null)
    {
        string overallGoal = _promptManager.OverallGoal;
        string systemPrompt = _promptManager.SystemPrompt;

        string prompt = await _promptBuilder.BuildExecutorPrompt(overallGoal, leftSteps, stages, currentStage, lastPatches, focusedGridView);

        HttpRequestMessage request = await _requestFactory.Create<List<CommentedTilePatchResponse>>(agent.GetConfiguration(), prompt, systemPrompt);

        HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        try {
            ExecutorAiResponse? aiResponse = await response.Content.ReadFromJsonAsync<ExecutorAiResponse>(cancellationToken);
            if (aiResponse?.Patches is null)
            {
                return [];
            }

            return aiResponse.Patches;
        }
        catch (JsonException jsonEx) {
            string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidAiResponseException("Executor failed to deserialize AI response.", jsonEx, rawContent);
        }
        catch (NotSupportedException ex) {
            string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidAiResponseException("Executor received an unsupported content type from AI.", ex, rawContent);
        }
    }
}
