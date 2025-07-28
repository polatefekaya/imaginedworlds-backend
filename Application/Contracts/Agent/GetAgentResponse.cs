namespace ImaginedWorlds.Application.Contracts.Agent;

public sealed record GetAgentResponse(
    Ulid Id,
    string DisplayName,
    string CodeName,
    string Description,
    Uri IconUrl
);