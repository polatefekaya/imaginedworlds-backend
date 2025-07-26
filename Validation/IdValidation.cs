using System;

namespace ImaginedWorlds.Validation;


public static class IdValidation
{
    public static void CheckUlid(Ulid ulidId)
    {
        if (ulidId == Ulid.Empty)
        {
            throw new ArgumentException("ID cannot be an empty ULID.", nameof(ulidId));
        }
    }
    
    /// <summary>
    /// Be cautious while using this, it throws an ArgumentException if the target Ulid is empty!
    /// </summary>
    /// <param name="ulid">The Ulid id to check</param>
    /// <returns>If it is not empty, it returns its value</returns>
    /// <exception cref="ArgumentException"></exception>
    public static Ulid CheckEmpty(this Ulid ulid)
    {
        if (ulid == Ulid.Empty)
        {
            throw new ArgumentException("ID cannot be an empty ULID.", nameof(ulid));
        }
        return ulid;
    }

    /// <summary>
    /// Be cautious while using this, it throws an ArgumentException if the target Ulid is empty!
    /// </summary>
    /// <param name="ulid">The Ulid id to check</param>
    /// <returns>If it is not empty, it returns its value</returns>
    /// <exception cref="ArgumentException"></exception>
    public static Ulid? CheckEmpty(this Ulid? ulid)
    {
        if (ulid is null) return null;

        if (ulid == Ulid.Empty)
        {
            throw new ArgumentException("ID cannot be an empty ULID.", nameof(ulid));
        }
        return ulid;
    }

}
