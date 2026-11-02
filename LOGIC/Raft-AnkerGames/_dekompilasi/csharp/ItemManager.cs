using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager
{
	private static List<Item_Base> allAvailableItems;

	public static bool ItemsLoaded = false;

	public static readonly string[] Paths = new string[584]
	{
		"IItems/Blueprints/Blueprint", "IItems/Blueprints/Blueprint_AnchorStationaryAdvanced", "IItems/Blueprints/Blueprint_Antenna", "IItems/Blueprints/Blueprint_BackpackAdvanced", "IItems/Blueprints/Blueprint_BatteryAdvanced", "IItems/Blueprints/Blueprint_BatteryCharger", "IItems/Blueprints/Blueprint_BiofuelExtractor", "IItems/Blueprints/Blueprint_BiofuelExtractorAdvanced", "IItems/Blueprints/Blueprint_Canteen", "IItems/Blueprints/Blueprint_Cropplot_Large_Tier3",
		"IItems/Blueprints/Blueprint_Cropplot_Medium_Tier3", "IItems/Blueprints/Blueprint_Cropplot_Small_Tier3", "IItems/Blueprints/Blueprint_DetailPlank", "IItems/Blueprints/Blueprint_ElectricGrill", "IItems/Blueprints/Blueprint_ElectricPurifier", "IItems/Blueprints/Blueprint_EngineControls", "IItems/Blueprints/Blueprint_Firework", "IItems/Blueprints/Blueprint_Fueltank", "IItems/Blueprints/Blueprint_HeadLight", "IItems/Blueprints/Blueprint_HeadLight_Advanced",
		"IItems/Blueprints/Blueprint_Machete", "IItems/Blueprints/Blueprint_MetalDetector", "IItems/Blueprints/Blueprint_MotorWheel", "IItems/Blueprints/Blueprint_Pipe_Fuel", "IItems/Blueprints/Blueprint_Pipe_Water", "IItems/Blueprints/Blueprint_Reciever", "IItems/Blueprints/Blueprint_Recycler", "IItems/Blueprints/Blueprint_SmelterElectric", "IItems/Blueprints/Blueprint_SteeringWheel", "IItems/Blueprints/Blueprint_Storage_Large",
		"IItems/Blueprints/Blueprint_TitaniumTools", "IItems/Blueprints/Blueprint_WaterTank", "IItems/Blueprints/Blueprint_WindTurbine", "IItems/Blueprints/Blueprint_ZiplineBase", "IItems/Blueprints/Blueprint_ZiplineTool", "IItems/Blueprints/Blueprint_ZiplineToolElectric", "IItems/Buildable items/Block_Floor_Tier3", "IItems/Buildable items/Block_Floor_Triangular_Tier3", "IItems/Buildable items/Block_Floor_Triangular_Tier3_Mirrored", "IItems/Buildable items/Block_Floor_Triangular_Wood",
		"IItems/Buildable items/Block_Floor_Wood", "IItems/Buildable items/Block_FoundationArmor", "IItems/Buildable items/Block_Foundation_Tier3", "IItems/Buildable items/Block_Foundation_Triangular_Tier3", "IItems/Buildable items/Block_Foundation_Triangular_Tier3_Mirrored", "IItems/Buildable items/Block_Foundation_Triangular_Wood", "IItems/Buildable items/Block_Foundation_Wood", "IItems/Buildable items/Block_HalfFloor_Tier3", "IItems/Buildable items/Block_HalfFloor_Triangular_Tier3", "IItems/Buildable items/Block_HalfFloor_Triangular_Tier3_Mirrored",
		"IItems/Buildable items/Block_HalfFloor_Triangular_Wood", "IItems/Buildable items/Block_HalfFloor_Wood", "IItems/Buildable items/Block_HorizontalPillar_Tier3", "IItems/Buildable items/Block_HorizontalPillar_Tier3_Half", "IItems/Buildable items/Block_HorizontalPillar_Wood", "IItems/Buildable items/Block_HorizontalPillar_Wood_Half", "IItems/Buildable items/Block_Ladder", "IItems/Buildable items/Block_Ladder_Half", "IItems/Buildable items/Block_Ladder_Tier3", "IItems/Buildable items/Block_Ladder_Tier3_Half",
		"IItems/Buildable items/Block_Pillar_Tier3", "IItems/Buildable items/Block_Pillar_Tier3_Half", "IItems/Buildable items/Block_Pillar_Wood", "IItems/Buildable items/Block_Pillar_Wood_Half", "IItems/Buildable items/Block_Roof_Thatch_Corner", "IItems/Buildable items/Block_Roof_Thatch_EndCap", "IItems/Buildable items/Block_Roof_Thatch_InvCorner", "IItems/Buildable items/Block_Roof_Thatch_LJunction", "IItems/Buildable items/Block_Roof_Thatch_Pyramid", "IItems/Buildable items/Block_Roof_Thatch_Straight",
		"IItems/Buildable items/Block_Roof_Thatch_StraightV", "IItems/Buildable items/Block_Roof_Thatch_TJunction", "IItems/Buildable items/Block_Roof_Thatch_XJunction", "IItems/Buildable items/Block_Roof_Tier3_Corner", "IItems/Buildable items/Block_Roof_Tier3_EndCap", "IItems/Buildable items/Block_Roof_Tier3_InvCorner", "IItems/Buildable items/Block_Roof_Tier3_LJunction", "IItems/Buildable items/Block_Roof_Tier3_Pyramid", "IItems/Buildable items/Block_Roof_Tier3_Straight", "IItems/Buildable items/Block_Roof_Tier3_StraightV",
		"IItems/Buildable items/Block_Roof_Tier3_TJunction", "IItems/Buildable items/Block_Roof_Tier3_XJunction", "IItems/Buildable items/Block_Roof_Wood_Corner", "IItems/Buildable items/Block_Roof_Wood_EndCap", "IItems/Buildable items/Block_Roof_Wood_InvCorner", "IItems/Buildable items/Block_Roof_Wood_LJunction", "IItems/Buildable items/Block_Roof_Wood_Pyramid", "IItems/Buildable items/Block_Roof_Wood_Straight", "IItems/Buildable items/Block_Roof_Wood_StraightV", "IItems/Buildable items/Block_Roof_Wood_TJunction",
		"IItems/Buildable items/Block_Roof_Wood_XJunction", "IItems/Buildable items/Block_Stair_Tier3", "IItems/Buildable items/Block_Stair_Tier3_Half", "IItems/Buildable items/Block_Stair_Wood", "IItems/Buildable items/Block_Stair_Wood_Half", "IItems/Buildable items/Block_Upgrade_Thatch", "IItems/Buildable items/Block_Upgrade_Tier3", "IItems/Buildable items/Block_Upgrade_Wood", "IItems/Buildable items/Block_Wall_Door_Thatch", "IItems/Buildable items/Block_Wall_Door_Tier3",
		"IItems/Buildable items/Block_Wall_Door_Wood", "IItems/Buildable items/Block_Wall_Fence_Plank", "IItems/Buildable items/Block_Wall_Fence_Rope", "IItems/Buildable items/Block_Wall_Fence_Tier3", "IItems/Buildable items/Block_Wall_Gate_Thatch", "IItems/Buildable items/Block_Wall_Gate_Tier3", "IItems/Buildable items/Block_Wall_Gate_Wood", "IItems/Buildable items/Block_Wall_Slope_Thatch", "IItems/Buildable items/Block_Wall_Slope_Thatch_Inverted", "IItems/Buildable items/Block_Wall_Slope_Tier3",
		"IItems/Buildable items/Block_Wall_Slope_Tier3_Inverted", "IItems/Buildable items/Block_Wall_Slope_Wood", "IItems/Buildable items/Block_Wall_Slope_Wood_Inverted", "IItems/Buildable items/Block_Wall_Thatch", "IItems/Buildable items/Block_Wall_Thatch_Half", "IItems/Buildable items/Block_Wall_Tier3", "IItems/Buildable items/Block_Wall_Tier3_Half", "IItems/Buildable items/Block_Wall_VSlope_Thatch", "IItems/Buildable items/Block_Wall_VSlope_Tier3", "IItems/Buildable items/Block_Wall_VSlope_Wood",
		"IItems/Buildable items/Block_Wall_Window_Thatch", "IItems/Buildable items/Block_Wall_Window_Thatch_Half", "IItems/Buildable items/Block_Wall_Window_Tier3", "IItems/Buildable items/Block_Wall_Window_Tier3_Half", "IItems/Buildable items/Block_Wall_Window_Wood", "IItems/Buildable items/Block_Wall_Window_Wood_Half", "IItems/Buildable items/Block_Wall_Wood", "IItems/Buildable items/Block_Wall_Wood_Half", "IItems/Buildable items/Repair", "IItems/Equipment/Backpack",
		"IItems/Equipment/Backpack_Advanced", "IItems/Equipment/Equipment_LeatherChest", "IItems/Equipment/Equipment_LeatherHelmet", "IItems/Equipment/Equipment_LeatherLegs", "IItems/Equipment/Flipper", "IItems/Equipment/Hat_Captain", "IItems/Equipment/Hat_Chef", "IItems/Equipment/Hat_Construction", "IItems/Equipment/Hat_Dev", "IItems/Equipment/Hat_Diving",
		"IItems/Equipment/Hat_Fishing", "IItems/Equipment/Hat_Glasses_Aviator", "IItems/Equipment/Hat_Glasses_Disguise", "IItems/Equipment/Hat_Mayor", "IItems/Equipment/Hat_Pilot", "IItems/Equipment/Hat_Pirate", "IItems/Equipment/Hat_Sailor", "IItems/Equipment/Hat_Tiki", "IItems/Equipment/HeadLight", "IItems/Equipment/HeadLight_Advanced",
		"IItems/Equipment/OxygenBottle", "IItems/Equipment/ZiplineTool", "IItems/Equipment/ZiplineTool_Electric", "IItems/Food/Banana", "IItems/Food/Berries_Red", "IItems/Food/Bucket_Milk", "IItems/Food/Canteen_Empty", "IItems/Food/Canteen_SaltWater", "IItems/Food/Canteen_Water", "IItems/Food/CaveMushroom",
		"IItems/Food/Chili", "IItems/Food/Claybowl_CoconutChicken", "IItems/Food/Claybowl_Empty", "IItems/Food/Claybowl_HeadBroth", "IItems/Food/Claybowl_HeartyStew", "IItems/Food/Claybowl_Leftover", "IItems/Food/Claybowl_SimpleFishStew", "IItems/Food/Claybowl_VegetableSoup", "IItems/Food/ClayPlate_BBQ", "IItems/Food/ClayPlate_CatfishDeluxe",
		"IItems/Food/ClayPlate_MushroomOmelette", "IItems/Food/ClayPlate_SalmonSalad", "IItems/Food/ClayPlate_SharkDinner", "IItems/Food/ClayPlate_SteakWithJam", "IItems/Food/ClayPlate_Sushi", "IItems/Food/Coconut", "IItems/Food/Cooked_Beet", "IItems/Food/Cooked_Catfish", "IItems/Food/Cooked_Drumstick", "IItems/Food/Cooked_GenericMeat",
		"IItems/Food/Cooked_Herring", "IItems/Food/Cooked_Mackerel", "IItems/Food/Cooked_Pomfret", "IItems/Food/Cooked_Potato", "IItems/Food/Cooked_Salmon", "IItems/Food/Cooked_Shark", "IItems/Food/Cooked_Tilapia", "IItems/Food/DrinkingGlass", "IItems/Food/DrinkingGlass_CoconutBeat", "IItems/Food/DrinkingGlass_Leftover",
		"IItems/Food/DrinkingGlass_Mangonana", "IItems/Food/DrinkingGlass_RedbeetShot", "IItems/Food/DrinkingGlass_RedMelon", "IItems/Food/DrinkingGlass_SilverSmoothie", "IItems/Food/DrinkingGlass_SimpleSmoothie", "IItems/Food/DrinkingGlass_SpicyPineberry", "IItems/Food/DrinkingGlass_StrawberryColada", "IItems/Food/Egg", "IItems/Food/HoneyComb", "IItems/Food/Jar_Honey",
		"IItems/Food/Juniper", "IItems/Food/Mango", "IItems/Food/Pineapple", "IItems/Food/PlasticBottle_Empty", "IItems/Food/PlasticBottle_SaltWater", "IItems/Food/PlasticBottle_Water", "IItems/Food/PlasticCup_Empty", "IItems/Food/PlasticCup_SaltWater", "IItems/Food/PlasticCup_Water", "IItems/Food/Raw_Beet",
		"IItems/Food/Raw_Catfish", "IItems/Food/Raw_Drumstick", "IItems/Food/Raw_GenericMeat", "IItems/Food/Raw_Herring", "IItems/Food/Raw_Mackerel", "IItems/Food/Raw_Pomfret", "IItems/Food/Raw_Potato", "IItems/Food/Raw_Salmon", "IItems/Food/Raw_Shark", "IItems/Food/Raw_Tilapia",
		"IItems/Food/SilverAlgae", "IItems/Food/Strawberry", "IItems/Food/Turmeric", "IItems/Food/Watermelon", "IItems/Materials/Battery", "IItems/Materials/Battery_Advanced", "IItems/Materials/BioFuel", "IItems/Materials/Bolt", "IItems/Materials/Brick_Dry", "IItems/Materials/CircuitBoard",
		"IItems/Materials/Clay", "IItems/Materials/Color_Black", "IItems/Materials/Color_Blue", "IItems/Materials/Color_Red", "IItems/Materials/Color_White", "IItems/Materials/Color_Yellow", "IItems/Materials/CopperIngot", "IItems/Materials/CopperOre", "IItems/Materials/Dirt", "IItems/Materials/ExplosiveGoo",
		"IItems/Materials/ExplosivePowder", "IItems/Materials/Feather", "IItems/Materials/Flower_Black", "IItems/Materials/Flower_Blue", "IItems/Materials/Flower_Red", "IItems/Materials/Flower_White", "IItems/Materials/Flower_Yellow", "IItems/Materials/Glass", "IItems/Materials/Head_AnglerFish", "IItems/Materials/Head_Bear",
		"IItems/Materials/Head_Boar", "IItems/Materials/Head_Hyena", "IItems/Materials/Head_HyenaBoss", "IItems/Materials/Head_MamaBear", "IItems/Materials/Head_PoisonPuffer", "IItems/Materials/Head_PolarBear", "IItems/Materials/Head_Screecher", "IItems/Materials/Head_Screecher_Caravan", "IItems/Materials/Head_Shark", "IItems/Materials/Hinge",
		"IItems/Materials/Jar_Bee", "IItems/Materials/Leather", "IItems/Materials/MetalIngot", "IItems/Materials/MetalOre", "IItems/Materials/Nail", "IItems/Materials/Plank", "IItems/Materials/Plastic", "IItems/Materials/Rope", "IItems/Materials/Sand", "IItems/Materials/Scrap",
		"IItems/Materials/SeaVine", "IItems/Materials/Stone", "IItems/Materials/Thatch", "IItems/Materials/TitaniumIngot", "IItems/Materials/TitaniumOre", "IItems/Materials/TradeToken", "IItems/Materials/Trashcube", "IItems/Materials/VineGoo", "IItems/Materials/Wool", "IItems/Other/Barrel",
		"IItems/Other/Cassette_Classical", "IItems/Other/Cassette_EDM", "IItems/Other/Cassette_Elevator", "IItems/Other/Cassette_Pop", "IItems/Other/Cassette_Rock", "IItems/Other/Cassette_TradingPost", "IItems/Other/DropItem", "IItems/Other/FakeTitaniumTools", "IItems/Other/FishingBait_Advanced", "IItems/Other/FishingBait_Expert",
		"IItems/Other/FishingBait_Simple", "IItems/Other/HealingSalve", "IItems/Other/HealingSalve_Good", "IItems/Other/Mystery_Package", "IItems/Other/TrophyFish_T1_Glow_Shrimp", "IItems/Other/TrophyFish_T1_NetNibbler", "IItems/Other/TrophyFish_T1_SharpCarp", "IItems/Other/TrophyFish_T2_Foamspitter", "IItems/Other/TrophyFish_T2_Pufferlisk", "IItems/Other/TrophyFish_T2_TrapSnapper",
		"IItems/Other/TrophyFish_T3_Lionfish", "IItems/Other/TrophyFish_T3_Lunarfish", "IItems/Other/TrophyFish_T3_Pentapus", "IItems/Other/TrophyFish_T3_PinkPike", "IItems/Other/TrophyFish_T3_RainbowLobster", "IItems/Other/TrophyFish_T3_Sharkeater", "IItems/Placeable items/Placeable", "IItems/Placeable items/Placeable_Anchor_Stationary", "IItems/Placeable items/Placeable_Anchor_Stationary_Advanced", "IItems/Placeable items/Placeable_Anchor_Throwable",
		"IItems/Placeable items/Placeable_Bathtub", "IItems/Placeable items/Placeable_BatteryCharger", "IItems/Placeable items/Placeable_Bed_Basic", "IItems/Placeable items/Placeable_Bed_Hammock", "IItems/Placeable items/Placeable_Bed_Wood", "IItems/Placeable items/Placeable_BeeHive", "IItems/Placeable items/Placeable_BiofuelExtractor", "IItems/Placeable items/Placeable_BiofuelExtractor_Advanced", "IItems/Placeable items/Placeable_BirdsNest", "IItems/Placeable items/Placeable_Bookpile",
		"IItems/Placeable items/Placeable_Bookpile_Standing", "IItems/Placeable items/Placeable_Brick_Wet", "IItems/Placeable items/Placeable_Calendar", "IItems/Placeable items/Placeable_Candelabra", "IItems/Placeable items/Placeable_CandleBottle", "IItems/Placeable items/Placeable_Candlestick", "IItems/Placeable items/Placeable_Chair_Basic", "IItems/Placeable items/Placeable_Chair_Stool", "IItems/Placeable items/Placeable_Chair_Stool_Small", "IItems/Placeable items/Placeable_Chair_Wood",
		"IItems/Placeable items/Placeable_Clock_Basic", "IItems/Placeable items/Placeable_Clock_Fine", "IItems/Placeable items/Placeable_CollectionNet_Advanced", "IItems/Placeable items/Placeable_CollectionNet_Basic", "IItems/Placeable items/Placeable_CookingPot", "IItems/Placeable items/Placeable_CookingStand_Food_Electric", "IItems/Placeable items/Placeable_CookingStand_Food_One", "IItems/Placeable items/Placeable_CookingStand_Food_Two", "IItems/Placeable items/Placeable_CookingStand_Purifier_One", "IItems/Placeable items/Placeable_CookingStand_Purifier_Two",
		"IItems/Placeable items/Placeable_CookingStand_Smelter", "IItems/Placeable items/Placeable_CookingStand_Smelter_Electric", "IItems/Placeable items/Placeable_Cropplot_Grass", "IItems/Placeable items/Placeable_Cropplot_Large", "IItems/Placeable items/Placeable_Cropplot_Large_Tier3", "IItems/Placeable items/Placeable_Cropplot_Medium", "IItems/Placeable items/Placeable_Cropplot_Medium_Tier3", "IItems/Placeable items/Placeable_Cropplot_Shoe", "IItems/Placeable items/Placeable_Cropplot_Small", "IItems/Placeable items/Placeable_Cropplot_Small_Tier3",
		"IItems/Placeable items/Placeable_Cupboard_Large", "IItems/Placeable items/Placeable_Cupboard_Medium", "IItems/Placeable items/Placeable_Cupboard_Small", "IItems/Placeable items/Placeable_Curtain_Horizontal", "IItems/Placeable items/Placeable_Curtain_Vertical", "IItems/Placeable items/Placeable_Cutlery", "IItems/Placeable items/Placeable_DetailPlank", "IItems/Placeable items/Placeable_DrawBridgeLong", "IItems/Placeable items/Placeable_DrawBridgeShort", "IItems/Placeable items/Placeable_Electric_Purifier",
		"IItems/Placeable items/Placeable_EngineControls", "IItems/Placeable items/Placeable_Figurine_Chicken", "IItems/Placeable items/Placeable_Figurine_Goat", "IItems/Placeable items/Placeable_Figurine_Llama", "IItems/Placeable items/Placeable_FirewoodRack", "IItems/Placeable items/Placeable_Firework", "IItems/Placeable items/Placeable_Flag_01", "IItems/Placeable items/Placeable_Flag_02", "IItems/Placeable items/Placeable_Flag_03", "IItems/Placeable items/Placeable_Flag_04",
		"IItems/Placeable items/Placeable_Flag_05", "IItems/Placeable items/Placeable_Flag_06", "IItems/Placeable items/Placeable_Flag_07", "IItems/Placeable items/Placeable_Flower_AloeVera", "IItems/Placeable items/Placeable_Flower_Cactus", "IItems/Placeable items/Placeable_Flower_Hibiscus", "IItems/Placeable items/Placeable_Flower_Monstera", "IItems/Placeable items/Placeable_Flower_Rose", "IItems/Placeable items/Placeable_Flower_Sunflower", "IItems/Placeable items/Placeable_FoundationCounter",
		"IItems/Placeable items/Placeable_Fridge", "IItems/Placeable items/Placeable_FuelTank", "IItems/Placeable items/Placeable_GiantClam", "IItems/Placeable items/Placeable_HonkHorn_1", "IItems/Placeable items/Placeable_HonkHorn_2", "IItems/Placeable items/Placeable_HonkHorn_3", "IItems/Placeable items/Placeable_Juicer", "IItems/Placeable items/Placeable_Lantern_Basic", "IItems/Placeable items/Placeable_Lantern_FireBasket", "IItems/Placeable items/Placeable_Lantern_Fireplace",
		"IItems/Placeable items/Placeable_Lantern_Metal", "IItems/Placeable items/Placeable_LuckyCat", "IItems/Placeable items/Placeable_MajorTom", "IItems/Placeable items/Placeable_MotivationalSign_1", "IItems/Placeable items/Placeable_MotivationalSign_2", "IItems/Placeable items/Placeable_MotivationalSign_3", "IItems/Placeable items/Placeable_MotivationalSign_4", "IItems/Placeable items/Placeable_MotorWheel", "IItems/Placeable items/Placeable_Mug", "IItems/Placeable items/Placeable_Nametag",
		"IItems/Placeable items/Placeable_OpenBook", "IItems/Placeable items/Placeable_Outhouse", "IItems/Placeable items/Placeable_Painting_André", "IItems/Placeable items/Placeable_Painting_Crafted_1", "IItems/Placeable items/Placeable_Painting_Crafted_2", "IItems/Placeable items/Placeable_Painting_Crafted_3", "IItems/Placeable items/Placeable_Painting_Crafted_4", "IItems/Placeable items/Placeable_Painting_Ellen", "IItems/Placeable items/Placeable_Painting_Felicia", "IItems/Placeable items/Placeable_Painting_Gabriel",
		"IItems/Placeable items/Placeable_Painting_Illayda", "IItems/Placeable items/Placeable_Painting_Petter", "IItems/Placeable items/Placeable_Painting_Semih", "IItems/Placeable items/Placeable_Painting_Tangaroa_1", "IItems/Placeable items/Placeable_Painting_Tangaroa_2", "IItems/Placeable items/Placeable_Painting_Tangaroa_3", "IItems/Placeable items/Placeable_Painting_Tangaroa_4", "IItems/Placeable items/Placeable_Painting_Tangaroa_5", "IItems/Placeable items/Placeable_Painting_William", "IItems/Placeable items/Placeable_PaintMill",
		"IItems/Placeable items/Placeable_PaperBundle", "IItems/Placeable items/Placeable_Piano", "IItems/Placeable items/Placeable_Pipe_Fuel", "IItems/Placeable items/Placeable_Pipe_Water", "IItems/Placeable items/Placeable_Quill", "IItems/Placeable items/Placeable_Radio", "IItems/Placeable items/Placeable_Reciever", "IItems/Placeable items/Placeable_Reciever_Antenna", "IItems/Placeable items/Placeable_Recycler", "IItems/Placeable items/Placeable_ResearchTable",
		"IItems/Placeable items/Placeable_RhinoSharkTrophy", "IItems/Placeable items/Placeable_Rug_Door", "IItems/Placeable items/Placeable_Rug_Rectangle", "IItems/Placeable items/Placeable_Rug_Round_Large", "IItems/Placeable items/Placeable_Rug_Round_Small", "IItems/Placeable items/Placeable_Sail", "IItems/Placeable items/Placeable_Scarecrow", "IItems/Placeable items/Placeable_Scarecrow_Advanced", "IItems/Placeable items/Placeable_ScrapMechanic", "IItems/Placeable items/Placeable_SharkTrophy",
		"IItems/Placeable items/Placeable_Shelf_Large", "IItems/Placeable items/Placeable_Shelf_Small_Basic", "IItems/Placeable items/Placeable_Shelf_Small_Fine", "IItems/Placeable items/Placeable_Shelf_Wood", "IItems/Placeable items/Placeable_Sign", "IItems/Placeable items/Placeable_Sofa", "IItems/Placeable items/Placeable_Sofa_2Seat", "IItems/Placeable items/Placeable_Sofa_3Seat", "IItems/Placeable items/Placeable_SpiralStair", "IItems/Placeable items/Placeable_SpiralStair_Half",
		"IItems/Placeable items/Placeable_Sprinkler", "IItems/Placeable items/Placeable_SteeringWheel", "IItems/Placeable items/Placeable_Storage_Large", "IItems/Placeable items/Placeable_Storage_Medium", "IItems/Placeable items/Placeable_Storage_Small", "IItems/Placeable items/Placeable_Streamer", "IItems/Placeable items/Placeable_StringLight_Horizontal", "IItems/Placeable items/Placeable_StringLight_Vertical", "IItems/Placeable items/Placeable_Surfboard", "IItems/Placeable items/Placeable_Table_Basic",
		"IItems/Placeable items/Placeable_Table_Round", "IItems/Placeable items/Placeable_Table_Round_Small", "IItems/Placeable items/Placeable_Table_Sofa_Round", "IItems/Placeable items/Placeable_Table_Sofa_Square", "IItems/Placeable items/Placeable_Table_Square", "IItems/Placeable items/Placeable_Table_Square_Small", "IItems/Placeable items/Placeable_Table_Square_Tall", "IItems/Placeable items/Placeable_TangaroaPlant_1", "IItems/Placeable items/Placeable_TangaroaPlant_2", "IItems/Placeable items/Placeable_TangaroaPlant_3",
		"IItems/Placeable items/Placeable_TangaroaPlant_4", "IItems/Placeable items/Placeable_TangaroaPlant_5", "IItems/Placeable items/Placeable_TangaroaPlant_6", "IItems/Placeable items/Placeable_Temperance_Cutlery", "IItems/Placeable items/Placeable_Temperance_Mug", "IItems/Placeable items/Placeable_Temperance_Painting_1", "IItems/Placeable items/Placeable_Temperance_Painting_2", "IItems/Placeable items/Placeable_Temperance_Painting_3", "IItems/Placeable items/Placeable_Temperance_Plant_1", "IItems/Placeable items/Placeable_Temperance_Plant_2",
		"IItems/Placeable items/Placeable_Temperance_Plant_3", "IItems/Placeable items/Placeable_Temperance_Rug_1", "IItems/Placeable items/Placeable_Temperance_Rug_2", "IItems/Placeable items/Placeable_TicTacToe", "IItems/Placeable items/Placeable_TikiPole_Complete", "IItems/Placeable items/Placeable_TikiPole_Feet", "IItems/Placeable items/Placeable_TikiPole_Mid_1", "IItems/Placeable items/Placeable_TikiPole_Mid_2", "IItems/Placeable items/Placeable_TikiPole_Top", "IItems/Placeable items/Placeable_ToyRobot",
		"IItems/Placeable items/Placeable_ToyRobot_Golden", "IItems/Placeable items/Placeable_Trashcan", "IItems/Placeable items/Placeable_TrophyBoard_Large", "IItems/Placeable items/Placeable_TrophyBoard_Medium", "IItems/Placeable items/Placeable_TrophyBoard_Small", "IItems/Placeable items/Placeable_Utopia_Bottle", "IItems/Placeable items/Placeable_Utopia_DanglyDecoration_1", "IItems/Placeable items/Placeable_Utopia_DanglyDecoration_2", "IItems/Placeable items/Placeable_Utopia_DanglyDecoration_3", "IItems/Placeable items/Placeable_Utopia_DanglyDecoration_4",
		"IItems/Placeable items/Placeable_Utopia_DanglyDecoration_5", "IItems/Placeable items/Placeable_Utopia_DanglyDecoration_6", "IItems/Placeable items/Placeable_Utopia_Herb_1", "IItems/Placeable items/Placeable_Utopia_Herb_2", "IItems/Placeable items/Placeable_Utopia_Herb_3", "IItems/Placeable items/Placeable_Utopia_Herb_4", "IItems/Placeable items/Placeable_Utopia_Herb_5", "IItems/Placeable items/Placeable_Utopia_Herb_6", "IItems/Placeable items/Placeable_Utopia_LightBottleWhite", "IItems/Placeable items/Placeable_Utopia_LightBottleYellow",
		"IItems/Placeable items/Placeable_Utopia_Plant_1", "IItems/Placeable items/Placeable_Utopia_Plant_2", "IItems/Placeable items/Placeable_Utopia_Plant_3", "IItems/Placeable items/Placeable_Wardrobe", "IItems/Placeable items/Placeable_Watertank", "IItems/Placeable items/Placeable_WindTurbine", "IItems/Placeable items/Placeable_WorldGlobe", "IItems/Placeable items/Placeable_ZiplineBase", "IItems/Placeable recipes/Placeable_Recipe", "IItems/Placeable recipes/Placeable_Recipe_BBQ",
		"IItems/Placeable recipes/Placeable_Recipe_CatfishDeluxe", "IItems/Placeable recipes/Placeable_Recipe_CoconutBeat", "IItems/Placeable recipes/Placeable_Recipe_CoconutChicken", "IItems/Placeable recipes/Placeable_Recipe_HeadBroth", "IItems/Placeable recipes/Placeable_Recipe_HeartyStew", "IItems/Placeable recipes/Placeable_Recipe_Mangonana", "IItems/Placeable recipes/Placeable_Recipe_MushroomOmelette", "IItems/Placeable recipes/Placeable_Recipe_RedbeetShot", "IItems/Placeable recipes/Placeable_Recipe_RedMelon", "IItems/Placeable recipes/Placeable_Recipe_SalmonSalad",
		"IItems/Placeable recipes/Placeable_Recipe_SharkDinner", "IItems/Placeable recipes/Placeable_Recipe_SilverSmoothie", "IItems/Placeable recipes/Placeable_Recipe_SimpleFishStew", "IItems/Placeable recipes/Placeable_Recipe_SimpleSmoothie", "IItems/Placeable recipes/Placeable_Recipe_SpicyPineberry", "IItems/Placeable recipes/Placeable_Recipe_SteakWithJam", "IItems/Placeable recipes/Placeable_Recipe_StrawberryColada", "IItems/Placeable recipes/Placeable_Recipe_Sushi", "IItems/Placeable recipes/Placeable_Recipe_VegetableSoup", "IItems/Seeds/Seed_Banana",
		"IItems/Seeds/Seed_Birch", "IItems/Seeds/Seed_Flower_Black", "IItems/Seeds/Seed_Flower_Blue", "IItems/Seeds/Seed_Flower_Red", "IItems/Seeds/Seed_Flower_White", "IItems/Seeds/Seed_Flower_Yellow", "IItems/Seeds/Seed_Grass", "IItems/Seeds/Seed_Mango", "IItems/Seeds/Seed_Palm", "IItems/Seeds/Seed_Pine",
		"IItems/Seeds/Seed_Pineapple", "IItems/Seeds/Seed_Strawberry", "IItems/Seeds/Seed_Watermelon", "IItems/Tools/Arrow_Metal", "IItems/Tools/Arrow_Stone", "IItems/Tools/Arrow_Titanium", "IItems/Tools/Axe", "IItems/Tools/Axe_Stone", "IItems/Tools/Axe_Titanium", "IItems/Tools/Binoculars",
		"IItems/Tools/Bow", "IItems/Tools/Bucket", "IItems/Tools/Compass", "IItems/Tools/DevSpear", "IItems/Tools/FishingRod", "IItems/Tools/FishingRod_Metal", "IItems/Tools/Hammer", "IItems/Tools/Hook_Plastic", "IItems/Tools/Hook_Scrap", "IItems/Tools/Hook_Titanium",
		"IItems/Tools/Machete", "IItems/Tools/MetalDetector", "IItems/Tools/NetCanister", "IItems/Tools/NetGun", "IItems/Tools/Paddle", "IItems/Tools/PaintBrush", "IItems/Tools/SharkBait", "IItems/Tools/Shear", "IItems/Tools/Shovel", "IItems/Tools/Spear_Scrap",
		"IItems/Tools/Spear_Wood", "IItems/Tools/SweepNet", "IItems/Tools/Sword_Titanium", "IItems/Tools/ThrowableAnchor"
	};

	public static void Initialize()
	{
		LoadAllItems();
	}

	public static int UniqueNameToIndex(string uniqueItemName)
	{
		foreach (Item_Base allAvailableItem in allAvailableItems)
		{
			if (allAvailableItem.UniqueName == uniqueItemName)
			{
				return allAvailableItem.UniqueIndex;
			}
		}
		return -1;
	}

	public static string UniqueIndexToName(int uniqueIndex)
	{
		foreach (Item_Base allAvailableItem in allAvailableItems)
		{
			if (allAvailableItem.UniqueIndex == uniqueIndex)
			{
				return allAvailableItem.UniqueName;
			}
		}
		return string.Empty;
	}

	public static Item_Base GetItemByIndex(int uniqueIndex)
	{
		if (uniqueIndex < 0)
		{
			return null;
		}
		if (allAvailableItems == null)
		{
			LoadAllItems();
		}
		return allAvailableItems.FirstOrDefault((Item_Base i) => i.UniqueIndex == uniqueIndex);
	}

	public static Item_Base GetItemByName(string uniqueName)
	{
		if (allAvailableItems == null)
		{
			LoadAllItems();
		}
		if (string.IsNullOrEmpty(uniqueName))
		{
			return null;
		}
		return allAvailableItems.FirstOrDefault((Item_Base i) => i.UniqueName == uniqueName);
	}

	public static Item_Base GetItemByNameContains(string nameContains, string excludeItemsContainingThis = null)
	{
		if (allAvailableItems == null)
		{
			LoadAllItems();
		}
		if (string.IsNullOrEmpty(nameContains))
		{
			return null;
		}
		if (string.IsNullOrEmpty(excludeItemsContainingThis))
		{
			return allAvailableItems.FirstOrDefault((Item_Base i) => i.UniqueName.Contains(nameContains, StringComparison.OrdinalIgnoreCase) && (!i.settings_buildable.HasBuildablePrefabs || i.settings_buildable.Placeable));
		}
		return allAvailableItems.FirstOrDefault((Item_Base i) => i.UniqueName.Contains(nameContains, StringComparison.OrdinalIgnoreCase) && !i.UniqueName.Contains(excludeItemsContainingThis, StringComparison.OrdinalIgnoreCase) && (!i.settings_buildable.HasBuildablePrefabs || i.settings_buildable.Placeable));
	}

	public static List<Item_Base> GetBuildableItems()
	{
		List<Item_Base> list = new List<Item_Base>();
		if (allAvailableItems != null)
		{
			for (int i = 0; i < allAvailableItems.Count; i++)
			{
				if (allAvailableItems[i].settings_buildable.HasBuildablePrefabs)
				{
					list.Add(allAvailableItems[i]);
				}
			}
		}
		return list;
	}

	public static List<Item_Base> GetAllItems()
	{
		LoadAllItems();
		return allAvailableItems;
	}

	public static IEnumerator PreloadAllItems()
	{
		if (!ItemsLoaded)
		{
			allAvailableItems = new List<Item_Base>(Paths.Length);
			string[] paths = Paths;
			foreach (string text in paths)
			{
				allAvailableItems.Add(Resources.Load<Item_Base>(text.Replace('\\', '/')));
				yield return null;
			}
			ItemsLoaded = true;
		}
	}

	private static void LoadAllItems()
	{
		if (!ItemsLoaded)
		{
			allAvailableItems = Resources.LoadAll<Item_Base>("IItems").ToList();
			ItemsLoaded = true;
		}
	}
}
