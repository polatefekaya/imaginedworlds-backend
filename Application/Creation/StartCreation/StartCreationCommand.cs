using Mediator;

namespace ImaginedWorlds.Application.Creation.StartCreation;

public sealed record StartCreationCommand(
    string UserPrompt,
    string AgentCodeName,
    string ConnectionId
) : ICommand<Ulid>;
