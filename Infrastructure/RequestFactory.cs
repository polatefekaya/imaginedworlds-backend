using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;
using Json.Schema;
using Json.Schema.Generation;

namespace ImaginedWorlds.Infrastructure;

public class RequestFactory : IRequestFactory
{
    private readonly ISecretVault _secretVault;

    public RequestFactory(ISecretVault secretVault)
    {
        _secretVault = secretVault;
    }
    
    public async Task<HttpRequestMessage> Create<TResponse>(
        ProviderConfiguration configuration,
        string promptText,
        string systemPrompt) where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var payload = new JsonObject();

        if (!string.IsNullOrEmpty(systemPrompt))
        {
            payload["system_instruction"] = new JsonObject
            {
                ["parts"] = new JsonArray(
                    new JsonObject { ["text"] = systemPrompt }
                )
            };
        }

        payload["contents"] = new JsonArray(
            new JsonObject
            {
                ["parts"] = new JsonArray(
                    new JsonObject { ["text"] = promptText }
                )
            }
        );

        if (typeof(TResponse) != typeof(string))
        {
            var schema = new JsonSchemaBuilder().FromType<TResponse>().Build();
            payload["generationConfig"] = new JsonObject
            {
                ["responseMimeType"] = "application/json",
                ["responseSchema"] = JsonSerializer.SerializeToNode(schema)
            };
        }

        var endpointUrl = new Uri($"{configuration.BaseUrl.AbsoluteUri.TrimEnd('/')}/v1beta/models/{configuration.ModelName}:generateContent");

        var request = new HttpRequestMessage(HttpMethod.Post, endpointUrl)
        {
            Content = new StringContent(payload.ToJsonString(), System.Text.Encoding.UTF8, "application/json")
        };

        string apiKey = await _secretVault.GetSecretAsync(configuration.ApiKeySecretName);
        request.Headers.TryAddWithoutValidation("x-goog-api-key", apiKey);

        return request;
    }
}
