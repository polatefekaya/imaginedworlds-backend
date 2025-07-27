using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Validation;

namespace ImaginedWorlds.Infrastructure;

public class PromptManager : IPromptManager
{
    public string OverallGoal { get; private set; }

    public string SystemPrompt { get; private set; }

    public void SetOverallGoal(string overallGoal)
    {
        overallGoal.CheckNullOrWhitespace();
        OverallGoal = overallGoal;
    }

    public void SetSystemPrompt(string systemPrompt)
    {
        systemPrompt.CheckNullOrWhitespace();
        SystemPrompt = systemPrompt;
    }
}
