using System;

namespace ImaginedWorlds.Domain.Common.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : IEquatable<TId>
{
    protected AggregateRoot(TId id) : base(id)
    {
    }

    protected AggregateRoot() { }
}
