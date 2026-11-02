#define UNITY_STANDALONE_WIN
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveAndLoad : MonoBehaviour, SceneEventInterface
{
	public struct LatestSaveInfo
	{
		public RGD_Game rgdGame;

		public DirectoryInfo rgdGameDirInfo;
	}

	public class Folders
	{
		public string Source { get; private set; }

		public string Target { get; private set; }

		public Folders(string source, string target)
		{
			Source = source;
			Target = target;
		}
	}

	public static Action LoadRGDAINetworkBehaviours;

	public static Action LoadComplete;

	public static string CurrentGameFileName;

	public static Guid WorldGuid;

	public static RGD_Game WorldToLoad;

	public static string AppPath;

	public static string WorldPath;

	public static string PlayerPath;

	public static string RGDUserPath;

	public static string BackupFolderPath;

	public static string BackupFolderName;

	public static bool IsGameLoading;

	public const string dateTimeFormattingSaveFile = "yyyy.MM.dd-H.mm";

	public const string dateTimeFormattingUI = "yyyy/MM/dd H:mm";

	public const int saveBackupCount = 8;

	public const string latestStringNameEnding = "-Latest";

	private string[] invalidFileNameChars = new string[4] { " ", ".", "!", "," };

	[HideInInspector]
	public Scene startScene;

	public GameObject unauthorizedExceptionWindow;

	private static uint globalObjectIndex;

	private static bool initialized;

	private static Network_Player localPlayer;

	private static RGD_Users rgd_users;

	public static WorldIndex WorldIndex;

	[SerializeField]
	private float autoSaveTimeDelay = 600f;

	[SerializeField]
	private float autoSaveTimer;

	public static int SaveIndex;

	public static bool IsPerformingASave;

	private List<RGD_Landmark> landmarkCache = new List<RGD_Landmark>(32);

	public static string GetSettingsPath()
	{
		ulong steamID = SteamUser.GetSteamID().m_SteamID;
		return Application.persistentDataPath + "\\User\\User_" + steamID + "\\Player\\";
	}

	private void Awake()
	{
		if (initialized)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		initialized = true;
		ComponentManager<SaveAndLoad>.Value = this;
		startScene = SceneManager.GetActiveScene();
		GameManager.QuickStartGame = startScene.name != Raft_Network.MenuSceneName;
		if (GameManager.QuickStartGame)
		{
			Raft_Network.GameSceneName = startScene.name;
			GameManager.IsInNewGame = true;
		}
		FileManager.OnFileSystemError = (Action<Exception, string, string>)Delegate.Combine(FileManager.OnFileSystemError, new Action<Exception, string, string>(SystemFailureDisplay));
		DontDestroyOnLoadManager.AddGameObject(gameObject);
	}

	private void Start()
	{
		ulong steamID = SteamUser.GetSteamID().m_SteamID;
		AppPath = Application.persistentDataPath + "\\User\\User_" + steamID + "\\";
		WorldPath = AppPath + "World\\";
		PlayerPath = AppPath + "Player\\";
		RGDUserPath = PlayerPath + "RGD_Users.rgd";
		BackupFolderName = "OldSaveSystem-Backup";
		BackupFolderPath = WorldPath + "\\" + BackupFolderName;
		CreateDirectory(Application.persistentDataPath + "\\User");
		CreateDirectory(AppPath);
		CreateNecessaryLoadDirectories();
	}

	private void OnDestroy()
	{
		FileManager.OnFileSystemError = (Action<Exception, string, string>)Delegate.Remove(FileManager.OnFileSystemError, new Action<Exception, string, string>(SystemFailureDisplay));
	}

	private void Update()
	{
		HandleAutoSave();
	}

	private void HandleAutoSave()
	{
		if (GeneralSettingsBox.AutoSave && Raft_Network.IsHost && !Raft_Network.InMenuScene && !GameManager.QuickStartGame)
		{
			autoSaveTimer += Time.deltaTime;
			if (autoSaveTimer >= autoSaveTimeDelay)
			{
				autoSaveTimer = 0f;
				SaveGame(sendRemoteMessage: true);
			}
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			LoadRGDAINetworkBehaviours = null;
			IsGameLoading = false;
			WorldGuid = Guid.Empty;
			globalObjectIndex = 0u;
			autoSaveTimer = 0f;
			WorldToLoad = null;
			SaveIndex = 0;
		}
	}

	public void SystemFailureDisplay(Exception e, string windowName, string url)
	{
		Helper.LogBuild("Something went wrong with file access " + e.Message);
		unauthorizedExceptionWindow.SetActive(value: true);
		unauthorizedExceptionWindow.GetComponentInChildren<InputField>().text = e.ToString();
		unauthorizedExceptionWindow.transform.FindChildRecursively("Unathorized File Title").GetComponent<Text>().text = windowName;
		unauthorizedExceptionWindow.GetComponentInChildren<OpenURL>().url = url;
	}

	public bool PlayerSaveExistsInWorld(Network_Player player)
	{
		if (player == null)
		{
			return false;
		}
		if (rgd_users == null)
		{
			LoadRGDUsers();
		}
		if (rgd_users != null)
		{
			return rgd_users.GetUser(WorldGuid, SteamUser.GetSteamID().m_SteamID) != null;
		}
		return false;
	}

	public static RGD_Game[] GetRGDGamesFromFolder(string folderPath)
	{
		List<RGD_Game> list = new List<RGD_Game>();
		FileInfo[] allRGDFileInfoFromFolder = GetAllRGDFileInfoFromFolder(folderPath);
		foreach (FileInfo fileInfo in allRGDFileInfoFromFolder)
		{
			if (fileInfo != null)
			{
				RGD_Game rGD_Game = Load<RGD_Game>(folderPath + fileInfo.Name);
				if (rGD_Game != null)
				{
					list.Add(rGD_Game);
				}
			}
		}
		list = list.OrderBy((RGD_Game g) => g.lastPlayedDateTicks).ToList();
		list.Reverse();
		return list.ToArray();
	}

	public async Task<RGD_Game[]> GetRGDGamesFromFolderAsync(string folderPath)
	{
		return await Task.Run(() => GetRGDGamesFromFolder(folderPath));
	}

	public static FileInfo[] GetAllRGDFileInfoFromFolder(string folderPath)
	{
		FileInfo[] files = new DirectoryInfo(folderPath).GetFiles();
		List<FileInfo> list = new List<FileInfo>();
		FileInfo[] array = files;
		foreach (FileInfo fileInfo in array)
		{
			if (fileInfo.Extension == ".rgd")
			{
				list.Add(fileInfo);
			}
		}
		return list.ToArray();
	}

	public string FindOldestSaveFromFolder(DirectoryInfo dirInfo)
	{
		DirectoryInfo[] directories = dirInfo.GetDirectories();
		DateTime b = DateTime.MaxValue;
		string result = "";
		DirectoryInfo[] array = directories;
		foreach (DirectoryInfo directoryInfo in array)
		{
			if (ConvertStringToDateTime(directoryInfo.Name, out var dateTime) && IsDateTimeNewer(dateTime, b))
			{
				b = dateTime;
				result = directoryInfo.FullName;
			}
		}
		return result;
	}

	public async Task<LatestSaveInfo> GetLatestRGDGameFromBackupFoldersAsync(DirectoryInfo worldDirInfo)
	{
		return await Task.Run(() =>
		{
			LatestSaveInfo result = default;
			DirectoryInfo rgdGameDirInfo = null;
			RGD_Game latestRGDGameFromBackupFolders = GetLatestRGDGameFromBackupFolders(worldDirInfo, out rgdGameDirInfo);
			result.rgdGame = latestRGDGameFromBackupFolders;
			result.rgdGameDirInfo = rgdGameDirInfo;
			return result;
		});
	}

	public static RGD_Game GetLatestRGDGameFromBackupFolders(DirectoryInfo worldDirInfo, out DirectoryInfo rgdGameDirInfo)
	{
		rgdGameDirInfo = null;
		DirectoryInfo[] directories = worldDirInfo.GetDirectories();
		List<DirectoryInfo> list = new List<DirectoryInfo>();
		DirectoryInfo[] array = directories;
		foreach (DirectoryInfo directoryInfo in array)
		{
			if (directoryInfo.FullName.EndsWith("-Latest"))
			{
				list.Add(directoryInfo);
			}
		}
		DateTime a = DateTime.MinValue;
		string text = string.Empty;
		foreach (DirectoryInfo item in list)
		{
			try
			{
				if (item.GetFiles().Length != 0)
				{
					ConvertBackupFolderNameToDateTime(item.Name, out var dateTime);
					if (IsDateTimeNewer(a, dateTime))
					{
						a = dateTime;
						text = item.FullName;
					}
				}
			}
			catch (DirectoryNotFoundException)
			{
				UnityEngine.Debug.LogWarning("Skipping inaccessible backup directory: " + item.FullName);
			}
		}
		bool flag = false;
		if (text != string.Empty)
		{
			rgdGameDirInfo = new DirectoryInfo(text);
			RGD_Game[] rGDGamesFromFolder = GetRGDGamesFromFolder(text + "\\");
			if (rGDGamesFromFolder.Length != 0)
			{
				flag = true;
				return rGDGamesFromFolder[0];
			}
			flag = false;
			return null;
		}
		if (!flag)
		{
			string text2 = string.Empty;
			DateTime value = DateTime.MinValue;
			array = directories;
			foreach (DirectoryInfo directoryInfo2 in array)
			{
				ConvertBackupFolderNameToDateTime(directoryInfo2.Name, out var dateTime2);
				if (dateTime2.CompareTo(value) > 0)
				{
					value = dateTime2;
					text2 = directoryInfo2.FullName;
				}
			}
			if (text2 != string.Empty)
			{
				RGD_Game[] rGDGamesFromFolder2 = GetRGDGamesFromFolder((rgdGameDirInfo = new DirectoryInfo(text2)).FullName + "\\");
				if (rGDGamesFromFolder2.Length != 0)
				{
					return rGDGamesFromFolder2[0];
				}
			}
		}
		return null;
	}

	private string GetLatestSaveDirectoryPath(string folderPath)
	{
		string[] directories = Directory.GetDirectories(folderPath);
		for (int i = 0; i < directories.Length; i++)
		{
			if (directories[i].EndsWith("-Latest"))
			{
				return directories[i];
			}
		}
		return "null";
	}

	private void CheckSaveLimit(string folderPath, int limit)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		string text = string.Empty;
		if (directories.Length > limit)
		{
			text = FindOldestSaveFromFolder(directoryInfo);
		}
		if (text != string.Empty)
		{
			try
			{
				Directory.Delete(text, recursive: true);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.Log(message);
			}
		}
	}

	public static bool ConvertBackupFolderNameToDateTime(string backupFolderName, out DateTime dateTime)
	{
		if (backupFolderName.EndsWith("-Latest"))
		{
			backupFolderName = backupFolderName.Substring(0, backupFolderName.Length - "-Latest".Length);
		}
		return ConvertStringToDateTime(backupFolderName, out dateTime);
	}

	private static bool ConvertStringToDateTime(string dateString, out DateTime dateTime)
	{
		bool result = false;
		if (DateTime.TryParseExact(dateString, "yyyy.MM.dd-H.mm", null, DateTimeStyles.AssumeLocal, out dateTime))
		{
			result = true;
		}
		return result;
	}

	private static bool IsDateTimeNewer(DateTime a, DateTime b)
	{
		return a.CompareTo(b) < 0;
	}

	public void CreateNecessaryLoadDirectories()
	{
		CreateDirectory(WorldPath);
		CreateDirectory(PlayerPath);
		CreateDirectory(BackupFolderPath);
	}

	private void CreateDirectoryAndMoveFile(string filePath, string directoryPath)
	{
		CreateDirectory(directoryPath);
		File.Move(filePath, directoryPath);
	}

	private DirectoryInfo CreateDirectory(string path)
	{
		try
		{
			if (!Directory.Exists(path))
			{
				return Directory.CreateDirectory(path);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning(ex.ToString() + " _|_ " + ex.Message);
		}
		return null;
	}

	private string CreateSaveDateFolderPath(string rootFolderPath, DateTime time)
	{
		return rootFolderPath + "\\" + time.ToString("yyyy.MM.dd-H.mm");
	}

	private void MoveDirectory(string source, string target)
	{
		Stack<Folders> stack = new Stack<Folders>();
		stack.Push(new Folders(source, target));
		while (stack.Count > 0)
		{
			Folders folders = stack.Pop();
			Directory.CreateDirectory(folders.Target);
			string[] files = Directory.GetFiles(folders.Source, "*.*");
			foreach (string text in files)
			{
				string text2 = Path.Combine(folders.Target, Path.GetFileName(text));
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				File.Move(text, text2);
			}
			files = Directory.GetDirectories(folders.Source);
			foreach (string text3 in files)
			{
				stack.Push(new Folders(text3, Path.Combine(folders.Target, Path.GetFileName(text3))));
			}
		}
		Directory.Delete(source, recursive: true);
	}

	public void ConvertAllOldSavesToNewFormat()
	{
		FileInfo[] allRGDFileInfoFromFolder = GetAllRGDFileInfoFromFolder(WorldPath);
		foreach (FileInfo fi in allRGDFileInfoFromFolder)
		{
			ConvertOldSaveFileIntoNewFormat(fi);
		}
	}

	private void ConvertOldSaveFileIntoNewFormat(FileInfo fi)
	{
		string text = fi.Name;
		string text2 = text.Substring(0, text.Length - 4);
		if (text2.Contains("."))
		{
			UnityEngine.Debug.Log("File " + text2 + " has '.'(period symbol) in name");
			return;
		}
		string text3 = fi.DirectoryName + "\\" + text2;
		bool flag = false;
		string[] array = null;
		string[] array2 = null;
		try
		{
			array = Directory.GetFiles(text3);
		}
		catch (Exception)
		{
		}
		try
		{
			array2 = Directory.GetDirectories(text3);
		}
		catch (Exception)
		{
		}
		if (Directory.Exists(text3))
		{
			if ((array != null && array.Length != 0) || (array2 != null && array2.Length != 0))
			{
				flag = true;
			}
		}
		else
		{
			CreateDirectory(text3);
		}
		if (flag)
		{
			return;
		}
		string text4 = CreateSaveDateFolderPath(text3, DateTime.Now) + "-Latest";
		try
		{
			fi.CopyTo(BackupFolderPath + "\\" + text);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.Log(message);
		}
		CreateDirectory(text4);
		try
		{
			fi.MoveTo(text4 + "\\" + text);
		}
		catch (Exception ex3)
		{
			UnityEngine.Debug.Log("Save file already existed at path " + text4 + "\n" + ex3);
		}
	}

	public static uint GetUniqueObjectIndex()
	{
		return ++globalObjectIndex;
	}

	private void SetNewGlobalObjectIndex(int newIndex)
	{
		globalObjectIndex = (uint)newIndex;
	}

	[Conditional("UNITY_STANDALONE_WIN")]
	private void Save(string filename, RGD raftgamedata)
	{
		if (!filename.EndsWith(".rgd"))
		{
			filename += ".rgd";
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = FileManager.CreateFile(filename);
		if (fileStream != null)
		{
			binaryFormatter.Serialize(fileStream, raftgamedata);
			fileStream.Close();
		}
	}

	public static T Load<T>(string filename)
	{
		if (!filename.EndsWith(".rgd"))
		{
			filename += ".rgd";
		}
		if (File.Exists(filename))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = FileManager.OpenFile(filename, FileMode.Open);
			bool flag = false;
			T result = default;
			if (fileStream != null)
			{
				try
				{
					result = (T)binaryFormatter.Deserialize(fileStream);
				}
				catch (Exception message)
				{
					flag = true;
					UnityEngine.Debug.Log(message);
					fileStream.Close();
				}
				fileStream.Close();
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				return result;
			}
		}
		return default;
	}

	public void ClearLandmarkCache()
	{
		UnityEngine.Debug.Log("Clear landmark cache");
		landmarkCache.Clear();
	}

	public void CreateLandmarkCache()
	{
		landmarkCache.Clear();
		for (int i = 0; i < WorldManager.AllLandmarks.Count; i++)
		{
			Landmark landmark = WorldManager.AllLandmarks[i];
			if (landmark != null)
			{
				RaftLandmark component = landmark.GetComponent<RaftLandmark>();
				if (component == null)
				{
					landmarkCache.Add(new RGD_Landmark(landmark));
				}
				else
				{
					landmarkCache.Add(new RGD_Landmark_Raft(component, landmark));
				}
			}
		}
		UnityEngine.Debug.Log($"Cached {landmarkCache.Count} landmarks");
	}

	public void SaveGame(bool sendRemoteMessage)
	{
		CanvasHelper value = ComponentManager<CanvasHelper>.Value;
		if (value != null)
		{
			value.SaveGameIcon(2f);
		}
		if (Raft_Network.IsHost)
		{
			SaveWorld();
		}
		SaveUser();
		SteamUserStats.StoreStats();
		if (!sendRemoteMessage)
		{
			return;
		}
		Raft_Network value2 = ComponentManager<Raft_Network>.Value;
		if (value2 != null)
		{
			if (Raft_Network.IsHost)
			{
				value2.RPC(new Message(Messages.Save), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				value2.SendP2P(value2.HostID, new Message(Messages.Save), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	private void SaveUser()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (!(localPlayer != null))
		{
			return;
		}
		if (rgd_users == null)
		{
			LoadRGDUsers();
			if (rgd_users == null)
			{
				rgd_users = new RGD_Users();
			}
		}
		if (rgd_users != null)
		{
			rgd_users.AddUser(WorldGuid, localPlayer);
			if (RGDUserPath != null)
			{
				Save(RGDUserPath, rgd_users);
			}
			Helper.LogBuild("Saving User");
		}
	}

	private async Task<byte[]> SerializeAndCompressType<T>(T objectToSerialize)
	{
		return await Task.Run(() =>
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter().Serialize(memoryStream, objectToSerialize);
			byte[] array = memoryStream.ToArray();
			byte[] dst = new byte[LZ4Unity.CompressBound(array.Length)];
			int num = LZ4Unity.Compress(array, ref dst);
			byte[] array2 = new byte[num];
			Array.Copy(dst, array2, num);
			int num2 = array.Length - num;
			UnityEngine.Debug.Log($"Saving {typeof(T).Name} original size: {array.Length} compressed size: {num} compression delta: {num2}");
			return array2;
		});
	}

	private async void SaveWorldAsync(RGD_Game worldSave, string directoryName, string fileName, int saveIndex)
	{
		IsPerformingASave = true;
		await SerializeAndCompressType(worldSave);
		IsPerformingASave = false;
	}

	private async void SaveUserAsync()
	{
		await SerializeAndCompressType(rgd_users);
	}

	public static string SanitizeFileName(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
		{
			return fileName;
		}
		return fileName.TrimEnd('.', ' ');
	}

	private void SaveWorld()
	{
		Helper.LogBuild("Saving world");
		if (GameManager.QuickStartGame)
		{
			CurrentGameFileName = "QuickStartGame";
		}
		DateTime now = DateTime.Now;
		string text = SanitizeFileName(CurrentGameFileName);
		string text2 = WorldPath + text;
		string text3 = CreateSaveDateFolderPath(text2, now);
		CreateDirectory(text2);
		string latestSaveDirectoryPath = GetLatestSaveDirectoryPath(text2);
		string target = latestSaveDirectoryPath.Replace("-Latest", "");
		if (!latestSaveDirectoryPath.Equals("null"))
		{
			try
			{
				MoveDirectory(latestSaveDirectoryPath, target);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}
		CreateDirectory(text3 + "-Latest");
		RGD_Game raftgamedata = CreateRGDGame();
		Save(text3 + "-Latest\\" + text, raftgamedata);
		CheckSaveLimit(WorldPath + text + "\\", 8);
	}

	public void RestoreUser()
	{
		if (rgd_users == null)
		{
			LoadRGDUsers();
		}
		Network_Player value = ComponentManager<Network_Player>.Value;
		rgd_users.GetUser(WorldGuid, SteamUser.GetSteamID().m_SteamID)?.rgdPlayer.RestorePlayer(value);
	}

	private IEnumerator LoadTextWriterLate(RGD_TextWriterObject rgdTextWriter, float delay)
	{
		yield return new WaitForSeconds(delay);
		MonoBehaviour_ID_Network networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex<TextWriterObject>(rgdTextWriter.connectedObjectIndex);
		if (networkIDFromObjectIndex != null)
		{
			TextWriterObject textWriterObject = networkIDFromObjectIndex as TextWriterObject;
			rgdTextWriter.RestoreTextWriterObject(textWriterObject);
		}
	}

	private Block RestoreBlock(BlockCreator blockCreator, RGD_Block rgdBlock)
	{
		if (rgdBlock == null)
		{
			return null;
		}
		bool p_corrected = false;
		int fixedObjectUniqueIndex = OutdatedObjectFixer.GetFixedObjectUniqueIndex(rgdBlock.BlockIndex, ref p_corrected);
		if (fixedObjectUniqueIndex == -1)
		{
			OutdatedObjectFixer.PerformObjectFixForNonCreatedBlock(rgdBlock.BlockIndex, rgdBlock);
		}
		Item_Base itemByIndex = ItemManager.GetItemByIndex(fixedObjectUniqueIndex);
		if (itemByIndex == null)
		{
			return null;
		}
		Block block = blockCreator.CreateBlock(itemByIndex, rgdBlock.LocalBlockPosition, rgdBlock.LocalBlockRotation, rgdBlock.DPSType, -1, replicating: true, rgdBlock.BlockObjectIndex, rgdBlock.NetworkedObjectIndex, NetworkUpdateManager.GetUniqueBehaviourIndex());
		rgdBlock.RestoreBlock(block);
		if (p_corrected)
		{
			OutdatedObjectFixer.PerformObjectFixForCreatedBlock(rgdBlock.BlockIndex, block);
		}
		return block;
	}

	public void LoadUsersAsync()
	{
	}

	private void LoadRGDUsers()
	{
		if (File.Exists(RGDUserPath))
		{
			rgd_users = Load<RGD_Users>(RGDUserPath);
		}
		else
		{
			rgd_users = new RGD_Users();
		}
	}

	private RGD_Game CreateRGDGame()
	{
		RGD_Game rGD_Game = new RGD_Game();
		if (landmarkCache.Count == 0)
		{
			UnityEngine.Debug.Log("First time saving world - create landmark cache");
			CreateLandmarkCache();
		}
		rGD_Game.landmarks = landmarkCache;
		DateTime now = DateTime.Now;
		rGD_Game.name = SanitizeFileName(CurrentGameFileName.TrimEnd());
		rGD_Game.lastPlayedDateTicks = now.Ticks;
		rGD_Game.mode = GameManager.GameMode;
		rGD_Game.biggestObjectIndex = (int)globalObjectIndex;
		rGD_Game.timePlayedSeconds = GameManager.TimePlayed;
		rGD_Game.versionNumber = (uint)Settings.SaveVersion;
		QuestItem[] questItems = QuestItemManager.QuestItems;
		if (questItems != null)
		{
			QuestItem[] array = questItems;
			foreach (QuestItem questItem in array)
			{
				if (questItem != null)
				{
					RGD_QuestItem item = new RGD_QuestItem(questItem);
					rGD_Game.landmarkQuestItems.Add(item);
				}
			}
		}
		List<Quest> quests = QuestProgressTracker.GetQuests();
		if (quests != null)
		{
			foreach (Quest item6 in quests)
			{
				if (item6 != null)
				{
					RGD_Quest item2 = new RGD_Quest(item6);
					rGD_Game.quests.Add(item2);
				}
			}
		}
		else
		{
			UnityEngine.Debug.Log("Unable to find quest data in saving");
		}
		WorldEvent[] worldEvents = WorldEventManager.worldEvents;
		if (worldEvents != null)
		{
			WorldEvent[] array2 = worldEvents;
			foreach (WorldEvent worldEvent in array2)
			{
				if (worldEvent != null)
				{
					RGD_WorldEvent item3 = new RGD_WorldEvent(worldEvent);
					rGD_Game.worldEvents.Add(item3);
				}
			}
		}
		else
		{
			UnityEngine.Debug.Log("Unable to find world events in saving");
		}
		ObjectSpawnerManager value = ComponentManager<ObjectSpawnerManager>.Value;
		if (value != null)
		{
			foreach (ObjectSpawner spawner in value.spawners)
			{
				foreach (PickupItem_Networked spawnedObject in spawner.spawnedObjects)
				{
					RGD_FloatingItem item4 = new RGD_FloatingItem(spawnedObject);
					rGD_Game.floatingItems.Add(item4);
				}
			}
		}
		for (int j = 0; j < WorldManager.AllLandmarks.Count; j++)
		{
			Landmark landmark = WorldManager.AllLandmarks[j];
			if (landmark != null && (landmark.isSpawned || rGD_Game.landmarks[j].rgdLandmarkSpawners != null))
			{
				rGD_Game.landmarks[j] = null;
				RaftLandmark component = landmark.GetComponent<RaftLandmark>();
				if (component == null)
				{
					rGD_Game.landmarks[j] = new RGD_Landmark(landmark);
				}
				else
				{
					rGD_Game.landmarks[j] = new RGD_Landmark_Raft(component, landmark);
				}
				landmarkCache[j] = rGD_Game.landmarks[j];
			}
		}
		MonoBehaviour_ID[] source = UnityEngine.Object.FindObjectsOfType<MonoBehaviour_ID>();
		TreasurePointManager value2 = ComponentManager<TreasurePointManager>.Value;
		List<MonoBehaviour_ID> list = source.ToList();
		foreach (KeyValuePair<Transform, List<TreasurePoint>> treasurePoint in value2.treasurePoints)
		{
			for (int k = 0; k < treasurePoint.Value.Count; k++)
			{
				if (treasurePoint.Value[k] != null && treasurePoint.Value[k].gameObject.activeSelf && !list.Contains(treasurePoint.Value[k]))
				{
					list.Add(treasurePoint.Value[k]);
				}
			}
		}
		source = list.ToArray();
		foreach (MonoBehaviour_ID monoBehaviour_ID in source)
		{
			if (monoBehaviour_ID == null)
			{
				continue;
			}
			RGD rGD = monoBehaviour_ID.Serialize_Save();
			if (rGD != null)
			{
				rGD_Game.behaviours.Add(rGD);
				continue;
			}
			List<RGD> list2 = monoBehaviour_ID.Serialize_SaveMultiple();
			if (list2.ContainsItems())
			{
				rGD_Game.behaviours.AddRange(list2);
			}
		}
		Network_Host value3 = ComponentManager<Network_Host>.Value;
		rGD_Game.world = new RGD_World(value3.skyController, value3.raft, value3.raftCollisionManager, value3.weatherManager);
		ChunkManager value4 = ComponentManager<ChunkManager>.Value;
		if (value4 != null)
		{
			List<ChunkPoint> allChunkPointsList = value4.GetAllChunkPointsList();
			List<RGD_ChunkPoint> list3 = new List<RGD_ChunkPoint>();
			foreach (ChunkPoint item7 in allChunkPointsList)
			{
				if (item7 != null && !item7.IsCorrupt)
				{
					RGD_ChunkPoint item5 = new RGD_ChunkPoint(item7);
					list3.Add(item5);
				}
			}
			if (list3.ContainsItems())
			{
				rGD_Game.world.rgdChunkPoints = list3.ToArray();
			}
			else
			{
				rGD_Game.world.rgdChunkPoints = null;
			}
			List<int> list4 = new List<int>();
			for (int m = 0; m < value4.currentSpawners.Count; m++)
			{
				list4.Add((int)value4.currentSpawners[m].UniqueIndex);
			}
			rGD_Game.world.chunkManagerSpawnerIndexes = list4.ToArray();
		}
		rGD_Game.researchTableWorld = new RGD_ResearchTableWorld(ComponentManager<Inventory_ResearchTable>.Value);
		NoteBook value5 = ComponentManager<NoteBook>.Value;
		rGD_Game.noteBook = new RGD_NoteBook(value5);
		rGD_Game.world.rgdRecieverFrequencies = new RGD_RecieverFrequencies();
		TradingPostUI value6 = ComponentManager<TradingPostUI>.Value;
		if (value6 != null)
		{
			rGD_Game.traderReputation = value6.CurrentReputation;
		}
		return rGD_Game;
	}

	public void RestoreRGDGame(RGD_Game game)
	{
		IsGameLoading = true;
		CurrentGameFileName = SanitizeFileName(game.name.TrimEnd());
		GameModeValueManager.SelectCurrentGameMode(game.mode);
		GameManager.TimePlayed = game.timePlayedSeconds;
		SetNewGlobalObjectIndex(game.biggestObjectIndex);
		if (game.landmarkQuestItems.Count > 0)
		{
			for (int i = 0; i < game.landmarkQuestItems.Count; i++)
			{
				RGD_QuestItem rGD_QuestItem = game.landmarkQuestItems[i];
				if (!(rGD_QuestItem == null))
				{
					QuestItemManager.GetQuestItemFromType(rGD_QuestItem.itemType)?.RestoreQuestItem(rGD_QuestItem);
				}
			}
		}
		else
		{
			UnityEngine.Debug.Log("Unable to find game questsItems in savefile");
		}
		if (game.worldEvents.Count > 0)
		{
			for (int j = 0; j < game.worldEvents.Count; j++)
			{
				RGD_WorldEvent rGD_WorldEvent = game.worldEvents[j];
				if (!(rGD_WorldEvent == null))
				{
					WorldEventManager.GetWorldEventFromType(rGD_WorldEvent.eventType)?.RestoreWorldEvent(rGD_WorldEvent);
				}
			}
		}
		if (game.quests.Count > 0)
		{
			for (int k = 0; k < game.quests.Count; k++)
			{
				RGD_Quest rGD_Quest = game.quests[k];
				if (rGD_Quest != null)
				{
					QuestProgressTracker.GetQuestByType(rGD_Quest.questType)?.RestoreQuest(rGD_Quest);
				}
			}
		}
		else
		{
			UnityEngine.Debug.Log("Unable to find game quests in savefile, defaulting as old save");
			if (game.world.radioTowerReached)
			{
				QuestProgressTracker.FinishQuestRPC(QuestType.RadioTowerReached);
				UnityEngine.Debug.Log("Setting wentToRadioTower quest to true since old save had it!");
			}
		}
		ObjectSpawnerManager value = ComponentManager<ObjectSpawnerManager>.Value;
		ChunkManager value2 = ComponentManager<ChunkManager>.Value;
		if (value != null && game.floatingItems != null)
		{
			foreach (RGD_FloatingItem rgdItem in game.floatingItems)
			{
				bool flag = false;
				ObjectSpawner objectSpawner = value.spawners.Find((ObjectSpawner s) => s.UniqueIndex == rgdItem.spawnerIndex);
				if (!(objectSpawner != null))
				{
					continue;
				}
				SpawnableFloatingObject spawnableFromIndex = objectSpawner.GetSpawnableFromIndex(rgdItem.uniqueSpawnableIndex);
				if (spawnableFromIndex != null)
				{
					if (game.versionNumber == 0 && spawnableFromIndex.typeToSpawn == ObjectSpawnType.AssetBundleScene)
					{
						OutdatedObjectFixer.ConvertLandmarkPoolIndexNumber(ref rgdItem.poolDuplicateIndex, spawnableFromIndex.assetBundleKey);
					}
					PickupItem_Networked pickupItem_Networked = objectSpawner.SpawnItem(rgdItem.Position.Value, rgdItem.Rotation.Value, spawnableFromIndex, rgdItem.floatingObjectIndex, NetworkUpdateManager.GetUniqueBehaviourIndex(), rgdItem.poolDuplicateIndex);
					if (flag && pickupItem_Networked != null && value2 != null && objectSpawner is ObjectSpawner_Chunk)
					{
						ChunkPoint chunkPoint = new ChunkPoint();
						chunkPoint.spawner = objectSpawner as ObjectSpawner_Chunk;
						chunkPoint.worldPosition = pickupItem_Networked.transform.position;
						chunkPoint.spawnableFloatingObject = spawnableFromIndex;
						chunkPoint.IsAddedByForce = false;
						chunkPoint.spawnedObject = pickupItem_Networked;
						chunkPoint.poolDuplicateIndex = pickupItem_Networked.poolDuplicateIndex;
						value2.AddChunkPoint(chunkPoint);
					}
				}
			}
		}
		if (game.world != null && value2 != null)
		{
			RGD_ChunkPoint[] rgdChunkPoints = game.world.rgdChunkPoints;
			if (rgdChunkPoints.ContainsItems())
			{
				RGD_ChunkPoint[] array = rgdChunkPoints;
				foreach (RGD_ChunkPoint rgdPoint in array)
				{
					value2.AddRGDChunkPoint(rgdPoint);
				}
			}
			if (game.world.chunkManagerSpawnerIndexes.ContainsItems())
			{
				int[] chunkManagerSpawnerIndexes = game.world.chunkManagerSpawnerIndexes;
				foreach (int rgdSpawnerIndex in chunkManagerSpawnerIndexes)
				{
					if (rgdSpawnerIndex >= 0)
					{
						ObjectSpawner_Chunk objectSpawner_Chunk = value2.allChunkSpawners.Find((ObjectSpawner_Chunk s) => s.UniqueIndex == rgdSpawnerIndex);
						if (objectSpawner_Chunk != null && !value2.currentSpawners.Contains(objectSpawner_Chunk))
						{
							value2.AddSpawnerToCurrentSpawners(objectSpawner_Chunk);
						}
					}
				}
			}
			if (value != null && value.spawners.ContainsItems() && value2 != null)
			{
				Dictionary<ChunkPointType, List<ChunkPoint>> allChunkPointsDictionary = value2.GetAllChunkPointsDictionary();
				for (int num3 = 0; num3 < value.spawners.Count; num3++)
				{
					ObjectSpawner objectSpawner2 = value.spawners[num3];
					if (objectSpawner2 == null || !objectSpawner2.spawnedObjects.ContainsItems() || !(objectSpawner2 is ObjectSpawner_Chunk))
					{
						continue;
					}
					ObjectSpawner_Chunk objectSpawner_Chunk2 = objectSpawner2 as ObjectSpawner_Chunk;
					if (objectSpawner_Chunk2 == null)
					{
						continue;
					}
					List<PickupItem_Networked> list = new List<PickupItem_Networked>();
					if (allChunkPointsDictionary != null && allChunkPointsDictionary.ContainsKey(objectSpawner_Chunk2.ChunkPointType))
					{
						List<ChunkPoint> list2 = allChunkPointsDictionary[objectSpawner_Chunk2.ChunkPointType];
						if (list2 == null)
						{
							continue;
						}
						for (int num4 = 0; num4 < objectSpawner_Chunk2.spawnedObjects.Count; num4++)
						{
							PickupItem_Networked pickupItem_Networked2 = objectSpawner_Chunk2.spawnedObjects[num4];
							if (pickupItem_Networked2 == null)
							{
								continue;
							}
							bool flag2 = false;
							for (int num5 = 0; num5 < list2.Count; num5++)
							{
								ChunkPoint chunkPoint2 = list2[num5];
								if (chunkPoint2 != null && chunkPoint2.spawnedObject != null && chunkPoint2.spawnedObject == pickupItem_Networked2)
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								list.AddUniqueOnly(pickupItem_Networked2);
							}
						}
						if (!list.ContainsItems())
						{
							continue;
						}
						for (int num6 = 0; num6 < list.Count; num6++)
						{
							PickupItem_Networked pickupItem_Networked3 = list[num6];
							if (!(pickupItem_Networked3 == null))
							{
								UnityEngine.Debug.Log("Removing landmark " + pickupItem_Networked3.name + " because there was no matching Chunkpoint");
								objectSpawner_Chunk2.RemoveItem(list[num6], removeUsingNetwork: false);
							}
						}
					}
					else
					{
						objectSpawner_Chunk2.ClearItems(clearUsingNetwork: false);
					}
				}
				if (rgdChunkPoints == null || rgdChunkPoints.Length == 0)
				{
					value2.GenerateChunkDataStartOfGame();
				}
			}
		}
		if (game.landmarks != null && WorldManager.AllLandmarks != null)
		{
			for (int num7 = 0; num7 < WorldManager.AllLandmarks.Count; num7++)
			{
				Landmark landmark = WorldManager.AllLandmarks[num7];
				if (!(landmark == null))
				{
					RGD_Landmark rGD_Landmark = game.landmarks.Find((RGD_Landmark l) => l.uniqueLandmarkIndex == landmark.uniqueLandmarkIndex);
					if (rGD_Landmark != null)
					{
						rGD_Landmark.RestoreLandmark(landmark, isHost: true);
					}
					else
					{
						landmark.AssignItemIndexes();
					}
				}
			}
		}
		BedManager bedManager = UnityEngine.Object.FindObjectOfType<BedManager>();
		Network_Host value3 = ComponentManager<Network_Host>.Value;
		NetworkIDManager value4 = ComponentManager<NetworkIDManager>.Value;
		Network_Player network_Player = ComponentManager<Raft_Network>.Value.GetLocalPlayer();
		PickupObjectManager value5 = ComponentManager<PickupObjectManager>.Value;
		UnityEngine.Object.FindObjectOfType<Buoyancy>();
		Network_Host_Entities value6 = ComponentManager<Network_Host_Entities>.Value;
		ObjectManager value7 = ComponentManager<ObjectManager>.Value;
		TreasurePointManager value8 = ComponentManager<TreasurePointManager>.Value;
		ChatManager value9 = ComponentManager<ChatManager>.Value;
		QuestItemManager value10 = ComponentManager<QuestItemManager>.Value;
		value6.rgdAINetworkBehaviour.Clear();
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		bool flag9 = false;
		bool flag10 = false;
		if (game.behaviours != null)
		{
			game.behaviours = game.behaviours.OrderBy((RGD b) => b.SortingOrder).ToList();
			for (int num8 = 0; num8 < game.behaviours.Count; num8++)
			{
				RGD rGD = game.behaviours[num8];
				switch (rGD.Type)
				{
				case RGDType.NetworkBehaviour_NetworkHost:
					(rGD as RGD_NetworkBehaviour).RestoreBehaviour(value3);
					flag3 = true;
					break;
				case RGDType.NetworkBehaviour_BedManager:
					(rGD as RGD_NetworkBehaviour).RestoreBehaviour(bedManager);
					flag4 = true;
					break;
				case RGDType.NetworkBehaviour_NetworkEntities:
					(rGD as RGD_NetworkBehaviour).RestoreIDBehaviour(value6);
					flag5 = true;
					break;
				case RGDType.NetworkBehaviour_ObjectSpawnerManager:
					(rGD as RGD_NetworkBehaviour).RestoreBehaviour(value);
					flag6 = true;
					break;
				case RGDType.NetworkBehaviour_PickupObjectManager:
					(rGD as RGD_NetworkBehaviour).RestoreBehaviour(value5);
					flag7 = true;
					break;
				case RGDType.NetworkBehaviour_ChatManager:
					(rGD as RGD_NetworkBehaviour).RestoreBehaviour(value9);
					flag8 = true;
					break;
				case RGDType.NetworkBehaviour_NetworkIDManager:
					(rGD as RGD_NetworkBehaviour).RestoreBehaviour(value4);
					flag9 = true;
					break;
				case RGDType.NetworkBehaviour_QuestItemManager:
					(rGD as RGD_NetworkBehaviour).RestoreIDBehaviour(value10);
					flag10 = true;
					break;
				case RGDType.Block:
				case RGDType.Block_CookingPot:
				case RGDType.Block_FuelTank:
				case RGDType.Block_ZiplineBase:
				case RGDType.Block_Foundation:
				case RGDType.Block_Firework:
				case RGDType.Block_Electric_Purifier:
				case RGDType.Block_EngineControls:
				case RGDType.Block_Fence:
				case RGDType.Block_Interactable:
				case RGDType.Block_TicTacToe:
				case RGDType.Block_ShortcutPlank:
				{
					RGD_Block rGD_Block = rGD as RGD_Block;
					if (rGD_Block != null)
					{
						RestoreBlock(network_Player.BlockCreator, rGD_Block);
					}
					break;
				}
				case RGDType.Block_Door:
				{
					RGD_Door rGD_Door = rGD as RGD_Door;
					Block block12 = RestoreBlock(network_Player.BlockCreator, rGD_Door);
					if (block12 != null)
					{
						rGD_Door.Restore(block12.GetComponent<Door>());
					}
					break;
				}
				case RGDType.Block_Cropplot:
				{
					RGD_Cropplot rGD_Cropplot = rGD as RGD_Cropplot;
					Block block8 = RestoreBlock(network_Player.BlockCreator, rGD_Cropplot);
					if (block8 != null)
					{
						rGD_Cropplot.RestoreCropplot(block8.GetComponent<Cropplot>(), network_Player.PlantManager);
					}
					break;
				}
				case RGDType.Block_CookingStand:
				case RGDType.Block_Electric_CookingStand:
				{
					RGD_CookingStand rGD_CookingStand = rGD as RGD_CookingStand;
					Block block5 = RestoreBlock(network_Player.BlockCreator, rGD_CookingStand);
					if (block5 != null)
					{
						rGD_CookingStand.RestoreStand(block5.GetComponent<Block_CookingStand>());
					}
					break;
				}
				case RGDType.Block_Storage:
				{
					RGD_Storage rGD_Storage = rGD as RGD_Storage;
					Block block3 = RestoreBlock(network_Player.BlockCreator, rGD_Storage);
					if (block3 != null)
					{
						rGD_Storage.RestoreInventory(block3.GetComponent<Storage_Small>().GetInventoryReference());
					}
					break;
				}
				case RGDType.Block_AnchorStationary:
				{
					RGD_AnchorStationary rGD_AnchorStationary = rGD as RGD_AnchorStationary;
					Block block24 = RestoreBlock(network_Player.BlockCreator, rGD_AnchorStationary);
					if (block24 != null)
					{
						rGD_AnchorStationary.RestoreAnchor(block24.GetComponent<Anchor_Stationary>());
					}
					break;
				}
				case RGDType.Block_AnchorThrowable:
				{
					RGD_AnchorThrowable rGD_AnchorThrowable = rGD as RGD_AnchorThrowable;
					Block block22 = RestoreBlock(network_Player.BlockCreator, rGD_AnchorThrowable);
					if (block22 != null)
					{
						rGD_AnchorThrowable.RestoreAnchorThrowable(block22, block22.GetComponent<Anchor_Throwable_Stand>());
					}
					break;
				}
				case RGDType.Block_ItemCollector:
				{
					RGD_ItemCollector rGD_ItemCollector = rGD as RGD_ItemCollector;
					Block block20 = RestoreBlock(network_Player.BlockCreator, rGD_ItemCollector);
					if (block20 != null)
					{
						rGD_ItemCollector.RestoreItemCollector(block20.GetComponentInChildren<ItemCollector>());
					}
					break;
				}
				case RGDType.Block_Scarecrow:
				{
					RGD_Scarecrow rGD_Scarecrow = rGD as RGD_Scarecrow;
					Block block18 = RestoreBlock(network_Player.BlockCreator, rGD_Scarecrow);
					if (block18 != null)
					{
						rGD_Scarecrow.RestoreScarecrow(block18.GetComponent<Scarecrow>());
					}
					break;
				}
				case RGDType.Block_Brick:
				{
					RGD_Brick rGD_Brick = rGD as RGD_Brick;
					Block block16 = RestoreBlock(network_Player.BlockCreator, rGD_Brick);
					if (block16 != null)
					{
						rGD_Brick.RestoreBrick(block16 as Brick_Wet);
					}
					break;
				}
				case RGDType.Block_BirdsNest:
				{
					RGD_Birdsnest rGD_Birdsnest = rGD as RGD_Birdsnest;
					Block block14 = RestoreBlock(network_Player.BlockCreator, rGD_Birdsnest);
					if (block14 != null)
					{
						rGD_Birdsnest.RestoreBirdsNest(block14.GetComponent<BirdsNest>());
					}
					break;
				}
				case RGDType.Block_ResearchTable:
				{
					RGD_ResearchTable rGD_ResearchTable = rGD as RGD_ResearchTable;
					Block block11 = RestoreBlock(network_Player.BlockCreator, rGD_ResearchTable);
					if (block11 != null)
					{
						rGD_ResearchTable.RestoreResearchTable(block11.GetComponent<ResearchTable>());
					}
					break;
				}
				case RGDType.Block_Sail:
				{
					RGD_Sail rGD_Sail = rGD as RGD_Sail;
					Block block9 = RestoreBlock(network_Player.BlockCreator, rGD_Sail);
					if (block9 != null)
					{
						rGD_Sail.RestoreSail(block9.GetComponent<Sail>());
					}
					break;
				}
				case RGDType.Block_Sharkbait:
				{
					RGD_Sharkbait rGD_Sharkbait = rGD as RGD_Sharkbait;
					Block block7 = RestoreBlock(network_Player.BlockCreator, rGD_Sharkbait);
					if (block7 != null)
					{
						rGD_Sharkbait.RestoreSharbait(block7.GetComponent<SharkBait>());
					}
					break;
				}
				case RGDType.Block_Reciever:
				{
					RGD_Reciever rGD_Reciever = rGD as RGD_Reciever;
					Block block4 = RestoreBlock(network_Player.BlockCreator, rGD_Reciever);
					if (block4 != null)
					{
						Reciever component2 = block4.GetComponent<Reciever>();
						if (component2 != null)
						{
							component2.Restore(rGD_Reciever);
						}
					}
					break;
				}
				case RGDType.Block_BioFuelCreator:
				{
					RGD_Block_BiofuelExtractor rGD_Block_BiofuelExtractor = rGD as RGD_Block_BiofuelExtractor;
					Block block2 = RestoreBlock(network_Player.BlockCreator, rGD_Block_BiofuelExtractor);
					if (block2 != null)
					{
						Placeable_BiofuelExtractor component = block2.GetComponent<Placeable_BiofuelExtractor>();
						if (component != null)
						{
							component.RestoreExtractor(rGD_Block_BiofuelExtractor.rgdBioFuelCreator);
						}
					}
					break;
				}
				case RGDType.Block_Recycler:
				{
					RGD_Block_Recycler rGD_Block_Recycler = rGD as RGD_Block_Recycler;
					Block block23 = RestoreBlock(network_Player.BlockCreator, rGD_Block_Recycler);
					if (block23 != null)
					{
						Placeable_RecyclerExtractor component3 = block23.GetComponent<Placeable_RecyclerExtractor>();
						if (component3 != null)
						{
							component3.RestoreExtractor(rGD_Block_Recycler.rgdRecycler);
						}
					}
					break;
				}
				case RGDType.Block_Sprinkler:
				{
					RGD_Block_Sprinkler rGD_Block_Sprinkler = rGD as RGD_Block_Sprinkler;
					Block block21 = RestoreBlock(network_Player.BlockCreator, rGD_Block_Sprinkler);
					if (block21 != null)
					{
						Sprinkler componentInChildren8 = block21.GetComponentInChildren<Sprinkler>();
						if (componentInChildren8 != null)
						{
							rGD_Block_Sprinkler.rgdSprinkler.RestoreSprinkler(componentInChildren8);
						}
					}
					break;
				}
				case RGDType.Block_Radio:
				{
					RGD_Block_Radio rGD_Block_Radio = rGD as RGD_Block_Radio;
					Block block19 = RestoreBlock(network_Player.BlockCreator, rGD_Block_Radio);
					if (block19 != null)
					{
						RadioPlayer componentInChildren7 = block19.GetComponentInChildren<RadioPlayer>();
						if (componentInChildren7 != null)
						{
							componentInChildren7.RestoreRadio(rGD_Block_Radio.rgdRadio);
						}
					}
					break;
				}
				case RGDType.Block_SteeringWheel:
				{
					RGD_SteeringWheel rGD_SteeringWheel = rGD as RGD_SteeringWheel;
					Block block17 = RestoreBlock(network_Player.BlockCreator, rGD_SteeringWheel);
					if (block17 != null)
					{
						SteeringWheel componentInChildren6 = block17.GetComponentInChildren<SteeringWheel>();
						if (componentInChildren6 != null)
						{
							componentInChildren6.RestoreWheel(rGD_SteeringWheel);
						}
					}
					break;
				}
				case RGDType.Block_Motor_Wheel:
				{
					RGD_MotorWheel rGD_MotorWheel = rGD as RGD_MotorWheel;
					Block block15 = RestoreBlock(network_Player.BlockCreator, rGD_MotorWheel);
					if (block15 != null)
					{
						MotorWheel componentInChildren5 = block15.GetComponentInChildren<MotorWheel>();
						if (componentInChildren5 != null)
						{
							componentInChildren5.RestoreMotor(rGD_MotorWheel);
						}
					}
					break;
				}
				case RGDType.Block_BatteryCharger:
				{
					RGD_Block_BatteryCharger rGD_Block_BatteryCharger = rGD as RGD_Block_BatteryCharger;
					Block block13 = RestoreBlock(network_Player.BlockCreator, rGD_Block_BatteryCharger);
					if (block13 != null)
					{
						BatteryCharger componentInChildren4 = block13.GetComponentInChildren<BatteryCharger>();
						if (componentInChildren4 != null)
						{
							rGD_Block_BatteryCharger.rgdBatteryCharger.RestoreBatteryCharger(componentInChildren4);
						}
					}
					break;
				}
				case RGDType.Block_WindTurbine:
				{
					RGD_Block_WindTurbine rGD_Block_WindTurbine = rGD as RGD_Block_WindTurbine;
					Block block10 = RestoreBlock(network_Player.BlockCreator, rGD_Block_WindTurbine);
					if (block10 != null)
					{
						WindTurbine componentInChildren3 = block10.GetComponentInChildren<WindTurbine>();
						if (componentInChildren3 != null)
						{
							rGD_Block_WindTurbine.rgdWindTurbine.Restore(componentInChildren3);
						}
					}
					break;
				}
				case RGDType.Entity_Seagull:
				{
					RGD_Seagull rGD_Seagull = rGD as RGD_Seagull;
					Seagull seagull = value6.CreateSeagull(rGD_Seagull.Position.Value, rGD_Seagull.entityObjectIndex, rGD_Seagull.objectIndex, NetworkUpdateManager.GetUniqueBehaviourIndex());
					if (seagull != null)
					{
						rGD_Seagull.RestoreSeagull(seagull);
					}
					break;
				}
				case RGDType.ID_TextWriterObject:
				{
					RGD_TextWriterObject rGD_TextWriterObject = rGD as RGD_TextWriterObject;
					if (rGD_TextWriterObject != null)
					{
						StartCoroutine(LoadTextWriterLate(rGD_TextWriterObject, 0.6f));
					}
					break;
				}
				case RGDType.AI_NetworkBehaviour:
				{
					RGD_AI_NetworkBehaviour rGD_AI_NetworkBehaviour = rGD as RGD_AI_NetworkBehaviour;
					if (rGD_AI_NetworkBehaviour != null)
					{
						value6.rgdAINetworkBehaviour.Add(rGD_AI_NetworkBehaviour);
					}
					break;
				}
				case RGDType.ID_TrophyHolder:
				{
					RGDTrophyHolder rGDTrophyHolder = rGD as RGDTrophyHolder;
					Block block6 = RestoreBlock(network_Player.BlockCreator, rGDTrophyHolder);
					if (block6 != null)
					{
						TrophyHolder componentInChildren2 = block6.GetComponentInChildren<TrophyHolder>();
						if (componentInChildren2 != null)
						{
							rGDTrophyHolder.RestoreTrophyHolder(componentInChildren2);
						}
					}
					break;
				}
				case RGDType.ID_ChickenEggs:
				{
					RGD_Chicken_Eggs rGD_Chicken_Eggs = rGD as RGD_Chicken_Eggs;
					if (!(rGD_Chicken_Eggs != null) || !rGD_Chicken_Eggs.rgdEggs.ContainsItems())
					{
						break;
					}
					RGD_Egg[] rgdEggs = rGD_Chicken_Eggs.rgdEggs;
					foreach (RGD_Egg rGD_Egg in rgdEggs)
					{
						if (rGD_Egg != null)
						{
							value7.Object_Chicken.LayEgg(rGD_Egg.localRaftPosition.Value, rGD_Egg.eggObjectIndex);
						}
					}
					break;
				}
				case RGDType.Block_BeeHive:
				{
					RGD_Block_BeeHive rGD_Block_BeeHive = rGD as RGD_Block_BeeHive;
					Block block = RestoreBlock(network_Player.BlockCreator, rGD_Block_BeeHive);
					if (rGD_Block_BeeHive != null && block != null)
					{
						BeeHive componentInChildren = block.GetComponentInChildren<BeeHive>();
						if (componentInChildren != null)
						{
							rGD_Block_BeeHive.RestoreBeehive(componentInChildren);
						}
					}
					break;
				}
				case RGDType.TreasurePoint_Landmark:
					(rGD as RGD_TreasurePoint_Landmark).CreateAndRestoreTreasurePoint(value8);
					break;
				case RGDType.TreasurePoint:
					UnityEngine.Debug.Log("Normal treasure points do nothing on restore atm.");
					break;
				}
			}
			if (!flag3 && value3 != null)
			{
				NetworkUpdateManager.SetIndexForBehaviour(value3);
				NetworkUpdateManager.AddBehaviour(value3);
			}
			if (!flag4 && bedManager != null)
			{
				NetworkUpdateManager.SetIndexForBehaviour(bedManager);
				NetworkUpdateManager.AddBehaviour(bedManager);
			}
			if (!flag5 && value6 != null)
			{
				value6.ObjectIndex = GetUniqueObjectIndex();
				NetworkIDManager.AddNetworkID(value6);
			}
			if (!flag6 && value != null)
			{
				NetworkUpdateManager.SetIndexForBehaviour(value);
				NetworkUpdateManager.AddBehaviour(value);
			}
			if (!flag7 && value5 != null)
			{
				NetworkUpdateManager.SetIndexForBehaviour(value5);
				NetworkUpdateManager.AddBehaviour(value5);
			}
			if (!flag8 && value9 != null)
			{
				NetworkUpdateManager.SetIndexForBehaviour(value9);
				NetworkUpdateManager.AddBehaviour(value9);
			}
			if (!flag9 && value4 != null)
			{
				NetworkUpdateManager.SetIndexForBehaviour(value4);
				NetworkUpdateManager.AddBehaviour(value4);
			}
			if (!flag10 && value10 != null)
			{
				value10.ObjectIndex = GetUniqueObjectIndex();
				NetworkIDManager.AddNetworkID(value10);
			}
			if (LoadRGDAINetworkBehaviours != null)
			{
				LoadRGDAINetworkBehaviours();
			}
		}
		RaftBounds value11 = ComponentManager<RaftBounds>.Value;
		if (value11 != null)
		{
			value11.Initialize();
		}
		RaftCollisionManager value12 = ComponentManager<RaftCollisionManager>.Value;
		if (value12 != null)
		{
			value12.Initialize();
		}
		Network_Host value13 = ComponentManager<Network_Host>.Value;
		if (game.world != null)
		{
			game.world.RestoreWorld(value13.skyController, value13.raft, value13.raftCollisionManager, value13.weatherManager);
		}
		if (game.researchTableWorld != null)
		{
			game.researchTableWorld.RestoreResearchTableWorld(ComponentManager<Inventory_ResearchTable>.Value);
		}
		if (game.noteBook != null)
		{
			game.noteBook.RestoreNoteBook(ComponentManager<NoteBook>.Value);
		}
		if (game.world != null && game.world.rgdRecieverFrequencies != null)
		{
			game.world.rgdRecieverFrequencies.RestoreFrequencies();
		}
		TradingPostUI value14 = ComponentManager<TradingPostUI>.Value;
		if (value14 != null)
		{
			value14.CurrentReputation = game.traderReputation;
		}
		if (LoadComplete != null)
		{
			LoadComplete();
		}
		IsGameLoading = false;
	}
}
