using System;

namespace ImaginedWorlds.Domain.Common.Primitives;

public interface IDomainEvent
{
    Ulid EventId { get; }
    DateTimeOffset OccurredOn { get; }
}
