using System;
using ImaginedWorlds.Domain.Agent;

namespace ImaginedWorlds.Application.Abstractions;

public interface IRequestFactory
{
    Task<HttpRequestMessage> Create(ProviderConfiguration configuration, string userPrompt, string systemPrompt, string outputJson);
}
