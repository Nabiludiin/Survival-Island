using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

namespace RG.Parsecs.Common;

public class StorageManager : MonoBehaviour
{
	public delegate void AssetBundleLoad(AssetBundle loadedAssetBundle);

	public delegate void AssetBundleUnload(List<string> unloadedBundles);

	public delegate void AssetsLoaded(UnityEngine.Object[] loadedAssets);

	private static Dictionary<string, Dictionary<string, AssetBundle>> _loadedAssetBundlesForScenes = null;

	private static Dictionary<string, AssetBundle> _loadedAssetBundles = null;

	private static Dictionary<string, Action<SpriteAtlas>> _spriteAtlasRequestCallbacks;

	private static List<SpriteAtlas> _loadedSpriteAtlases;

	private static int _requestedAtlases;

	private static bool _initialized = false;

	private static readonly float MAX_WAIT_TIME_FOR_ATLAS_REQUEST = 2f;

	private static StorageManager Instance => Singleton<GameManager>.Instance.StorageManager;

	private static string CurrentlyLoadedSceneName { get; set; }

	private static string CurrentlyUnloadedSceneName { get; set; }

	public static LoadAssetsRequest<T> CreateLoadAssetsRequest<T>(string assetBundlePath, string assetBundleName) where T : UnityEngine.Object
	{
		Initialize();
		LoadAssetsRequest<T> loadAssetsRequest = new LoadAssetsRequest<T>(Instance, assetBundlePath, assetBundleName) ?? throw new UnityException("LoadAssetsRequest<T> is null!");
		loadAssetsRequest.AssetsLoaded = (AssetsLoaded)Delegate.Combine(loadAssetsRequest.AssetsLoaded, new AssetsLoaded(OnAssetsLoaded));
		loadAssetsRequest.AssetBundleLoaded = (AssetBundleLoad)Delegate.Combine(loadAssetsRequest.AssetBundleLoaded, new AssetBundleLoad(OnAssetBundleLoaded));
		return loadAssetsRequest;
	}

	public static LoadAssetRequest<T> CreateLoadAssetRequest<T>(string assetBundlePath, string assetBundleName, string assetName) where T : UnityEngine.Object
	{
		Initialize();
		LoadAssetRequest<T> loadAssetRequest = new LoadAssetRequest<T>(Instance, assetBundlePath, assetBundleName, assetName) ?? throw new UnityException("LoadAssetRequest<T> is null!");
		loadAssetRequest.AssetsLoaded = (AssetsLoaded)Delegate.Combine(loadAssetRequest.AssetsLoaded, new AssetsLoaded(OnAssetsLoaded));
		loadAssetRequest.AssetBundleLoaded = (AssetBundleLoad)Delegate.Combine(loadAssetRequest.AssetBundleLoaded, new AssetBundleLoad(OnAssetBundleLoaded));
		return loadAssetRequest;
	}

	public static IEnumerator AssignSpriteAtlases()
	{
		float _startedWaitingForRequests = Time.realtimeSinceStartup;
		yield return new WaitUntil(() => _loadedSpriteAtlases.Count == _spriteAtlasRequestCallbacks.Count || Time.realtimeSinceStartup > _startedWaitingForRequests + MAX_WAIT_TIME_FOR_ATLAS_REQUEST);
		for (int num = 0; num < _loadedSpriteAtlases.Count; num++)
		{
			if (_spriteAtlasRequestCallbacks.ContainsKey(_loadedSpriteAtlases[num].tag))
			{
				_spriteAtlasRequestCallbacks[_loadedSpriteAtlases[num].tag](_loadedSpriteAtlases[num]);
				_spriteAtlasRequestCallbacks.Remove(_loadedSpriteAtlases[num].tag);
			}
		}
		yield return null;
	}

	public static bool AreAllAtlasesAssigned()
	{
		if (_spriteAtlasRequestCallbacks == null || _loadedSpriteAtlases == null)
		{
			return true;
		}
		if (_spriteAtlasRequestCallbacks.Count == 0)
		{
			return _loadedSpriteAtlases.Count == 0;
		}
		return false;
	}

	public static void SetCurrentLoadingInfo(string loadedSceneName, string unloadedSceneName)
	{
		CurrentlyLoadedSceneName = loadedSceneName;
		if (string.IsNullOrEmpty(unloadedSceneName))
		{
			CurrentlyUnloadedSceneName = null;
		}
		else
		{
			CurrentlyUnloadedSceneName = ((unloadedSceneName.Contains("Scav") || unloadedSceneName.Contains("Tut")) ? "Scavenge_start" : unloadedSceneName);
		}
	}

	public static UnloadRequest CreateUnloadAllAssetBundlesRequest(bool unloadAllUnusedObjects = true, bool unloadAllUnusedAssets = false)
	{
		UnloadRequest unloadRequest = new UnloadRequest(Instance, _loadedAssetBundles.Values.GetEnumerator(), unloadAllUnusedObjects, unloadAllUnusedAssets);
		unloadRequest.AssetBundleUnloaded = (AssetBundleUnload)Delegate.Combine(unloadRequest.AssetBundleUnloaded, new AssetBundleUnload(OnAssetBundlesUnloaded));
		return unloadRequest;
	}

	public static UnloadRequest CreateUnloadAssetBundlesRequest(AssetBundle[] bundlesToUnload, bool unloadAllUnusedObjects = true, bool unloadAllUnusedAssets = false)
	{
		UnloadRequest unloadRequest = new UnloadRequest(Instance, bundlesToUnload, unloadAllUnusedObjects, unloadAllUnusedAssets);
		unloadRequest.AssetBundleUnloaded = (AssetBundleUnload)Delegate.Combine(unloadRequest.AssetBundleUnloaded, new AssetBundleUnload(OnAssetBundlesUnloaded));
		return unloadRequest;
	}

	public static UnloadRequest CreateUnloadAssetRequest(AssetBundle bundleToUnload, bool unloadAllUnusedObjects = true, bool unloadAllUnusedAssets = false)
	{
		UnloadRequest unloadRequest = new UnloadRequest(Instance, new AssetBundle[1] { bundleToUnload }, unloadAllUnusedObjects, unloadAllUnusedAssets);
		unloadRequest.AssetBundleUnloaded = (AssetBundleUnload)Delegate.Combine(unloadRequest.AssetBundleUnloaded, new AssetBundleUnload(OnAssetBundlesUnloaded));
		return unloadRequest;
	}

	public static UnloadRequest CreateUnloadSceneBundlesRequest(string sceneName, bool unloadAllUnusedObjects = true, bool unloadAllUnusedAssets = false)
	{
		if (_loadedAssetBundlesForScenes == null)
		{
			return new UnloadRequest(Instance);
		}
		if (!_loadedAssetBundlesForScenes.ContainsKey(sceneName))
		{
			throw new UnityException(sceneName + " does not have any Asset Bundles assigned in Storage Manager.");
		}
		UnloadRequest unloadRequest = new UnloadRequest(Instance, _loadedAssetBundlesForScenes[sceneName].Values.GetEnumerator(), unloadAllUnusedObjects, unloadAllUnusedAssets);
		unloadRequest.AssetBundleUnloaded = (AssetBundleUnload)Delegate.Combine(unloadRequest.AssetBundleUnloaded, new AssetBundleUnload(OnAssetBundlesUnloaded));
		return unloadRequest;
	}

	public static bool IsBundleLoaded(string bundleName)
	{
		if (_loadedAssetBundles == null)
		{
			return false;
		}
		return _loadedAssetBundles.ContainsKey(bundleName);
	}

	public static AssetBundle GetLoadedAssetBundle(string bundleName)
	{
		if (IsBundleLoaded(bundleName))
		{
			return _loadedAssetBundles[bundleName];
		}
		return null;
	}

	private static void OnAssetBundlesUnloaded(List<string> bundles)
	{
		for (int i = 0; i < bundles.Count; i++)
		{
			string text = bundles[i];
			RemoveAssetBundleToLoadedAssetBundles(text);
			RemoveAssetBundleFromSceneBundlesList(text, (!string.IsNullOrEmpty(CurrentlyUnloadedSceneName)) ? CurrentlyUnloadedSceneName : SceneManager.GetActiveScene().name);
		}
	}

	private static void OnAssetBundleLoaded(AssetBundle loadedAssetBundle)
	{
		AddAssetBundleToLoadedAssetBundles(loadedAssetBundle);
		AddAssetBundleToSceneBundlesList(loadedAssetBundle, (!string.IsNullOrEmpty(CurrentlyLoadedSceneName)) ? CurrentlyLoadedSceneName : SceneManager.GetActiveScene().name);
	}

	private static void OnAssetsLoaded(UnityEngine.Object[] loadedAssets)
	{
		for (int i = 0; i < loadedAssets.Length; i++)
		{
			SpriteAtlas spriteAtlas = loadedAssets[i] as SpriteAtlas;
			if (spriteAtlas != null && !_loadedSpriteAtlases.Contains(spriteAtlas))
			{
				_loadedSpriteAtlases.Add(spriteAtlas);
			}
		}
	}

	public static void Initialize()
	{
		if (!_initialized)
		{
			RegisterOnAtlasRequested();
			_initialized = true;
		}
	}

	private static void RemoveAssetBundleToLoadedAssetBundles(string bundle)
	{
		if (_loadedAssetBundles != null && _loadedAssetBundles.ContainsKey(bundle))
		{
			_loadedAssetBundles.Remove(bundle);
		}
	}

	private static void AddAssetBundleToLoadedAssetBundles(AssetBundle loadedBundle)
	{
		if (_loadedAssetBundles == null)
		{
			_loadedAssetBundles = new Dictionary<string, AssetBundle>();
		}
		if (!_loadedAssetBundles.ContainsKey(loadedBundle.name))
		{
			_loadedAssetBundles.Add(loadedBundle.name, loadedBundle);
		}
	}

	private static void RemoveAssetBundleFromSceneBundlesList(string assetBundle, string sceneName)
	{
		if (_loadedAssetBundlesForScenes != null && _loadedAssetBundlesForScenes.ContainsKey(sceneName))
		{
			if (_loadedAssetBundlesForScenes[sceneName].ContainsKey(assetBundle))
			{
				_loadedAssetBundlesForScenes[sceneName].Remove(assetBundle);
			}
			if (_loadedAssetBundlesForScenes[sceneName].Count <= 0)
			{
				_loadedAssetBundles.Remove(sceneName);
			}
		}
	}

	private static void AddAssetBundleToSceneBundlesList(AssetBundle assetBundle, string sceneName)
	{
		if (_loadedAssetBundlesForScenes == null)
		{
			_loadedAssetBundlesForScenes = new Dictionary<string, Dictionary<string, AssetBundle>>();
		}
		if (_loadedAssetBundlesForScenes.ContainsKey(sceneName))
		{
			_loadedAssetBundlesForScenes[sceneName].Add(assetBundle.name, assetBundle);
			return;
		}
		Dictionary<string, AssetBundle> value = new Dictionary<string, AssetBundle> { { assetBundle.name, assetBundle } };
		_loadedAssetBundlesForScenes.Add(sceneName, value);
	}

	private static void RegisterOnAtlasRequested()
	{
		_loadedSpriteAtlases = new List<SpriteAtlas>();
		_spriteAtlasRequestCallbacks = new Dictionary<string, Action<SpriteAtlas>>();
		SpriteAtlasManager.atlasRequested += SpriteAtlasManagerOnAtlasRequested;
		SpriteAtlasManager.atlasRegistered += SpriteAtlasManagerOnAtlasRegistered;
	}

	private static void SpriteAtlasManagerOnAtlasRegistered(SpriteAtlas obj)
	{
	}

	private static void SpriteAtlasManagerOnAtlasRequested(string tag, Action<SpriteAtlas> action)
	{
		for (int i = 0; i < _loadedSpriteAtlases.Count; i++)
		{
			if (_loadedSpriteAtlases[i].tag == tag)
			{
				action(_loadedSpriteAtlases[i]);
				return;
			}
		}
		if (!_spriteAtlasRequestCallbacks.ContainsKey(tag))
		{
			_spriteAtlasRequestCallbacks.Add(tag, action);
		}
	}
}
