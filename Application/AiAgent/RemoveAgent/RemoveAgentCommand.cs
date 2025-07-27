using Mediator;

namespace ImaginedWorlds.Application.AiAgent.RemoveAgent;

public sealed record RemoveAgentCommand(
    string CodeName
) : ICommand<Ulid>;