using System;

namespace ImaginedWorlds.Domain.Common.Primitives;

public abstract record DomainEvent : IDomainEvent
{
    public Ulid EventId { get; }
    public DateTimeOffset OccurredOn { get; }

    protected DomainEvent()
    {
        this.EventId = Ulid.NewUlid();
        this.OccurredOn = DateTimeOffset.UtcNow;
    }
}
