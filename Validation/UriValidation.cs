using System;

namespace ImaginedWorlds.Validation;

public static class UriValidation
{
    public static void CheckNull(this Uri? url)
    {
        ArgumentNullException.ThrowIfNull(url);
    }
}
