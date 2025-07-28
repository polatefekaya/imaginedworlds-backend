namespace ImaginedWorlds.Domain.Grid;

public enum TileType
{
    // --- System & Meta Tiles (0-9) ---
    Empty = 0,                   // The void, the default state, nothingness
    Placeholder = 1,             // A temporary marker for AI during generation or planning
    Boundary = 2,                // An impassable map edge or world border
    FogOfWar = 3,                // Unexplored territory
    Error = 4,                   // A corrupted or invalid tile state

    // --- Basic Terrain & Ground Cover (10-49) ---
    Grassland = 10,              // Standard grassy plains
    Dirt = 11,                   // Bare earth, good for paths or fields
    Rock = 12,                   // Bare rock face, unbuildable
    Sand = 13,                   // Desert or beach sand
    Snow = 14,                   // Snow-covered ground
    Tundra = 15,                 // Cold, sparse, frozen terrain
    SwampGround = 16,            // Murky, wet earth
    Mud = 17,                    // Wet, soft earth, may slow movement
    Gravel = 18,                 // Small, loose stones
    Clay = 19,                   // Red or grey clay deposit
    CharredGround = 20,          // Earth scorched by fire
    SaltFlats = 21,              // Flat, barren land covered in salt
    CrystalGround = 22,          // Ground infused with crystalline structures
    VolcanicAsh = 23,            // Fine ash from a volcano
    ObsidianShardPlains = 24,    // Ground covered in sharp volcanic glass
    Permafrost = 25,             // Permanently frozen subsoil

    // --- Water & Liquid Bodies (50-99) ---
    OceanDeep = 50,              // Deep, dark, open sea
    OceanShallow = 51,           // Lighter, coastal water, may be walkable
    River = 52,                  // A flowing body of fresh water
    Lake = 53,                   // A still body of fresh water
    SwampWater = 54,             // Stagnant, murky water
    HotSpring = 55,              // Naturally heated water pool
    Waterfall = 56,              // A cascade of water
    Rapids = 57,                 // Fast-moving, turbulent water
    Geyser = 58,                 // A natural hot spring that erupts
    Marsh = 59,                  // Wetland with grasses and reeds
    Quicksand = 60,              // A treacherous patch of liquified sand
    TarPit = 61,                 // A pit of natural asphalt
    LavaRiver = 62,              // A flowing river of molten rock
    LavaLake = 63,               // A lake of molten rock
    AcidPool = 64,               // A pool of corrosive acid
    FrozenRiver = 65,            // A river frozen solid
    FrozenLake = 66,             // A lake frozen solid
    IceSheet = 67,               // A floating sheet of ice on water
    UndergroundRiver = 68,       // A river flowing through a cavern

    // --- Vegetation & Biomes (100-149) ---
    DeciduousForest = 100,       // Leafy, seasonal forest
    PineForest = 101,            // Evergreen, coniferous forest (Taiga)
    Jungle = 102,                // Dense, tropical vegetation
    Farmland = 103,              // Cultivated land for crops
    Shrubland = 104,             // Sparse bushes and shrubs
    Savannah = 105,              // Grassy plains with scattered trees
    Orchard = 106,               // Cultivated fruit trees
    Vineyard = 107,              // Cultivated grape vines
    FlowerField = 108,           // A field of wildflowers
    BambooForest = 109,          // A forest of bamboo stalks
    MangroveForest = 110,        // A coastal forest in saline water
    MushroomGrove = 111,         // An area with oversized or glowing mushrooms
    PetrifiedForest = 112,       // A forest where the trees have turned to stone

    // --- Geological Features (150-199) ---
    Mountain = 150,              // A standard mountain, likely impassable
    MountainPeakSnow = 151,      // The snow-capped peak of a mountain
    Hills = 152,                 // Rolling hills, less steep than mountains
    VolcanoDormant = 153,        // A dormant volcano
    VolcanoActive = 154,         // An active, erupting volcano
    CaveEntrance = 155,          // An entrance into an underground system
    Canyon = 156,                // A deep gorge or ravine
    Plateau = 157,               // An area of high, flat ground
    Mesa = 158,                  // An isolated, flat-topped hill with steep sides
    Oasis = 159,                 // A small patch of life in a desert

    // --- Infrastructure & Transportation (200-249) ---
    RoadPaved = 200,             // Modern asphalt or concrete road
    RoadDirt = 201,              // Simple dirt path or unpaved road
    RoadCobblestone = 202,       // Historic stone road
    Bridge = 203,                // A bridge structure (wood, stone, steel)
    Railway = 204,               // Train tracks
    WallStone = 205,             // City, castle, or fortification walls
    TunnelEntrance = 206,        // Entrance to a tunnel through a mountain
    Highway = 207,               // Multi-lane major road
    Overpass = 208,              // A bridge for a road over another road
    Fence = 209,                 // A simple wooden or wire fence
    PowerLines = 210,            // Electrical power lines
    Pipeline = 211,              // An above-ground pipeline for oil or gas
    AirportRunway = 212,         // A runway for aircraft
    Heliport = 213,              // A landing pad for helicopters
    SubwayEntrance = 214,        // An entrance to a subterranean rail system

    // --- Buildings: Residential (250-299) ---
    BuildingResidentialLow = 250,   // Single-family homes, villas
    BuildingResidentialMid = 251,   // Apartment blocks, townhouses
    BuildingResidentialHigh = 252,  // Skyscrapers, high-rise condos
    BuildingShack = 253,            // A poorly constructed hut or shanty
    BuildingManor = 254,            // A large, stately home or mansion
    BuildingPalace = 255,           // A grand royal or state residence
    BuildingTenement = 256,         // A run-down, overcrowded apartment building

    // --- Buildings: Commercial (300-349) ---
    BuildingCommercialLow = 300,      // Shops, small markets, taverns
    BuildingCommercialMid = 301,      // Office buildings, malls, hotels
    BuildingCommercialHigh = 302,     // Corporate towers, financial centers
    BuildingMarketStall = 303,        // An open-air market stall
    BuildingInn = 304,                // An inn for travelers
    BuildingBank = 305,               // A bank or treasury
    BuildingSkyscraper = 306,         // A very tall modern building

    // --- Buildings: Industrial & Utility (350-399) ---
    BuildingIndustrialFactory = 350,  // Factories with smokestacks
    BuildingIndustrialWarehouse = 351,// Large storage buildings
    HarborDocks = 352,                // Docks, piers, and cranes for a port
    Mine = 353,                       // A mine entrance or facility (coal, iron, etc.)
    PowerPlant = 354,                 // Power generation (nuclear, coal, etc.)
    LumberMill = 355,                 // A mill for processing timber
    Quarry = 356,                     // A site for excavating stone or slate
    OilDerrick = 357,                 // An oil rig on land
    HydroelectricDam = 358,           // A dam for generating power
    WindTurbine = 359,                // A modern windmill for power
    SolarFarm = 360,                  // An area covered with solar panels
    RecyclingPlant = 361,             // A facility for processing waste

    // --- Buildings: Civic, Military, & Special (400-449) ---
    BuildingTownHall = 400,           // A civic administrative building
    BuildingHospital = 401,           // A hospital or clinic
    BuildingSchool = 402,             // A school or university
    BuildingLibrary = 403,            // A public library or archive
    BuildingMuseum = 404,             // A museum or gallery
    BuildingWatchtower = 405,         // A military watchtower
    BuildingBarracks = 406,           // Housing for soldiers
    BuildingFortress = 407,           // A military stronghold
    BuildingStadium = 408,            // A sports stadium or colosseum
    BuildingTheater = 409,            // A place for performances

    // --- Landmarks & Ruins (450-499) ---
    Ruins = 450,                      // Crumbling walls of an ancient structure
    Monument = 451,                   // A statue, obelisk, or special landmark
    Castle = 452,                     // A central castle or keep structure
    Lighthouse = 453,                 // A lighthouse on the coast
    Graveyard = 454,                  // A cemetery with tombstones
    Pyramid = 455,                    // An ancient pyramid
    Monolith = 456,                   // A single, large standing stone of significance
    Shipwreck = 457,                  // The wreck of a ship on a coast or underwater
    AncientBattlefield = 458,         // An old battlefield, perhaps with lingering spirits

    // --- Sci-Fi & Fantasy (500-549) ---
    FloatingIsland = 500,             // An island suspended in the air
    MagicPortal = 501,                // A shimmering gateway to another location
    EnchantedForest = 502,            // A forest with magical properties
    CrystalSpires = 503,              // Towering spires made of crystal
    AlienStructure = 504,             // A building of non-human origin
    CrashedSpaceship = 505,           // The wreckage of a spacecraft
    ForceFieldGenerator = 506,        // A device projecting an energy barrier
    TeleporterPad = 507,              // A sci-fi teleportation device
    DomeCityFragment = 508,           // Part of a sealed, domed city

    // --- Overlays & Effects (900+) ---
    Fire = 900,                       // An overlay to show something is on fire
    NeonGlow = 901,                   // For cyberpunk cities, an overlay of neon light
    Flood = 902,                      // An overlay for temporarily flooded tiles
    Mist = 903,                       // An overlay of thick fog or mist
    Contamination = 904,              // A toxic or radioactive contamination effect
    Blessed = 905,                    // A holy or magically blessed area
    Cursed = 906,                     // A magically cursed or blighted area
    ScorchMark = 907,                 // A mark left by an explosion or intense heat
}