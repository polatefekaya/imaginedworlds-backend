using System;
using ImaginedWorlds.Application.Abstractions;

namespace ImaginedWorlds.Infrastructure;

public class ConfigurationSecretVault : ISecretVault
{
    private readonly IConfiguration _configuration;

    public ConfigurationSecretVault(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        string secret = _configuration[$"ApiKey:{secretName}"]
            ?? throw new ArgumentNullException($"No secret found with name: {secretName}");

        return secret;
    }
}
