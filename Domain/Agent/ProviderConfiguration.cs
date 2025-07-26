namespace ImaginedWorlds.Domain.Agent;

public record ProviderConfiguration
{
    /// <example>"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent"</example>
    public required Uri EndpointUrl { get; init; }

    /// <summary>
    /// The HTTP method to use. Almost always "POST".
    /// </summary>
    public required string HttpMethod { get; init; }

    /// <example>For Gemini: "{\"contents\":[{\"parts\":[{\"text\":\"{{PROMPT}}\"}]}]}"</example>
    public required string RequestBodyTemplate { get; init; }

    /// <example>For Gemini: "candidates[0].content.parts[0].text"</example>
    public required string ResponseBodyPath { get; init; }

    /// <summary>
    /// A dictionary of static headers to include.
    /// </summary>
    /// <example>Content-Type<example/>
    public Dictionary<string, string> StaticHeaders { get; init; } = new();

    /// <summary>
    /// The name of the header used for authentication.
    /// </summary>
    /// <example>Authorization<example/>
    public string? ApiKeyHeaderName { get; init; }

    /// <example>Chrono-Api-Key<example/>
    public required string ApiKeySecretName { get; init; }
}