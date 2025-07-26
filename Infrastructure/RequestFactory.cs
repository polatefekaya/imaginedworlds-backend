using System;
using System.Text.Json;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;

namespace ImaginedWorlds.Infrastructure;

public class RequestFactory : IRequestFactory
{
    public Task<HttpRequestMessage> Create(ProviderConfiguration configuration, string userPrompt, string systemPrompt, string outputJson)
    {
        string requestBody = configuration.RequestBodyTemplate.Replace("{{PROMPT}}", JsonEncodedText.Encode(userPrompt).ToString()).Replace("{{SYSTEM_PROMPT}}", JsonEncodedText.Encode(systemPrompt).ToString()).Replace("{{OUTPUT_JSON}}", JsonEncodedText.Encode(outputJson).ToString());

        var request = new HttpRequestMessage(new HttpMethod(configuration.HttpMethod), configuration.EndpointUrl)
        {
            Content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json")
        };

        foreach (var header in configuration.StaticHeaders)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (!string.IsNullOrEmpty(configuration.ApiKeyHeaderName))
        {
            // Fetch the actual secret from your vault using the name stored in the config.
            string apiKey = await _secretVault.GetSecretAsync(configuration.ApiKeySecretName);
            request.Headers.TryAddWithoutValidation(configuration.ApiKeyHeaderName, apiKey);
        }
    }
}
