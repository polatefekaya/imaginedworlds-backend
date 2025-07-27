using ImaginedWorlds.Domain.Agent;
using Mediator;

namespace ImaginedWorlds.Application.AiAgent.UpdateAgentDetails;

public sealed record UpdateAgentDetailsCommand(
    string CodeName,
    UpdateAgentDetailsParameters Parameters
) : ICommand<Ulid>;
