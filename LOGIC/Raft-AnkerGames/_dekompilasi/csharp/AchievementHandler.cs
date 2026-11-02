using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public static class AchievementHandler
{
	private static Dictionary<AchievementType, Achievement> Achievements;

	private static int Stat_build_foundationCount;

	private static int Stat_build_paintCount;

	private static int Stat_build_removeCount;

	private static int Stat_player_sharkKills;

	private static int Stat_player_birdKills;

	private static int Stat_player_hookCount;

	private static int Stat_player_deaths;

	private static int Stat_player_stoneBirdKills;

	private static int Stat_player_pufferfishKills;

	private static int Stat_player_boarKills;

	private static int Stat_player_capturedAnimals;

	private static int Stat_player_ratKills;

	private static int Stat_player_bearKills;

	private static int Stat_player_anglerFishKills;

	private static int Stat_player_captures_bee;

	private static int Stat_player_excevations_treasure;

	private static int Stat_player_botKills;

	private static int Stat_player_zipline_distance;

	private static int Stat_player_token_spend_tangaroa;

	private static int Stat_player_instrumentNotes_played;

	private static int Stat_player_fireworks_launched;

	public static int Stat_test_2;

	public static int Stat_test_3;

	public static int Stat_test_4;

	public static int Stat_test_5;

	private static int Stat_player_zipline_distanceOneGo;

	private static bool TempStat_builtSail;

	private static bool TempStat_builtBigAnchor;

	private static bool TempStat_cookedARecipie;

	private static bool TempStat_allMysteryLootUnlocked;

	public static bool UserStatsRecieved;

	private static bool initialized;

	private static bool storeStats;

	private static int TempStat_pipesPlaced;

	private static int TempStat_beeHivesPlaced;

	private static List<int> uniquePaintingIndexes = new List<int>();

	public static bool ShouldTrackAchievements => GameModeValueManager.GetCurrentGameModeValue().achievementVariables.trackAchievments;

	public static void Initialize()
	{
		if (!initialized)
		{
			initialized = true;
			BlockCreator.PlaceBlockCallStackAlways = (Action<Block, Network_Player, bool, int>)Delegate.Combine(BlockCreator.PlaceBlockCallStackAlways, new Action<Block, Network_Player, bool, int>(OnCreateBlock));
			BlockCreator.RemoveBlockCallStackAlways = (Action<List<Block>, Network_Player>)Delegate.Combine(BlockCreator.RemoveBlockCallStackAlways, new Action<List<Block>, Network_Player>(OnRemoveBlock));
			InitializeStatsAndAchievements();
		}
	}

	private static void InitializeStatsAndAchievements()
	{
		SteamUserStats.GetStat(StatType.stat_build_foundationCount.ToString(), out Stat_build_foundationCount);
		SteamUserStats.GetStat(StatType.stat_build_paintCount.ToString(), out Stat_build_paintCount);
		SteamUserStats.GetStat(StatType.stat_build_removeCount.ToString(), out Stat_build_removeCount);
		SteamUserStats.GetStat(StatType.stat_player_birdKills.ToString(), out Stat_player_birdKills);
		SteamUserStats.GetStat(StatType.stat_player_sharkKills.ToString(), out Stat_player_sharkKills);
		SteamUserStats.GetStat(StatType.stat_player_deaths.ToString(), out Stat_player_deaths);
		SteamUserStats.GetStat(StatType.stat_player_hookCount.ToString(), out Stat_player_hookCount);
		SteamUserStats.GetStat(StatType.stat_player_pufferKills.ToString(), out Stat_player_pufferfishKills);
		SteamUserStats.GetStat(StatType.stat_player_stoneBirdKills.ToString(), out Stat_player_stoneBirdKills);
		SteamUserStats.GetStat(StatType.stat_player_boarKills.ToString(), out Stat_player_boarKills);
		SteamUserStats.GetStat(StatType.stat_player_capturedAnimals.ToString(), out Stat_player_capturedAnimals);
		SteamUserStats.GetStat(StatType.stat_player_ratKills.ToString(), out Stat_player_ratKills);
		SteamUserStats.GetStat(StatType.stat_player_bearKills.ToString(), out Stat_player_bearKills);
		SteamUserStats.GetStat(StatType.stat_player_anglerFishKills.ToString(), out Stat_player_anglerFishKills);
		SteamUserStats.GetStat(StatType.stat_player_captures_bee.ToString(), out Stat_player_captures_bee);
		SteamUserStats.GetStat(StatType.stat_player_excevations_treasure.ToString(), out Stat_player_excevations_treasure);
		SteamUserStats.GetStat(StatType.stat_player_botKills.ToString(), out Stat_player_botKills);
		SteamUserStats.GetStat(StatType.stat_player_zipline_distance.ToString(), out Stat_player_zipline_distance);
		SteamUserStats.GetStat(StatType.stat_player_zipline_distanceOneGo.ToString(), out Stat_player_zipline_distanceOneGo);
		SteamUserStats.GetStat(StatType.stat_player_token_spend_tangaroa.ToString(), out Stat_player_token_spend_tangaroa);
		SteamUserStats.GetStat(StatType.stat_player_instrumentNotes_played.ToString(), out Stat_player_instrumentNotes_played);
		SteamUserStats.GetStat(StatType.stat_player_fireworks_launched.ToString(), out Stat_player_fireworks_launched);
		SteamUserStats.GetStat(StatType.stat_test_2.ToString(), out Stat_test_2);
		SteamUserStats.GetStat(StatType.stat_test_3.ToString(), out Stat_test_3);
		SteamUserStats.GetStat(StatType.stat_test_4.ToString(), out Stat_test_4);
		SteamUserStats.GetStat(StatType.stat_test_5.ToString(), out Stat_test_5);
		Achievements = new Dictionary<AchievementType, Achievement>();
		for (int i = 0; i < 105; i++)
		{
			AddAchievement((AchievementType)i);
		}
	}

	public static void OnJoinGame()
	{
		WorldManager.DayCounterChange = (Action<int>)Delegate.Combine(WorldManager.DayCounterChange, new Action<int>(OnDayCountChange));
	}

	public static void OnLeaveGame()
	{
		TempStat_builtSail = false;
		TempStat_builtBigAnchor = false;
		TempStat_pipesPlaced = 0;
		TempStat_beeHivesPlaced = 0;
		TempStat_cookedARecipie = false;
		TempStat_allMysteryLootUnlocked = false;
	}

	public static void Update()
	{
		if (!initialized)
		{
			return;
		}
		if (ShouldTrackAchievements && Achievements != null)
		{
			foreach (Achievement value in Achievements.Values)
			{
				EvaluateAchievement(value);
			}
		}
		if (storeStats)
		{
			storeStats = !SteamUserStats.StoreStats();
		}
	}

	public static void SetStat(StatType type, int value)
	{
		if (UserStatsRecieved && ShouldTrackAchievements)
		{
			SteamUserStats.SetStat(type.ToString(), value);
			storeStats = true;
		}
	}

	public static bool UnlockAchievement(AchievementType type)
	{
		if (!UserStatsRecieved)
		{
			return false;
		}
		if (!ShouldTrackAchievements)
		{
			return false;
		}
		if (Achievements.ContainsKey(type))
		{
			Achievement achievement = Achievements[type];
			if (!HasUnlocked(achievement.type))
			{
				UnlockAchievement(achievement);
			}
			return true;
		}
		return false;
	}

	public static bool HasUnlocked(AchievementType type)
	{
		if (Achievements.ContainsKey(type))
		{
			return Achievements[type].achieved;
		}
		return true;
	}

	public static void ResetAchievements()
	{
		initialized = false;
		BlockCreator.PlaceBlockCallStackAlways = (Action<Block, Network_Player, bool, int>)Delegate.Remove(BlockCreator.PlaceBlockCallStackAlways, new Action<Block, Network_Player, bool, int>(OnCreateBlock));
		BlockCreator.RemoveBlockCallStackAlways = (Action<List<Block>, Network_Player>)Delegate.Remove(BlockCreator.RemoveBlockCallStackAlways, new Action<List<Block>, Network_Player>(OnRemoveBlock));
		OnLeaveGame();
		SteamUserStats.ResetAllStats(bAchievementsToo: true);
		SteamUserStats.RequestCurrentStats();
	}

	public static void AddCapturedAnimal(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_player_capturedAnimals += count;
			SetStat(StatType.stat_player_capturedAnimals, Stat_player_capturedAnimals);
		}
	}

	public static void AddPlayerTokenSpendTangaroa(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_player_token_spend_tangaroa += count;
			SetStat(StatType.stat_player_token_spend_tangaroa, Stat_player_token_spend_tangaroa);
		}
	}

	public static void AddPlayerZiplineDistance(float distance)
	{
		if (ShouldTrackAchievements)
		{
			Stat_player_zipline_distance += (int)distance;
			SetStat(StatType.stat_player_zipline_distance, Stat_player_zipline_distance);
			if (distance > (float)Stat_player_zipline_distanceOneGo)
			{
				Stat_player_zipline_distanceOneGo = (int)distance;
				SetStat(StatType.stat_player_zipline_distanceOneGo, Stat_player_zipline_distanceOneGo);
			}
		}
	}

	public static void AddPlayeDeath(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_player_deaths += count;
			SetStat(StatType.stat_player_deaths, Stat_player_deaths);
		}
	}

	public static void AddBuildRemoveCount(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_build_removeCount += count;
			SetStat(StatType.stat_build_removeCount, Stat_build_removeCount);
		}
	}

	public static void AddBuildPaintCount(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_build_paintCount += count;
			SetStat(StatType.stat_build_paintCount, Stat_build_paintCount);
		}
	}

	public static void AddPlayerHookCount(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_player_hookCount += count;
			SetStat(StatType.stat_player_hookCount, Stat_player_hookCount);
		}
	}

	public static void AddPlayerExcevationsTreasure(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_player_excevations_treasure += count;
			SetStat(StatType.stat_player_excevations_treasure, Stat_player_excevations_treasure);
		}
	}

	public static void AddPlayerBirdKills(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_player_birdKills += count;
			SetStat(StatType.stat_player_birdKills, Stat_player_birdKills);
		}
	}

	public static void AddPlayerInstrumentNotesPlayed(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_player_instrumentNotes_played += count;
			SetStat(StatType.stat_player_instrumentNotes_played, Stat_player_instrumentNotes_played);
		}
	}

	public static void AddPlayerFireworksLaunched(int count = 1)
	{
		if (ShouldTrackAchievements)
		{
			Stat_player_fireworks_launched += count;
			SetStat(StatType.stat_player_fireworks_launched, Stat_player_fireworks_launched);
		}
	}

	public static void AddEntityKillByType(AI_NetworkBehaviourType type)
	{
		switch (type)
		{
		case AI_NetworkBehaviourType.PufferFish:
			SetStat(StatType.stat_player_pufferKills, ++Stat_player_pufferfishKills);
			break;
		case AI_NetworkBehaviourType.Boar:
			SetStat(StatType.stat_player_boarKills, ++Stat_player_boarKills);
			break;
		case AI_NetworkBehaviourType.Shark:
			SetStat(StatType.stat_player_sharkKills, ++Stat_player_sharkKills);
			break;
		case AI_NetworkBehaviourType.BugSwarm_Bee:
			SetStat(StatType.stat_player_captures_bee, ++Stat_player_captures_bee);
			break;
		case AI_NetworkBehaviourType.Bear:
		case AI_NetworkBehaviourType.PolarBear:
			SetStat(StatType.stat_player_bearKills, ++Stat_player_bearKills);
			break;
		case AI_NetworkBehaviourType.MamaBear:
			SetStat(StatType.stat_player_bearKills, ++Stat_player_bearKills);
			UnlockAchievement(AchievementType.ach_mamaBearKill);
			break;
		case AI_NetworkBehaviourType.StoneBird:
		case AI_NetworkBehaviourType.StoneBird_Caravan:
			SetStat(StatType.stat_player_stoneBirdKills, ++Stat_player_stoneBirdKills);
			break;
		case AI_NetworkBehaviourType.Rat:
		case AI_NetworkBehaviourType.Rat_Tangaroa:
			SetStat(StatType.stat_player_ratKills, ++Stat_player_ratKills);
			break;
		case AI_NetworkBehaviourType.ButlerBot:
			SetStat(StatType.stat_player_botKills, ++Stat_player_botKills);
			break;
		case AI_NetworkBehaviourType.Boss_Varuna:
			SetStat(StatType.stat_player_sharkKills, ++Stat_player_sharkKills);
			UnlockAchievement(AchievementType.ach_varunaBossKill);
			break;
		case AI_NetworkBehaviourType.AnglerFish:
			SetStat(StatType.stat_player_anglerFishKills, ++Stat_player_anglerFishKills);
			break;
		case AI_NetworkBehaviourType.None:
		case AI_NetworkBehaviourType.Llama:
		case AI_NetworkBehaviourType.Goat:
		case AI_NetworkBehaviourType.Chicken:
		case AI_NetworkBehaviourType.TEST:
		case AI_NetworkBehaviourType.Pig:
		case AI_NetworkBehaviourType.Roach:
			break;
		}
	}

	private static void AddAchievement(AchievementType type)
	{
		Achievements.Add(type, new Achievement(type));
	}

	private static void EvaluateAchievement(Achievement ach)
	{
		if (!ShouldTrackAchievements || ach == null || ach.achieved)
		{
			return;
		}
		switch (ach.type)
		{
		case AchievementType.ach_sailor:
			if (TempStat_builtBigAnchor && TempStat_builtSail)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_farmer:
			if (PlantManager.allCropplots != null && PlantManager.allCropplots.Count >= 20)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_hoarder:
			if (StorageManager.allStorages != null && StorageManager.allStorages.Count >= 20)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_cemetary:
			if (Stat_player_deaths >= 25)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_build_reorganizer:
			if (Stat_build_removeCount >= 100)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_cookedRecipe:
			if (TempStat_cookedARecipie)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_build_foundation_10:
			if (Stat_build_foundationCount >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_build_foundation_100:
			if (Stat_build_foundationCount >= 100)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_build_foundation_1000:
			if (Stat_build_foundationCount >= 1000)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_sharkKiller_1:
			if (Stat_player_sharkKills >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_sharkKiller_10:
			if (Stat_player_sharkKills >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_sharkKiller_50:
			if (Stat_player_sharkKills >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_gatherer_100:
			if (Stat_player_hookCount >= 100)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_gatherer_500:
			if (Stat_player_hookCount >= 500)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_gatherer_5000:
			if (Stat_player_hookCount >= 5000)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_birdKiller_1:
			if (Stat_player_birdKills >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_birdKiller_10:
			if (Stat_player_birdKills >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_birdKiller_50:
			if (Stat_player_birdKills >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_pufferKiller_1:
			if (Stat_player_pufferfishKills >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_pufferKiller_10:
			if (Stat_player_pufferfishKills >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_pufferKiller_50:
			if (Stat_player_pufferfishKills >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_stoneBirdKiller_1:
			if (Stat_player_stoneBirdKills >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_stoneBirdKiller_10:
			if (Stat_player_stoneBirdKills >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_stoneBirdKiller_50:
			if (Stat_player_stoneBirdKills >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_boarKiller_1:
			if (Stat_player_boarKills >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_boarKiller_10:
			if (Stat_player_boarKills >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_boarKiller_50:
			if (Stat_player_boarKills >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_build_painter_50:
			if (Stat_build_paintCount >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_build_painter_200:
			if (Stat_build_paintCount >= 200)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_build_painter_1000:
			if (Stat_build_paintCount >= 1000)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_captureAnimal_1:
			if (Stat_player_capturedAnimals >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_captureAnimal_10:
			if (Stat_player_capturedAnimals >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_captureAnimal_50:
			if (Stat_player_capturedAnimals >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_ratKiller_1:
			if (Stat_player_ratKills >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_ratKiller_10:
			if (Stat_player_ratKills >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_ratKiller_50:
			if (Stat_player_ratKills >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_bearKiller_1:
			if (Stat_player_bearKills >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_bearKiller_10:
			if (Stat_player_bearKills >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_bearKiller_50:
			if (Stat_player_bearKills >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_anglerFishKiller_1:
			if (Stat_player_anglerFishKills >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_anglerFishKiller_10:
			if (Stat_player_anglerFishKills >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_anglerFishKiller_50:
			if (Stat_player_anglerFishKills >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_botKiller_1:
			if (Stat_player_botKills >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_botKiller_10:
			if (Stat_player_botKills >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_botKiller_20:
			if (Stat_player_botKills >= 20)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_zip_100m:
			if (Stat_player_zipline_distance >= 100)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_zip_500m:
			if (Stat_player_zipline_distance >= 500)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_zip_1500m:
			if (Stat_player_zipline_distance >= 1500)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_zip_OneGo:
			if (Stat_player_zipline_distanceOneGo >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_excevator_1:
			if (Stat_player_excevations_treasure >= 1)
			{
				UnlockAchievement(AchievementType.ach_excevator_1);
			}
			break;
		case AchievementType.ach_excevator_20:
			if (Stat_player_excevations_treasure >= 20)
			{
				UnlockAchievement(AchievementType.ach_excevator_20);
			}
			break;
		case AchievementType.ach_excevator_50:
			if (Stat_player_excevations_treasure >= 50)
			{
				UnlockAchievement(AchievementType.ach_excevator_50);
			}
			break;
		case AchievementType.ach_pipes_30:
			if (TempStat_pipesPlaced >= 30)
			{
				UnlockAchievement(AchievementType.ach_pipes_30);
			}
			break;
		case AchievementType.ach_beekeeper_10:
			if (TempStat_beeHivesPlaced >= 10)
			{
				UnlockAchievement(AchievementType.ach_beekeeper_10);
			}
			break;
		case AchievementType.ach_beeCaptures_1:
			if (Stat_player_captures_bee >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_beeCaptures_10:
			if (Stat_player_captures_bee >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_beeCaptures_25:
			if (Stat_player_captures_bee >= 25)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_instrument_notes_1:
			if (Stat_player_instrumentNotes_played >= 1)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_token_tangaroa_10:
			if (Stat_player_token_spend_tangaroa >= 10)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_token_tangaroa_20:
			if (Stat_player_token_spend_tangaroa >= 20)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_token_tangaroa_30:
			if (Stat_player_token_spend_tangaroa >= 30)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_fireworks_50:
			if (Stat_player_fireworks_launched >= 50)
			{
				UnlockAchievement(ach);
			}
			break;
		case AchievementType.ach_researcher:
		case AchievementType.ach_radiotower:
		case AchievementType.ach_build_cookingstandOne:
		case AchievementType.ach_build_purifierOne:
		case AchievementType.ach_survivor_day:
		case AchievementType.ach_survivor_week:
		case AchievementType.ach_survivor_month:
		case AchievementType.ach_survivor_year:
		case AchievementType.ach_diveDeep:
		case AchievementType.ach_reach_pilotIsland:
		case AchievementType.ach_reach_bigIsland:
		case AchievementType.ach_captureRareAnimal:
		case AchievementType.ach_reach_captainIsland:
		case AchievementType.ach_mamaBearKill:
		case AchievementType.ach_balboaBicycle:
		case AchievementType.ach_fixErrol:
		case AchievementType.ach_brunosToolboard:
		case AchievementType.ach_tinyLittleMurderer:
		case AchievementType.ach_balboaMadeYouLook:
		case AchievementType.ach_vasagatan_Bomb:
		case AchievementType.ach_allNotes_RadioTower:
		case AchievementType.ach_allNotes_Vasagatan:
		case AchievementType.ach_allNotes_Balboa:
		case AchievementType.ach_allRBPaintings:
		case AchievementType.ach_allNotes_Caravan:
		case AchievementType.ach_allNotes_Tangaroa:
		case AchievementType.ach_tiki_completed:
		case AchievementType.ach_secret_tangaroa_claw:
		case AchievementType.ach_tangaroa_cockpitLaunch:
		case AchievementType.ach_hidden_tangaroa_room:
		case AchievementType.ach_allMysteryLootUnlocked:
		case AchievementType.ach_varunaBossKill:
			break;
		}
	}

	public static void EvalueateNoteBookAchievement(ChunkPointType type, NoteBookUI noteBookUI)
	{
		if (noteBookUI.IsAllNotesUnlockedOfType(type))
		{
			switch (type)
			{
			case ChunkPointType.Landmark_RadioTower:
				UnlockAchievement(AchievementType.ach_allNotes_RadioTower);
				break;
			case ChunkPointType.Landmark_Vasagatan:
				UnlockAchievement(AchievementType.ach_allNotes_Vasagatan);
				break;
			case ChunkPointType.Landmark_Balboa:
				UnlockAchievement(AchievementType.ach_allNotes_Balboa);
				break;
			case ChunkPointType.Landmark_CaravanIsland:
				UnlockAchievement(AchievementType.ach_allNotes_Caravan);
				break;
			case ChunkPointType.Landmark_Tangaroa:
				UnlockAchievement(AchievementType.ach_allNotes_Tangaroa);
				break;
			case ChunkPointType.Landmark_Temperance:
				UnlockAchievement(AchievementType.ach_allNotes_Temperance);
				break;
			case ChunkPointType.Landmark_VarunaPoint:
				UnlockAchievement(AchievementType.ach_allNotes_Varuna);
				break;
			case ChunkPointType.Landmark_FloatingRaft:
			case ChunkPointType.Landmark_Boat:
			case ChunkPointType.Landmark_Test:
				break;
			}
		}
	}

	private static void UnlockAchievement(Achievement ach)
	{
		if (ach != null)
		{
			ach.achieved = true;
			SteamUserStats.SetAchievement(ach.type.ToString());
			storeStats = true;
		}
	}

	private static void OnCreateBlock(Block block, Network_Player player, bool replicating, int hotSlotIndex)
	{
		if (!(block == null) && !(player == null) && player.IsLocalPlayer && ShouldTrackAchievements)
		{
			CheckAchievementForBuiltUniqueBlockIndex(block.buildableItem.UniqueIndex, replicating);
		}
	}

	public static void OnRemoveBlock(List<Block> blocks, Network_Player playerRemovingBlock)
	{
		if (blocks == null || playerRemovingBlock == null || playerRemovingBlock == null || !playerRemovingBlock.IsLocalPlayer || !ShouldTrackAchievements)
		{
			return;
		}
		for (int i = 0; i < blocks.Count; i++)
		{
			Block block = blocks[i];
			if (!(block == null) && !(block.buildableItem == null))
			{
				CheckAchievementForRemovedUniqueBlockIndex(block.buildableItem.UniqueIndex);
			}
		}
	}

	public static void CheckAchievementForBuiltUniqueBlockIndex(int uniqueIndex, bool replicating)
	{
		switch (uniqueIndex)
		{
		case 2:
		case 189:
		case 382:
		case 383:
		case 387:
			if (!replicating)
			{
				SetStat(StatType.stat_build_foundationCount, ++Stat_build_foundationCount);
			}
			break;
		case 43:
			if (!replicating)
			{
				UnlockAchievement(AchievementType.ach_build_cookingstandOne);
			}
			break;
		case 91:
			if (!replicating)
			{
				UnlockAchievement(AchievementType.ach_build_purifierOne);
			}
			break;
		case 126:
			TempStat_builtSail = true;
			break;
		case 6:
			TempStat_builtBigAnchor = true;
			break;
		case 331:
		case 332:
		case 333:
		case 334:
		case 335:
		case 336:
		case 337:
		case 338:
			if (!uniquePaintingIndexes.Contains(uniqueIndex))
			{
				uniquePaintingIndexes.Add(uniqueIndex);
				if (uniquePaintingIndexes.Count >= 8)
				{
					UnlockAchievement(AchievementType.ach_allRBPaintings);
				}
			}
			break;
		case 277:
		case 341:
			TempStat_pipesPlaced++;
			break;
		case 290:
			TempStat_beeHivesPlaced++;
			break;
		}
	}

	public static void CheckAchievementForRemovedUniqueBlockIndex(int uniqueIndex)
	{
		switch (uniqueIndex)
		{
		case 126:
			TempStat_builtSail = false;
			break;
		case 6:
			TempStat_builtBigAnchor = false;
			break;
		case 331:
		case 332:
		case 333:
		case 334:
		case 335:
		case 336:
		case 337:
		case 338:
			if (uniquePaintingIndexes.Contains(uniqueIndex))
			{
				uniquePaintingIndexes.Remove(uniqueIndex);
			}
			break;
		case 277:
		case 341:
			TempStat_pipesPlaced = Mathf.Clamp(TempStat_pipesPlaced - 1, 0, int.MaxValue);
			break;
		case 290:
			TempStat_beeHivesPlaced = Mathf.Clamp(TempStat_beeHivesPlaced - 1, 0, int.MaxValue);
			break;
		}
	}

	public static void CheckAchievementForOnRestoreBlockIndex(int uniqueIndex)
	{
		switch (uniqueIndex)
		{
		case 126:
			TempStat_builtSail = true;
			break;
		case 6:
			TempStat_builtBigAnchor = true;
			break;
		}
	}

	public static void CheckMysterPackageAchievement(List<Item_Base> items)
	{
		if (HasUnlockedAllItems(items))
		{
			UnlockAchievement(AchievementType.ach_allMysteryLootUnlocked);
		}
	}

	private static bool HasUnlockedAllItems(List<Item_Base> items)
	{
		if (items == null)
		{
			return false;
		}
		foreach (Item_Base item in items)
		{
			if (SO_MysteryPackageLoot.IsPossibleYieldItem(item))
			{
				return false;
			}
		}
		return true;
	}

	private static void OnDayCountChange(int dayCount)
	{
		if (ShouldTrackAchievements)
		{
			if (dayCount >= 1)
			{
				UnlockAchievement(AchievementType.ach_survivor_day);
			}
			if (dayCount >= 7)
			{
				UnlockAchievement(AchievementType.ach_survivor_week);
			}
			if (dayCount >= 30)
			{
				UnlockAchievement(AchievementType.ach_survivor_month);
			}
			if (dayCount >= 365)
			{
				UnlockAchievement(AchievementType.ach_survivor_year);
			}
		}
	}
}
