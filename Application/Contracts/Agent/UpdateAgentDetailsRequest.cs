using ImaginedWorlds.Domain.Agent;

namespace ImaginedWorlds.Application.Contracts.Agent;

public sealed record UpdateAgentDetailsRequest(
    string? DisplayName,
    string? Description,
    Uri? IconUrl,
    ProviderConfiguration? ProviderConfiguration
);