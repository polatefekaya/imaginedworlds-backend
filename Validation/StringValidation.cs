using System;

namespace ImaginedWorlds.Validation;

public static class StringValidation
{
    public static void CheckNullOrWhitespace(this string? value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value);
    }
}