using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetBundles.Manager;

public class SceneLoader : MonoBehaviour
{
	private static string kNEXTSCENE = "NextScene";

	public static string currentSceneName;

	public string sceneName;

	public bool isAdditive;

	public GameObject loadEventHandler;

	public static string NextScene
	{
		get
		{
			return PlayerPrefs.GetString(kNEXTSCENE, currentSceneName);
		}
		set
		{
			PlayerPrefs.SetString(kNEXTSCENE, value);
		}
	}

	private IEnumerator Start()
	{
		yield return StartCoroutine(Initialize());
		yield return StartCoroutine(InitializeLevelAsync(sceneName, isAdditive));
	}

	protected IEnumerator Initialize()
	{
		currentSceneName = sceneName;
		if (!string.IsNullOrEmpty(NextScene))
		{
			sceneName = NextScene;
		}
		Object.DontDestroyOnLoad(gameObject);
		AssetBundleLoadManifestOperation assetBundleLoadManifestOperation = AssetBundleManager.Initialize();
		if (assetBundleLoadManifestOperation != null)
		{
			yield return StartCoroutine(assetBundleLoadManifestOperation);
		}
	}

	protected IEnumerator InitializeLevelAsync(string sceneNamePath, bool isAdditive)
	{
		string levelName = Path.GetFileNameWithoutExtension(sceneNamePath);
		string assetBundleName = Settings.Map.GetAssetBundleName(sceneNamePath);
		float startTime = Time.realtimeSinceStartup;
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(assetBundleName, levelName, isAdditive);
		if (request == null)
		{
			if (loadEventHandler != null)
			{
				loadEventHandler.SendMessage("OnSceneLoadError");
			}
			yield break;
		}
		yield return StartCoroutine(request);
		if (request.IsError() && request == null)
		{
			if (loadEventHandler != null)
			{
				loadEventHandler.SendMessage("OnSceneLoadError");
			}
			yield break;
		}
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));
		float num = Time.realtimeSinceStartup - startTime;
		Debug.Log("Finished loading scene " + levelName + " in " + num + " seconds");
		if (loadEventHandler != null)
		{
			loadEventHandler.SendMessage("OnSceneLoaded");
		}
		Object.Destroy(gameObject);
	}
}
