using System;
using ImaginedWorlds.Validation;

namespace ImaginedWorlds.Domain.Common.Primitives;

public abstract record AggregatedDomainEvent : DomainEvent, IAggregatedDomainEvent
{
    public Ulid AggregateId { get; }

    protected AggregatedDomainEvent(Ulid aggregateId)
        : base()
    {
        aggregateId.CheckEmpty();
        
        AggregateId = aggregateId;
    }
}
