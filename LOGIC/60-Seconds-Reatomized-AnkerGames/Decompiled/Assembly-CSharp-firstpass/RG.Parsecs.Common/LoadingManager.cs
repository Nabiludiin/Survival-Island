using System;
using System.Collections;
using System.Collections.Generic;
using RG.Core.SaveSystem;
using RG.Parsecs.Opening;
using RG.Parsecs.Scavenge;
using RG.Parsecs.Survival;
using RG.VirtualInput;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RG.Parsecs.Common;

public class LoadingManager : MonoBehaviourExtended, IBaseManager
{
	public class LoadingEventArgs : EventArgs
	{
		private readonly string _unloadedSceneName;

		private readonly string _loadedSceneName;

		public string LoadedSceneName => _loadedSceneName;

		public string UnloadedSceneName => _unloadedSceneName;

		public LoadingEventArgs(string unloadedSceneName, string loadedSceneName)
		{
			_unloadedSceneName = unloadedSceneName;
			_loadedSceneName = loadedSceneName;
		}
	}

	public delegate IEnumerator AssetLoadEventHandler();

	[SerializeField]
	[Tooltip("Name of the loading screen scene.")]
	private string _loadingSceneName;

	[SerializeField]
	[Tooltip("Safe time before loading screen will be unloaded, in seconds.")]
	private float _timeToUnload;

	[SerializeField]
	[Tooltip("Safe time before loading screen will be unloaded, in seconds.")]
	private float _shortTimeToUnload = 1f;

	private bool _currentlyLoading;

	[SerializeField]
	private LoadingObscurerController loadingObscurerController;

	public string SceneBeingLoaded { get; private set; }

	public bool LevelsLoaded { get; private set; }

	public bool CurrentlyLoading => _currentlyLoading;

	public Scene CurrentScene => SceneManager.GetActiveScene();

	public LoadingObscurerController ObscurerController => loadingObscurerController;

	public event EventHandler OnLoadingStarted;

	public event EventHandler OnLoadingFinished;

	public event EventHandler BeforeUnloadLoadingScene;

	public event AssetLoadEventHandler LoadAssetsHandler;

	public event AssetLoadEventHandler UnloadAssetsHandler;

	private void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoad;
	}

	public void LoadScene(string sceneName, AssetLoadEventHandler loadAssets = null, AssetLoadEventHandler unloadAssets = null)
	{
		List<Scene> list = new List<Scene>();
		list.Add(SceneManager.GetActiveScene());
		LoadScene(sceneName, list, null, loadAssets, unloadAssets);
	}

	public void LoadScene(string sceneName, AssetLoadEventHandler loadAssets, AssetLoadEventHandler unloadAssets, string loadingSceneName)
	{
		List<Scene> list = new List<Scene>();
		list.Add(SceneManager.GetActiveScene());
		LoadScene(sceneName, list, null, loadAssets, unloadAssets, loadingSceneName);
	}

	public void LoadScene(string sceneName, string saveEventToLoad, AssetLoadEventHandler loadAssets = null, AssetLoadEventHandler unloadAssets = null)
	{
		List<Scene> list = new List<Scene>();
		list.Add(SceneManager.GetActiveScene());
		LoadScene(sceneName, list, saveEventToLoad, loadAssets, unloadAssets);
	}

	public void LoadScene(string sceneName, List<Scene> scenesToUnload, string saveEventToLoad, AssetLoadEventHandler loadAssets = null, AssetLoadEventHandler unloadAssets = null, string loadingSceneName = null)
	{
		if (_currentlyLoading)
		{
			Debug.LogWarning("Loading can't be started because there is already one ongoing.");
			return;
		}
		_currentlyLoading = true;
		SceneBeingLoaded = sceneName;
		StartCoroutine(LoadSceneWithLoadingScreen(sceneName, scenesToUnload, loadAssets, unloadAssets, saveEventToLoad, loadingSceneName));
	}

	public void LoadSave(AssetLoadEventHandler loadAssets = null, AssetLoadEventHandler unloadAssets = null)
	{
		List<Scene> list = new List<Scene>();
		list.Add(SceneManager.GetActiveScene());
		if (_currentlyLoading)
		{
			Debug.LogWarning("Loading can't be started because there is already one ongoing.");
			return;
		}
		_currentlyLoading = true;
		StartCoroutine(LoadGameFomSave(list, loadAssets, unloadAssets));
	}

	private IEnumerator LoadSceneWithLoadingScreen(string sceneName, List<Scene> scenesToUnload, AssetLoadEventHandler loadAssets, AssetLoadEventHandler unloadAssets, string saveEventToLoad, string loadingSceneName = null)
	{
		LevelsLoaded = false;
		string loadingScene = _loadingSceneName;
		LoadAssetsHandler += loadAssets;
		Scene currentScene = scenesToUnload[0];
		StorageManager.SetCurrentLoadingInfo(sceneName, currentScene.name);
		if (OnLoadingStarted != null)
		{
			OnLoadingStarted(this, new LoadingEventArgs(currentScene.name, sceneName));
		}
		yield return loadingObscurerController.ShowObscurer();
		if (!string.IsNullOrEmpty(loadingSceneName))
		{
			if (loadingSceneName == "Opening")
			{
				Application.backgroundLoadingPriority = ThreadPriority.Low;
			}
			loadingScene = loadingSceneName;
		}
		yield return SceneManager.LoadSceneAsync(loadingScene);
		if (loadingScene.Equals("Opening"))
		{
			SplashManager.Instance.SplashVisible = true;
		}
		bool loadFromSave = false;
		if (!string.IsNullOrEmpty(saveEventToLoad))
		{
			StorageDataManager.TheInstance.Load(saveEventToLoad, () =>
			{
				loadFromSave = true;
			}, useCloudSave: false);
			while (!loadFromSave)
			{
				yield return null;
			}
		}
		foreach (Scene item in scenesToUnload)
		{
			if (!string.IsNullOrEmpty(item.path))
			{
				yield return SceneManager.UnloadSceneAsync(item);
			}
		}
		yield return loadingObscurerController.HideObscurer();
		yield return InvokeAllUnloadAssetCalls();
		yield return InvokeAllLoadAssetCalls();
		if (!StorageManager.AreAllAtlasesAssigned())
		{
			yield return StorageManager.AssignSpriteAtlases();
		}
		if (loadingScene.Equals("Opening"))
		{
			yield return new WaitWhile(() => SplashManager.Instance.SplashVisible);
		}
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		LevelsLoaded = true;
		if (loadFromSave)
		{
			UnityEngine.Object.FindObjectOfType<SchedulerManager>().FromSave = true;
			yield return new WaitForEndOfFrame();
		}
		if (sceneName.Equals("Survival"))
		{
			yield return new WaitForSeconds(_shortTimeToUnload);
		}
		else
		{
			yield return new WaitForSeconds(_timeToUnload);
		}
		if (ScavengePlayersManager.Instance != null)
		{
			while (ScavengePlayersManager.Instance.ScavengeManager == null)
			{
				yield return null;
			}
			yield return loadingObscurerController.ShowObscurer();
			while (!ScavengePlayersManager.Instance.ScavengeManager.Loaded)
			{
				yield return null;
			}
		}
		else
		{
			Singleton<VirtualInputManager>.Instance.VisualManager.MouseCursorVisible = true;
		}
		if (BeforeUnloadLoadingScene != null)
		{
			BeforeUnloadLoadingScene(this, new LoadingEventArgs(currentScene.name, sceneName));
		}
		if (!sceneName.Equals("Scavenge_start"))
		{
			yield return loadingObscurerController.ShowObscurer();
		}
		yield return SceneManager.UnloadSceneAsync(loadingScene);
		if (sceneName.Equals("Survival"))
		{
			Singleton<VirtualInputManager>.Instance.VisualManager.MouseCursorVisible = false;
			loadingObscurerController.InstantlyHideObscurer();
		}
		else
		{
			yield return loadingObscurerController.HideObscurer();
		}
		UnloadAssetsHandler += unloadAssets;
		_currentlyLoading = false;
		SceneBeingLoaded = null;
		StorageManager.SetCurrentLoadingInfo(null, null);
		Application.backgroundLoadingPriority = ThreadPriority.Normal;
		if (OnLoadingFinished != null)
		{
			OnLoadingFinished(this, new LoadingEventArgs(currentScene.name, sceneName));
		}
	}

	private IEnumerator LoadGameFomSave(List<Scene> scenesToUnload, AssetLoadEventHandler loadAssets = null, AssetLoadEventHandler unloadAssets = null)
	{
		LoadAssetsHandler += loadAssets;
		Scene currentScene = scenesToUnload[0];
		StorageManager.SetCurrentLoadingInfo("Survival", currentScene.name);
		if (OnLoadingStarted != null)
		{
			OnLoadingStarted(this, new LoadingEventArgs(currentScene.name, "Survival"));
		}
		yield return loadingObscurerController.ShowObscurer();
		yield return SceneManager.LoadSceneAsync(_loadingSceneName, LoadSceneMode.Additive);
		foreach (Scene item in scenesToUnload)
		{
			yield return SceneManager.UnloadSceneAsync(item);
		}
		yield return InvokeAllUnloadAssetCalls();
		yield return InvokeAllLoadAssetCalls();
		if (!StorageManager.AreAllAtlasesAssigned())
		{
			yield return StorageManager.AssignSpriteAtlases();
		}
		yield return SceneManager.LoadSceneAsync("Survival", LoadSceneMode.Additive);
		UnityEngine.Object.FindObjectOfType<SchedulerManager>().FromSave = true;
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(_shortTimeToUnload);
		yield return SceneManager.UnloadSceneAsync(_loadingSceneName);
		loadingObscurerController.InstantlyHideObscurer();
		UnloadAssetsHandler += unloadAssets;
		_currentlyLoading = false;
		SceneBeingLoaded = null;
		StorageManager.SetCurrentLoadingInfo(null, null);
		if (OnLoadingFinished != null)
		{
			OnLoadingFinished(this, new LoadingEventArgs(currentScene.name, "Survival"));
		}
	}

	private void OnSceneLoad(Scene loadedScene, LoadSceneMode sceneMode)
	{
		if (!IsSceneLoadingScreen(loadedScene))
		{
			SetSceneAsActive(loadedScene);
		}
	}

	private void SetSceneAsActive(Scene scene)
	{
		SceneManager.SetActiveScene(scene);
	}

	private bool IsSceneLoadingScreen(Scene scene)
	{
		return scene == SceneManager.GetSceneByName(_loadingSceneName);
	}

	private IEnumerator InvokeAllLoadAssetCalls()
	{
		if (LoadAssetsHandler != null)
		{
			for (int i = 0; i < LoadAssetsHandler.GetInvocationList().Length; i++)
			{
				yield return LoadAssetsHandler.GetInvocationList()[i].DynamicInvoke();
			}
			LoadAssetsHandler = null;
		}
		yield return null;
	}

	private IEnumerator InvokeAllUnloadAssetCalls()
	{
		if (UnloadAssetsHandler != null)
		{
			for (int i = 0; i < UnloadAssetsHandler.GetInvocationList().Length; i++)
			{
				yield return UnloadAssetsHandler.GetInvocationList()[i].DynamicInvoke();
			}
			UnloadAssetsHandler = null;
		}
		yield return null;
	}
}
