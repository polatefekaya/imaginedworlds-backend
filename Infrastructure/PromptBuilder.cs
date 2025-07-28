using System;
using System.Text;
using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Application.Contracts;
using ImaginedWorlds.Domain.Creation.ConstrustionPlan;
using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Infrastructure;

public class PromptBuilder : IPromptBuilder
{
    string architectPromt = """
        ### INSTRUCTION ###
        Your task is to analyze the user's creative vision provided below and decompose it into a high-level, logical construction plan. The plan must consist of 5 to 10 distinct stages.

        ### RULES ###
        1.  **Logical Order:** The stages must be in a logical construction order (e.g., terraforming must happen before building structures).
        2.  **Relevance:** Only generate stages that are directly relevant to the user's vision. Do not invent concepts not mentioned or implied in the prompt.
        3.  **Step Estimation:** For each stage, provide an `estimated_steps` value between 20 (very simple) and 200 (very complex) to represent its relative complexity. This will control the duration of the stage in the simulation. STRICTLY 20 to 200 STEPS, NOTHING MORE, NOTHING LESS.
        4.  **JSON Output Only:** Your entire response MUST be a single, valid JSON object. Do not include any explanatory text, markdown formatting, or comments before or after the JSON block.

        ### USER'S VISION ###
        {{USER_PROMPT}}

        ### REQUIRED OUTPUT FORMAT ###
        You must structure your response as a valid JSON object matching this C# record:
        public record ConstructionPlanResponse(string OverallPlan, List<Stage> Stages); While each Stage has a Name, Description, TargetedStepCount. EVERY FIELD IS A MUST TO FILL.
    """;

    string executorPrompt = """
        ### CONTEXT ###
        You are currently executing a grand plan to build a world based on the user's overall goal.

        **Overall Goal:** {{OVERALL_GOAL}}

        **Grand Construction Plan (Current stage is highlighted):**
        {{CONSTRUCTION_PLAN}}

        **Current Stage Context:**
        {{STAGE_CONTEXT}}

         **Recent Actions in this Stage:**
        {{LAST_STEPS_SUMMARY}}

        **Detailed Focus Area Grid (10x10):**
        This is the specific area you must work in right now. Coordinates are local (0-9). If the area is empty, that means nothing made to there yet.
        {{FOCUS_AREA_GRID}}

        ### INSTRUCTION ###
        Your task is to analyze all the context provided above. Based on your role, the overall goal, and the current state of the focus area, decide on the next batch of 5 to 15 tactical actions to perform.

        ### RULES ###
        1.  **Use Available Tiles Only:** You MUST only use tools from the list provided below. Do not invent new tools.
        2.  **Local Coordinates:** All `x` and `y` parameters in your actions MUST be local to the 10x10 focus area (i.e., between 0 and 9).
        3.  **Respect Existing State:** Analyze the `FOCUS_AREA_GRID` carefully. Do not place a building on a tile that is already water. Do not place a road where a building already exists unless your goal is to replace it.
        4.  **Incremental Progress:** Your actions should be small, logical, and incremental. Do not try to complete the entire stage in one batch. Build organically.
        5.  **JSON Output Only:** Your entire response MUST be a single, valid JSON object. Do not add any explanatory text, markdown formatting, or comments outside of the JSON block. YOU HAVE TO CREATE A COMMENT FOR EACH TILE PLACEMENT.
        6. **Use Available Tiles' Integer Values** You have to just use available tiles' integer values to prevent any error.

        ### AVAILABLE TILES ###
        {{AVAILABLE_TILES}}
    """;

    string focuserPrompt = """
        ### INSTRUCTION ###
        You are The Director, a strategic AI that analyzes the entire world from a high level. Your task is to examine the low-resolution `WORLD_SUMMARY` and the `GRAND_PLAN`. Based on this information, you must decide which 10x10 region of the world requires the most urgent attention to advance the current stage of the plan.

        ### RULES ###
        1.  **Analyze the Big Picture:** Use the `WORLD_SUMMARY` to identify key features like coastlines (where `Water` meets `Sand`), urban centers (areas with `Roads` and `Buildings`), and undeveloped areas (`Grassland`, `Dirt`).
        2.  **Follow the Plan:** Your decision MUST be guided by the currently `**(IN PROGRESS)**` stage in the `GRAND_PLAN`. If the stage is "Building Roads," focus on an area that logically connects two important zones. If the stage is "Terraforming," focus on a relevant empty or incomplete area. Do not work on a stage that is `PENDING`.
        3.  **Return Coordinates Only:** Your response MUST be a single, valid JSON object containing the top-left `x` and `y` coordinates of the 10x10 region you have chosen. The coordinates must be in the full 100x100 world space (i.e., `x` and `y` from 0 to 100). I have clipping handler logic on my code, so you can take any area from the table's bounds.

        ### WORLD STATE ###
        **Overall Goal:** {{OVERALL_GOAL}}

        **GRAND PLAN (Current stage is highlighted):**
        {{CONSTRUCTION_PLAN}}

        **World Summary (Low-Resolution 20x20 Priority-Based Grid):**
        {{GRID_SUMMARY}}
    """;

    public Task<string> BuildArchitectPrompt(string userPrompt)
    {
        // No {{SYSTEM_PROMPT}} in this one as it's a high-level task.
        string builtPrompt = architectPromt
            .Replace("{{USER_PROMPT}}", userPrompt);

        return Task.FromResult(builtPrompt);
    }

    public Task<string> BuildFocuserPrompt(string overallGoal, IReadOnlyList<Stage> stages, Stage currentStage, IReadOnlyList<CommentedTilePatchResponse>? lastCommentedPatches, byte[,] summarizedGrid)
    {
        var sb = new StringBuilder(focuserPrompt);
        
        sb.Replace("{{OVERALL_GOAL}}", overallGoal);
        sb.Replace("{{CONSTRUCTION_PLAN}}", FormatStages(stages, currentStage));
        sb.Replace("{{GRID_SUMMARY}}", SerializeGrid(summarizedGrid));
        
        return Task.FromResult(sb.ToString());
    }
    
    public Task<string> BuildExecutorPrompt(string overallGoal, int leftSteps, IReadOnlyList<Stage> stages, Stage currentStage, IReadOnlyList<CommentedTilePatchResponse>? lastCommentedPatches, byte[,] focusedPart)
    {
        var sb = new StringBuilder(executorPrompt);
        
        sb.Replace("{{OVERALL_GOAL}}", overallGoal);
        sb.Replace("{{CONSTRUCTION_PLAN}}", FormatStages(stages, currentStage));
        sb.Replace("{{LAST_STEPS_SUMMARY}}", FormatLastPatches(lastCommentedPatches));
        sb.Replace("{{FOCUS_AREA_GRID}}", SerializeGrid(focusedPart));
        sb.Replace("{{AVAILABLE_TILES}}", FormatAvailableTiles());

        return Task.FromResult(sb.ToString());
    }

    private string FormatStages(IReadOnlyList<Stage> stages, Stage currentStage)
    {
        var sb = new StringBuilder();
        int currentStageIndex = stages.ToList().IndexOf(currentStage);

        for (int i = 0; i < stages.Count; i++)
        {
            string status;
            if (i < currentStageIndex) status = "(COMPLETED)";
            else if (i == currentStageIndex) status = "**(IN PROGRESS)**";
            else status = "(PENDING)";
            
            sb.AppendLine($"{i + 1}. {stages[i].Name} {status}");
        }
        return sb.ToString();
    }

    private string SerializeGrid(byte[,] grid)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        var sb = new StringBuilder();
        sb.Append('[');
        for (int y = 0; y < height; y++)
        {
            sb.Append('[');
            for (int x = 0; x < width; x++)
            {
                sb.Append(grid[y, x]);
                if (x < width - 1) sb.Append(',');
            }
            sb.Append(']');
            if (y < height - 1) sb.Append(',');
        }
        sb.Append(']');
        return sb.ToString();
    }
    
    private string FormatAvailableTiles()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Your primary tool is `place_tile(x, y, type)`. The available 'type' values are:");
        foreach (TileType tileType in Enum.GetValues(typeof(TileType)))
        {
            sb.AppendLine($"- {tileType} = {(int)tileType}");
        }
        return sb.ToString();
    }

    private string FormatLastPatches(IReadOnlyList<CommentedTilePatchResponse>? patches)
    {
        if (patches is null || patches.Count == 0)
        {
            return "No previous actions in this focus area yet. You are starting fresh.";
        }

        var sb = new StringBuilder();
        sb.AppendLine("Here are the last few actions that were just taken:");
        foreach (var patch in patches)
        {
            sb.AppendLine($"- Placed '{patch.TileType}' ({(int)patch.TileType}) at ({patch.X},{patch.Y}). Reason: \"{patch.Comment}\"");
        }
        return sb.ToString();
    }
}
