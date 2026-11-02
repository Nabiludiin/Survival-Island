using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleHooker : MonoBehaviour, SceneEventInterface
{
	private AssetBundleManager bundleManager;

	public static Action OnBundleLoaded;

	public static Action OnBundleAssetsLoaded;

	public static bool isInitialized;

	[SerializeField]
	public Dictionary<string, List<AsyncedLandmark>> landmarkDict = new Dictionary<string, List<AsyncedLandmark>>();

	private void Awake()
	{
		if (!isInitialized)
		{
			isInitialized = true;
			ComponentManager<AssetBundleHooker>.Value = this;
			DontDestroyOnLoadManager.AddGameObject(gameObject);
			if (bundleManager == null)
			{
				bundleManager = GetComponent<AssetBundleManager>();
			}
		}
	}

	public int GetLandmarkCountFromAssetBundleKey(string bundleKey)
	{
		if (bundleManager == null)
		{
			bundleManager = GetComponent<AssetBundleManager>();
		}
		List<AsyncedLandmark> value = new List<AsyncedLandmark>();
		landmarkDict.TryGetValue(bundleKey, out value);
		if (value == null || value.Count <= 0)
		{
			return 0;
		}
		return value.Count;
	}

	public List<AsyncedLandmark> GetLandmarksFromAssetBundleKey(string bundleKey)
	{
		if (bundleManager == null)
		{
			bundleManager = GetComponent<AssetBundleManager>();
		}
		List<AsyncedLandmark> value = new List<AsyncedLandmark>();
		landmarkDict.TryGetValue(bundleKey, out value);
		return value;
	}

	public AsyncedLandmark GetRandomLandmarkFromAssetBundleKey(string bundleKey)
	{
		if (bundleManager == null)
		{
			bundleManager = GetComponent<AssetBundleManager>();
		}
		List<AsyncedLandmark> value = new List<AsyncedLandmark>();
		landmarkDict.TryGetValue(bundleKey, out value);
		if (value == null || value.Count <= 0)
		{
			return null;
		}
		int index = UnityEngine.Random.Range(0, value.Count);
		return value[index];
	}

	public AsyncedLandmark GetLandmarkFromUniqueIndex(int p_uniqueIndex)
	{
		if (bundleManager == null)
		{
			bundleManager = GetComponent<AssetBundleManager>();
		}
		foreach (KeyValuePair<string, List<AsyncedLandmark>> item in landmarkDict)
		{
			for (int i = 0; i < item.Value.Count; i++)
			{
				if (item.Value[i].landmark.uniqueLandmarkIndex == p_uniqueIndex)
				{
					return item.Value[i];
				}
			}
		}
		return null;
	}

	public GameObject GetLandmarkFromSceneName(string p_sceneName)
	{
		List<AsyncedLandmark> value = new List<AsyncedLandmark>();
		string key = p_sceneName.Split('_').First();
		landmarkDict.TryGetValue(key, out value);
		if (value.Count <= 0)
		{
			Debug.Log("Unable to find landmark object by scenename" + p_sceneName);
			return null;
		}
		for (int i = 0; i < value.Count; i++)
		{
			if (value[i].sceneName == p_sceneName)
			{
				return GetGameObjectsFromScene(p_sceneName)[0];
			}
		}
		Debug.Log("Unable to find landmark object by scenename" + p_sceneName);
		return null;
	}

	public GameObject[] GetGameObjectsFromScene(string p_sceneName)
	{
		return SceneManager.GetSceneByName(Path.GetFileNameWithoutExtension(p_sceneName)).GetRootGameObjects();
	}

	public void SortIntoCorrectList(AsyncedLandmark p_asyncedLandmark)
	{
		string[] array = p_asyncedLandmark.sceneName.Split('#');
		if (array.Length < 2)
		{
			Debug.LogError("Can't sort landmark into list: " + p_asyncedLandmark.sceneName);
			return;
		}
		p_asyncedLandmark.scene = SceneManager.GetSceneByName(Path.GetFileNameWithoutExtension(p_asyncedLandmark.sceneName));
		GameObject[] rootGameObjects = p_asyncedLandmark.scene.GetRootGameObjects();
		if (rootGameObjects != null && rootGameObjects.Length >= 1)
		{
			p_asyncedLandmark.landmark = rootGameObjects[0].GetComponent<Landmark>();
			if (p_asyncedLandmark.landmark == null)
			{
				Debug.LogError("Could not find Landmark script on object '" + rootGameObjects[0].transform.name + "' in scene: " + p_asyncedLandmark.sceneName);
			}
			p_asyncedLandmark.landmark.Initialize();
			if (!WorldManager.AllLandmarks.Contains(p_asyncedLandmark.landmark))
			{
				WorldManager.AllLandmarks.Add(p_asyncedLandmark.landmark);
			}
		}
		else
		{
			Debug.Log("Unable to find any sort of object on the asynced-landmark");
		}
		string key = array[1];
		if (!landmarkDict.ContainsKey(key))
		{
			landmarkDict.Add(key, new List<AsyncedLandmark>());
		}
		landmarkDict[key].Add(p_asyncedLandmark);
	}

	public void SendBundleLoadedEvent()
	{
		if (OnBundleLoaded != null)
		{
			OnBundleLoaded();
		}
	}

	public void SendOnAssetsInBundleLoaded()
	{
		if (OnBundleAssetsLoaded != null)
		{
			OnBundleAssetsLoaded();
		}
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			OnBundleLoaded = null;
			OnBundleAssetsLoaded = null;
			landmarkDict.Clear();
		}
	}
}
