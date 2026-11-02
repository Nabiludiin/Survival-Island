using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AssetBundleHooker))]
public class AssetBundleManager : MonoBehaviour, SceneEventInterface
{
	public enum AtStartSetting
	{
		DoNotLoad,
		LoadBundle,
		LoadBundleAndScenes
	}

	public bool loadScenesAsync = true;

	public AtStartSetting atStartSetting;

	public bool allowSceneActivation;

	[SerializeField]
	private List<AsyncedLandmark> asyncedLandmarks;

	public static bool isInitialized = false;

	private static string fullFilePath = Application.streamingAssetsPath + "/AssetBundles";

	[SerializeField]
	private string assetBundleTag;

	private string fileDirectory;

	private AssetBundleHooker assetHooker;

	private AssetBundle bundleLandmark;

	private Coroutine loadSceneEnumerator;

	private Coroutine loadBundleEnumerator;

	private static bool allScenesLoaded = false;

	private static bool startedLoadingBundle = false;

	private static bool startedLoadingScenes = false;

	public List<AsyncedLandmark> AsyncedLandmarks => asyncedLandmarks;

	public bool IsBundleLoaded => bundleLandmark != null;

	public static bool IsAllScenesLoaded
	{
		get
		{
			return allScenesLoaded;
		}
		set
		{
			allScenesLoaded = value;
		}
	}

	private void OnApplicationQuit()
	{
		StopAllCoroutines();
		UnloadBundle(removeObjects: true);
	}

	private void Awake()
	{
		if (isInitialized)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		DontDestroyOnLoadManager.AddGameObject(gameObject);
		asyncedLandmarks = new List<AsyncedLandmark>();
		CreateAssetBundleDirectory();
		if (assetHooker == null)
		{
			assetHooker = GetComponent<AssetBundleHooker>();
		}
		SceneManager.sceneLoaded += SceneLoad;
		isInitialized = true;
	}

	private void Start()
	{
		if (GameManager.QuickStartGame && atStartSetting == AtStartSetting.DoNotLoad)
		{
			IsAllScenesLoaded = true;
		}
	}

	private void SceneLoad(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == Raft_Network.GameSceneName)
		{
			OnLoadGame();
		}
	}

	public void LoadAssetBundle()
	{
		loadBundleEnumerator = StartCoroutine(LoadBundleFromFileAsync());
	}

	public void AddLandmarksToHooker()
	{
		for (int i = 0; i < asyncedLandmarks.Count; i++)
		{
			assetHooker.SortIntoCorrectList(asyncedLandmarks[i]);
		}
	}

	public void OnLoadGame()
	{
		switch (atStartSetting)
		{
		case AtStartSetting.LoadBundleAndScenes:
			if (!startedLoadingBundle)
			{
				LoadAssetBundle();
			}
			if (!IsAllScenesLoaded)
			{
				StartSceneLoading();
			}
			break;
		case AtStartSetting.DoNotLoad:
			IsAllScenesLoaded = true;
			break;
		case AtStartSetting.LoadBundle:
			break;
		}
	}

	public void StartSceneLoading()
	{
		if (loadScenesAsync)
		{
			if (!startedLoadingScenes)
			{
				loadSceneEnumerator = StartCoroutine(LoadScenePaths());
			}
		}
		else
		{
			LoadScenePathsNonAsync();
		}
	}

	public void UnloadScene(string p_sceneName)
	{
		Scene sceneByName = SceneManager.GetSceneByName(p_sceneName);
		SceneManager.UnloadSceneAsync(sceneByName);
	}

	public void SetAllLandmarksActivity(bool value)
	{
		for (int i = 0; i < asyncedLandmarks.Count; i++)
		{
			if (!asyncedLandmarks[i].aO.isDone)
			{
				continue;
			}
			GameObject[] gameObjectsFromScene = GetGameObjectsFromScene(asyncedLandmarks[i].sceneName);
			if (gameObjectsFromScene != null)
			{
				for (int j = 0; j < gameObjectsFromScene.Length; j++)
				{
					gameObjectsFromScene[j].SetActive(value);
				}
			}
		}
	}

	public bool SetSpecificLandmarkActivity(string p_landMarkName, bool value)
	{
		for (int i = 0; i < asyncedLandmarks.Count; i++)
		{
			if (!(asyncedLandmarks[i].sceneName == p_landMarkName) || !asyncedLandmarks[i].aO.isDone)
			{
				continue;
			}
			GameObject[] gameObjectsFromScene = GetGameObjectsFromScene(asyncedLandmarks[i].sceneName);
			if (gameObjectsFromScene != null)
			{
				int num = 0;
				if (num < gameObjectsFromScene.Length)
				{
					gameObjectsFromScene[num].SetActive(value);
					return true;
				}
			}
		}
		return false;
	}

	private static void CreateAssetBundleDirectory()
	{
		if (!Directory.Exists(fullFilePath))
		{
			Debug.Log("Path does not exists, creating folder AssetBundles");
			Directory.CreateDirectory(fullFilePath);
		}
	}

	private IEnumerator LoadSpecificScene(string specificSceneName)
	{
		while (!IsBundleLoaded)
		{
			yield return null;
		}
		string[] allScenePaths = bundleLandmark.GetAllScenePaths();
		for (int i = 0; i < allScenePaths.Length; i++)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(allScenePaths[i]);
			if (!fileNameWithoutExtension.Equals(specificSceneName))
			{
				continue;
			}
			AsyncOperation t_async = SceneManager.LoadSceneAsync(fileNameWithoutExtension, LoadSceneMode.Additive);
			AsyncedLandmark t_landMark = new AsyncedLandmark(t_async, fileNameWithoutExtension);
			asyncedLandmarks.Add(t_landMark);
			t_async.allowSceneActivation = allowSceneActivation;
			if (!t_async.allowSceneActivation)
			{
				while (t_async.progress < 0.9f)
				{
					yield return null;
				}
			}
			t_async.allowSceneActivation = true;
			assetHooker.SortIntoCorrectList(t_landMark);
			yield break;
		}
		Debug.Log("Unable to find scene in bundle with name " + specificSceneName);
	}

	public void LoadScenePathsNonAsync()
	{
		string[] allScenePaths = bundleLandmark.GetAllScenePaths();
		foreach (string obj in allScenePaths)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(obj);
			AsyncedLandmark asyncedLandmark = new AsyncedLandmark(fileNameWithoutExtension);
			SceneManager.LoadScene(obj, LoadSceneMode.Additive);
			asyncedLandmark.scene = SceneManager.GetSceneByName(fileNameWithoutExtension);
			asyncedLandmarks.Add(asyncedLandmark);
		}
		Invoke("waitAFrameCosWaitForSceneObjects", 0.1f);
	}

	private void waitAFrameCosWaitForSceneObjects()
	{
		UnloadBundle(removeObjects: false);
		assetHooker.SendOnAssetsInBundleLoaded();
		AddLandmarksToHooker();
		startedLoadingScenes = false;
		IsAllScenesLoaded = true;
	}

	private IEnumerator LoadScenePaths()
	{
		startedLoadingScenes = true;
		while (bundleLandmark == null)
		{
			yield return null;
		}
		string[] allScenePaths = bundleLandmark.GetAllScenePaths();
		string[] array = allScenePaths;
		for (int i = 0; i < array.Length; i++)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(array[i]);
			AsyncOperation t_async = SceneManager.LoadSceneAsync(fileNameWithoutExtension, LoadSceneMode.Additive);
			AsyncedLandmark item = new AsyncedLandmark(t_async, fileNameWithoutExtension);
			asyncedLandmarks.Add(item);
			t_async.allowSceneActivation = allowSceneActivation;
			if (!t_async.allowSceneActivation)
			{
				while (t_async.progress < 0.9f)
				{
					yield return null;
				}
			}
			t_async.allowSceneActivation = true;
		}
		while (!CheckIfBundleIsLoaded())
		{
			yield return null;
		}
		UnloadBundle(removeObjects: false);
		assetHooker.SendOnAssetsInBundleLoaded();
		AddLandmarksToHooker();
		startedLoadingScenes = false;
		IsAllScenesLoaded = true;
	}

	private IEnumerator LoadBundleFromFileAsync()
	{
		if (!IsBundleLoaded && !startedLoadingBundle)
		{
			startedLoadingBundle = true;
			AssetBundleCreateRequest t_assetBundleRequest = null;
			try
			{
				t_assetBundleRequest = AssetBundle.LoadFromFileAsync(fullFilePath + "/" + assetBundleTag);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			yield return t_assetBundleRequest;
			bundleLandmark = t_assetBundleRequest.assetBundle;
			if (!(bundleLandmark == null))
			{
				startedLoadingBundle = false;
				assetHooker.SendBundleLoadedEvent();
			}
		}
	}

	public GameObject[] GetGameObjectsFromScene(string p_sceneName)
	{
		return SceneManager.GetSceneByName(Path.GetFileNameWithoutExtension(p_sceneName)).GetRootGameObjects();
	}

	public bool CheckIfBundleIsLoaded()
	{
		bool result = true;
		for (int i = 0; i < asyncedLandmarks.Count; i++)
		{
			if (!asyncedLandmarks[i].aO.isDone)
			{
				result = false;
				return false;
			}
		}
		return result;
	}

	private void CheckIfSceneLoadingIsDone()
	{
		for (int i = 0; i < asyncedLandmarks.Count; i++)
		{
			if (asyncedLandmarks[i].aO.isDone)
			{
				asyncedLandmarks[i].isSpawned = true;
			}
			else
			{
				asyncedLandmarks[i].isSpawned = false;
			}
		}
	}

	private void UnloadBundle(bool removeObjects)
	{
		if (bundleLandmark != null)
		{
			bundleLandmark.Unload(removeObjects);
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			asyncedLandmarks.Clear();
		}
	}
}
