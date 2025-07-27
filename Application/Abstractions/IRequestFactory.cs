using System;
using ImaginedWorlds.Domain.Agent;

namespace ImaginedWorlds.Application.Abstractions;

public interface IRequestFactory
{
    Task<HttpRequestMessage> Create<TResponse>(
        ProviderConfiguration configuration,
        string promptText,
        string systemPrompt) where TResponse : class;
}
