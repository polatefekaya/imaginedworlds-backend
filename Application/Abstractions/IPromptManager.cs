using System;

namespace ImaginedWorlds.Application.Abstractions;

public interface IPromptManager
{
    public string OverallGoal { get; }
    public string SystemPrompt { get; }

    void SetOverallGoal(string overallGoal);
    void SetSystemPrompt(string systemPrompt);
}
