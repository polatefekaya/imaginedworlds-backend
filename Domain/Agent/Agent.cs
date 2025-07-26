using System;
using System.Text.Json;
using ImaginedWorlds.Domain.Common.Primitives;
using ImaginedWorlds.Validation;

namespace ImaginedWorlds.Domain.Agent;

public class Agent : AggregateRoot<Ulid>, IAgent
{
    public string DisplayName { get; private set; }
    public string CodeName { get; private set; }
    public string Description { get; private set; }
    public string ProviderConfiguration { get; private set; }
    public Uri IconUrl { get; private set; }

    private Agent(Ulid id, string name, string codeName, string description, string providerConfiguration, Uri iconUrl) : base(id)
    {
        providerConfiguration.CheckNullOrWhitespace();
        iconUrl.CheckNull();
        name.CheckNullOrWhitespace();
        description.CheckNullOrWhitespace();
        codeName.CheckNullOrWhitespace();

        CodeName = codeName;
        ProviderConfiguration = providerConfiguration;
        DisplayName = name;
        Description = description;
        IconUrl = iconUrl;
    }

    public static Agent Create(string name, string codeName, string description, ProviderConfiguration providerConfiguration, Uri iconUrl)
    {
        string configJson = JsonSerializer.Serialize(providerConfiguration, providerConfiguration.GetType());
        configJson.CheckNullOrWhitespace();

        return new(Ulid.NewUlid(), name, codeName, description, configJson, iconUrl);
    }

    private void ChangeDisplayName(string displayName)
    {
        displayName.CheckNullOrWhitespace();
        if (string.Equals(DisplayName, displayName, StringComparison.Ordinal)) return;

        DisplayName = displayName;
    }

    private void ChangeCodeName(string codeName)
    {
        codeName.CheckNullOrWhitespace();
        if (string.Equals(CodeName, codeName, StringComparison.Ordinal)) return;

        CodeName = codeName;
    }

    private void ChangeDescription(string description)
    {
        description.CheckNullOrWhitespace();
        if (string.Equals(Description, description, StringComparison.Ordinal)) return;

        Description = description;
    }

    private void ChangeProviderConfiguration(ProviderConfiguration providerConfiguration)
    {
        ArgumentNullException.ThrowIfNull(providerConfiguration);
        string configJson = JsonSerializer.Serialize(providerConfiguration, providerConfiguration.GetType());
        configJson.CheckNullOrWhitespace();

        if (string.Equals(ProviderConfiguration, configJson, StringComparison.Ordinal)) return;

        ProviderConfiguration = configJson;
    }

    private void ChangeIconUrl(Uri iconUrl)
    {
        iconUrl.CheckNull();
        if (Uri.Equals(IconUrl, iconUrl)) return;

        IconUrl = iconUrl;
    }

    public void UpdateAgentDetails(UpdateAgentDetailsParameters parameters)
    {
        ApplyIfProvided(parameters.DisplayName, ChangeDisplayName);
        ApplyIfProvided(parameters.Description, ChangeDescription);
        ApplyIfProvided(parameters.IconUrl, ChangeIconUrl);
        ApplyIfProvided(parameters.ProviderConfiguration, ChangeProviderConfiguration);
        ApplyIfProvided(parameters.CodeName, ChangeCodeName);
    }

    public ProviderConfiguration GetConfiguration()
    {
        return JsonSerializer.Deserialize<ProviderConfiguration>(ProviderConfiguration)
               ?? throw new InvalidOperationException("Failed to deserialize provider configuration.");
    }

}
