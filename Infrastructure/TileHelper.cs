using System;
using ImaginedWorlds.Domain.Grid;

namespace ImaginedWorlds.Infrastructure;

public static class TileHelper
{
    public static readonly IReadOnlyDictionary<TileType, int> TilePriorities = new Dictionary<TileType, int>
    {
        // Priority 0: Critical Overlays & Effects (Always on top)
        { TileType.Fire, 0 },
        { TileType.Flood, 0 },
        { TileType.Contamination, 0 },
        { TileType.ScorchMark, 0 },

        // Priority 1: Aesthetic & Magical Overlays
        { TileType.NeonGlow, 1 },
        { TileType.Mist, 1 },
        { TileType.Blessed, 1 },
        { TileType.Cursed, 1 },

        // Priority 2: Unique Landmarks & Interactive Sci-Fi/Fantasy
        { TileType.MagicPortal, 2 },
        { TileType.TeleporterPad, 2 },
        { TileType.Monument, 2 },
        { TileType.Monolith, 2 },
        { TileType.CrashedSpaceship, 2 },
        { TileType.ForceFieldGenerator, 2 },

        // Priority 3: Major Unique Structures & Buildings
        { TileType.Castle, 3 },
        { TileType.Pyramid, 3 },
        { TileType.Lighthouse, 3 },
        { TileType.BuildingSkyscraper, 3 },
        { TileType.HydroelectricDam, 3 },
        { TileType.DomeCityFragment, 3 },

        // Priority 4: High-Density & Important Buildings
        { TileType.BuildingResidentialHigh, 4 },
        { TileType.BuildingCommercialHigh, 4 },
        { TileType.PowerPlant, 4 },
        { TileType.BuildingFortress, 4 },
        { TileType.BuildingStadium, 4 },
        { TileType.BuildingHospital, 4 },
        { TileType.BuildingTownHall, 4 },

        // Priority 5: Major Infrastructure
        { TileType.AirportRunway, 5 },
        { TileType.Highway, 5 },
        { TileType.Overpass, 5 },
        { TileType.Bridge, 5 },
        { TileType.Railway, 5 },
        { TileType.TunnelEntrance, 5 },
        { TileType.SubwayEntrance, 5 },

        // Priority 6: Mid-Density Buildings & Fortifications
        { TileType.BuildingResidentialMid, 6 },
        { TileType.BuildingCommercialMid, 6 },
        { TileType.BuildingIndustrialFactory, 6 },
        { TileType.BuildingBank, 6 },
        { TileType.BuildingSchool, 6 },
        { TileType.BuildingTheater, 6 },
        { TileType.WallStone, 6 },
        { TileType.AlienStructure, 6 },

        // Priority 7: Low-Density Buildings & Minor Infrastructure
        { TileType.BuildingResidentialLow, 7 },
        { TileType.BuildingCommercialLow, 7 },
        { TileType.BuildingShack, 7 },
        { TileType.BuildingInn, 7 },
        { TileType.BuildingManor, 7 },
        { TileType.BuildingTenement, 7 },
        { TileType.HarborDocks, 7 },
        { TileType.RoadPaved, 7 },
        { TileType.RoadCobblestone, 7 },

        // Priority 8: Common Buildings, Utilities & Minor Infrastructure
        { TileType.BuildingIndustrialWarehouse, 8 },
        { TileType.BuildingBarracks, 8 },
        { TileType.BuildingWatchtower, 8 },
        { TileType.BuildingMarketStall, 8 },
        { TileType.WindTurbine, 8 },
        { TileType.SolarFarm, 8 },
        { TileType.LumberMill, 8 },
        { TileType.RecyclingPlant, 8 },
        { TileType.RoadDirt, 8 },
        { TileType.Fence, 8 },
        { TileType.Heliport, 8 },

        // Priority 9: Special Terrain Features, Ruins & Large Utilities
        { TileType.VolcanoActive, 9 },
        { TileType.Geyser, 9 },
        { TileType.CrystalSpires, 9 },
        { TileType.FloatingIsland, 9 },
        { TileType.Ruins, 9 },
        { TileType.AncientBattlefield, 9 },
        { TileType.Graveyard, 9 },
        { TileType.Shipwreck, 9 },
        { TileType.OilDerrick, 9 },
        { TileType.Pipeline, 9 },
        { TileType.PowerLines, 9 },

        // Priority 10: Major Geological Features
        { TileType.Mountain, 10 },
        { TileType.MountainPeakSnow, 10 },
        { TileType.VolcanoDormant, 10 },
        { TileType.Canyon, 10 },
        { TileType.Mesa, 10 },
        { TileType.Plateau, 10 },
        { TileType.CaveEntrance, 10 },

        // Priority 11: Dense Vegetation & Biomes
        { TileType.Jungle, 11 },
        { TileType.PineForest, 11 },
        { TileType.DeciduousForest, 11 },
        { TileType.EnchantedForest, 11 },
        { TileType.PetrifiedForest, 11 },
        { TileType.MangroveForest, 11 },
        { TileType.MushroomGrove, 11 },
        { TileType.BambooForest, 11 },
        { TileType.Savannah, 11 },
        { TileType.FlowerField, 11 },

        // Priority 12: Water Bodies
        { TileType.Waterfall, 12 },
        { TileType.Rapids, 12 },
        { TileType.River, 12 },
        { TileType.LavaRiver, 12 },
        { TileType.UndergroundRiver, 12 },
        { TileType.Lake, 12 },
        { TileType.LavaLake, 12 },
        { TileType.AcidPool, 12 },
        { TileType.SwampWater, 12 },
        { TileType.FrozenRiver, 12 },
        { TileType.FrozenLake, 12 },

        // Priority 13: Ground-Level Features & Sparse Vegetation
        { TileType.Oasis, 13 },
        { TileType.Farmland, 13 },
        { TileType.Orchard, 13 },
        { TileType.Vineyard, 13 },
        { TileType.Shrubland, 13 },
        { TileType.Hills, 13 },
        { TileType.Mine, 13 },
        { TileType.Quarry, 13 },

        // Priority 14: Coastlines & Special Ground
        { TileType.OceanShallow, 14 },
        { TileType.HotSpring, 14 },
        { TileType.Quicksand, 14 },
        { TileType.TarPit, 14 },
        { TileType.CrystalGround, 14 },
        { TileType.IceSheet, 14 },
        { TileType.Marsh, 14 },

        // Priority 15: Deep Water & Barren Land
        { TileType.OceanDeep, 15 },
        { TileType.SaltFlats, 15 },
        { TileType.ObsidianShardPlains, 15 },
        { TileType.CharredGround, 15 },
        { TileType.Rock, 15 },

        // Priority 16: Base Ground Cover
        { TileType.Grassland, 16 },
        { TileType.Dirt, 16 },
        { TileType.Sand, 16 },
        { TileType.Snow, 16 },
        { TileType.Tundra, 16 },
        { TileType.SwampGround, 16 },
        { TileType.Mud, 16 },
        { TileType.Gravel, 16 },
        { TileType.Clay, 16 },
        { TileType.VolcanicAsh, 16 },
        { TileType.Permafrost, 16 },

        // Priority 17: System & Meta Tiles (Lowest Priority)
        { TileType.Placeholder, 17 },
        { TileType.FogOfWar, 17 },
        { TileType.Empty, 18 },
        { TileType.Boundary, 18 },
        { TileType.Error, 18 },
    };
    
    public static byte[,] CreatePrioritySummary(GridTerrain fullGrid)
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
                        TileType currentTile = (TileType)fullGrid.GridView[y][x];

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
