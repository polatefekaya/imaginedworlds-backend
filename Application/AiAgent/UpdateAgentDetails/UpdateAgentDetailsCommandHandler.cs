using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;
using Mediator;

namespace ImaginedWorlds.Application.AiAgent.UpdateAgentDetails;

public class UpdateAgentDetailsCommandHandler : ICommandHandler<UpdateAgentDetailsCommand, Ulid>
{
    private readonly IAgentRepository _agentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAgentDetailsCommandHandler(IAgentRepository agentRepository, IUnitOfWork unitOfWork)
    {
        _agentRepository = agentRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Ulid> Handle(UpdateAgentDetailsCommand command, CancellationToken cancellationToken)
    {
        var agent = await _agentRepository.GetByCodeNameAsync(command.CodeName, cancellationToken)
            ?? throw new Exception($"Agent with code name '{command.CodeName}' not found.");

        agent.UpdateAgentDetails(command.Parameters);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return agent.Id;
    }
}
