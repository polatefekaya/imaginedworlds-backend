using System;

namespace ImaginedWorlds.Domain.Common.Primitives;

public interface IAggregatedDomainEvent : IDomainEvent
{
    public Ulid AggregateId { get; }
}
