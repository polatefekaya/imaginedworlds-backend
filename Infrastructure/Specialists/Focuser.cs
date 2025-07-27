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

public class Focuser : IFocuser
{
    private readonly IPromptManager _promptManager;
    private readonly IPromptBuilder _promptBuilder;
    private readonly IRequestFactory _requestFactory;
    private readonly HttpClient _httpClient;
    public Focuser(IPromptManager promptManager, IPromptBuilder promptBuilder, IRequestFactory requestFactory, IHttpClientFactory httpClientFactory)
    {
        _promptManager = promptManager;
        _promptBuilder = promptBuilder;
        _requestFactory = requestFactory;
        _httpClient = httpClientFactory.CreateClient("llm-client");
    }

    public async Task<FocusResponse> Focus(Agent agent, IReadOnlyList<CommentedTilePatchResponse> lastPatches, IReadOnlyList<Stage> stages, Stage currentStage, GridTerrain.Grid2DView gridView, CancellationToken cancellationToken)
    {
        byte[,] gridSummary = TileHelper.CreatePrioritySummary(gridView);
        string overallGoal = _promptManager.OverallGoal;
        string systemPrompt = _promptManager.SystemPrompt;

        string prompt = await _promptBuilder.BuildFocuserPrompt(overallGoal, stages, currentStage, lastPatches, gridSummary);

        HttpRequestMessage message = await _requestFactory.Create<FocusResponse>(agent.GetConfiguration(), prompt, systemPrompt);
        
        HttpResponseMessage response = await _httpClient.SendAsync(message, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        try {
            FocusResponse? focusResponse = await response.Content.ReadFromJsonAsync<FocusResponse>(cancellationToken);

            if (focusResponse is null)
            {
                 string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
                 throw new InvalidAiResponseException("Focuser received a null or empty response from AI.", null, rawContent);
            }

            GridHelper.CheckBounds(gridView.Height, gridView.Width, new(focusResponse.X, focusResponse.Y));
            return focusResponse;
        }
        catch (JsonException jsonEx)
        {
            string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidAiResponseException("Focuser failed to deserialize AI response.", jsonEx, rawContent);
        }
        catch (NotSupportedException ex)
        {
             string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
             throw new InvalidAiResponseException("Focuser received an unsupported content type from AI.", ex, rawContent);
        }
    }
}
