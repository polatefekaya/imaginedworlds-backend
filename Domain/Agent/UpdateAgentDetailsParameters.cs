namespace ImaginedWorlds.Domain.Agent;

public record struct UpdateAgentDetailsParameters(
    string? DisplayName,
    string? CodeName,
    string? Description,
    ProviderConfiguration? ProviderConfiguration,
    Uri? IconUrl
);