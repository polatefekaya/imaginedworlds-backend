using ImaginedWorlds.Domain.Agent;

namespace ImaginedWorlds.Application.Contracts.Agent;

public sealed record AddAgentRequest(
    string DisplayName,
    string CodeName,
    string Description,
    Uri IconUrl,
    ProviderConfiguration ProviderConfiguration
);