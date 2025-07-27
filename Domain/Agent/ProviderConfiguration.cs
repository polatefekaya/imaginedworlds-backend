namespace ImaginedWorlds.Domain.Agent;

public record ProviderConfiguration
{
    public required Uri BaseUrl { get; init; }
    public required string ModelName { get; init; }
    public required string ApiKeySecretName { get; init; }
}