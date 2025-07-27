namespace ImaginedWorlds.Application.Contracts.Creation;

public sealed record StartCreationRequest(
    string UserPrompt,
    string AgentCodeName,
    string ConnectionId
);