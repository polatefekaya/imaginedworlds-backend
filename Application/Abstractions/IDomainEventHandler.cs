using System;
using ImaginedWorlds.Domain.Common.Primitives;

namespace ImaginedWorlds.Application.Abstractions;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
}
