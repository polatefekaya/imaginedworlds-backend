using System;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;

namespace ImaginedWorlds.Infrastructure;

public class PromptBuilder : IPromptBuilder
{
    public Task<string> BuildArchitectPrompt(string userPrompt)
    {
        throw new NotImplementedException();
    }

    public Task<string> BuildExecutorPrompt(string overallGoal, IReadOnlyList<Stage> stages, IReadOnlyList<CommentedTilePatchResponse> lastCommentedPatches, int stepCount, byte[,] focusedPart)
    {
        throw new NotImplementedException();
    }
}
