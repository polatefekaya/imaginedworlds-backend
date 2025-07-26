using System;

namespace ImaginedWorlds.Domain.Common.Primitives;

public interface IEntity
{
    IReadOnlyList<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
