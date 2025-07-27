using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;
using Mediator;

namespace ImaginedWorlds.Application.AiAgent.RemoveAgent;

public sealed class RemoveAgentCommandHandler : ICommandHandler<RemoveAgentCommand, Ulid>
{
    private readonly IAgentRepository _agentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveAgentCommandHandler(IAgentRepository agentRepository, IUnitOfWork unitOfWork)
    {
        _agentRepository = agentRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Ulid> Handle(RemoveAgentCommand command, CancellationToken cancellationToken)
    {
        Ulid id = await _agentRepository.RemoveByCodeName(command.CodeName, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}
