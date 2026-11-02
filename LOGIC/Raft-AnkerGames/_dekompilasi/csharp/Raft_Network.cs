using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFab.Party;
using PlayFab.ProfilesModels;
using Steamworks;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Raft_Network : MonoBehaviour, SceneEventInterface
{
	public delegate void JoinResult(CSteamID remoteID, InitiateResult result);

	public delegate void PlayerAction(Network_Player playerNetwork);

	public delegate void OnDisconnectCallback(Network_UserId steamID, DisconnectReason disconnectReason);

	public static Action OnWorldReceived;

	public static Action OnWorldReceivedLate;

	public static RequestJoinAuthSetting CurrentRequestJoinAuthSetting;

	public float connectTimeoutTime;

	[HideInInspector]
	public Network_Host hostNetwork;

	[HideInInspector]
	public bool gameManagerStartCalled;

	public Dictionary<Network_UserId, Network_Player> remoteUsers = new Dictionary<Network_UserId, Network_Player>();

	public Network_Player playerPrefab;

	public static string STATUS_Hosting = "Hosting game";

	public static string STATUS_Hosting_Password = STATUS_Hosting + " protected";

	public static string STATUS_RemoteClient = "In game as client";

	public static string CONNECT_DELIMITER = "-connect";

	public static string PASSWORD_DELIMITER = ":PW";

	public static string PresenceKeyConnect = "connect";

	public static string PresenceKeyStatus = "status";

	public static string PresenceKeyGameInfo = "gameInfo";

	public static bool InMenuScene = true;

	public static bool IsAllLandmarksLoaded = false;

	public static bool WorldHasBeenRecieved = false;

	public static string GameSceneName = "MainScene";

	public static string MenuSceneName = "MainMenuScene";

	private NetworkIDManager networkIDManager;

	private Network_Player localPlayer;

	private Settings settings;

	private LoadSceneManager loadSceneManager;

	private GameObject bedRespawnPoint;

	private Coroutine worldLoadedCoroutine;

	private Coroutine worldRequestedCoroutine;

	private static bool isHost;

	private Network_UserId hostID;

	private Network_UserId localSteamID = 1uL;

	private bool connectedToHost;

	private float ping;

	private float totalBytesSent;

	private float lastSecondBytes;

	private float bytesSentPerSecond;

	private float maxBytePackage;

	private Scene startScene;

	private static bool initialized;

	private Callback<UserStatsReceived_t> OnUserStatsRecievedCallback;

	public const int MaxPlayers = 10;

	public const int Version = 1;

	public static bool SignedIntoPlayfab = false;

	private Dictionary<Network_UserId, PlayerInfo> playerInfos = new Dictionary<Network_UserId, PlayerInfo>();

	private Dictionary<Network_UserId, string> blockedUserIds = new Dictionary<Network_UserId, string>();

	private HashSet<Network_UserId> reportedUserIds = new HashSet<Network_UserId>();

	private int reportsAvailable = 1;

	private string currentNetworkId = "";

	private string worldCode = "";

	private VoiceOverlay voiceOverlay;

	private bool voiceDisabled;

	private string errorMessage = "";

	public Action OnPlayerJoinOrLeaves;

	public Action<CrossplayJoinErrorCode> OnCrossplayJoinComplete;

	public ProfanityFilter ProfanityFilter = new ProfanityFilter();

	private Callback<GameRichPresenceJoinRequested_t> m_gameRichPressenceJoined;

	private Callback<P2PSessionRequest_t> m_sessionRequest;

	private CSteamID m_inviteFrom;

	private string m_inviteFromPass;

	private bool m_receivedInvite;

	private bool m_hostedPlayFab;

	private CSteamID m_currentSteamHost;

	public static bool InSinglePlayerMode
	{
		get
		{
			if (CurrentRequestJoinAuthSetting == RequestJoinAuthSetting.ALLOW_NONE)
			{
				return IsHost;
			}
			return false;
		}
	}

	public static bool IsHost => isHost;

	public bool IsConnectedToHost
	{
		get
		{
			if (HostID.IsValid())
			{
				return connectedToHost;
			}
			return false;
		}
	}

	public float Ping => ping * 1000f;

	public float BytesSentPerSecond => bytesSentPerSecond;

	public float TotalKiloBytesSentPerSecond => bytesSentPerSecond / 1000f;

	public float TotalBytesSent => totalBytesSent;

	public float MaxBytePackage => maxBytePackage;

	public Network_UserId HostID => hostID;

	public Network_UserId LocalSteamID => localSteamID;

	public int PlayerCount => remoteUsers.Count;

	public NetworkIDManager NetworkIDManager
	{
		get
		{
			if (networkIDManager == null)
			{
				networkIDManager = ComponentManager<NetworkIDManager>.Value;
				if (networkIDManager == null)
				{
					networkIDManager = UnityEngine.Object.FindObjectOfType<NetworkIDManager>();
				}
			}
			return networkIDManager;
		}
	}

	public string WorldCode => worldCode;

	public bool VoiceDisabled => ComponentManager<Settings>.Value.audioBox.VoicechatDisabled;

	public string NetworkId => currentNetworkId;

	public Dictionary<Network_UserId, string> BlockedUserIds => blockedUserIds;

	public CSteamID CurrentSteamHost => m_currentSteamHost;

	public static event JoinResult OnJoinResult;

	public static event PlayerAction OnPlayerInitialized;

	public static event OnDisconnectCallback OnDisconnectEvent;

	private void Awake()
	{
		if (initialized)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		initialized = true;
		isHost = true;
	}

	private void OnEnable()
	{
		ComponentManager<Raft_Network>.Value = this;
		SceneManager.sceneLoaded += OnSceneLoaded;
		loadSceneManager = GetComponent<LoadSceneManager>();
		bedRespawnPoint = GameObject.FindWithTag("RespawnPointBed");
		if (string.IsNullOrEmpty(startScene.name))
		{
			startScene = SceneManager.GetActiveScene();
		}
		DontDestroyOnLoadManager.AddGameObject(gameObject);
		Platform_Init();
	}

	private void Start()
	{
		settings = ComponentManager<Settings>.Value;
		SteamUserStats.RequestCurrentStats();
		if (GameManager.QuickStartGame)
		{
			ComponentManager<SceneLoader>.Value.LoadScenes();
		}
	}

	private void Update()
	{
		Platform_Update();
	}

	private void OnApplicationQuit()
	{
		Platform_Shutdown();
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		switch (sceneEvent)
		{
		case SceneEvent.JoinGame:
			AchievementHandler.OnJoinGame();
			break;
		case SceneEvent.LeaveGame:
			WorldHasBeenRecieved = false;
			OnJoinResult = null;
			OnPlayerInitialized = null;
			OnDisconnectEvent = null;
			OnWorldReceived = null;
			OnWorldReceivedLate = null;
			CurrentRequestJoinAuthSetting = RequestJoinAuthSetting.ALLOW_NONE;
			SteamFriends.ClearRichPresence();
			AchievementHandler.OnLeaveGame();
			GameManager.Password = string.Empty;
			gameManagerStartCalled = false;
			if (worldLoadedCoroutine != null)
			{
				StopCoroutine(worldLoadedCoroutine);
				worldLoadedCoroutine = null;
			}
			if (worldRequestedCoroutine != null)
			{
				StopCoroutine(worldRequestedCoroutine);
				worldRequestedCoroutine = null;
			}
			Platform_LeaveGame();
			if (ComponentManager<Settings>.Value.IsOpen)
			{
				ComponentManager<Settings>.Value.Close();
			}
			break;
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
	{
		if (settings == null)
		{
			settings = ComponentManager<Settings>.Value;
		}
		if (scene.name == MenuSceneName)
		{
			InMenuScene = true;
			Helper.SetCursorVisibleAndLockState(state: true, CursorLockMode.None);
		}
		else if (scene.name == GameSceneName && !GameManager.IsLeavingGame)
		{
			bedRespawnPoint = GameObject.FindWithTag("RespawnPointBed");
			InMenuScene = false;
			if (IsHost)
			{
				Helper.LogBuild("Semih_Network: GameScene loaded as host... Initiating connection to self");
				OnIncommingConnection(LocalSteamID, settings.Current.character);
				worldLoadedCoroutine = StartCoroutine(WorldHasBeenLoaded());
			}
			else
			{
				worldRequestedCoroutine = StartCoroutine(RequestWorldAsClient());
			}
		}
	}

	private IEnumerator WorldHasBeenLoaded()
	{
		Helper.LogBuild("World has been loaded");
		while (!gameManagerStartCalled)
		{
			yield return null;
		}
		Helper.LogBuild("Waiting for landmark scenes to load");
		while (!IsAllLandmarksLoaded)
		{
			yield return null;
		}
		Helper.LogBuild("Landmark scenes has been loaded");
		Helper.LogBuild("Calling SceneEvent.JoinGame");
		Helper.BroadCastToScene(SceneEvent.JoinGame);
		Helper.LogBuild("Calling OnWorldReceivedEvent");
		if (OnWorldReceived != null)
		{
			OnWorldReceived();
		}
		WorldHasBeenRecieved = true;
		Helper.LogBuild("Waiting 0.25 seconds to call OnWorldReceivedLateEvent");
		yield return new WaitForSeconds(0.25f);
		Helper.LogBuild("Calling OnWorldReceivedLateEvent");
		if (OnWorldReceivedLate != null)
		{
			OnWorldReceivedLate();
		}
		if (isHost && CurrentRequestJoinAuthSetting != RequestJoinAuthSetting.ALLOW_NONE && CurrentRequestJoinAuthSetting != RequestJoinAuthSetting.INVITE_ONLY)
		{
			if (GameManager.HasPassword)
			{
				SteamFriends.SetRichPresence(PresenceKeyStatus, STATUS_Hosting_Password);
			}
			else
			{
				SteamFriends.SetRichPresence(PresenceKeyStatus, STATUS_Hosting);
			}
			string text = SteamUser.GetSteamID().ToString();
			string text2 = (GameManager.HasPassword ? PASSWORD_DELIMITER : string.Empty);
			string pchValue = CONNECT_DELIMITER + " " + text + text2;
			SteamFriends.SetRichPresence(PresenceKeyConnect, pchValue);
			SteamFriends.SetRichPresence(PresenceKeyGameInfo, EncodeGameInfo(GameManager.GameMode, GameManager.Crossplay));
		}
	}

	private IEnumerator RequestWorldAsClient()
	{
		while (!hostID.IsValid())
		{
			yield return null;
		}
		while (!IsAllLandmarksLoaded)
		{
			yield return null;
		}
		WorldHasBeenRecieved = false;
		Platform_RequestWorldAsClient();
		Helper.LogBuild("Semih_Network: GameScene loaded as client... Requesting world");
	}

	public void HostGame(RequestJoinAuthSetting joinAuthSetting, string password)
	{
		if (settings == null)
		{
			settings = ComponentManager<Settings>.Value;
		}
		CurrentRequestJoinAuthSetting = joinAuthSetting;
		if (!string.IsNullOrEmpty(password) && joinAuthSetting != RequestJoinAuthSetting.ALLOW_NONE && joinAuthSetting != RequestJoinAuthSetting.INVITE_ONLY)
		{
			GameManager.Password = password;
		}
		else
		{
			GameManager.Password = string.Empty;
		}
		CleanupState();
		isHost = true;
		connectedToHost = true;
		hostID = LocalSteamID;
		gameManagerStartCalled = false;
		worldLoadedCoroutine = null;
		Platform_HostGame();
	}

	private void LoadScene(string sceneName)
	{
		if (loadSceneManager == null)
		{
			SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}
		else if (sceneName.Equals(GameSceneName))
		{
			loadSceneManager.LoadScene(sceneName, loadAdditively: false);
		}
		else if (sceneName.Equals(MenuSceneName))
		{
			loadSceneManager.LoadScene(sceneName, loadAdditively: false);
		}
	}

	private void OnIncommingConnection(Network_UserId remoteID, RGD_Settings_Character characterSettings)
	{
		if (IsHost)
		{
			if (remoteID == HostID)
			{
				AddPlayer(remoteID, characterSettings);
			}
			else
			{
				Platform_AddRemoteClient(remoteID, characterSettings);
			}
		}
	}

	public void CheckForNameCollision(Network_Player netPlayer, Network_UserId userId)
	{
		string visualName = netPlayer.visualName;
		int num = 1;
		while (true)
		{
			Network_Player playerFromVisualName = GetPlayerFromVisualName(visualName);
			if (!(playerFromVisualName != null) || !(playerFromVisualName.steamID != userId))
			{
				break;
			}
			visualName = netPlayer.visualName + "(" + num + ")";
			num++;
		}
		if (num != 1)
		{
			netPlayer.visualName = visualName;
		}
	}

	private Network_Player InternalAddPlayer(Network_UserId steamID, RGD_Settings_Character characterSettings)
	{
		if (remoteUsers.ContainsKey(steamID))
		{
			Debug.LogError($"Player {steamID} already exists in dictionary");
			return null;
		}
		Network_Player network_Player = UnityEngine.Object.Instantiate(playerPrefab, bedRespawnPoint.transform.position, Quaternion.identity);
		remoteUsers.Add(steamID, network_Player);
		network_Player.OnPlayerCreated(this, steamID, characterSettings);
		network_Player.SetVisualName(characterSettings.Name, WorldHasBeenRecieved);
		SetupPlayerNetworkState(network_Player, steamID);
		return network_Player;
	}

	private void AddPlayer(Network_UserId steamID, RGD_Settings_Character characterSettings)
	{
		Network_Player network_Player = InternalAddPlayer(steamID, characterSettings);
		if (!(network_Player == null))
		{
			if (IsHost)
			{
				RPCExclude(new Message_Player_Create(Messages.CreatePlayer, network_Player), Target.Other, steamID, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			if (OnPlayerInitialized != null)
			{
				OnPlayerInitialized(network_Player);
			}
			if (remoteUsers.Count > 1)
			{
				AchievementHandler.UnlockAchievement(AchievementType.ach_playwithfriend);
			}
			OnPlayerJoinOrLeaves?.Invoke();
		}
	}

	private void AddPlayer(Message_Player_Create msg)
	{
		if (bedRespawnPoint == null)
		{
			return;
		}
		Network_Player network_Player = InternalAddPlayer(msg.UserId, msg.characterSettings);
		if (!(network_Player == null))
		{
			network_Player.HandleInitialAnimationValues(msg);
			network_Player.SetIndexRemotePlayer(msg.ObjectIndex, msg.BehaviourIndex);
			if (OnPlayerInitialized != null)
			{
				OnPlayerInitialized(network_Player);
			}
			if (msg.dead)
			{
				StartCoroutine(KillLater(1f, network_Player));
			}
			if (remoteUsers.Count > 1)
			{
				AchievementHandler.UnlockAchievement(AchievementType.ach_playwithfriend);
			}
			if (msg.noteBookOpen && network_Player.NoteBookUI != null)
			{
				network_Player.NoteBookUI.isDisplayed = msg.noteBookOpen;
				network_Player.NoteBookUI.SetBookActive(value: true, atStart: true);
			}
			if (msg.hazmatTimer > 0f && network_Player.HazmatSuit != null)
			{
				CoroutineManager.Singleton.AddCoroutine(network_Player.HazmatSuit.EquipDelayed(1f, msg.hazmatTimer));
			}
			OnPlayerJoinOrLeaves?.Invoke();
		}
	}

	private IEnumerator KillLater(float timeDelay, Network_Player player)
	{
		yield return new WaitForSeconds(timeDelay);
		player.PlayerScript.Kill(replicating: true);
	}

	public void TryToJoinGame(Network_UserId hostID, string password)
	{
		if (IsConnectedToHost)
		{
			Helper.LogBuild("Can't join game, you are already connected to a host");
		}
		else
		{
			Platform_TryToJoinGame(hostID, password);
		}
	}

	public void LeaveGame(DisconnectReason reason, SceneName sceneName, bool saveGame, bool force = false)
	{
		if (IsConnectedToHost || force)
		{
			if (saveGame)
			{
				ComponentManager<SaveAndLoad>.Value.SaveGame(isHost);
			}
			GameManager.IsLeavingGame = true;
			StartCoroutine(Disconnect(reason, sceneName, 0.1f));
		}
	}

	private IEnumerator Disconnect(DisconnectReason reason, SceneName sceneName, float timeDelay)
	{
		Debug.Log($"Local disconnect {reason}");
		yield return new WaitForSeconds(timeDelay);
		while (!IsAllLandmarksLoaded)
		{
			yield return null;
		}
		while (LoadSceneManager.IsLoadingScene)
		{
			yield return null;
		}
		Helper.BroadCastToScene(SceneEvent.LeaveGame);
		remoteUsers.Clear();
		string text;
		switch (sceneName)
		{
		case SceneName.Exit:
			Application.Quit();
			yield break;
		default:
			text = GameSceneName;
			break;
		case SceneName.Lobby:
			text = MenuSceneName;
			break;
		case SceneName.None:
			yield break;
		}
		string sceneName2 = text;
		LoadScene(sceneName2);
	}

	public Network_UserId GetIDFromPlayer(Network_Player player)
	{
		foreach (KeyValuePair<Network_UserId, Network_Player> remoteUser in remoteUsers)
		{
			if (remoteUser.Value == player)
			{
				return remoteUser.Key;
			}
		}
		return 0uL;
	}

	public Network_Player GetPlayerFromVisualName(string name)
	{
		foreach (Network_Player value in remoteUsers.Values)
		{
			if (value.visualName == name)
			{
				return value;
			}
		}
		return null;
	}

	public Network_Player GetPlayerFromID(Network_UserId id)
	{
		if (id.IsValid())
		{
			if (remoteUsers.ContainsKey(id))
			{
				return remoteUsers[id];
			}
			return null;
		}
		return null;
	}

	public Network_Player GetRandomPlayer()
	{
		if (remoteUsers == null || remoteUsers.Count == 0)
		{
			return null;
		}
		int index = UnityEngine.Random.Range(0, remoteUsers.Count);
		return remoteUsers.ElementAt(index).Value;
	}

	public Network_Player GetLocalPlayer()
	{
		if (remoteUsers == null || remoteUsers.Count == 0)
		{
			return null;
		}
		if (localPlayer != null)
		{
			return localPlayer;
		}
		foreach (KeyValuePair<Network_UserId, Network_Player> remoteUser in remoteUsers)
		{
			if (remoteUser.Value.IsLocalPlayer)
			{
				localPlayer = remoteUser.Value;
				return localPlayer;
			}
		}
		return null;
	}

	private List<Message> GetWorld()
	{
		List<Message> list = new List<Message>();
		List<Message> list2 = new List<Message>();
		List<Message> list3 = new List<Message>();
		Message item = null;
		Message item2 = null;
		List<Message> list4 = new List<Message>();
		Message item3 = null;
		Message item4 = null;
		Message item5 = null;
		Message item6 = null;
		Message item7 = null;
		Message item8 = null;
		Message item9 = null;
		foreach (KeyValuePair<uint, MonoBehaviour_Network> networkedBehaviour in NetworkUpdateManager.NetworkedBehaviours)
		{
			MonoBehaviour_Network value = networkedBehaviour.Value;
			value.Clean();
			value.Serialize_Create();
			if (value.MessagesStack.Count == 0)
			{
				continue;
			}
			for (int i = 0; i < value.MessagesStack.Count; i++)
			{
				Message message = value.MessagesStack[i];
				if (message.Type == Messages.CreatePlayer)
				{
					list2.Insert(0, message);
				}
				else if (message.Type == Messages.ObjectSpawner_Create)
				{
					item = message;
				}
				else if (message.Type == Messages.BlockCreator_CreateEndMessage)
				{
					item2 = message;
				}
				else if (message.Type == Messages.BlockCreator_Create)
				{
					list4.Add(message);
				}
				else if (message.Type == Messages.PlantManagerCreate)
				{
					item3 = message;
				}
				else if (message.Type == Messages.StorageManager_Create)
				{
					item4 = message;
				}
				else if (message.Type == Messages.PlayerNetworkManager_Create)
				{
					item5 = message;
				}
				else if (message.Type == Messages.Create_Chat_Manager)
				{
					item6 = message;
				}
				else if (message.Type == Messages.CreateNetworkIDManager)
				{
					item7 = message;
				}
				else if (message.Type == Messages.CreateAINetworkBehaviour)
				{
					list3.Add(message);
				}
				else if (message.Type == Messages.WorldManager_CreateLandmarks)
				{
					item8 = message;
				}
				else if (message.Type == Messages.CreateHostNetwork)
				{
					item9 = message;
				}
				else
				{
					list.Add(message);
				}
			}
			value.Clean();
		}
		list.Insert(0, item7);
		list.Insert(0, item8);
		list.Insert(0, item9);
		list.InsertRange(0, list3);
		list.Insert(0, item6);
		list.Insert(0, item5);
		list.Insert(0, item4);
		list.Insert(0, item3);
		list.Insert(0, item);
		list.Insert(0, item2);
		list.InsertRange(0, list4);
		list.InsertRange(0, list2);
		list.Insert(0, new Message_WorldInfo(Messages.WorldGuid, SaveAndLoad.WorldGuid));
		list.Add(new Message(Messages.WorldReceived));
		return list;
	}

	private void HandleMessage(Message message, Network_UserId remoteID, ref bool worldReceived)
	{
		switch (message.Type)
		{
		case Messages.Ping:
			if (IsHost)
			{
				Message_PingPong message_PingPong = message as Message_PingPong;
				message_PingPong.Type = Messages.Pong;
				SendP2P(remoteID, message_PingPong, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			break;
		case Messages.Pong:
		{
			Message_PingPong message_PingPong2 = message as Message_PingPong;
			ping = Time.time - message_PingPong2.timeSent;
			break;
		}
		case Messages.WorldReceived:
			worldReceived = true;
			break;
		case Messages.WorldGuid:
		{
			Message_WorldInfo message_WorldInfo = message as Message_WorldInfo;
			SaveAndLoad.WorldGuid = new Guid(message_WorldInfo.worldGuid);
			GameManager.FriendlyFire = message_WorldInfo.friendlyFire;
			TradingPostUI value2 = ComponentManager<TradingPostUI>.Value;
			if (value2 != null)
			{
				value2.CurrentReputation = message_WorldInfo.traderReputation;
			}
			break;
		}
		case Messages.Save:
		{
			SaveAndLoad value = ComponentManager<SaveAndLoad>.Value;
			if (value != null)
			{
				value.SaveGame(sendRemoteMessage: false);
				if (IsHost)
				{
					RPCExclude(message, Target.Other, remoteID, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			break;
		}
		case Messages.CreatePlayer:
			if (!IsHost)
			{
				Message_Player_Create msg = message as Message_Player_Create;
				AddPlayer(msg);
			}
			break;
		case Messages.CreateHostNetwork:
		{
			Message_NetworkBehaviour message_NetworkBehaviour5 = message as Message_NetworkBehaviour;
			Network_Host network_Host = UnityEngine.Object.FindObjectOfType<Network_Host>();
			network_Host.BehaviourIndex = message_NetworkBehaviour5.BehaviourIndex;
			network_Host.ObjectIndex = message_NetworkBehaviour5.ObjectIndex;
			NetworkUpdateManager.AddBehaviour(network_Host);
			break;
		}
		case Messages.CreateNetworkIDManager:
		{
			Message_NetworkBehaviour message_NetworkBehaviour4 = message as Message_NetworkBehaviour;
			NetworkIDManager networkIDManager = UnityEngine.Object.FindObjectOfType<NetworkIDManager>();
			networkIDManager.BehaviourIndex = message_NetworkBehaviour4.BehaviourIndex;
			networkIDManager.ObjectIndex = message_NetworkBehaviour4.ObjectIndex;
			NetworkUpdateManager.AddBehaviour(networkIDManager);
			break;
		}
		case Messages.ObjectSpawner_Create:
		{
			Message_NetworkBehaviour message_NetworkBehaviour3 = message as Message_NetworkBehaviour;
			ObjectSpawnerManager objectSpawnerManager = UnityEngine.Object.FindObjectOfType<ObjectSpawnerManager>();
			objectSpawnerManager.BehaviourIndex = message_NetworkBehaviour3.BehaviourIndex;
			objectSpawnerManager.ObjectIndex = message_NetworkBehaviour3.ObjectIndex;
			NetworkUpdateManager.AddBehaviour(objectSpawnerManager);
			break;
		}
		case Messages.CreateBedManager:
		{
			Message_NetworkBehaviour message_NetworkBehaviour2 = message as Message_NetworkBehaviour;
			BedManager bedManager = UnityEngine.Object.FindObjectOfType<BedManager>();
			bedManager.BehaviourIndex = message_NetworkBehaviour2.BehaviourIndex;
			bedManager.ObjectIndex = message_NetworkBehaviour2.ObjectIndex;
			NetworkUpdateManager.AddBehaviour(bedManager);
			break;
		}
		case Messages.CreateNetworkPickupManager:
		{
			Message_PickupManager_Create message_PickupManager_Create = message as Message_PickupManager_Create;
			PickupObjectManager pickupObjectManager = UnityEngine.Object.FindObjectOfType<PickupObjectManager>();
			pickupObjectManager.BehaviourIndex = message_PickupManager_Create.BehaviourIndex;
			pickupObjectManager.ObjectIndex = message_PickupManager_Create.ObjectIndex;
			NetworkUpdateManager.AddBehaviour(pickupObjectManager);
			break;
		}
		case Messages.Create_Chat_Manager:
		{
			Message_NetworkBehaviour message_NetworkBehaviour = message as Message_NetworkBehaviour;
			ChatManager chatManager = UnityEngine.Object.FindObjectOfType<ChatManager>();
			chatManager.BehaviourIndex = message_NetworkBehaviour.BehaviourIndex;
			chatManager.ObjectIndex = message_NetworkBehaviour.ObjectIndex;
			NetworkUpdateManager.AddBehaviour(chatManager);
			break;
		}
		}
	}

	private void OnDisconnect(Network_UserId remoteID, DisconnectReason disconnectReason, bool saveGame)
	{
		if (!remoteID.IsValid())
		{
			return;
		}
		Debug.Log($"Call Disconnect for {remoteID}. Reason: {disconnectReason} Save: {saveGame}");
		if (OnDisconnectEvent != null)
		{
			OnDisconnectEvent(remoteID, disconnectReason);
		}
		if (remoteID == hostID)
		{
			if (!Application.isEditor && !IsHost)
			{
				errorMessage = Helper.GetTerm("Menu/Disconnected");
				LeaveGame(DisconnectReason.HostDisconnected, SceneName.Lobby, saveGame);
				CleanupState();
			}
			if (IsHost)
			{
				ComponentManager<PauseMenu>.Value.ShowDisconnectPopup();
			}
		}
		else if (remoteUsers.ContainsKey(remoteID))
		{
			RemovePlayer(remoteID);
			if (IsHost)
			{
				ComponentManager<SaveAndLoad>.Value.SaveGame(sendRemoteMessage: true);
			}
		}
	}

	private void RemovePlayer(Network_UserId id)
	{
		if (remoteUsers.ContainsKey(id))
		{
			Network_Player network_Player = remoteUsers[id];
			if (network_Player.RessurectComponent.IsCarrying)
			{
				network_Player.RessurectComponent.StopCarryingPlayer(manipulatePosition: true);
			}
			UnityEngine.Object.Destroy(network_Player.gameObject);
			remoteUsers.Remove(id);
			OnPlayerJoinOrLeaves?.Invoke();
		}
	}

	private void SendPing()
	{
		if (IsConnectedToHost)
		{
			if (IsHost)
			{
				ping = 0f;
			}
			else
			{
				SendP2P(HostID, new Message_PingPong(Messages.Ping, Time.time), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	private void CalculateBytes()
	{
		bytesSentPerSecond = lastSecondBytes / 1f;
		lastSecondBytes = 0f;
	}

	private InitiateResult CanUserJoinFriendCheck(CSteamID remoteID)
	{
		switch (CurrentRequestJoinAuthSetting)
		{
		case RequestJoinAuthSetting.ALLOW_NONE:
			Debug.Log(SteamFriends.GetFriendPersonaName(remoteID) + " tried to join but you are not allowing friends to join");
			return InitiateResult.Fail_AllowNoFriends;
		case RequestJoinAuthSetting.ALLOW_FRIENDS:
			if (SteamFriends.GetFriendRelationship(remoteID) != EFriendRelationship.k_EFriendRelationshipFriend)
			{
				return InitiateResult.Fail_NotFriendWithHost;
			}
			break;
		case RequestJoinAuthSetting.ALLOW_ALL:
			return InitiateResult.Success;
		}
		return InitiateResult.Success;
	}

	private InitiateResult CanUserJoinMe(CSteamID remoteID, int appBuildID, string password)
	{
		if (!IsHost)
		{
			return InitiateResult.Fail_NotHost;
		}
		if (appBuildID != Settings.AppBuildID)
		{
			return InitiateResult.Fail_MissmatchAppBuildID;
		}
		if (GameManager.HasPassword && password != GameManager.Password)
		{
			return InitiateResult.Fail_WrongPassword;
		}
		if (!LoadSceneManager.IsGameSceneLoaded)
		{
			return InitiateResult.Fail_NotHost;
		}
		if (CanUserJoinFriendCheck(remoteID) == InitiateResult.Success && GameManager.HasPassword && password != GameManager.Password)
		{
			return InitiateResult.Fail_WrongPassword;
		}
		return InitiateResult.Success;
	}

	private void UserStatsRecieved(UserStatsReceived_t callback)
	{
		AchievementHandler.UserStatsRecieved = callback.m_eResult == EResult.k_EResultOK;
		AchievementHandler.Initialize();
	}

	public static string EncodeConString(string networkId, GameMode gameMode, bool isCrossplay)
	{
		return HttpUtility.UrlEncode(networkId + (char)gameMode + (isCrossplay ? '1' : '0'));
	}

	public static GameInfo DecodeConString(string bytes)
	{
		string text = HttpUtility.UrlDecode(bytes);
		return new GameInfo
		{
			networkId = text.Substring(0, text.Length - 2),
			gameMode = (GameMode)text[text.Length - 2],
			isCrossplay = (text[text.Length - 1] != '0')
		};
	}

	public static string EncodeGameInfo(GameMode gameMode, bool isCrossplay)
	{
		return HttpUtility.UrlEncode((char)gameMode + (isCrossplay ? "1" : "0"));
	}

	public static GameInfo DecodeGameInfo(string bytes)
	{
		string text = HttpUtility.UrlDecode(bytes);
		return new GameInfo
		{
			gameMode = (GameMode)text[text.Length - 2],
			isCrossplay = (text[text.Length - 1] != '0')
		};
	}

	private void GameRichPressenceJoinRequested(GameRichPresenceJoinRequested_t data)
	{
		Debug.Log("Received Connect string: " + data.m_rgchConnect);
		if (data.m_rgchConnect != string.Empty)
		{
			string[] array = data.m_rgchConnect.Split(CONNECT_DELIMITER);
			if (array.Length > 1)
			{
				ParseConnectString(array[1].TrimStart());
			}
		}
	}

	private void SessionRequest(P2PSessionRequest_t data)
	{
		SteamNetworking.AcceptP2PSessionWithUser(data.m_steamIDRemote);
	}

	private void Platform_Init()
	{
		PlayFabManager instance = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance;
		instance.OnPlayFabSignedIn = (UnityAction)Delegate.Combine(instance.OnPlayFabSignedIn, new UnityAction(OnPlayFabSignedIn));
		PlayFabManager instance2 = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance;
		instance2.OnJoinWorld = (UnityAction<string, string>)Delegate.Combine(instance2.OnJoinWorld, new UnityAction<string, string>(OnJoinWorldThroughCrossPlay));
		if (Application.isEditor)
		{
			PlayFabMultiplayerManager.Get().LogLevel = PlayFabMultiplayerManager.LogLevelType.Verbose;
		}
		else
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == "-verbose")
				{
					PlayFabMultiplayerManager.Get().LogLevel = PlayFabMultiplayerManager.LogLevelType.Verbose;
					break;
				}
			}
		}
		PlayFabMultiplayerManager.Get().OnNetworkJoined += OnNetworkJoined;
		PlayFabMultiplayerManager.Get().OnNetworkChanged += OnNetworkChanged;
		PlayFabMultiplayerManager.Get().OnNetworkLeft += OnNetworkLeft;
		PlayFabMultiplayerManager.Get().OnRemotePlayerJoined += OnRemotePlayerJoinedNetwork;
		PlayFabMultiplayerManager.Get().OnRemotePlayerLeft += OnRemotePlayerLeftNetwork;
		PlayFabMultiplayerManager.Get().OnDataMessageNoCopyReceived += OnDataMessageReceived;
		PlayFabMultiplayerManager.Get().OnError += OnError;
		InvokeRepeating("SendPing", 2f, 2f);
		InvokeRepeating("CalculateBytes", 1f, 1f);
		m_gameRichPressenceJoined = Callback<GameRichPresenceJoinRequested_t>.Create(GameRichPressenceJoinRequested);
		m_sessionRequest = Callback<P2PSessionRequest_t>.Create(SessionRequest);
		OnUserStatsRecievedCallback = Callback<UserStatsReceived_t>.Create(UserStatsRecieved);
	}

	private void OnError(object sender, PlayFabMultiplayerManagerErrorArgs args)
	{
		switch (args.Code)
		{
		case 4237:
			ForceLeaveSession(LeaveSessionReason.GameIsFull);
			return;
		case 4103:
			ForceLeaveSession(LeaveSessionReason.InternetLoss, args.Code);
			return;
		case 63:
		case 73:
			ForceLeaveSession(LeaveSessionReason.ConnectionLoss, args.Code);
			return;
		}
		Debug.LogError(args.Type);
		Debug.LogError(args.Code);
		Debug.LogError(args.Message);
	}

	public void CleanupState()
	{
		if (isHost && !worldCode.IsNullOrEmpty() && GameManager.Crossplay)
		{
			PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
			{
				FunctionName = "clear_game_network_cross_play",
				FunctionParameter = new
				{
					worldCode = WorldCode
				}
			}, (ExecuteCloudScriptResult _) =>
			{
				Debug.Log("Clear game network cross play");
			}, PlayFabManager.DisplayPlayFabError);
		}
		connectedToHost = false;
		hostID = 0uL;
		currentNetworkId = "";
		worldCode = "";
		playerInfos.Clear();
		m_hostedPlayFab = false;
		m_currentSteamHost = CSteamID.Nil;
	}

	private void ForceLeaveSession(LeaveSessionReason reason, int code = 0)
	{
		if (!GameManager.IsLeavingGame)
		{
			bool saveGame = false;
			switch (reason)
			{
			case LeaveSessionReason.GameIsFull:
				errorMessage = Helper.GetTerm("Menu/WorldFull");
				break;
			case LeaveSessionReason.InternetLoss:
				errorMessage = Helper.GetTerm("Menu/LostConnectionToInternet");
				saveGame = !isHost;
				break;
			case LeaveSessionReason.ConnectionLoss:
				errorMessage = Helper.GetTerm("Menu/IssuesConnecting") + " Code: " + code;
				saveGame = !isHost;
				break;
			case LeaveSessionReason.Blocked:
				errorMessage = Helper.GetTerm("Menu/BlockedByHost");
				break;
			case LeaveSessionReason.InviteAccepted:
				saveGame = true;
				break;
			case LeaveSessionReason.MissmatchVersion:
				errorMessage = Helper.GetTerm("Menu/ConnectFail_MissmatchID");
				break;
			}
			LeaveGame(DisconnectReason.SelfDisconnected, SceneName.Lobby, saveGame, force: true);
		}
		Debug.Log($"Leave: {reason}");
		CleanupState();
	}

	private void DownloadBlockList()
	{
		PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
		{
			FunctionName = "get_blocked_users",
			FunctionParameter = new
			{
				playerId = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance.MasterAccountId
			}
		}, (ExecuteCloudScriptResult result) =>
		{
			object functionResult = result.FunctionResult;
			if (functionResult == null || !((JsonObject)functionResult).TryGetValue("Data", out var value))
			{
				return;
			}
			foreach (KeyValuePair<string, object> item in (JsonObject)value)
			{
				if (item.Key != "DataVersion" && item.Key != "PlayFabId")
				{
					try
					{
						blockedUserIds.Add(new Network_UserId(item.Key), (string)item.Value);
					}
					catch (Exception)
					{
						Debug.LogError("Failed to add blocked user " + item.Key);
						throw;
					}
				}
			}
		}, PlayFabManager.DisplayPlayFabError);
	}

	public void AddToBlockList(string _onlineId, string titleId)
	{
		PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
		{
			FunctionName = "block_user",
			FunctionParameter = new
			{
				playerId = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance.MasterAccountId,
				onlineId = _onlineId,
				blockId = titleId
			}
		}, (ExecuteCloudScriptResult result) =>
		{
			_ = result.FunctionResult;
		}, PlayFabManager.DisplayPlayFabError);
		if (isHost)
		{
			SendP2P(new Network_UserId(titleId), new Message(Messages.KickUser), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		blockedUserIds.Add(new Network_UserId(titleId), _onlineId);
	}

	public void UnblockUser(string titleId)
	{
		PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
		{
			FunctionName = "unblock_user",
			FunctionParameter = new
			{
				playerId = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance.MasterAccountId,
				blockId = titleId
			}
		}, (ExecuteCloudScriptResult result) =>
		{
			_ = result.FunctionResult;
		}, PlayFabManager.DisplayPlayFabError);
		blockedUserIds.Remove(new Network_UserId(titleId));
	}

	public bool IsUserBlocked(string titleId)
	{
		return blockedUserIds.ContainsKey(new Network_UserId(titleId));
	}

	public bool IsUserReported(string titleId)
	{
		return reportedUserIds.Contains(new Network_UserId(titleId));
	}

	public void ReportUser(string titleId)
	{
		if (reportedUserIds.Contains(titleId))
		{
			return;
		}
		if (reportsAvailable > 0)
		{
			PlayFabClientAPI.ReportPlayer(new ReportPlayerClientRequest
			{
				ReporteeId = titleId
			}, (ReportPlayerClientResult result) =>
			{
				reportsAvailable = result.SubmissionsRemaining;
			}, PlayFabManager.DisplayPlayFabError);
		}
		reportedUserIds.Add(new Network_UserId(titleId));
	}

	public void OnPlayFabSignedIn()
	{
		localSteamID = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance.TitleAccountId;
		SignedIntoPlayfab = true;
		DownloadBlockList();
		if (startScene.name == GameSceneName)
		{
			HostGame(RequestJoinAuthSetting.ALLOW_NONE, string.Empty);
		}
		if (isHost && connectedToHost)
		{
			hostID = localSteamID;
		}
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			Debug.Log(commandLineArgs[i]);
			if (commandLineArgs[i] == CONNECT_DELIMITER)
			{
				Debug.Log("OS SignIn Connect string: " + commandLineArgs[i + 1]);
				ParseConnectString(commandLineArgs[i + 1]);
			}
		}
	}

	private void ParseConnectString(string connect)
	{
		if (!(connect != string.Empty))
		{
			return;
		}
		string[] array = connect.Split(new string[1] { PASSWORD_DELIMITER }, StringSplitOptions.None);
		if (array.Length > 1)
		{
			string s = array[0];
			string text = array[1];
			CSteamID cSteamID = new CSteamID(ulong.Parse(s));
			if (cSteamID != m_currentSteamHost)
			{
				m_inviteFrom = cSteamID;
				m_inviteFromPass = ((text == string.Empty) ? PASSWORD_DELIMITER : text);
				m_receivedInvite = true;
			}
		}
		else
		{
			CSteamID cSteamID2 = new CSteamID(ulong.Parse(array[0]));
			if (cSteamID2 != m_currentSteamHost)
			{
				m_inviteFrom = cSteamID2;
				m_inviteFromPass = string.Empty;
				m_receivedInvite = true;
			}
		}
	}

	public void Platform_TryToJoinGame(Network_UserId hostID, string password)
	{
	}

	public void CancelTryingToConnect(CSteamID connectingTo)
	{
		SteamNetworking.CloseP2PSessionWithUser(connectingTo);
	}

	private void JoinNetwork(GameInfo gameInfo)
	{
		isHost = false;
		gameManagerStartCalled = false;
		worldLoadedCoroutine = null;
		CurrentRequestJoinAuthSetting = RequestJoinAuthSetting.INVITE_ONLY;
		PlayFabMultiplayerManager.Get().JoinNetwork(gameInfo.networkId);
		GameModeValueManager.SelectCurrentGameMode(gameInfo.gameMode);
		GameManager.Crossplay = gameInfo.isCrossplay;
		LoadScene(GameSceneName);
	}

	public void OnJoinWorldThroughCrossPlay(string _worldCode, string _password)
	{
		if (_worldCode == string.Empty)
		{
			_worldCode = worldCode;
		}
		object functionParameter = ((!(_password == string.Empty)) ? ((object)new
		{
			worldCode = _worldCode,
			password = _password
		}) : ((object)new
		{
			worldCode = _worldCode
		}));
		PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
		{
			FunctionName = "get_game_network_cross_play",
			FunctionParameter = functionParameter
		}, (ExecuteCloudScriptResult result) =>
		{
			object functionResult = result.FunctionResult;
			if (functionResult != null)
			{
				JsonObject jsonObject = (JsonObject)functionResult;
				object value5;
				if (jsonObject.TryGetValue("network", out var value))
				{
					GameInfo gameInfo = new GameInfo
					{
						isCrossplay = true
					};
					if (((JsonObject)value).TryGetValue("Value", out var value2))
					{
						gameInfo.networkId = (string)value2;
						if (jsonObject.TryGetValue("gameMode", out var value3) && ((JsonObject)value3).TryGetValue("Value", out var value4))
						{
							gameInfo.gameMode = (GameMode)int.Parse((string)value4);
							JoinNetwork(gameInfo);
						}
					}
				}
				else if (jsonObject.TryGetValue("ec", out value5))
				{
					CrossplayJoinErrorCode obj = (CrossplayJoinErrorCode)Convert.ToInt32(value5);
					OnCrossplayJoinComplete(obj);
				}
			}
		}, PlayFabManager.DisplayPlayFabError);
		worldCode = _worldCode;
	}

	private void Platform_Shutdown()
	{
		PlayFabMultiplayerManager.Get().LeaveNetwork();
	}

	private void Platform_Update()
	{
		if (PlayFabMultiplayerManager.Get().State == PlayFabMultiplayerManagerState.ConnectedToNetwork && LoadSceneManager.IsGameSceneLoaded && !voiceDisabled)
		{
			if (voiceOverlay == null)
			{
				voiceOverlay = ComponentManager<CanvasHelper>.Value.voiceOverlay;
			}
			int num = 0;
			foreach (PlayFabPlayer remotePlayer in PlayFabMultiplayerManager.Get().RemotePlayers)
			{
				if (remotePlayer.ChatState == ChatState.Talking)
				{
					Network_Player playerFromID = GetPlayerFromID(new Network_UserId(remotePlayer.EntityKey.Id));
					if (playerFromID != null)
					{
						voiceOverlay.PushVoiceOverlay(num, playerFromID.playerNameTextMesh.text);
					}
				}
				num++;
			}
			if (PlayFabMultiplayerManager.Get().LocalPlayer.IsMuted != ComponentManager<Settings>.Value.audioBox.IsMuted)
			{
				PlayFabMultiplayerManager.Get().LocalPlayer.IsMuted = ComponentManager<Settings>.Value.audioBox.IsMuted;
			}
		}
		ReadP2P_Channel_Connecting();
		if (GameManager.IsLeavingGame || !m_receivedInvite)
		{
			return;
		}
		if (LoadSceneManager.IsGameSceneLoaded && m_currentSteamHost != m_inviteFrom)
		{
			ForceLeaveSession(LeaveSessionReason.InviteAccepted);
		}
		else if (!connectedToHost)
		{
			if (m_inviteFromPass == PASSWORD_DELIMITER)
			{
				StartMenuScreen startMenuScreen = UnityEngine.Object.FindObjectOfType<StartMenuScreen>();
				startMenuScreen.joinGameBox.joinPasswordBox.openedViaFriendJoinArgument = m_inviteFrom;
				startMenuScreen.joinGameBox.joinPasswordBox.Open();
			}
			else
			{
				TryToJoinGame(m_inviteFrom, m_inviteFromPass);
			}
			m_receivedInvite = false;
		}
	}

	private void Platform_LeaveGame()
	{
		if (PlayFabMultiplayerManager.Get().State == PlayFabMultiplayerManagerState.ConnectedToNetwork)
		{
			PlayFabMultiplayerManager.Get().LeaveNetwork();
			return;
		}
		Debug.Log("SinglePlayerLeave");
		CleanupState();
	}

	private void Platform_HostGame()
	{
		if (CurrentRequestJoinAuthSetting != RequestJoinAuthSetting.ALLOW_NONE && GameManager.Crossplay)
		{
			PlayFabMultiplayerManager.Get().CreateAndJoinNetwork(new PlayFabNetworkConfiguration
			{
				MaxPlayerCount = 10u
			});
			m_hostedPlayFab = true;
		}
		if (startScene.name != GameSceneName)
		{
			LoadScene(GameSceneName);
		}
		m_currentSteamHost = SteamUser.GetSteamID();
	}

	private void Platform_RequestWorldAsClient()
	{
		SendP2P(hostID, new Message_PlayerJoined(Messages.PlayerJoined, settings.Current.character), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Session);
		SendP2P(hostID, new Message(Messages.RequestWorld), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Session);
	}

	private void Platform_AddRemoteClient(Network_UserId remoteID, RGD_Settings_Character characterSettings)
	{
		AddPlayer(remoteID, characterSettings);
	}

	public ulong GetPlatformSpecificId(Network_UserId networkId)
	{
		if (!GetPlayerInfoFromNetworkId(networkId, out var playerInfo))
		{
			return 0uL;
		}
		return ulong.Parse(playerInfo.platformId);
	}

	private IEnumerator RetrieveAvatarForOtherPlayer(Network_Player networkPlayer, PlayFabPlayer player, bool worldHasBeenReceived)
	{
		ulong platformSpecificId;
		while (true)
		{
			if (GameManager.IsLeavingGame || GetPlayFabPlayerFromNetworkId(player.EntityKey.Id) == null)
			{
				yield break;
			}
			platformSpecificId = GetPlatformSpecificId(player.EntityKey.Id);
			if (platformSpecificId != 0L)
			{
				break;
			}
			yield return null;
		}
		networkPlayer.RetrieveAvatar(platformSpecificId, worldHasBeenReceived);
		yield return null;
	}

	private void SetupPlayerNetworkState(Network_Player networkPlayer, Network_UserId steamID)
	{
		if (!VoiceDisabled && steamID != localSteamID && !IsUserBlocked(steamID))
		{
			PlayFabPlayer playFabPlayerFromNetworkId = GetPlayFabPlayerFromNetworkId(steamID);
			if (playFabPlayerFromNetworkId != null)
			{
				playFabPlayerFromNetworkId.IsMuted = false;
			}
		}
		if (networkPlayer.characterSettings.Platform != 0)
		{
			return;
		}
		if (steamID == localSteamID)
		{
			networkPlayer.SetSmallSteamProfile(SteamUser.GetSteamID());
			return;
		}
		PlayFabPlayer playFabPlayerFromNetworkId2 = GetPlayFabPlayerFromNetworkId(steamID);
		if (playFabPlayerFromNetworkId2 != null)
		{
			StartCoroutine(RetrieveAvatarForOtherPlayer(networkPlayer, playFabPlayerFromNetworkId2, WorldHasBeenRecieved));
		}
	}

	private void OnRemotePlayerJoinedNetwork(object sender, PlayFabPlayer player)
	{
		Debug.Log($"OnRemotePlayerJoinedNetwork: {player.EntityKey.Id} joined which is host: {(Network_UserId)player.EntityKey.Id == hostID}. I am host {isHost}");
		player.IsMuted = true;
		bool flag = IsUserBlocked(player.EntityKey.Id);
		if (isHost)
		{
			SendP2P(player.EntityKey.Id, new Message_InitiateConnectionResult(Messages.InitiateResult, flag ? InitiateResult.Fail : InitiateResult.Success, GameManager.GameMode), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Session);
		}
		if (flag)
		{
			return;
		}
		PlayFabProfilesAPI.GetProfile(new GetEntityProfileRequest
		{
			Entity = new PlayFab.ProfilesModels.EntityKey
			{
				Id = player.EntityKey.Id,
				Type = player.EntityKey.Type
			}
		}, (GetEntityProfileResponse result) =>
		{
			PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
			{
				FunctionName = "get_platform_id",
				FunctionParameter = new
				{
					playerId = result.Profile.Lineage.MasterPlayerAccountId
				}
			}, (ExecuteCloudScriptResult executeCloudScriptResult) =>
			{
				object functionResult = executeCloudScriptResult.FunctionResult;
				if (functionResult != null)
				{
					JsonObject jsonObject = (JsonObject)functionResult;
					if (jsonObject.TryGetValue("Platform", out var value) && jsonObject.TryGetValue("Id", out var value2) && !GameManager.IsLeavingGame && GetPlayFabPlayerFromNetworkId(player.EntityKey.Id) != null)
					{
						PlayerInfo value3 = new PlayerInfo
						{
							platform = (string)value,
							platformId = (string)value2
						};
						playerInfos.TryAdd(player.EntityKey.Id, value3);
					}
				}
			}, PlayFabManager.DisplayPlayFabError);
		}, PlayFabManager.DisplayPlayFabError);
	}

	private void OnRemotePlayerLeftNetwork(object sender, PlayFabPlayer player)
	{
		Debug.Log($"OnRemotePlayerLeftNetwork: {player.EntityKey.Id} left which is host: {(Network_UserId)player.EntityKey.Id == hostID}. I am host {isHost}");
		playerInfos.Remove(player.EntityKey.Id);
		if (!GameManager.IsLeavingGame)
		{
			bool saveGame = (Network_UserId)player.EntityKey.Id != hostID;
			OnDisconnect(player.EntityKey.Id, DisconnectReason.SelfDisconnected, saveGame);
		}
	}

	private void SetGameNetworkCrossPlay(string networkId, string _password)
	{
		object functionParameter = ((!(_password == string.Empty)) ? ((object)new
		{
			entity = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance.TitleAccountId,
			network = networkId,
			gameMode = (int)GameManager.GameMode,
			password = _password
		}) : ((object)new
		{
			entity = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance.TitleAccountId,
			network = networkId,
			gameMode = (int)GameManager.GameMode
		}));
		PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
		{
			FunctionName = "set_game_network_cross_play",
			FunctionParameter = functionParameter
		}, (ExecuteCloudScriptResult result) =>
		{
			Debug.Log("set_game_network_cross_play called");
			object functionResult = result.FunctionResult;
			if (functionResult != null && ((JsonObject)functionResult).TryGetValue("worldCode", out var value))
			{
				worldCode = (string)value;
			}
		}, PlayFabManager.DisplayPlayFabError);
	}

	private void OnNetworkJoined(object sender, string networkId)
	{
		Debug.Log($"OnNetworkJoined: joined the network as host: {isHost}, setting up the network {networkId}");
		currentNetworkId = networkId;
		if (isHost && GameManager.Crossplay)
		{
			SetGameNetworkCrossPlay(networkId, GameManager.Password);
		}
	}

	private void UpdateGameNetworkCrossPlay(string _worldCode, string newNetworkId, string _password)
	{
		object functionParameter = ((!(_password == string.Empty)) ? ((object)new
		{
			entity = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance.TitleAccountId,
			worldCode = _worldCode,
			network = newNetworkId,
			gameMode = (int)GameManager.GameMode,
			password = _password
		}) : ((object)new
		{
			entity = SimpleMonoBehaviourSingleton<PlayFabManager>.Instance.TitleAccountId,
			worldCode = _worldCode,
			network = newNetworkId,
			gameMode = (int)GameManager.GameMode
		}));
		PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
		{
			FunctionName = "update_game_network_cross_play",
			FunctionParameter = functionParameter
		}, (ExecuteCloudScriptResult result) =>
		{
			Debug.Log("update_game_network_cross_play called");
		}, PlayFabManager.DisplayPlayFabError);
	}

	private void OnNetworkChanged(object sender, string newNetworkId)
	{
		Debug.Log($"OnNetworkChanged: {currentNetworkId} changed to {newNetworkId} isHost: {isHost}");
		_ = PlayFabMultiplayerManager.Get().RemotePlayers.Count;
		currentNetworkId = newNetworkId;
		if (GameManager.Crossplay)
		{
			UpdateGameNetworkCrossPlay(worldCode, newNetworkId, GameManager.Password);
		}
	}

	private void OnNetworkLeft(object sender, string networkId)
	{
		Debug.Log($"OnNetworkLeft: left the network as host: {isHost}");
		Debug.Log("OnNetworkLeft");
		CleanupState();
	}

	private IEnumerator SendWorld(Network_UserId id)
	{
		bool allInitialized = false;
		while (!allInitialized)
		{
			allInitialized = true;
			foreach (KeyValuePair<Network_UserId, Network_Player> remoteUser in remoteUsers)
			{
				if (!remoteUser.Value.initialized)
				{
					allInitialized = false;
					break;
				}
			}
			if (!allInitialized)
			{
				yield return null;
			}
		}
		Message_Compound message = new Message_Compound(GetWorld());
		SendP2P(id, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
	}

	private void ParseRemoteMessage(Message message, PlayFabPlayer from)
	{
		switch (message.Type)
		{
		case Messages.KickUser:
			ForceLeaveSession(LeaveSessionReason.Blocked);
			return;
		case Messages.InitiateResult:
		{
			Message_InitiateConnectionResult message_InitiateConnectionResult = message as Message_InitiateConnectionResult;
			if (connectedToHost)
			{
				return;
			}
			Debug.Log($"InitiateResult received = {message_InitiateConnectionResult.result}");
			if (!GameManager.IsLeavingGame)
			{
				if (message_InitiateConnectionResult.result == InitiateResult.Fail)
				{
					ForceLeaveSession(LeaveSessionReason.Blocked);
					return;
				}
				if (message_InitiateConnectionResult.result == InitiateResult.VersionZeroSuccess)
				{
					ForceLeaveSession(LeaveSessionReason.MissmatchVersion);
					return;
				}
				connectedToHost = true;
				hostID = from.EntityKey.Id;
			}
			break;
		}
		case Messages.NetworkVersion:
		{
			Message_NetworkVersion message_NetworkVersion = message as Message_NetworkVersion;
			if (connectedToHost)
			{
				return;
			}
			Debug.Log($"NetworkVersion received = {message_NetworkVersion.version}");
			if (!GameManager.IsLeavingGame)
			{
				if (message_NetworkVersion.version != 1)
				{
					ForceLeaveSession(LeaveSessionReason.MissmatchVersion);
					return;
				}
				connectedToHost = true;
				hostID = from.EntityKey.Id;
			}
			break;
		}
		case Messages.RequestWorld:
			if (isHost)
			{
				Debug.Log("Received a world request");
				StartCoroutine(SendWorld(from.EntityKey.Id));
			}
			break;
		case Messages.PlayerJoined:
		{
			Debug.Log("Received a player joined request");
			Message_PlayerJoined message_PlayerJoined = message as Message_PlayerJoined;
			if (message_PlayerJoined.characterSettings.Unlocks != byte.MaxValue)
			{
				SendP2P(from.EntityKey.Id, new Message(Messages.KickUser), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				return;
			}
			SendP2P(from.EntityKey.Id, new Message_NetworkVersion(Messages.NetworkVersion, 1), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Session);
			if (SceneManager.GetActiveScene().name == GameSceneName)
			{
				OnIncommingConnection(from.EntityKey.Id, message_PlayerJoined.characterSettings);
			}
			break;
		}
		}
		bool worldReceived = false;
		HandleMessage(message, from.EntityKey.Id, ref worldReceived);
		NetworkUpdateManager.DeserializeSingleMessage(message, from.EntityKey.Id);
		if ((!IsHost & worldReceived) && worldLoadedCoroutine == null)
		{
			Debug.Log("Start the post world load coroutine");
			worldLoadedCoroutine = StartCoroutine(WorldHasBeenLoaded());
		}
	}

	private unsafe void OnDataMessageReceived(object sender, PlayFabPlayer from, IntPtr ptr, uint size)
	{
		int num = LZ4Unity.Decompress(ptr, (IntPtr)LZ4Unity.DecompressedBuffer.GetUnsafePtr(), (int)size, LZ4Unity.DecompressedBuffer.Length);
		FastBufferReader reader = new FastBufferReader((byte*)LZ4Unity.DecompressedBuffer.GetUnsafeReadOnlyPtr(), Allocator.None, num);
		if (reader.TryBeginRead(num))
		{
			Message message = FastMessageDeserializer.DeserializeMessage<Message>(reader);
			if (message == null)
			{
				return;
			}
			if (message.Type == Messages.Compound)
			{
				foreach (Message message2 in (message as Message_Compound).messages)
				{
					ParseRemoteMessage(message2, from);
				}
				return;
			}
			ParseRemoteMessage(message, from);
		}
		else
		{
			Debug.LogError($"Failed to receive message size: {num}");
		}
	}

	private void ParseLocalMessage(Message message, Network_UserId remoteID)
	{
		if (message.Type == Messages.Compound)
		{
			foreach (Message message2 in (message as Message_Compound).messages)
			{
				NetworkUpdateManager.DeserializeSingleMessage(message2, remoteID);
			}
			return;
		}
		NetworkUpdateManager.DeserializeSingleMessage(message, remoteID);
	}

	private unsafe void SendP2PInternal(Message message, IEnumerable<PlayFabPlayer> remotes, DeliveryOption deliveryOption)
	{
		if (remotes != null && remotes.Any())
		{
			int num = 0;
			using (FastBufferWriter writer = new FastBufferWriter(256, Allocator.Temp, 10485760))
			{
				message.SerializeFast(writer);
				int num2 = LZ4Unity.Compress((IntPtr)writer.GetUnsafePtr(), (IntPtr)LZ4Unity.CompressedBuffer.GetUnsafePtr(), writer.Length, LZ4Unity.CompressedBuffer.Length);
				PlayFabMultiplayerManager.Get().SendDataMessage((IntPtr)LZ4Unity.CompressedBuffer.GetUnsafePtr(), (uint)num2, remotes, deliveryOption);
				num = num2;
			}
			totalBytesSent += num;
			lastSecondBytes += num;
			if ((float)num > maxBytePackage)
			{
				maxBytePackage = num;
			}
		}
	}

	public void SendP2P(Network_UserId steamID, Message message, EP2PSend sendType, NetworkChannel channel)
	{
		if (steamID == LocalSteamID)
		{
			ParseLocalMessage(message, steamID);
		}
		else
		{
			if (PlayFabMultiplayerManager.Get().State != PlayFabMultiplayerManagerState.ConnectedToNetwork)
			{
				return;
			}
			PlayFabPlayer playFabPlayer = null;
			foreach (PlayFabPlayer remotePlayer in PlayFabMultiplayerManager.Get().RemotePlayers)
			{
				if ((Network_UserId)remotePlayer.EntityKey.Id == steamID)
				{
					playFabPlayer = remotePlayer;
					break;
				}
			}
			if (playFabPlayer != null)
			{
				DeliveryOption deliveryOption = ((sendType == EP2PSend.k_EP2PSendReliable) ? DeliveryOption.Guaranteed : DeliveryOption.BestEffort);
				SendP2PInternal(message, new PlayFabPlayer[1] { playFabPlayer }, deliveryOption);
			}
		}
	}

	public void SendP2P(Network_UserId steamID, List<Message> messages, EP2PSend sendType, NetworkChannel channel)
	{
		if (steamID == LocalSteamID)
		{
			foreach (Message message in messages)
			{
				ParseLocalMessage(message, steamID);
			}
			return;
		}
		if (sendType == EP2PSend.k_EP2PSendReliable)
		{
			SendP2P(steamID, new Message_Compound(messages), sendType, channel);
			return;
		}
		foreach (Message message2 in messages)
		{
			SendP2P(steamID, message2, sendType, channel);
		}
	}

	public void RPC(Message message, Target target, EP2PSend sendType, NetworkChannel channel)
	{
		if (target == Target.All)
		{
			ParseLocalMessage(message, localSteamID);
		}
		if (PlayFabMultiplayerManager.Get().State != PlayFabMultiplayerManagerState.ConnectedToNetwork)
		{
			return;
		}
		IList<PlayFabPlayer> remotePlayers = PlayFabMultiplayerManager.Get().RemotePlayers;
		if (remotePlayers.Count != 0)
		{
			DeliveryOption deliveryOption = ((sendType == EP2PSend.k_EP2PSendReliable) ? DeliveryOption.Guaranteed : DeliveryOption.BestEffort);
			if (target == Target.All)
			{
				SendP2PInternal(message, remotePlayers, deliveryOption);
			}
			else
			{
				SendP2PInternal(message, remotePlayers, deliveryOption);
			}
		}
	}

	public void RPC(List<Message> messages, Target target, EP2PSend sendType, NetworkChannel channel)
	{
		foreach (Message message in messages)
		{
			RPC(message, target, sendType, channel);
		}
	}

	public void RPCExclude(Message message, Target target, Network_UserId excludeID, EP2PSend sendType, NetworkChannel channel)
	{
		if (PlayFabMultiplayerManager.Get().State != PlayFabMultiplayerManagerState.ConnectedToNetwork)
		{
			return;
		}
		IList<PlayFabPlayer> remotePlayers = PlayFabMultiplayerManager.Get().RemotePlayers;
		if (remotePlayers.Count == 0)
		{
			return;
		}
		List<PlayFabPlayer> list = new List<PlayFabPlayer>();
		foreach (PlayFabPlayer item in remotePlayers)
		{
			if (!((Network_UserId)item.EntityKey.Id == localSteamID) && !((Network_UserId)item.EntityKey.Id == excludeID))
			{
				list.Add(item);
			}
		}
		DeliveryOption deliveryOption = ((sendType == EP2PSend.k_EP2PSendReliable) ? DeliveryOption.Guaranteed : DeliveryOption.BestEffort);
		SendP2PInternal(message, list, deliveryOption);
	}

	public string PopErrorMessage()
	{
		if (errorMessage != "")
		{
			string result = string.Copy(errorMessage);
			errorMessage = "";
			return result;
		}
		return "";
	}

	public PlayFabPlayer GetPlayFabPlayerFromNetworkId(Network_UserId networkId)
	{
		if (PlayFabMultiplayerManager.Get().State == PlayFabMultiplayerManagerState.ConnectedToNetwork)
		{
			foreach (PlayFabPlayer remotePlayer in PlayFabMultiplayerManager.Get().RemotePlayers)
			{
				if ((Network_UserId)remotePlayer.EntityKey.Id == networkId)
				{
					return remotePlayer;
				}
			}
		}
		return null;
	}

	public bool GetPlayerInfoFromNetworkId(Network_UserId networkId, out PlayerInfo playerInfo)
	{
		return playerInfos.TryGetValue(networkId, out playerInfo);
	}

	public void TryToJoinGame(CSteamID hostId, string password)
	{
		if (m_currentSteamHost != hostId)
		{
			GameManager.Password = password;
			Message_InitiateConnection message_InitiateConnection = new Message_InitiateConnection(Messages.InitiateSession, Settings.AppBuildID, password);
			using FastBufferWriter writer = new FastBufferWriter(256, Allocator.Temp, 10485760);
			message_InitiateConnection.SerializeFast(writer);
			SteamNetworking.SendP2PPacket(hostId, writer.ToArray(), (uint)writer.Length, EP2PSend.k_EP2PSendReliable, 1);
		}
	}

	private void SendInitiateResultSteam(CSteamID remoteID, InitiateResult result)
	{
		Message_InitiateConnectionResultSteam message_InitiateConnectionResultSteam = new Message_InitiateConnectionResultSteam(Messages.InitiateResultSteam, result, EncodeConString(currentNetworkId, GameManager.GameMode, GameManager.Crossplay));
		using FastBufferWriter writer = new FastBufferWriter(256, Allocator.Temp, 10485760);
		message_InitiateConnectionResultSteam.SerializeFast(writer);
		SteamNetworking.SendP2PPacket(remoteID, writer.ToArray(), (uint)writer.Length, EP2PSend.k_EP2PSendReliable, 1);
	}

	private IEnumerator SendResultOnceNetworkIsEstablished(CSteamID remoteID)
	{
		while (currentNetworkId == string.Empty)
		{
			if (GameManager.IsLeavingGame || !m_hostedPlayFab)
			{
				yield break;
			}
			yield return null;
		}
		SendInitiateResultSteam(remoteID, InitiateResult.Success);
	}

	private void ReadP2P_Channel_Connecting()
	{
		if (!SteamNetworking.IsP2PPacketAvailable(out var pcubMsgSize, 1))
		{
			return;
		}
		byte[] array = new byte[pcubMsgSize];
		if (!SteamNetworking.ReadP2PPacket(array, pcubMsgSize, out var pcubMsgSize2, out var psteamIDRemote, 1))
		{
			return;
		}
		FastBufferReader reader = new FastBufferReader(array, Allocator.Temp, (int)pcubMsgSize2);
		if (reader.TryBeginRead((int)pcubMsgSize2))
		{
			Message message = FastMessageDeserializer.DeserializeMessage<Message>(reader);
			if (message == null)
			{
				SteamNetworking.CloseP2PSessionWithUser(psteamIDRemote);
				return;
			}
			switch (message.Type)
			{
			case Messages.InitiateSession:
			{
				if (!IsHost)
				{
					break;
				}
				Message_InitiateConnection message_InitiateConnection = message as Message_InitiateConnection;
				InitiateResult initiateResult = CanUserJoinMe(psteamIDRemote, message_InitiateConnection.appBuildID, message_InitiateConnection.password);
				if (initiateResult != InitiateResult.Success)
				{
					SendInitiateResultSteam(psteamIDRemote, initiateResult);
					break;
				}
				if (currentNetworkId != string.Empty)
				{
					SendInitiateResultSteam(psteamIDRemote, InitiateResult.Success);
					break;
				}
				if (!m_hostedPlayFab && CurrentRequestJoinAuthSetting != RequestJoinAuthSetting.ALLOW_NONE)
				{
					PlayFabMultiplayerManager.Get().CreateAndJoinNetwork(new PlayFabNetworkConfiguration
					{
						MaxPlayerCount = 10u
					});
					m_hostedPlayFab = true;
				}
				StartCoroutine(SendResultOnceNetworkIsEstablished(psteamIDRemote));
				break;
			}
			case Messages.InitiateResultSteam:
				if (!connectedToHost && m_currentSteamHost != psteamIDRemote)
				{
					Message_InitiateConnectionResultSteam message_InitiateConnectionResultSteam = message as Message_InitiateConnectionResultSteam;
					if (OnJoinResult != null)
					{
						OnJoinResult(psteamIDRemote, message_InitiateConnectionResultSteam.result);
					}
					if (message_InitiateConnectionResultSteam.result == InitiateResult.Success)
					{
						GameInfo gameInfo = DecodeConString(message_InitiateConnectionResultSteam.conString);
						JoinNetwork(gameInfo);
						m_currentSteamHost = psteamIDRemote;
						string text = psteamIDRemote.ToString();
						string text2 = (GameManager.HasPassword ? "_#PW#_:" : string.Empty);
						string pchValue = text + text2;
						SteamFriends.SetRichPresence(PresenceKeyConnect, pchValue);
						SteamFriends.SetRichPresence(PresenceKeyStatus, STATUS_RemoteClient);
					}
					SteamNetworking.CloseP2PSessionWithUser(psteamIDRemote);
				}
				break;
			}
		}
		else
		{
			Debug.LogError($"Failed to receive message size: {pcubMsgSize2}");
		}
	}
}
