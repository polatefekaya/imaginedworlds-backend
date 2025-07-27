using System;

namespace ImaginedWorlds.Application.Abstractions;

public interface ISecretVault
{
    public Task<string> GetSecretAsync(string secretName);
}
