using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;
using Mediator;

namespace ImaginedWorlds.Application.AiAgent.AddAgent;

public class AddAgentCommandHandler : ICommandHandler<AddAgentCommand, Ulid>
{
    private readonly IAgentRepository _agentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddAgentCommandHandler(IAgentRepository agentRepository, IUnitOfWork unitOfWork)
    {
        _agentRepository = agentRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Ulid> Handle(AddAgentCommand command, CancellationToken cancellationToken)
    {
        Agent newAgent = Agent.Create(
            command.DisplayName,
            command.CodeName,
            command.Description,
            command.ProviderConfiguration,
            command.IconUrl
        );

        _agentRepository.Add(newAgent, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newAgent.Id;
    }
}
