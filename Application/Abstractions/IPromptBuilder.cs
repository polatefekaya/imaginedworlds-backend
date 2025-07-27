using System;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Application.Abstractions;

public interface IPromptBuilder
{
    Task<string> BuildArchitectPrompt(string userPrompt);
    Task<string> BuildExecutorPrompt(string overallGoal, int leftSteps, IReadOnlyList<Stage> stages, Stage currentStage,IReadOnlyList<CommentedTilePatchResponse>? lastCommentedPatches, byte[,] focusedPart);
    Task<string> BuildFocuserPrompt(string overallGoal, IReadOnlyList<Stage> stages, Stage currentStage, IReadOnlyList<CommentedTilePatchResponse> lastCommentedPatches, byte[,] summarizedGrid);
}
