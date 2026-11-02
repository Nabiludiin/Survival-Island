using System;
using System.Collections.Generic;
using UltimateWater;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

public class GameManager : SingletonGeneric<GameManager>, SceneEventInterface
{
	public static bool IsLeavingGame = false;

	public static bool IsQuittingGame = false;

	public static bool IsQuittingApplication = false;

	public static string Password = null;

	public static float OceanBottom = -40f;

	public static GameMode GameMode = GameMode.Normal;

	public static bool FriendlyFire = false;

	public static bool IsInNewGame = false;

	public static bool QuickStartGame = false;

	public static float TimePlayed = 0f;

	public static int targetFrameRate = 60;

	public static bool Crossplay = true;

	public AnalyticsEvent trackOnLaunchgameEvent;

	public Texture2D cursorTexture;

	public Transform raftColliderParent;

	public Water water;

	public FadePanel sleepFadePanel;

	public Buoyancy buoyancy;

	public Shader blockPaintShader;

	public Color shaderGreen = Color.black;

	public Color shaderRed = Color.black;

	public Material ghostMaterialRed;

	public Material ghostMaterialGreen;

	public Transform lockedPivot;

	public Transform buoyancyPivot;

	public Transform raftFollowParent;

	public Transform raftTransform;

	public static bool HasPassword => !string.IsNullOrEmpty(Password);

	private void OnEnable()
	{
		Application.quitting += OnApplicationQuit;
	}

	private void OnDisable()
	{
		Application.quitting -= OnApplicationQuit;
	}

	private void Awake()
	{
		TimePlayed = 0f;
		IsQuittingGame = false;
		if (!IsLeavingGame)
		{
			Raft_Network.OnWorldReceived = (Action)Delegate.Combine(Raft_Network.OnWorldReceived, new Action(OnWorldRecieved));
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
		}
		ItemManager.Initialize();
		RecieverFrequency.InitializeAllFrequencies();
		QuestProgressTracker.AwakeCall();
		Inventory.Parent = GameObject.FindWithTag("InventoryParent").transform;
		ComponentManager<AzureSkyController>.Value = UnityEngine.Object.FindObjectOfType<AzureSkyController>();
		ComponentManager<Inventory_ResearchTable>.Value = GameObject.FindWithTag("InventoryParent").GetComponentInChildren<Inventory_ResearchTable>(includeInactive: true);
	}

	private void Start()
	{
		Helper.Start();
		QuestProgressTracker.StartCall();
		Invoke("LateStart", 0.2f);
	}

	private void LateStart()
	{
		ComponentManager<Raft_Network>.Value.gameManagerStartCalled = true;
	}

	private void Update()
	{
		AchievementHandler.Update();
		Helper.Update();
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
		if (Application.targetFrameRate != targetFrameRate)
		{
			Application.targetFrameRate = targetFrameRate;
		}
		TimePlayed += Time.deltaTime;
	}

	private void OnApplicationQuit()
	{
		IsQuittingGame = true;
		IsQuittingApplication = true;
	}

	public void OnWorldRecieved()
	{
		CanvasHelper value = ComponentManager<CanvasHelper>.Value;
		value.OpenMenuCloseOther(MenuType.Inventory);
		value.CloseMenu(MenuType.Inventory);
		Raft_Network value2 = ComponentManager<Raft_Network>.Value;
		Network_Player network_Player = ComponentManager<Network_Player>.Value;
		SaveAndLoad value3 = ComponentManager<SaveAndLoad>.Value;
		ChunkManager value4 = ComponentManager<ChunkManager>.Value;
		if (network_Player == null)
		{
			Helper.LogBuild("GameManager: LocalPlayer is null at DelayedStart");
			Helper.LogBuild("GameManager: Attempting to find players in scene (" + SceneManager.GetActiveScene().name + ")");
			Network_Player[] array = UnityEngine.Object.FindObjectsOfType<Network_Player>();
			if (array != null)
			{
				Helper.LogBuild("GameManager: Found " + array.Length + " number of players in scene");
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null && array[i].IsLocalPlayer)
					{
						network_Player = array[i];
						Helper.LogBuild("GameManager: Sucess! Found local player in scene");
						break;
					}
				}
				if (network_Player == null)
				{
					Helper.LogBuild("GameManager: Failed! Did not find local player in scene");
				}
			}
			else
			{
				Helper.LogBuild("GameManager: Failed! No players were found in scene");
			}
			if (network_Player == null)
			{
				Helper.LogBuild("GameManager: Attempting to find local player via Networkmanager");
				if (value2 == null)
				{
					Helper.LogBuild("GameManager: Can't find search via Networkmanager because it is null");
				}
				else
				{
					Helper.LogBuild("GameManager: Networkmanager has " + value2.remoteUsers.Count + " player references");
					network_Player = value2.GetLocalPlayer();
					if (network_Player == null)
					{
						Helper.LogBuild("GameManager: Failed! Did not find local player in Networkmanager");
					}
					else
					{
						Helper.LogBuild("GameManager: Success! Found local player in Networkmanager");
					}
				}
			}
		}
		else
		{
			Helper.LogBuild("GameManager: Managed to find local player on first try!");
		}
		if (Raft_Network.IsHost)
		{
			if (IsInNewGame || QuickStartGame)
			{
				network_Player.BlockCreator.CreateNewGameLayout();
				if (value4 != null)
				{
					value4.GenerateChunkDataStartOfGame();
				}
			}
			if (IsInNewGame)
			{
				Debug.Log("Creating world");
				SaveAndLoad.WorldGuid = Guid.NewGuid();
				SaveAndLoad.WorldIndex = new WorldIndex(8);
				network_Player.Inventory.GiveStartingItems();
				value3.ClearLandmarkCache();
				value3.SaveGame(sendRemoteMessage: false);
			}
			else if (!QuickStartGame)
			{
				Debug.Log("Restoring world");
				value3.RestoreRGDGame(SaveAndLoad.WorldToLoad);
				value3.CreateLandmarkCache();
				if (value3.PlayerSaveExistsInWorld(network_Player))
				{
					value3.RestoreUser();
					return;
				}
				network_Player.SetToValidSpawnPoint();
				network_Player.Inventory.GiveStartingItems();
				value3.SaveGame(sendRemoteMessage: false);
			}
		}
		else if (value3.PlayerSaveExistsInWorld(network_Player))
		{
			value3.RestoreUser();
		}
		else
		{
			network_Player.SetToValidSpawnPoint();
			network_Player.Inventory.GiveStartingItems();
			value3.SaveGame(sendRemoteMessage: false);
		}
	}

	public void OnWorldRecievedLate()
	{
		if (IsInNewGame || QuickStartGame)
		{
			SO_GameModeValue currentGameModeValue = GameModeValueManager.GetCurrentGameModeValue();
			if (currentGameModeValue != null)
			{
				if (currentGameModeValue.gameMode == GameMode.Creative)
				{
					ObjectSpawnerManager value = ComponentManager<ObjectSpawnerManager>.Value;
					if (value != null)
					{
						value.ClearAllSpawners(clearUsingNetwork: false);
					}
					else
					{
						Debug.Log("Can't clear spawners in Creative mode, ObjectSpawnerManager is null");
					}
				}
				else if (Raft_Network.IsHost && currentGameModeValue.raftSpecificVariables.prewarmFloatingItems)
				{
					ObjectSpawnerManager value2 = ComponentManager<ObjectSpawnerManager>.Value;
					if (value2 != null)
					{
						value2.PrewarmStart();
					}
					else
					{
						Debug.Log("Can't start prewarm coroutine, ObjectSpawnerManager is null");
					}
				}
			}
			else
			{
				Debug.Log("GameManager.WorldRecievedLate()    -    GameModeValue = null");
			}
		}
		if (Raft_Network.IsHost && QuickStartGame && IsInNewGame)
		{
			Landmark[] array = UnityEngine.Object.FindObjectsOfType<Landmark>();
			if (array.ContainsItems())
			{
				Landmark[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].ResetPickups();
				}
			}
		}
		SO_GameModeValue currentGameModeValue2 = GameModeValueManager.GetCurrentGameModeValue();
		if (currentGameModeValue2 != null)
		{
			AnalyticsHelper.CustomEventString(trackOnLaunchgameEvent, new Dictionary<string, object> { { "Difficulty", currentGameModeValue2.gameMode } });
		}
		else
		{
			Debug.Log("GameModeSo was null, unable to send analytics");
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		switch (sceneEvent)
		{
		case SceneEvent.LeaveGame:
			IsQuittingGame = true;
			Helper.OnLeaveGame();
			QuestProgressTracker.Reset();
			Dialogue.OnVoiceDataTracked = (Dialogue.OnVoiceDataUnTracked = null);
			Dialogue.ActiveTrackedDialogues.Clear();
			PlayerItemManager.IsBusy = false;
			break;
		case SceneEvent.JoinGame:
			IsQuittingGame = false;
			break;
		}
	}
}
