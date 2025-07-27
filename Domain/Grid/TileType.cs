namespace ImaginedWorlds.Domain.Grid;

public enum TileType
{
    Empty = 0,            // The void, the default state
    Placeholder = 1,      // A temporary marker for the AI during planning

    Grassland = 10,       // Standard grassy plains
    Dirt = 11,            // Bare earth, good for paths or fields
    Rock = 12,            // Bare rock face, unbuildable
    Sand = 13,            // Desert or beach sand
    Snow = 14,            // Snow-covered ground
    Tundra = 15,          // Cold, sparse terrain
    SwampGround = 16,     // Murky, wet earth

    OceanDeep = 20,       // Deep, dark water
    OceanShallow = 21,    // Lighter, coastal water
    River = 22,           // Flowing body of water
    Lake = 23,            // Still body of water
    SwampWater = 24,      // Stagnant, swampy water

    DeciduousForest = 30, // Leafy, seasonal forest
    PineForest = 31,      // Evergreen, coniferous forest
    Jungle = 32,          // Dense, tropical vegetation
    Farmland = 33,        // Cultivated land for crops
    Shrubland = 34,       // Sparse bushes and shrubs
    Mountain = 35,
    RoadPaved = 40,       // Modern asphalt road
    RoadDirt = 41,        // Simple dirt path or unpaved road
    RoadCobblestone = 42, // Historic stone road
    Bridge = 43,          // A bridge structure over water or gaps
    Railway = 44,         // Train tracks
    WallStone = 45,       // City or castle walls
    TunnelEntrance = 46,  // Entrance to a tunnel through a mountain

    BuildingResidentialLow = 50,  // Single-family homes, villas
    BuildingResidentialMid = 51,  // Apartment blocks, townhouses
    BuildingResidentialHigh = 52, // Skyscrapers, high-rise condos

    BuildingCommercialLow = 60,   // Shops, small markets
    BuildingCommercialMid = 61,   // Office buildings, malls
    BuildingCommercialHigh = 62,  // Corporate towers

    BuildingIndustrialFactory = 70, // Factories with smokestacks
    BuildingIndustrialWarehouse = 71, // Large storage buildings
    HarborDocks = 72,           // Docks, piers, and cranes for a port
    Mine = 73,                  // A mine entrance or facility
    PowerPlant = 74,            // Power generation facility

    Ruins = 80,             // Crumbling walls of an ancient structure
    Monument = 81,          // A statue, obelisk, or special landmark
    Castle = 82,            // A central castle or keep structure
    Lighthouse = 83,        // A lighthouse on the coast
    Oasis = 84,             // A small patch of life in a desert

    Fire = 90,              // An overlay to show something is on fire
    NeonGlow = 91,          // For cyberpunk cities, an overlay of neon light
}
