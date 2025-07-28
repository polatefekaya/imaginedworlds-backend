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
    private readonly ILogger<IFocuser> _logger;

    public Focuser(IPromptManager promptManager, IPromptBuilder promptBuilder, IRequestFactory requestFactory, IHttpClientFactory httpClientFactory, ILogger<IFocuser> logger)
    {
        _promptManager = promptManager;
        _promptBuilder = promptBuilder;
        _requestFactory = requestFactory;
        _logger = logger;

        _httpClient = httpClientFactory.CreateClient("llm-client");
    }

    public async Task<FocusResponse> Focus(Agent agent, IReadOnlyList<CommentedTilePatchResponse> lastPatches, IReadOnlyList<Stage> stages, Stage currentStage, GridTerrain gridTerrain, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        byte[,] gridSummary = TileHelper.CreatePrioritySummary(gridTerrain);
        string overallGoal = _promptManager.OverallGoal;
        string systemPrompt = _promptManager.SystemPrompt;

        string prompt = await _promptBuilder.BuildFocuserPrompt(overallGoal, stages, currentStage, lastPatches, gridSummary);

        HttpRequestMessage message = await _requestFactory.Create<FocusResponse>(agent.GetConfiguration(), prompt, systemPrompt);
        
        HttpResponseMessage response = await _httpClient.SendAsync(message, cancellationToken);
        _logger.LogDebug("Response received for Focuser's Focus method, response: {reponse}", await response.Content.ReadAsStringAsync(cancellationToken));
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

            FocusResponse? focusResponse = JsonSerializer.Deserialize<FocusResponse>(innerJsonText);
            if (focusResponse is null)
            {
                _logger.LogDebug("FocusResponse is null");
                string rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidAiResponseException("Focuser received a null or empty response from AI.", null, rawContent);
            }

            _logger.LogDebug("Focus Response got: {response}", focusResponse.ToString());
            GridHelper.CheckBounds(gridTerrain.Height, gridTerrain.Width, new(focusResponse.X, focusResponse.Y), _logger);
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
