using System;
using System.Collections.Generic;
using MeshCombineStudio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[Serializable]
	public class LandmarkDataConnection
	{
		public string bundleKey;

		public List<LandmarkData> landmarkData = new List<LandmarkData>();

		public LandmarkDataConnection(string bundleKey, LandmarkData data)
		{
			this.bundleKey = bundleKey;
			landmarkData.Add(data);
		}
	}

	public enum LoadSetting
	{
		DoNotLoad,
		LoadAll
	}

	[Serializable]
	public class LandmarkData
	{
		public GameObject go;

		public Landmark landmark;

		public uint index;

		public LandmarkData(GameObject go, Landmark landmark, uint index)
		{
			this.go = go;
			this.landmark = landmark;
			this.index = index;
		}
	}

	public LoadSetting atStartLoadSetting = LoadSetting.LoadAll;

	public Scene scene;

	public List<AsyncOperation> asyncOperations = new List<AsyncOperation>();

	public List<LandmarkData> loadedLandmarks = new List<LandmarkData>();

	public List<LandmarkDataConnection> loadedLandmarkConnections = new List<LandmarkDataConnection>();

	private int loadedScenesCount;

	private int scenesToLoadCount;

	public float LoadSceneProgressNormalized
	{
		get
		{
			if (Raft_Network.IsAllLandmarksLoaded)
			{
				return 1f;
			}
			return Mathf.Clamp01((float)loadedScenesCount / (float)scenesToLoadCount);
		}
	}

	private void Awake()
	{
		ComponentManager<SceneLoader>.Value = this;
		if (!Application.isEditor)
		{
			atStartLoadSetting = LoadSetting.LoadAll;
		}
		if (atStartLoadSetting == LoadSetting.DoNotLoad)
		{
			scenesToLoadCount = 0;
		}
		else
		{
			scenesToLoadCount = SceneManager.sceneCountInBuildSettings - 2;
		}
		if (Raft_Network.GameSceneName != "MainScene")
		{
			scenesToLoadCount--;
		}
	}

	private void OnDestroy()
	{
		asyncOperations.Clear();
		loadedLandmarks.Clear();
		loadedLandmarkConnections.Clear();
	}

	public GameObject SpawnLandmarkFromUniqueIndex(int index)
	{
		GameObject gameObject = null;
		foreach (LandmarkData loadedLandmark in loadedLandmarks)
		{
			if (index == loadedLandmark.index)
			{
				if (loadedLandmark.go == null)
				{
					Debug.Log("Can't spawn landmark with index " + index + " from addressable system");
				}
				else
				{
					gameObject = loadedLandmark.go;
				}
				break;
			}
			if (gameObject != null)
			{
				break;
			}
		}
		if (gameObject == null)
		{
			Debug.Log("Unable to find dictionary key with index " + index);
		}
		return gameObject;
	}

	public List<LandmarkData> GetLandmarksFromAssetBundleKey(string bundleKey)
	{
		return loadedLandmarkConnections.Find((LandmarkDataConnection c) => c.bundleKey == bundleKey)?.landmarkData;
	}

	public void LoadScenes()
	{
		if (atStartLoadSetting == LoadSetting.DoNotLoad || scenesToLoadCount == 0)
		{
			Raft_Network.IsAllLandmarksLoaded = true;
			return;
		}
		for (int i = 2; i < scenesToLoadCount + 2; i++)
		{
			string sceneName = NameFromIndex(i);
			Helper.LogBuild("SceneLoader: Loading " + sceneName);
			AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			asyncOperations.Add(async);
			async.completed += (AsyncOperation handle) =>
			{
				OnSceneActivate(SceneManager.GetSceneByName(sceneName));
				Helper.LogBuild("SceneLoader: Finished " + sceneName);
				asyncOperations.Remove(async);
				if (asyncOperations.Count == 0)
				{
					Resources.UnloadUnusedAssets();
				}
			};
		}
	}

	private void OnSceneActivate(Scene scene)
	{
		loadedScenesCount++;
		GameObject[] rootGameObjects = scene.GetRootGameObjects();
		if (rootGameObjects.Length != 0)
		{
			GameObject gameObject = rootGameObjects[0];
			MeshCombiner[] componentsInChildren = gameObject.GetComponentsInChildren<MeshCombiner>(includeInactive: true);
			foreach (MeshCombiner meshCombiner in componentsInChildren)
			{
				if (meshCombiner != null && meshCombiner.combineInRuntime)
				{
					meshCombiner.ManuallyCombine_RAFT(scene.name);
				}
			}
			gameObject.SetActiveSafe(value: false);
			Landmark component = gameObject.GetComponent<Landmark>();
			if (!(component != null))
			{
				Debug.Log("landmark is null on loaded scene, probably something wrong.", gameObject.transform.gameObject);
				scenesToLoadCount--;
				return;
			}
			InitializeLoadedLandmark(component);
			AddLoadedLandmark(gameObject, component);
			gameObject.SetActiveSafe(value: false);
		}
		if (loadedScenesCount >= scenesToLoadCount)
		{
			Raft_Network.IsAllLandmarksLoaded = true;
		}
	}

	private void AddLoadedLandmark(GameObject go, Landmark landmark)
	{
		LandmarkData landmarkData = new LandmarkData(go, landmark, landmark.uniqueLandmarkIndex);
		loadedLandmarks.Add(landmarkData);
		AddDictionairyConnection(landmarkData, landmark.name);
		landmark.Initialize();
	}

	private void TrackAddressableLandmark(string assetBundleKey, LandmarkData landmarkData)
	{
		foreach (LandmarkDataConnection loadedLandmarkConnection in loadedLandmarkConnections)
		{
			if (loadedLandmarkConnection.bundleKey == assetBundleKey)
			{
				loadedLandmarkConnection.landmarkData.AddUniqueOnly(landmarkData);
				return;
			}
		}
		loadedLandmarkConnections.Add(new LandmarkDataConnection(assetBundleKey, landmarkData));
	}

	private void AddDictionairyConnection(LandmarkData landmarkData, string landmarkName)
	{
		string[] array = landmarkName.Split(char.Parse("#"));
		if (!array[0].IsNullOrEmpty() && !array[1].IsNullOrEmpty())
		{
			int.Parse(array[0]);
			string assetBundleKey = array[1];
			TrackAddressableLandmark(assetBundleKey, landmarkData);
		}
	}

	private void InitializeLoadedLandmark(Landmark landmark)
	{
		landmark.Initialize();
		if (!GameManager.IsLeavingGame && !WorldManager.AllLandmarks.Contains(landmark))
		{
			WorldManager.AllLandmarks.Add(landmark);
		}
	}

	private static string NameFromIndex(int BuildIndex)
	{
		string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
		int num = scenePathByBuildIndex.LastIndexOf('/');
		string text = scenePathByBuildIndex.Substring(num + 1);
		return text[..text.LastIndexOf('.')];
	}
}
