namespace ImaginedWorlds.Domain.Agent;

public record StructuredOutputConfig
{
    public required string Strategy { get; init; } // "JsonSchema" or "PromptInjection"
    public string? SchemaWrapperTemplate { get; init; }
    public string? SchemaWrapperKey { get; init; }
    public string? InjectionTemplate { get; init; }
}