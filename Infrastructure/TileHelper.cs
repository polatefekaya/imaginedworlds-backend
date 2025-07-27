using System;
using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Infrastructure;

public static class TileHelper
{
    private static readonly Dictionary<TileType, int> TilePriorities = new()
    {
        { TileType.Fire, 0 },
        { TileType.NeonGlow, 0 },

        { TileType.Castle, 1 },
        { TileType.Monument, 1 },
        { TileType.Lighthouse, 1 },

        { TileType.Bridge, 2 },
        { TileType.Railway, 2 },
        { TileType.TunnelEntrance, 2 },
        { TileType.RoadPaved, 2 },
        { TileType.RoadCobblestone, 2 },
        { TileType.HarborDocks, 2 },

        { TileType.BuildingResidentialHigh, 3 },
        { TileType.BuildingCommercialHigh, 3 },
        { TileType.PowerPlant, 3 },

        { TileType.BuildingResidentialMid, 4 },
        { TileType.BuildingCommercialMid, 4 },
        { TileType.BuildingIndustrialFactory, 4 },
        { TileType.BuildingResidentialLow, 4 },
        { TileType.BuildingCommercialLow, 4 },
        { TileType.BuildingIndustrialWarehouse, 4 },
        { TileType.WallStone, 4 },
        { TileType.Mine, 4 },
        { TileType.Ruins, 4 },
        { TileType.RoadDirt, 4 },

        { TileType.Oasis, 5 },

        { TileType.River, 6 },
        { TileType.Lake, 6 },
        { TileType.OceanDeep, 6 },
        { TileType.OceanShallow, 6 },
        { TileType.SwampWater, 6 },

        { TileType.Jungle, 7 },
        { TileType.PineForest, 7 },
        { TileType.DeciduousForest, 7 },

        { TileType.Mountain, 8 },
        { TileType.Rock, 8 },
        { TileType.Farmland, 8 },

        { TileType.Sand, 9 },
        { TileType.Snow, 9 },
        { TileType.Tundra, 9 },
        { TileType.SwampGround, 9 },
        { TileType.Shrubland, 9 },
        { TileType.Dirt, 9 },
        { TileType.Grassland, 9 },

        { TileType.Placeholder, 10 },
        { TileType.Empty, 10 }
    };
    
    public static byte[,] CreatePrioritySummary(GridTerrain.Grid2DView fullGrid)
    {
        var summary = new byte[20, 20];

        for (int summaryY = 0; summaryY < 20; summaryY++)
        {
            for (int summaryX = 0; summaryX < 20; summaryX++)
            {
                int startX = summaryX * 5;
                int startY = summaryY * 5;

                TileType highestPriorityTile = TileType.Empty;
                int lowestPriorityValue = int.MaxValue;

                for (int y = startY; y < startY + 5; y++)
                {
                    for (int x = startX; x < startX + 5; x++)
                    {
                        //This line can be wrong
                        TileType currentTile = (TileType)fullGrid[y][x];

                        if (TilePriorities.TryGetValue(currentTile, out int currentPriority))
                        {
                            if (currentPriority < lowestPriorityValue)
                            {
                                lowestPriorityValue = currentPriority;
                                highestPriorityTile = currentTile;
                            }
                        }
                    }
                }
                summary[summaryX, summaryY] = (byte)highestPriorityTile;
            }
        }
        return summary;
    }
}   
