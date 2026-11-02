using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;
using UnityEngine.AzureSky;

public class Cheat : MonoBehaviour_ID_Network
{
	public static Raft raft;

	private static CanvasHelper canvas;

	private static Network_Player player;

	private static WeatherManager weatherManager;

	private static Network_Host_Entities networkHostEntities;

	private static ChunkManager chunkManager;

	private static RaftCollisionManager raftColliManager;

	private static TreasurePointManager treasurePointManager;

	private static Raft_Network network;

	private static SO_SteamUser[] steamUsers;

	private static RemoteConfigManager remoteConfigManager;

	public static bool UseCheats = false;

	public static bool UseGodMode = false;

	public static bool UseOldWater = false;

	public static bool ForceRecieverToggle = false;

	public static bool ForceReflectionsOff = false;

	public static float FireworkLifeTime = 0f;

	public static float TargetTimeScale = 1f;

	public static bool UnlockAllCrafting = false;

	private static Vector3 markLocation = Vector3.zero;

	private static int _TestIndex = 0;

	public static void Initialize()
	{
		player = ComponentManager<Network_Player>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		weatherManager = UnityEngine.Object.FindObjectOfType<WeatherManager>();
		raft = ComponentManager<global::Raft>.Value;
		networkHostEntities = ComponentManager<Network_Host_Entities>.Value;
		network = ComponentManager<Raft_Network>.Value;
		chunkManager = ComponentManager<ChunkManager>.Value;
		treasurePointManager = ComponentManager<TreasurePointManager>.Value;
		LoadSteamUserSO();
		UseCheats = AllowCheatsForLocalPlayer();
	}

	public static void LoadSteamUserSO()
	{
		if (steamUsers == null)
		{
			steamUsers = Resources.LoadAll<SO_SteamUser>("SteamUsers");
		}
	}

	public static void HandleCheatCode(string p_message, string p_commandDelimeter, Network_UserId p_steamID)
	{
		string text = p_message.Substring(p_commandDelimeter.Length, p_message.Length - 1);
		if (text == string.Empty)
		{
			return;
		}
		string[] array = text.Split(' ');
		if (array.Length != 0)
		{
			if ("give".Contains(array[0]) && array.Length >= 2)
			{
				Give(array, p_steamID);
			}
			else if ("set".Contains(array[0]) && array.Length >= 3)
			{
				Set(array, p_steamID);
			}
			else if ("shift".Contains(array[0]))
			{
				WorldShift(p_steamID);
			}
			else if ("reset".Contains(array[0]) && array.Length >= 2)
			{
				Reset(array, p_steamID);
			}
			else if ("landmark".Contains(array[0]) && array.Length >= 2)
			{
				Landmark(array, p_steamID);
			}
			else if ("spawn".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Spawn(array, p_steamID);
			}
			else if ("tp".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Teleport(array, p_steamID);
			}
			else if ("show".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Show(array, p_steamID);
			}
			else if ("animals".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Animals(array, p_steamID);
			}
			else if ("questitem".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				QuestItem(array, p_steamID);
			}
			else if ("quest".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Quest(array, p_steamID);
			}
			else if ("raft".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Raft(array, p_steamID);
			}
			else if ("weather".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Weather(array, p_steamID);
			}
			else if ("unlock".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Unlock(array, p_steamID);
			}
			else if ("notebook".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Notebook(array, p_steamID);
			}
			else if ("nuke".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				Nuke();
			}
			else if ("build".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				Build(array, p_steamID);
			}
			else if ("camera".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 2)
			{
				Camera(array, p_steamID);
			}
			else if ("weight".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length == 1)
			{
				ComponentManager<ChatTextFieldController>.Value.AddUITextMessage("Raft weight = " + RaftWeightManager.FoundationWeight, p_steamID);
			}
			else if ("reciever".Contains(array[0], StringComparison.OrdinalIgnoreCase) && array.Length >= 1)
			{
				Reciever(array, p_steamID);
			}
			else if ("reflection".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				Reflections(array, p_steamID);
			}
			else if ("kit".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				Kit(array, p_steamID);
			}
			else if ("environmentlight".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				EnvironmentLight(array, p_steamID);
			}
			else if ("simulate".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				Simulate(array, p_steamID);
			}
			else if ("cook".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				CookingTable(array, p_steamID);
			}
			else if ("mark".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				Mark(array, p_steamID);
			}
			else if ("recall".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				Recall(array, p_steamID);
			}
			else if ("worldeventstop".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				WorldEventStop(array, p_steamID);
			}
			else if ("worldevent".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				WorldEvent(array, p_steamID);
			}
			else if ("print".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				PrintCheat(array, p_steamID);
			}
			else if ("hazmat".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				HandleHazmatSuit(array, p_steamID);
			}
			else if ("oldwater".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				HandleOldWater(array, p_steamID);
			}
			else if ("bakeblocks".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				HandleBakingBlocks(array, p_steamID);
			}
			else if ("honk".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				Honk(array, p_steamID);
			}
			else if ("DestroyBlock".Contains(array[0], StringComparison.OrdinalIgnoreCase))
			{
				DestroyBlocks(array, p_steamID);
			}
		}
	}

	public static void HandleTerminalCommand(string p_message, string p_commandDelimeter, Network_UserId p_steamID)
	{
		string text = p_message.Substring(p_commandDelimeter.Length, p_message.Length - 1);
		if (!(text == string.Empty))
		{
			string[] array = text.Split(' ');
			if (array.Length != 0 && array[0].Contains("cheat") && RemoteConfigManager.ExternalLocalUserIsDevCheck(network.LocalSteamID))
			{
				ChatManager.SendDebugChatMessage("LocalPlayer is considered a DEV.");
			}
		}
	}

	public static bool AllowCheatsForLocalPlayer()
	{
		if (player == null)
		{
			player = ComponentManager<Network_Player>.Value;
		}
		if (player == null)
		{
			return false;
		}
		return AllowCheatsForUser(SteamUser.GetSteamID());
	}

	public static bool AllowCheatsForUser(CSteamID cSteamID)
	{
		bool flag = false;
		bool result = false;
		LoadSteamUserSO();
		if (!steamUsers.ContainsItems())
		{
			result = false;
		}
		flag = RemoteConfigManager.validDevIds.Contains(cSteamID.m_SteamID) || RemoteConfigManager.CheckIfUserIsDev(cSteamID.m_SteamID);
		if (flag)
		{
			return flag;
		}
		for (int i = 0; i < steamUsers.Length; i++)
		{
			if (steamUsers[i].ID == cSteamID.m_SteamID && steamUsers[i].userLevel != SO_SteamUser.UserLevel.NONE)
			{
				result = true;
			}
		}
		return result;
	}

	public static bool IsLocalPlayerDev()
	{
		if (player == null)
		{
			player = ComponentManager<Network_Player>.Value;
		}
		if (player == null)
		{
			return false;
		}
		return IsUserDev(player.steamID);
	}

	public static bool IsUserDev(Network_UserId cSteamID)
	{
		bool flag = false;
		bool result = false;
		LoadSteamUserSO();
		flag = RemoteConfigManager.validDevIds.Contains(cSteamID) || RemoteConfigManager.CheckIfUserIsDev(cSteamID);
		if (flag)
		{
			return flag;
		}
		for (int i = 0; i < steamUsers.Length; i++)
		{
			if (steamUsers[i].ID == cSteamID && steamUsers[i].userLevel == SO_SteamUser.UserLevel.DEVELOPER)
			{
				result = true;
			}
		}
		return result;
	}

	public static void TeleportToRaycast()
	{
		if (Helper.HitAtCursor(out var hit, 128f, -1))
		{
			Vector3 vector = hit.point + Vector3.up * 1.5f;
			bool flag = false;
			if (vector != Vector3.zero)
			{
				player.transform.SetParentSafe(flag ? SingletonGeneric<GameManager>.Singleton.lockedPivot : null);
				player.PersonController.SwitchControllerType(ControllerType.Ground);
				player.transform.position = vector;
				Physics.SyncTransforms();
			}
		}
	}

	public static void LandmarkTest()
	{
		ChunkPointType[] array = new ChunkPointType[11]
		{
			ChunkPointType.Landmark_Big,
			ChunkPointType.Landmark_Pilot,
			ChunkPointType.Landmark_Boat,
			ChunkPointType.Landmark_RadioTower,
			ChunkPointType.Landmark_Vasagatan,
			ChunkPointType.Landmark_Balboa,
			ChunkPointType.Landmark_CaravanIsland,
			ChunkPointType.Landmark_Tangaroa,
			ChunkPointType.Landmark_VarunaPoint,
			ChunkPointType.Landmark_Temperance,
			ChunkPointType.Landmark_Utopia
		};
		chunkManager.AddChunkPointCheat(array[_TestIndex], global::Raft.direction.normalized * 150f);
		_TestIndex++;
		_TestIndex %= array.Length;
	}

	public static void LandmarkRandom()
	{
		Array values = Enum.GetValues(typeof(ChunkPointType));
		ChunkPointType pointType = (ChunkPointType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		chunkManager.AddChunkPointCheat(pointType, global::Raft.direction.normalized * 150f);
	}

	private static void Landmark(string[] entries, Network_UserId steamID)
	{
		string text = entries[1];
		Array values = Enum.GetValues(typeof(ChunkPointType));
		int num = 150;
		foreach (ChunkPointType item in values)
		{
			if (text.Contains("clear", StringComparison.OrdinalIgnoreCase) && entries.Length >= 3)
			{
				string text2 = entries[2];
				if (text2.Contains("all", StringComparison.OrdinalIgnoreCase))
				{
					chunkManager.ClearAllChunkPoints();
					break;
				}
				if (item.ToString().Contains(text2, StringComparison.OrdinalIgnoreCase))
				{
					chunkManager.ClearChunkPoints(item);
					break;
				}
			}
			else if (item.ToString().Contains(text, StringComparison.OrdinalIgnoreCase))
			{
				switch (item)
				{
				case ChunkPointType.Landmark_Small:
				case ChunkPointType.Landmark_RadioTower:
					num = 50;
					break;
				case ChunkPointType.Landmark_FloatingRaft:
					num = 30;
					break;
				}
				chunkManager.AddChunkPointCheat(item, global::Raft.direction.normalized * num);
				break;
			}
		}
	}

	private static void Give(string[] entries, Network_UserId steamID)
	{
		if (!IsSteamIDLocalPlayer(steamID))
		{
			return;
		}
		string text = entries[1];
		bool flag = text.All(char.IsDigit);
		int amountFromEntries = GetAmountFromEntries(entries);
		if (flag)
		{
			Item_Base itemByIndex = ItemManager.GetItemByIndex(int.Parse(text));
			if (itemByIndex != null)
			{
				player.Inventory.AddItem(itemByIndex.UniqueName, amountFromEntries);
			}
			return;
		}
		string excludeItemsContainingThis = (text.Contains("blueprint", StringComparison.OrdinalIgnoreCase) ? null : "blueprint");
		Item_Base itemByNameContains = ItemManager.GetItemByNameContains(text, excludeItemsContainingThis);
		if (itemByNameContains != null)
		{
			player.Inventory.AddItem(itemByNameContains.UniqueName, amountFromEntries);
		}
	}

	private static void Spawn(string[] entries, Network_UserId p_steamID)
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		string value = entries[1];
		Network_Player playerFromID = network.GetPlayerFromID(p_steamID);
		foreach (AI_NetworkBehaviourType value2 in Enum.GetValues(typeof(AI_NetworkBehaviourType)))
		{
			if (!value2.ToString().Contains(value, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			int amountFromEntries = GetAmountFromEntries(entries);
			for (int i = 0; i < amountFromEntries; i++)
			{
				Vector3 spawnPosition = playerFromID.FeetPosition + playerFromID.transform.forward * 2f;
				switch (value2)
				{
				case AI_NetworkBehaviourType.Shark:
					spawnPosition = networkHostEntities.GetSharkSpawnPosition();
					break;
				case AI_NetworkBehaviourType.PufferFish:
					spawnPosition = playerFromID.FeetPosition + playerFromID.transform.forward * 4f;
					if (spawnPosition.y > -1f)
					{
						spawnPosition.y = -1f;
					}
					break;
				case AI_NetworkBehaviourType.StoneBird:
				case AI_NetworkBehaviourType.StoneBird_Caravan:
					spawnPosition = playerFromID.FeetPosition + new Vector3(UnityEngine.Random.Range(3f, 10f), 10f, UnityEngine.Random.Range(3f, 10f));
					if (spawnPosition.y < 15f)
					{
						spawnPosition.y = 15f;
					}
					break;
				case AI_NetworkBehaviourType.Llama:
				case AI_NetworkBehaviourType.Goat:
				case AI_NetworkBehaviourType.Chicken:
					spawnPosition = playerFromID.FeetPosition + playerFromID.transform.forward;
					break;
				case AI_NetworkBehaviourType.Dolphin:
				case AI_NetworkBehaviourType.Turtle:
				case AI_NetworkBehaviourType.Stingray:
					if (!networkHostEntities.GetSpawnPositionDontCollideWithChunkPoint(ref spawnPosition, 50f))
					{
						return;
					}
					break;
				case AI_NetworkBehaviourType.Whale:
					if (!networkHostEntities.GetSpawnPositionDontCollideWithChunkPoint(ref spawnPosition, 300f))
					{
						return;
					}
					break;
				case AI_NetworkBehaviourType.BirdPack:
					if (!networkHostEntities.GetBirdpackSpawnPosition(ref spawnPosition))
					{
						return;
					}
					break;
				}
				AI_NetworkBehaviour aI_NetworkBehaviour = networkHostEntities.CreateAINetworkBehaviour(value2, spawnPosition);
				if (aI_NetworkBehaviour is AI_NetworkBehaviour_Domestic)
				{
					(aI_NetworkBehaviour as AI_NetworkBehaviour_Domestic).QuickTameLate();
				}
			}
			break;
		}
	}

	private static void Set(string[] entries, Network_UserId p_steamID)
	{
		string text = entries[1];
		if (text.All(char.IsDigit))
		{
			return;
		}
		float num = 1f;
		string text2 = entries[2];
		if (!string.IsNullOrEmpty(text2))
		{
			try
			{
				num = float.Parse(text2);
			}
			catch (Exception)
			{
			}
		}
		switch (text)
		{
		case "thirst":
			if (IsSteamIDLocalPlayer(p_steamID))
			{
				player.Stats.stat_thirst.Normal.NormalValue = num;
			}
			break;
		case "bonusthirst":
			if (IsSteamIDLocalPlayer(p_steamID))
			{
				player.Stats.stat_thirst.Bonus.NormalValue = num;
			}
			break;
		case "hunger":
			if (IsSteamIDLocalPlayer(p_steamID))
			{
				player.Stats.stat_hunger.Normal.NormalValue = num;
			}
			break;
		case "bonushunger":
			if (IsSteamIDLocalPlayer(p_steamID))
			{
				player.Stats.stat_hunger.Bonus.NormalValue = num;
			}
			break;
		case "bonushealth":
			if (IsSteamIDLocalPlayer(p_steamID))
			{
				player.Stats.stat_BonusHealth.NormalValue = num;
			}
			break;
		case "health":
			if (!IsSteamIDLocalPlayer(p_steamID))
			{
				break;
			}
			if (num == 0f)
			{
				Message_NetworkBehaviour message = new Message_NetworkBehaviour(Messages.PlayerKilled, player);
				if (Raft_Network.IsHost)
				{
					player.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					player.PlayerScript.Kill();
				}
				else
				{
					player.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			player.Stats.stat_health.NormalValue = num;
			break;
		case "oxygen":
			if (IsSteamIDLocalPlayer(p_steamID))
			{
				player.Stats.stat_oxygen.NormalValue = num;
			}
			break;
		case "gametime":
			GameManager.TimePlayed = num;
			ComponentManager<ChatTextFieldController>.Value.CreateLocalChatMessage("Changing game time played to: " + num + " in seconds");
			if (chunkManager != null)
			{
				chunkManager.checkExtraSpawners.SetProgress(chunkManager.checkExtraSpawners.GetCooldown());
			}
			break;
		case "time":
		{
			AzureSkyController value = ComponentManager<AzureSkyController>.Value;
			if (value != null)
			{
				value.timeOfDay.hour = num;
			}
			break;
		}
		case "blockhealth":
			if (IsSteamIDLocalPlayer(p_steamID))
			{
				SetBlockHealth(num, p_steamID);
			}
			break;
		case "gamemode":
			if (Raft_Network.IsHost)
			{
				GameModeValueManager.SelectCurrentGameMode((GameMode)Mathf.RoundToInt(num));
			}
			break;
		case "anchor":
			if (Raft_Network.IsHost)
			{
				if (text2.Contains("true", StringComparison.OrdinalIgnoreCase))
				{
					raft.AddAnchor(playSound: false);
				}
				else if (text2.Contains("false", StringComparison.OrdinalIgnoreCase))
				{
					raft.RemoveAnchor(int.MaxValue);
				}
			}
			break;
		case "fps":
			if (IsSteamIDLocalPlayer(p_steamID))
			{
				GameManager.targetFrameRate = (int)num;
				Application.targetFrameRate = (int)num;
			}
			break;
		case "frequency":
			Frequencies(entries, p_steamID, num);
			break;
		case "durability":
			SetDurability(entries, p_steamID, (int)num);
			break;
		case "fireworklife":
			FireworkLifeTime = num;
			break;
		case "timescale":
			if (num <= 0f)
			{
				SetTargetTimeScale(1f);
			}
			else
			{
				SetTargetTimeScale(num);
			}
			ChatManager.SendDebugChatMessage($"Target timescale = {TargetTimeScale}");
			break;
		}
	}

	public static void SetTargetTimeScale(float newScale)
	{
		TargetTimeScale = Mathf.Clamp(newScale, 0.01f, 150f);
	}

	private static void SetDurability(string[] entries, Network_UserId p_steamID, int amount)
	{
		Slot selectedHotbarSlot = player.Inventory.GetSelectedHotbarSlot();
		if (selectedHotbarSlot != null && selectedHotbarSlot.HasValidItemInstance())
		{
			selectedHotbarSlot.itemInstance.Uses = amount;
		}
	}

	private static void Frequencies(string[] entries, Network_UserId p_steamID, float amount)
	{
		if (entries.Length == 4)
		{
			string text = entries[3];
			RecieverFrequency recieverFrequency = RecieverFrequency.AllFrequencies[(int)amount];
			if (recieverFrequency != null && text.Length == recieverFrequency.numbers.Length)
			{
				for (int i = 0; i < recieverFrequency.numbers.Length; i++)
				{
					int num = (int)char.GetNumericValue(text.ElementAt(i));
					recieverFrequency.numbers[i] = num;
				}
			}
		}
		else
		{
			Reciever[] array = UnityEngine.Object.FindObjectsOfType<Reciever>();
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetFrequencyIndexCheat((int)amount);
			}
		}
	}

	private static void Show(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		string text = entries[1];
		if (text.Contains("landmark"))
		{
			ChatTextFieldController value = ComponentManager<ChatTextFieldController>.Value;
			if (player != null && value != null)
			{
				Landmark landmark = null;
				float num = float.MaxValue;
				for (int i = 0; i < WorldManager.AllLandmarks.Count; i++)
				{
					Landmark landmark2 = WorldManager.AllLandmarks[i];
					if (landmark2 != null && landmark2.gameObject.activeInHierarchy)
					{
						float num2 = landmark2.transform.position.DistanceXZ(player.transform.position);
						if (num2 < num)
						{
							num = num2;
							landmark = landmark2;
						}
					}
				}
				string p_ChatMessage = ((landmark == null) ? "Can't find any landmark" : ("Landmark Index [" + landmark.uniqueLandmarkIndex + "] " + num.ToString("F1") + " units away from you."));
				value.AddUITextMessage(p_ChatMessage, player.steamID);
			}
		}
		if (text.Contains("treasure"))
		{
			ComponentManager<ChatTextFieldController>.Value.AddUITextMessage("There are currently " + treasurePointManager.GetTreasurePointsInAreaTo(player.transform.position, 9999f, includeUnBuried: true, includeUnrevealed: true).Count + " treasurePoints", player.steamID);
		}
	}

	private static void Quest(string[] entries, Network_UserId p_steamID)
	{
		Quest questByName = QuestProgressTracker.GetQuestByName(entries[1]);
		if (questByName != null)
		{
			ChatTextFieldController value = ComponentManager<ChatTextFieldController>.Value;
			if (player != null && value != null)
			{
				string p_ChatMessage = "Quest '" + questByName.questType.ToString() + "' = " + questByName.isQuestComplete;
				value.AddUITextMessage(p_ChatMessage, player.steamID);
			}
		}
	}

	private static void Raft(string[] entries, Network_UserId p_steamID)
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		string text = entries[1];
		if (text.Equals("unstuck"))
		{
			if (raftColliManager == null)
			{
				raftColliManager = raft.transform.parent.GetComponentInChildren<RaftCollisionManager>();
			}
			Network_Player playerFromID = network.GetPlayerFromID(p_steamID);
			if (playerFromID != null)
			{
				raftColliManager.transform.position = playerFromID.transform.position;
			}
			else
			{
				Debug.Log("Unable to find player with id in cheat");
			}
		}
		else
		{
			if (!text.Equals("target"))
			{
				return;
			}
			Reciever reciever = UnityEngine.Object.FindObjectOfType<Reciever>();
			Vector3 vector = Vector3.zero;
			bool flag = false;
			if (reciever != null)
			{
				foreach (Reciever_Dot recieverRadarDot in reciever.GetRecieverRadarDots())
				{
					if (recieverRadarDot.targetedByReciever)
					{
						vector = recieverRadarDot.chunkPoint.worldPosition;
						flag = true;
					}
				}
			}
			if (flag)
			{
				Vector3.Distance(raft.transform.position, vector);
				float num = float.Parse(entries[2]);
				Vector3 normalized = (vector - raft.transform.position).normalized;
				normalized.y = 0f;
				if (raftColliManager == null)
				{
					raftColliManager = raft.transform.parent.GetComponentInChildren<RaftCollisionManager>();
				}
				raftColliManager.transform.position = vector - normalized * num;
				ChatManager.SendDebugChatMessage("Moving raft to reciever dot" + entries[2]);
			}
			else
			{
				ChatManager.SendDebugChatMessage("Unsable To: Move Raft no Rec target! ");
			}
		}
	}

	private static void Weather(string[] entries, Network_UserId p_steamID)
	{
		if (Raft_Network.IsHost)
		{
			string text = entries[1];
			bool flag = entries.Length == 3 && entries[2].Contains("lerp", StringComparison.OrdinalIgnoreCase);
			if (!text.IsNullOrEmpty())
			{
				weatherManager.SetWeather(text, !flag);
			}
		}
	}

	private static void Unlock(string[] entries, Network_UserId p_steamID)
	{
		bool flag = IsSteamIDLocalPlayer(p_steamID);
		bool flag2 = (entries.Length == 4 && entries[3] == "all") || (entries.Length == 3 && entries[2] == "all");
		if (!entries[1].Contains("character", StringComparison.OrdinalIgnoreCase) || (!flag && !(!flag & flag2)))
		{
			return;
		}
		int amountFromEntries = GetAmountFromEntries(entries, -1);
		if (amountFromEntries != -1)
		{
			SO_Character characterFromIndex = CharacterManager.GetCharacterFromIndex(amountFromEntries);
			if (characterFromIndex != null && CharacterManager.UnlockCharacterByIndex(amountFromEntries))
			{
				ChatManager.SendDebugChatMessage("Unlocked character: " + characterFromIndex.displayName);
			}
		}
		else
		{
			CharacterManager.UnlockAllCharacters();
			ChatManager.SendDebugChatMessage("Unlocking all characters");
		}
	}

	private static void Notebook(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		NoteBook value = ComponentManager<NoteBook>.Value;
		string text = entries[1];
		if (text.Equals("all"))
		{
			value.UnlockAllNotes();
			return;
		}
		int num = int.Parse(text);
		if (num >= 0 && value != null)
		{
			value.UnlockSpecificNoteNetworked(num, showNotification: true);
			ComponentManager<ChatTextFieldController>.Value.AddUITextMessage("Unlocked note: " + num, player.steamID);
		}
	}

	private static void QuestItem(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID) || string.IsNullOrEmpty(entries[1]))
		{
			return;
		}
		if (entries[1] == "check")
		{
			QuestItemManager.LogAllQuestItems();
			return;
		}
		if (entries[1] == "clear")
		{
			QuestItemManager.ClearAllQuestItems();
			return;
		}
		QuestItemType[] array = (QuestItemType[])Enum.GetValues(typeof(QuestItemType));
		for (int i = 0; i < array.Length; i++)
		{
			QuestItemType questItemType = array[i];
			if (questItemType.ToString().Contains(entries[1], StringComparison.OrdinalIgnoreCase))
			{
				int amountFromEntries = GetAmountFromEntries(entries);
				ComponentManager<QuestItemManager>.Value.AddQuestItemsNetworked(true, new QuestItem(QuestItemManager.GetSOQuestItemFromType(questItemType), amountFromEntries));
			}
		}
	}

	public static void Animals(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		AI_NetworkBehaviour_Domestic[] array = UnityEngine.Object.FindObjectsOfType<AI_NetworkBehaviour_Domestic>();
		string text = entries[1];
		if (text.Contains("starve") && Raft_Network.IsHost)
		{
			AI_NetworkBehaviour_Domestic[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].HungerTimer = 0f;
			}
			Debug.Log("Cheat::Starving animals");
		}
		else if (text.Contains("feed") && Raft_Network.IsHost)
		{
			AI_NetworkBehaviour_Domestic[] array2 = array;
			foreach (AI_NetworkBehaviour_Domestic obj in array2)
			{
				obj.HungerTimer = GameModeValueManager.GetCurrentGameModeValue().domesticVariables.hungerGain;
				obj.SetToMoveState();
			}
			Debug.Log("Cheat::Feeding animals");
		}
		else if (text.Contains("changematerial"))
		{
			ChangeMaterial(entries, p_steamID);
		}
	}

	private static void Camera(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		string value = entries[1];
		float num = 1f;
		if (entries.Length >= 3 && !string.IsNullOrEmpty(entries[2]))
		{
			try
			{
				num = float.Parse(entries[2]);
			}
			catch (Exception message)
			{
				Debug.Log(message);
			}
		}
		if ("acceleration".Contains(value, StringComparison.OrdinalIgnoreCase))
		{
			player.flightCamera.accelerationSpeed = num;
		}
		else if ("deacceleration".Contains(value, StringComparison.OrdinalIgnoreCase))
		{
			player.flightCamera.deAccelerationSpeed = num;
		}
		else if ("maxspeed".Contains(value, StringComparison.OrdinalIgnoreCase))
		{
			player.flightCamera.maxVelocity = num;
		}
		else if ("mouselerp".Contains(value, StringComparison.OrdinalIgnoreCase))
		{
			player.flightCamera.mouseLookScript.lerpSpeed = num;
		}
		else if ("target".Contains(value, StringComparison.OrdinalIgnoreCase))
		{
			if (entries.Length >= 3)
			{
				if (entries[2] == "null")
				{
					player.flightCamera.cameraLockTarget = null;
					return;
				}
				Network_Player playerFromVisualName = network.GetPlayerFromVisualName(entries[2]);
				player.flightCamera.cameraLockTarget = ((playerFromVisualName != null) ? playerFromVisualName.transform : null);
			}
		}
		else if ("targetoffset".Contains(value, StringComparison.OrdinalIgnoreCase))
		{
			player.flightCamera.cameraOffset = num;
		}
	}

	private static void Kit(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID) || entries.Length < 2)
		{
			return;
		}
		string text = entries[1];
		if (text.Contains("treasure", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Shovel", 1);
			player.Inventory.AddItem("MetalDetector", 1);
		}
		else if ("painting_redbeet".Contains(text, StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Placeable_Painting_André", 1);
			player.Inventory.AddItem("Placeable_Painting_Ellen", 1);
			player.Inventory.AddItem("Placeable_Painting_Felicia", 1);
			player.Inventory.AddItem("Placeable_Painting_Gabriel", 1);
			player.Inventory.AddItem("Placeable_Painting_Illayda", 1);
			player.Inventory.AddItem("Placeable_Painting_Petter", 1);
			player.Inventory.AddItem("Placeable_Painting_Semih", 1);
			player.Inventory.AddItem("Placeable_Painting_William", 1);
		}
		else if ("painting_tangaroa".Contains(text, StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Placeable_Painting_Tangaroa_1", 1);
			player.Inventory.AddItem("Placeable_Painting_Tangaroa_2", 1);
			player.Inventory.AddItem("Placeable_Painting_Tangaroa_3", 1);
			player.Inventory.AddItem("Placeable_Painting_Tangaroa_4", 1);
			player.Inventory.AddItem("Placeable_Painting_Tangaroa_5", 1);
		}
		else if (text.Contains("paintbrush", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("PaintBrush", 1);
			player.Inventory.AddItem("Color_Red", 10000);
			player.Inventory.AddItem("Color_Yellow", 10000);
			player.Inventory.AddItem("Color_Blue", 10000);
			player.Inventory.AddItem("Color_Black", 10000);
			player.Inventory.AddItem("Color_White", 10000);
		}
		else if (text.Contains("resources", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Scrap", 1000);
			player.Inventory.AddItem("Rope", 1000);
			player.Inventory.AddItem("Thatch", 1000);
			player.Inventory.AddItem("SeaVine", 1000);
			player.Inventory.AddItem("Stone", 1000);
			player.Inventory.AddItem("Plastic", 1000);
			player.Inventory.AddItem("Plank", 1000);
			player.Inventory.AddItem("MetalIngot", 1000);
			player.Inventory.AddItem("Nail", 1000);
		}
		else if (text.Contains("seedflower", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Seed_Flower_Black", 1000);
			player.Inventory.AddItem("Seed_Flower_White", 1000);
			player.Inventory.AddItem("Seed_Flower_Yellow", 1000);
			player.Inventory.AddItem("Seed_Flower_Blue", 1000);
			player.Inventory.AddItem("Seed_Flower_Red", 1000);
		}
		else if (text.Contains("flower", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Placeable_Flower_AloeVera", 100);
			player.Inventory.AddItem("Placeable_Flower_Monstera", 100);
			player.Inventory.AddItem("Placeable_Flower_Hibiscus", 100);
			player.Inventory.AddItem("Placeable_Flower_Cactus", 100);
			player.Inventory.AddItem("Placeable_Flower_Rose", 100);
			player.Inventory.AddItem("Placeable_Flower_Sunflower", 100);
			player.Inventory.AddItem("Placeable_TangaroaPlant_1", 100);
			player.Inventory.AddItem("Placeable_TangaroaPlant_2", 100);
			player.Inventory.AddItem("Placeable_TangaroaPlant_3", 100);
			player.Inventory.AddItem("Placeable_TangaroaPlant_4", 100);
			player.Inventory.AddItem("Placeable_TangaroaPlant_5", 100);
			player.Inventory.AddItem("Placeable_TangaroaPlant_6", 100);
		}
		else if (text.Contains("seed", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Seed_Banana", 500);
			player.Inventory.AddItem("Seed_Strawberry", 500);
			player.Inventory.AddItem("Seed_Birch", 500);
			player.Inventory.AddItem("Seed_Pine", 500);
			player.Inventory.AddItem("Seed_Palm", 500);
			player.Inventory.AddItem("Seed_Mango", 500);
			player.Inventory.AddItem("Seed_Watermelon", 500);
			player.Inventory.AddItem("Seed_Pineapple", 500);
		}
		else if (text.Contains("armor", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Equipment_LeatherChest", 1);
			player.Inventory.AddItem("Equipment_LeatherHelmet", 1);
			player.Inventory.AddItem("Equipment_LeatherLegs", 1);
			player.Inventory.AddItem("Backpack", 1);
		}
		else if (text.Contains("tiki", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Placeable_TikiPole_Feet", 1);
			player.Inventory.AddItem("Placeable_TikiPole_Mid_1", 1);
			player.Inventory.AddItem("Placeable_TikiPole_Mid_2", 1);
			player.Inventory.AddItem("Placeable_TikiPole_Top", 1);
			player.Inventory.AddItem("Placeable_TikiPole_Complete", 1);
		}
		else if (text.Contains("radio", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Placeable_Radio", 1);
			player.Inventory.AddItem("Cassette_EDM", 1);
			player.Inventory.AddItem("Cassette_Pop", 1);
			player.Inventory.AddItem("Cassette_Rock", 1);
			player.Inventory.AddItem("Cassette_Classical", 1);
			player.Inventory.AddItem("Cassette_Elevator", 1);
		}
		else if (text.Contains("cassette", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Cassette_EDM", 1);
			player.Inventory.AddItem("Cassette_Pop", 1);
			player.Inventory.AddItem("Cassette_Rock", 1);
			player.Inventory.AddItem("Cassette_Classical", 1);
			player.Inventory.AddItem("Cassette_Elevator", 1);
		}
		else if (text.Contains("hat", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Hat_Captain", 1);
			player.Inventory.AddItem("Hat_Construction", 1);
			player.Inventory.AddItem("Hat_Tiki", 1);
			player.Inventory.AddItem("Hat_Mayor", 1);
			player.Inventory.AddItem("Hat_Pilot", 1);
			player.Inventory.AddItem("Hat_Chef", 1);
			player.Inventory.AddItem("Hat_Glasses_Disguise", 1);
			player.Inventory.AddItem("Hat_Glasses_Aviator", 1);
			player.Inventory.AddItem("Hat_Sailor", 1);
			player.Inventory.AddItem("Hat_Fishing", 1);
			player.Inventory.AddItem("Hat_Diving", 1);
			player.Inventory.AddItem("Hat_Pirate", 1);
			player.Inventory.AddItem("HeadLight", 1);
			player.Inventory.AddItem("HeadLight_Advanced", 1);
			player.Inventory.AddItem("Equipment_LeatherHelmet", 1);
		}
		else if (text.Contains("head", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Head_Shark", 1);
			player.Inventory.AddItem("Head_Boar", 1);
			player.Inventory.AddItem("Head_Bear", 1);
			player.Inventory.AddItem("Head_MamaBear", 1);
			player.Inventory.AddItem("Head_PolarBear", 1);
			player.Inventory.AddItem("Head_PoisonPuffer", 1);
			player.Inventory.AddItem("Head_Screecher", 1);
			player.Inventory.AddItem("Head_Screecher_Caravan", 1);
			player.Inventory.AddItem("Head_AnglerFish", 1);
			player.Inventory.AddItem("Head_Hyena", 1);
			player.Inventory.AddItem("Head_HyenaBoss", 1);
		}
		else if (text.Contains("utopia", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Placeable_Utopia_Bottle", 5);
			player.Inventory.AddItem("Placeable_Utopia_DanglyDecoration_1", 5);
			player.Inventory.AddItem("Placeable_Utopia_DanglyDecoration_2", 5);
			player.Inventory.AddItem("Placeable_Utopia_DanglyDecoration_3", 5);
			player.Inventory.AddItem("Placeable_Utopia_DanglyDecoration_4", 5);
			player.Inventory.AddItem("Placeable_Utopia_DanglyDecoration_5", 5);
			player.Inventory.AddItem("Placeable_Utopia_DanglyDecoration_6", 5);
			player.Inventory.AddItem("Placeable_Utopia_Herb_1", 5);
			player.Inventory.AddItem("Placeable_Utopia_Herb_2", 5);
			player.Inventory.AddItem("Placeable_Utopia_Herb_3", 5);
			player.Inventory.AddItem("Placeable_Utopia_Herb_4", 5);
			player.Inventory.AddItem("Placeable_Utopia_Herb_5", 5);
			player.Inventory.AddItem("Placeable_Utopia_Herb_6", 5);
			player.Inventory.AddItem("Placeable_Utopia_LightBottleWhite", 5);
			player.Inventory.AddItem("Placeable_Utopia_LightBottleYellow", 5);
			player.Inventory.AddItem("Placeable_Utopia_Plant_1", 5);
			player.Inventory.AddItem("Placeable_Utopia_Plant_2", 5);
			player.Inventory.AddItem("Placeable_Utopia_Plant_3", 5);
		}
		else if (text.Contains("temperance", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Placeable_Temperance_Painting_1", 5);
			player.Inventory.AddItem("Placeable_Temperance_Painting_2", 5);
			player.Inventory.AddItem("Placeable_Temperance_Painting_3", 5);
			player.Inventory.AddItem("Placeable_Temperance_Plant_1", 5);
			player.Inventory.AddItem("Placeable_Temperance_Plant_2", 5);
			player.Inventory.AddItem("Placeable_Temperance_Plant_3", 5);
			player.Inventory.AddItem("Placeable_Temperance_Rug_1", 5);
			player.Inventory.AddItem("Placeable_Temperance_Rug_2", 5);
			player.Inventory.AddItem("Placeable_Temperance_Mug", 5);
			player.Inventory.AddItem("Placeable_Temperance_Cutlery", 5);
		}
		else if (text.Contains("recipebowl", StringComparison.OrdinalIgnoreCase))
		{
			SO_CookingTable_Recipe[] array = Resources.LoadAll<SO_CookingTable_Recipe>("SO_CookingRecipes");
			foreach (SO_CookingTable_Recipe sO_CookingTable_Recipe in array)
			{
				if (sO_CookingTable_Recipe.IsValid)
				{
					player.Inventory.AddItem(sO_CookingTable_Recipe.Result.UniqueName, 1);
				}
			}
		}
		else if (text.Contains("recipepaper", StringComparison.OrdinalIgnoreCase))
		{
			SO_CookingTable_Recipe[] array = Resources.LoadAll<SO_CookingTable_Recipe>("SO_CookingRecipes");
			foreach (SO_CookingTable_Recipe sO_CookingTable_Recipe2 in array)
			{
				if (sO_CookingTable_Recipe2.IsValid)
				{
					player.Inventory.AddItem("Placeable_Recipe_" + sO_CookingTable_Recipe2.name.Split('_').Last(), 1);
				}
			}
		}
		else if (text.Contains("foodraw", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem(ItemManager.GetItemByIndex(15).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(129).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(98).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(289).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(131).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(16).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(132).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(17).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(130).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(18).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(138).UniqueName, 5);
		}
		else if (text.Contains("foodcooked", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem(ItemManager.GetItemByIndex(11).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(135).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(99).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(288).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(136).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(12).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(134).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(13).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(133).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(14).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(137).UniqueName, 5);
		}
		else if (text.Contains("buff", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem(ItemManager.GetItemByIndex(534).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(509).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(508).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(504).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(533).UniqueName, 5);
		}
		else if (text.Contains("battery", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem(ItemManager.GetItemByIndex(176).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(535).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(293).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(259).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(322).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(173).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(366).UniqueName, 5);
		}
		else if (text.Contains("playtest", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem("Arrow_Metal", 40);
			player.Inventory.AddItem("Bow", 1);
			player.Inventory.AddItem("Machete", 1);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(14).UniqueName, 20);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(630).UniqueName, 1);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(630).UniqueName, 1);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(534).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(509).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(508).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(504).UniqueName, 5);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(533).UniqueName, 5);
			player.Inventory.AddItemAndEquip("HeadLight");
			player.Inventory.AddItemAndEquip("Equipment_LeatherChest");
			player.Inventory.AddItemAndEquip("Equipment_LeatherLegs");
			player.Inventory.AddItemAndEquip("Equipment_LeatherHelmet");
		}
		else if (text.Contains("fishing", StringComparison.OrdinalIgnoreCase))
		{
			player.Inventory.AddItem(ItemManager.GetItemByIndex(579).UniqueName, 20);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(580).UniqueName, 20);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(581).UniqueName, 20);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(33).UniqueName, 1);
			player.Inventory.AddItem(ItemManager.GetItemByIndex(169).UniqueName, 1);
		}
		else if (text.Contains("trophyfish", StringComparison.OrdinalIgnoreCase))
		{
			for (int j = 567; j <= 578; j++)
			{
				player.Inventory.AddItem(ItemManager.GetItemByIndex(j).UniqueName, 5);
			}
		}
		if (!text.Contains("blueprint", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		List<Item_Base> allItems = ItemManager.GetAllItems();
		for (int k = 0; k < allItems.Count; k++)
		{
			if (allItems[k].settings_recipe.IsBlueprint)
			{
				player.Inventory.AddItem(allItems[k].UniqueName, 1);
			}
		}
	}

	private static void Mark(string[] entries, Network_UserId p_steamID)
	{
		if (IsSteamIDLocalPlayer(p_steamID))
		{
			markLocation = player.transform.position;
			Vector3 vector = markLocation;
			ChatManager.SendDebugChatMessage("'Marking' Position : " + vector.ToString());
		}
	}

	private static void Recall(string[] entries, Network_UserId p_steamID)
	{
		if (IsSteamIDLocalPlayer(p_steamID))
		{
			player.transform.position = markLocation;
			Vector3 vector = markLocation;
			ChatManager.SendDebugChatMessage("'Recalling to' Position : " + vector.ToString());
		}
	}

	private static void WorldEvent(string[] entries, Network_UserId p_steamID)
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		foreach (SO_WorldEvent.WorldEventType value2 in Enum.GetValues(typeof(SO_WorldEvent.WorldEventType)))
		{
			if (!value2.ToString().Contains(entries[1], StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			WorldEventManager value = ComponentManager<WorldEventManager>.Value;
			WorldEvent worldEventFromType = WorldEventManager.GetWorldEventFromType(value2);
			bool flag = false;
			flag = true;
			if (worldEventFromType.eventData.IsEventStartable() | flag)
			{
				if (flag)
				{
					foreach (Restriction eventStartRestriction in worldEventFromType.eventData.eventStartRestrictions)
					{
						eventStartRestriction.IsRestricted(worldEventFromType);
					}
				}
				if (worldEventFromType.isActive)
				{
					ChatManager.SendDebugChatMessage("event via cheat is already active!: " + worldEventFromType.eventData.worldEventType.ToString() + " Forcing: " + flag);
					continue;
				}
				value.StartEvent(worldEventFromType);
				ChatManager.SendDebugChatMessage("Starting event via cheat: " + worldEventFromType.eventData.worldEventType.ToString() + " Forcing: " + flag);
			}
			else
			{
				ChatManager.SendDebugChatMessage("Unable to start event " + worldEventFromType.eventData.worldEventType.ToString() + " was restricted");
			}
		}
	}

	public static void ChangeMaterial(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		if (Helper.HitAtCursor(out var hit, 15f, 1 << LayerMask.NameToLayer("Ragdoll")))
		{
			Network_Entity_Redirect componentInParent = hit.collider.transform.GetComponentInParent<Network_Entity_Redirect>();
			if (!(componentInParent != null))
			{
				return;
			}
			Network_Entity entity = componentInParent.entity;
			if (!(entity != null))
			{
				return;
			}
			RareMaterial componentInChildren = entity.GetComponentInChildren<RareMaterial>();
			if (entries.Length > 2 && entries[2] != string.Empty)
			{
				if (uint.TryParse(entries[2], out var result))
				{
					componentInChildren.AssignSpecificMaterial(result);
				}
				else
				{
					componentInChildren.AssignRandomRareMaterial();
				}
			}
			else
			{
				componentInChildren.AssignRandomRareMaterial();
			}
			if (Raft_Network.IsHost)
			{
				network.RPC(new Message_RareMaterial(Messages.RareMaterial, network.NetworkIDManager, entity.gameObject.GetComponent<AI_NetworkBehaviour>().ObjectIndex, componentInChildren), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, new Message_RareMaterial(Messages.RareMaterial, network.NetworkIDManager, entity.gameObject.GetComponent<AI_NetworkBehaviour>().ObjectIndex, componentInChildren), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			ChatManager.LocalDebugChatMessage("Unable to find valid target to change material on, are you aiming at one?");
		}
	}

	private static void WorldEventStop(string[] entries, Network_UserId p_steamID)
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		foreach (SO_WorldEvent.WorldEventType value2 in Enum.GetValues(typeof(SO_WorldEvent.WorldEventType)))
		{
			if (value2.ToString().Contains(entries[1], StringComparison.OrdinalIgnoreCase))
			{
				WorldEventManager value = ComponentManager<WorldEventManager>.Value;
				WorldEvent worldEventFromType = WorldEventManager.GetWorldEventFromType(value2);
				ChatManager.SendDebugChatMessage("Stopping event via cheat:" + worldEventFromType.eventData.worldEventType);
				value.StopEvent(worldEventFromType);
			}
		}
	}

	private static void HandleHazmatSuit(string[] entries, Network_UserId p_steamID)
	{
		if (IsSteamIDLocalPlayer(p_steamID))
		{
			if (entries[1].Contains("true", StringComparison.OrdinalIgnoreCase))
			{
				player.HazmatSuit.EquipNetworked();
			}
			else if (entries[1].Contains("false", StringComparison.OrdinalIgnoreCase))
			{
				player.HazmatSuit.UnEquipNetworked();
			}
		}
	}

	private static void HandleOldWater(string[] entries, Network_UserId p_steamID)
	{
		if (IsSteamIDLocalPlayer(p_steamID))
		{
			UseOldWater = !UseOldWater;
			ChatManager.SendDebugChatMessage($"UseOldWater = {UseOldWater}");
		}
	}

	private static void Honk(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		List<Block> placedBlocks = BlockCreator.GetPlacedBlocks();
		for (int i = 0; i < placedBlocks.Count; i++)
		{
			if (!(placedBlocks[i] == null) && placedBlocks[i].GetComponentInChildren<Placeable_Interactable_HonkHorn>() != null)
			{
				InteractableButton_Networked_Placeable componentInChildren = placedBlocks[i].GetComponentInChildren<InteractableButton_Networked_Placeable>();
				if (componentInChildren != null)
				{
					componentInChildren.ButtonPress_Fake();
				}
			}
		}
	}

	private static void DestroyBlocks(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID) || entries[1] == string.Empty)
		{
			return;
		}
		Network_Player value = ComponentManager<Network_Player>.Value;
		List<Block> placedBlocks = BlockCreator.GetPlacedBlocks();
		uint result = 1u;
		uint.TryParse(entries[1], out result);
		for (int i = 0; i < placedBlocks.Count; i++)
		{
			if (!(placedBlocks[i] == null) && Vector3.Distance(value.FeetPosition, placedBlocks[i].transform.position) <= (float)result)
			{
				BlockCreator.RemoveBlock(placedBlocks[i], value, updateRaftBounds: true);
			}
		}
	}

	private static void HandleBakingBlocks(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		BlockCollisionConsolidator blockCollisionConsolidator = ComponentManager<global::Raft>.Value.blockCollisionConsolidator;
		if (entries[1].Contains("true", StringComparison.OrdinalIgnoreCase))
		{
			if (!blockCollisionConsolidator.enabled)
			{
				blockCollisionConsolidator.enabled = true;
				ChatManager.SendDebugChatMessage("Using meshcollider = true");
			}
		}
		else if (entries[1].Contains("false", StringComparison.OrdinalIgnoreCase) && blockCollisionConsolidator.enabled)
		{
			blockCollisionConsolidator.enabled = false;
			ChatManager.SendDebugChatMessage("Using meshcollider = false");
		}
	}

	private static void PrintCheat(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		if (entries[1].Contains("worldevent", StringComparison.OrdinalIgnoreCase))
		{
			string text = "Active Events Are: ";
			WorldEvent[] worldEvents = WorldEventManager.worldEvents;
			foreach (WorldEvent worldEvent in worldEvents)
			{
				if (worldEvent.isActive)
				{
					text = text + worldEvent.eventData.worldEventType.ToString() + " / ";
				}
			}
			ChatManager.SendDebugChatMessage(text);
		}
		else if ("gametime".Contains(entries[1], StringComparison.OrdinalIgnoreCase))
		{
			ChatManager.SendDebugChatMessage("GameTime = " + GameManager.TimePlayed.ToString("F1"));
		}
	}

	private static void Simulate(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		string text = entries[1];
		if (text.Contains("pickup"))
		{
			float radius = float.Parse(entries[2]);
			RaycastHit[] array = Physics.SphereCastAll(player.transform.position, radius, Vector3.up, float.PositiveInfinity, LayerMasks.MASK_Item, QueryTriggerInteraction.Collide);
			foreach (RaycastHit raycastHit in array)
			{
				PickupItem component = raycastHit.transform.gameObject.GetComponent<PickupItem>();
				if (component != null)
				{
					player.PickupScript.PickupItemByType(component, forcePickup: true);
				}
			}
		}
		else if (text.Contains("motorweight"))
		{
			ChatManager.SendDebugChatMessage("MotorStenght : " + RaftVelocityManager.motorStrength + " Raft Weight : " + RaftWeightManager.FoundationWeight + "Raft Anchors :" + raft.AnchorCount);
		}
		else
		{
			if (!"interact".Contains(text, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			RaycastInteractable raycastInteractable = Helper.FindInteractable(1000f);
			if (!(raycastInteractable != null))
			{
				return;
			}
			QuestInteractable componentInChildren = raycastInteractable.GetComponentInChildren<QuestInteractable>();
			if (componentInChildren != null)
			{
				bool flag = entries.Length >= 3 && "force".Contains(entries[2], StringComparison.OrdinalIgnoreCase);
				componentInChildren.SendOnInteractNetwork(player, flag);
				if (flag)
				{
					ChatManager.SendDebugChatMessage("Forced interaction with: " + componentInChildren.name);
				}
				else
				{
					ChatManager.SendDebugChatMessage("Interacted with: " + componentInChildren.name);
				}
			}
		}
	}

	private static void Reflections(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID))
		{
			return;
		}
		string text = string.Empty;
		if (entries.Length >= 2)
		{
			text = entries[1];
		}
		ReflectionProbeUpdater componentInChildren = player.GetComponentInChildren<ReflectionProbeUpdater>(includeInactive: true);
		if (text == string.Empty)
		{
			ForceReflectionsOff = !ForceReflectionsOff;
			ChatManager.LocalDebugChatMessage("Reflections = " + (ForceReflectionsOff ? "Off" : "On"));
		}
		else if (text.Contains("on", StringComparison.OrdinalIgnoreCase))
		{
			ForceReflectionsOff = false;
			ChatManager.LocalDebugChatMessage("Reflections = On");
		}
		else if (text.Contains("off", StringComparison.OrdinalIgnoreCase))
		{
			ForceReflectionsOff = true;
			ChatManager.LocalDebugChatMessage("Reflections = Off");
		}
		else if (text.Contains("follow", StringComparison.OrdinalIgnoreCase))
		{
			if (componentInChildren != null)
			{
				componentInChildren.reflectionSphere.transform.position = player.transform.position + player.transform.forward;
				if (componentInChildren.reflectionSphere.transform.parent == componentInChildren.transform)
				{
					componentInChildren.reflectionSphere.transform.SetParentSafe(player.transform.parent);
				}
				else
				{
					componentInChildren.reflectionSphere.transform.SetParentSafe(componentInChildren.transform);
				}
				ChatManager.LocalDebugChatMessage("Reflection sphere follow = " + ((componentInChildren.reflectionSphere.transform.parent == componentInChildren.transform) ? "On" : "Off"));
			}
		}
		else if ("sphere".Contains(text, StringComparison.OrdinalIgnoreCase) && componentInChildren != null)
		{
			componentInChildren.reflectionSphere.gameObject.SetActiveSafe(!componentInChildren.reflectionSphere.gameObject.activeSelf);
			if (componentInChildren.reflectionSphere.gameObject.activeSelf)
			{
				componentInChildren.reflectionSphere.transform.position = player.transform.position + player.transform.forward;
				componentInChildren.reflectionSphere.transform.SetParentSafe(componentInChildren.transform);
			}
			ChatManager.LocalDebugChatMessage("Reflection sphere = " + (componentInChildren.reflectionSphere.gameObject.activeSelf ? "On" : "Off"));
		}
	}

	private static void EnvironmentLight(string[] entries, Network_UserId p_steamID)
	{
		if (IsSteamIDLocalPlayer(p_steamID))
		{
			_ = string.Empty;
			if (entries.Length >= 2)
			{
				_ = entries[1];
			}
			PlayerEnvironmentLightManager componentInChildren = player.GetComponentInChildren<PlayerEnvironmentLightManager>(includeInactive: true);
			componentInChildren.pointLight.gameObject.SetActiveSafe(!componentInChildren.pointLight.gameObject.activeSelf);
			ChatManager.LocalDebugChatMessage("Environment light = " + (componentInChildren.pointLight.gameObject.activeSelf ? "On" : "Off"));
		}
	}

	private static void Reciever(string[] entries, Network_UserId p_steamID)
	{
		if (entries[1].Contains("force"))
		{
			ForceRecieverToggle = !ForceRecieverToggle;
			ChatManager.LocalDebugChatMessage("Setting Reciever Mode-Forced:" + ForceRecieverToggle);
		}
	}

	private static void Build(string[] entries, Network_UserId p_steamID)
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		List<Block> placedBlocks = BlockCreator.GetPlacedBlocks();
		string obj = entries[1];
		int num = 1;
		if (entries.Length >= 3)
		{
			string text = entries[2];
			if (!string.IsNullOrEmpty(text) && text.All(char.IsDigit))
			{
				num = int.Parse(text);
			}
		}
		if (!obj.Contains("foundation"))
		{
			return;
		}
		Item_Base itemByIndex = ItemManager.GetItemByIndex(2);
		Vector3 vector = new Vector3(-0.75f, 0f, -0.75f);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Vector3 position = vector + new Vector3((float)i * 1.5f, 0f, (float)j * 1.5f);
				BlockCreator.SnapBuildPosition(ref position);
				if (!PlacedBlockContainsPosition(position))
				{
					player.BlockCreator.CreateBlockCheat(itemByIndex, position, Vector3.zero, DPS.Default, -1);
				}
			}
		}
		bool PlacedBlockContainsPosition(Vector3 localPosition)
		{
			for (int k = 0; k < placedBlocks.Count; k++)
			{
				if (placedBlocks[k] != null && Vector3.Distance(placedBlocks[k].transform.localPosition, localPosition) < 0.05f)
				{
					return true;
				}
			}
			return false;
		}
	}

	private static void Reset(string[] entries, Network_UserId p_steamID)
	{
		bool flag = IsSteamIDLocalPlayer(p_steamID);
		bool flag2 = (entries.Length == 4 && entries[3] == "all") || (entries.Length == 3 && entries[2] == "all");
		string value = entries[1];
		if (flag && "achievements".Contains(value, StringComparison.OrdinalIgnoreCase))
		{
			AchievementHandler.ResetAchievements();
			ChatManager.LocalDebugChatMessage("Achievements has been reset");
		}
		else if (flag && "interactables".Contains(value, StringComparison.OrdinalIgnoreCase))
		{
			QuestEventBase[] array = UnityEngine.Object.FindObjectsOfType<QuestEventBase>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ResetFromCheat();
			}
			ChatManager.SendDebugChatMessage("Resetting interactables");
		}
		else if ("characters".Contains(value, StringComparison.OrdinalIgnoreCase) && (flag || (!flag & flag2)))
		{
			int amountFromEntries = GetAmountFromEntries(entries, -1);
			if (amountFromEntries != -1)
			{
				SO_Character characterFromIndex = CharacterManager.GetCharacterFromIndex(amountFromEntries);
				if (characterFromIndex != null && CharacterManager.ResetCharacterUnlock(amountFromEntries))
				{
					ChatManager.SendDebugChatMessage("Reseting character: " + characterFromIndex.displayName);
				}
			}
			else
			{
				CharacterManager.ResetAllCharacterUnlocks();
				ChatManager.SendDebugChatMessage("Resetting all character unlocks");
			}
		}
		else if ("buff".Contains(value, StringComparison.OrdinalIgnoreCase) & flag)
		{
			player.Stats.buffManager.ClearBuffs();
			player.HazmatSuit.UnEquipNetworked();
			ChatManager.SendDebugChatMessage("Removing all buffs");
		}
		else if ("timescale".Contains(value, StringComparison.OrdinalIgnoreCase) & flag)
		{
			SetTargetTimeScale(1f);
			ChatManager.SendDebugChatMessage($"Target timescale = {TargetTimeScale}");
		}
	}

	private static void Nuke()
	{
		Network_Entity[] array = UnityEngine.Object.FindObjectsOfType<Network_Entity>();
		if (array.Length != 0)
		{
			Network_Entity[] array2 = array;
			foreach (Network_Entity network_Entity in array2)
			{
				ComponentManager<Network_Host>.Value.DamageEntity(network_Entity, network_Entity.transform, 9999f, network_Entity.transform.position, Vector3.up, EntityType.Player);
			}
		}
	}

	private static void Teleport(string[] entries, Network_UserId p_steamID)
	{
		string text = entries[1];
		Vector3 vector = Vector3.zero;
		bool flag = false;
		if (IsSteamIDLocalPlayer(p_steamID) && text.Contains("raft", StringComparison.OrdinalIgnoreCase))
		{
			GameObject gameObject = GameObject.FindWithTag("RespawnPointBed");
			if (gameObject != null)
			{
				vector = gameObject.transform.position;
				flag = true;
			}
		}
		else if (!IsSteamIDLocalPlayer(p_steamID) || !text.Contains("selection", StringComparison.OrdinalIgnoreCase))
		{
			if (IsSteamIDLocalPlayer(p_steamID) && text.Contains("treasure", StringComparison.OrdinalIgnoreCase))
			{
				TreasurePoint closestTreasurePointTo = treasurePointManager.GetClosestTreasurePointTo(player.FeetPosition, 9999f, includeUnBuried: false, includeUnrevealed: true, out var _);
				ChatTextFieldController value = ComponentManager<ChatTextFieldController>.Value;
				if (closestTreasurePointTo != null)
				{
					player.transform.position = closestTreasurePointTo.transform.position + Vector3.up * 3f;
					value.AddUITextMessage("Teleporting to closest treasure", p_steamID);
				}
				else
				{
					value.AddUITextMessage("Unable To find treasurePoint via cheat", p_steamID);
				}
			}
			else if (IsSteamIDLocalPlayer(p_steamID) && entries.Length == 2)
			{
				Network_Player playerFromName = GetPlayerFromName(text);
				if (playerFromName != null)
				{
					vector = playerFromName.transform.position;
					flag = playerFromName.transform.ParentedToRaft();
				}
				else
				{
					foreach (ObjectOfInterestType value2 in Enum.GetValues(typeof(ObjectOfInterestType)))
					{
						if (!value2.ToString().Contains(text, StringComparison.OrdinalIgnoreCase))
						{
							continue;
						}
						Transform objectOfInterest = ObjectManager.GetObjectOfInterest(value2);
						if (objectOfInterest != null)
						{
							if (objectOfInterest.gameObject.activeInHierarchy)
							{
								vector = objectOfInterest.position;
								flag = false;
							}
							else
							{
								ChatManager.SendDebugChatMessage("Location " + value2.ToString() + " is not active in hierarchy");
							}
						}
						break;
					}
				}
			}
			else if (entries.Length == 3)
			{
				string obj = entries[2];
				Network_Player playerFromName2 = GetPlayerFromName(text);
				Network_Player playerFromName3 = GetPlayerFromName(obj);
				if (playerFromName2 != null && playerFromName3 != null)
				{
					playerFromName2.transform.SetParentSafe(playerFromName2.transform.ParentedToRaft() ? SingletonGeneric<GameManager>.Singleton.lockedPivot : null);
					playerFromName2.PersonController.SwitchControllerType(ControllerType.Ground);
					playerFromName2.transform.position = playerFromName3.transform.position;
				}
				return;
			}
		}
		if (vector != Vector3.zero)
		{
			player.transform.SetParentSafe(flag ? SingletonGeneric<GameManager>.Singleton.lockedPivot : null);
			player.PersonController.SwitchControllerType(ControllerType.Ground);
			player.transform.position = vector;
		}
		Physics.SyncTransforms();
	}

	private static void SetBlockHealth(float amount, Network_UserId p_steamID)
	{
		if (IsSteamIDLocalPlayer(p_steamID) && Helper.HitAtCursor(out var hit, Player.UseDistance, LayerMasks.MASK_Block))
		{
			Block componentInParent = hit.transform.GetComponentInParent<Block>();
			if (componentInParent != null)
			{
				componentInParent.SetHealth((int)((float)componentInParent.MaxHealth * amount));
				BlockCreator.UpdateHealthNetwork(componentInParent);
			}
		}
	}

	private static void CookingTable(string[] entries, Network_UserId p_steamID)
	{
		if (!IsSteamIDLocalPlayer(p_steamID) || entries.Length <= 1 || !Helper.HitAtCursor(out var hit, Player.UseDistance, LayerMasks.MASK_Block))
		{
			return;
		}
		CookingTable componentInParent = hit.transform.GetComponentInParent<CookingTable>();
		if (!(componentInParent != null))
		{
			return;
		}
		CookingTable_Slot[] componentsInChildren = componentInParent.GetComponentsInChildren<CookingTable_Slot>(includeInactive: true);
		if (componentsInChildren == null)
		{
			return;
		}
		SO_CookingTable_Recipe[] array = Resources.LoadAll<SO_CookingTable_Recipe>("SO_CookingRecipes");
		if (array == null || !array.ContainsItems())
		{
			return;
		}
		SO_CookingTable_Recipe[] array2 = array;
		foreach (SO_CookingTable_Recipe sO_CookingTable_Recipe in array2)
		{
			if (!sO_CookingTable_Recipe.IsValid || sO_CookingTable_Recipe.RecipeType != componentInParent.cookingType || !sO_CookingTable_Recipe.Result.UniqueName.Contains(entries[1], StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			CostMultiple[] recipeCost = sO_CookingTable_Recipe.RecipeCost;
			int num = 0;
			for (int j = 0; j < recipeCost.Length; j++)
			{
				CookingTable_Slot cookingTable_Slot = null;
				CostMultiple costMultiple = recipeCost[j];
				for (int k = 0; k < costMultiple.amount; k++)
				{
					cookingTable_Slot = componentsInChildren[num];
					if (cookingTable_Slot.CurrentItem != null)
					{
						cookingTable_Slot.OnPickupItem(null, cookingTable_Slot, cookingTable_Slot.CurrentItem);
					}
					cookingTable_Slot.OnInsertItem(null, cookingTable_Slot, new ItemInstance(costMultiple.items[0], 1, costMultiple.items[0].MaxUses));
					num++;
				}
			}
			break;
		}
	}

	private static void WorldShift(Network_UserId p_steamID)
	{
		if (Raft_Network.IsHost)
		{
			WorldShiftManager worldShiftManager = UnityEngine.Object.FindObjectOfType<WorldShiftManager>();
			if (worldShiftManager != null)
			{
				worldShiftManager.ResetToCenter();
			}
		}
	}

	private static int GetAmountFromEntries(string[] entries, int defaultAmount = 1)
	{
		if (entries.Length >= 3)
		{
			string text = entries[2];
			if (!string.IsNullOrEmpty(text) && text.All(char.IsDigit))
			{
				defaultAmount = int.Parse(text);
			}
		}
		return defaultAmount;
	}

	private static bool IsSteamIDLocalPlayer(Network_UserId p_steamID)
	{
		if (player.steamID == p_steamID)
		{
			return true;
		}
		return false;
	}

	private static Network_Player GetPlayerFromName(string name)
	{
		if (network != null && network.remoteUsers != null)
		{
			foreach (Network_Player value in network.remoteUsers.Values)
			{
				if (value.characterSettings.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
				{
					return value;
				}
			}
		}
		return null;
	}
}
