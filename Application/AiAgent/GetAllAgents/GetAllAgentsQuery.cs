using ImaginedWorlds.Application.Contracts.Agent;
using Mediator;

namespace ImaginedWorlds.Application.AiAgent.GetAllAgents;

public sealed record GetAllAgentsQuery : IQuery<IReadOnlyList<GetAgentResponse>>;
