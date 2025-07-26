using System;

namespace ImaginedWorlds.Domain.Common.Primitives;

public interface IIdentifiable<out TId>
{
    TId Id { get; }
}