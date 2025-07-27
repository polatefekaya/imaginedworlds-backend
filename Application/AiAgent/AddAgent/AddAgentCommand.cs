using ImaginedWorlds.Domain.Agent;
using Mediator;

namespace ImaginedWorlds.Application.AiAgent.AddAgent;

public sealed record AddAgentCommand(
    string DisplayName,
    string CodeName,
    string Description,
    Uri IconUrl,
    ProviderConfiguration ProviderConfiguration
) : ICommand<Ulid>;