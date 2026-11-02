using System.Collections;
using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;

namespace RG.Parsecs.Common;

public class ContentManager : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("_availableContent")]
	[Tooltip("Reference to Scriptable Object containing current data what to load in scenes.")]
	private AvailableContentsPacks _availableContentWin;

	[SerializeField]
	[Tooltip("Reference to Scriptable Object containing current data what to load in scenes.")]
	private AvailableContentsPacks _availableContentOsx;

	[SerializeField]
	[Tooltip("Reference to Scriptable Object containing current data what to load in scenes.")]
	private AvailableContentsPacks _availableContentNX;

	[SerializeField]
	[Tooltip("What variants of asset bundles should be loaded.")]
	private string _spriteAtlasVariant = string.Empty;

	private static List<SpriteAtlas> _loadedSurvivalSpriteAtlases = new List<SpriteAtlas>();

	private static List<SpriteAtlas> _loadedCommonSpriteAtlases = new List<SpriteAtlas>();

	private static List<SpriteAtlas> _loadedScavangeAtlases = new List<SpriteAtlas>();

	private static List<SpriteAtlas> _loadedMenuAtlases = new List<SpriteAtlas>();

	[SerializeField]
	private List<SpriteAtlas> _atlasesInBuild = new List<SpriteAtlas>();

	private static Dictionary<string, Sprite> _requestedSprites = new Dictionary<string, Sprite>();

	private List<Deck> _decks;

	private AvailableContentsPacks _availableContent => Application.platform switch
	{
		RuntimePlatform.OSXEditor => _availableContentOsx, 
		RuntimePlatform.OSXPlayer => _availableContentOsx, 
		RuntimePlatform.WindowsPlayer => _availableContentWin, 
		RuntimePlatform.WindowsEditor => _availableContentWin, 
		_ => throw new UnityException("Unsupported platforms for Asset Bundles!"), 
	};

	public List<Deck> Decks => _decks;

	public IEnumerator LoadSurvivalAssetBundles()
	{
		_decks = new List<Deck>();
		_loadedSurvivalSpriteAtlases = new List<SpriteAtlas>();
		for (int i = 0; i < _availableContent.ContentPacks.Count; i++)
		{
			if (_availableContent.ContentPacks[i].Name != "Survival" || !_availableContent.ContentPacks[i].Enabled)
			{
				continue;
			}
			for (int k = 0; k < _availableContent.ContentPacks[i].SpriteAtlases.Count; k++)
			{
				LoadAssetsRequest<SpriteAtlas> loadRequest = StorageManager.CreateLoadAssetsRequest<SpriteAtlas>(_availableContent.ContentPacks[i].SpriteAtlases[k].GetFullDirectoryPath(), _availableContent.ContentPacks[i].SpriteAtlases[k].GetNameWithVariant(_spriteAtlasVariant));
				yield return loadRequest.StartRequest();
				for (int j = 0; j < loadRequest.AllAssets.Length; j++)
				{
					Debug.LogFormat("<color=00ffff>Asset Bundles | Survival Loaded Atlas: {0}</color>", loadRequest.AllAssets[j]);
					_loadedSurvivalSpriteAtlases.Add(loadRequest.AllAssets[j]);
				}
			}
		}
		yield return null;
	}

	public IEnumerator UnloadSurvivalAssetBundles()
	{
		UnloadRequest unloadRequest = StorageManager.CreateUnloadSceneBundlesRequest("Survival", unloadAllUnusedObjects: false, unloadAllUnusedAssets: true);
		yield return unloadRequest.StartRequest();
		if (_loadedSurvivalSpriteAtlases != null)
		{
			_loadedSurvivalSpriteAtlases.Clear();
		}
	}

	public IEnumerator LoadCommonAsset()
	{
		_loadedCommonSpriteAtlases = new List<SpriteAtlas>();
		bool flag = false;
		for (int i = 0; i < _availableContent.ContentPacks.Count; i++)
		{
			if (_availableContent.ContentPacks[i].Name != "Common" || !_availableContent.ContentPacks[i].Enabled)
			{
				continue;
			}
			for (int j = 0; j < _availableContent.ContentPacks[i].SpriteAtlases.Count; j++)
			{
				LoadAssetsRequest<SpriteAtlas> loadRequest = StorageManager.CreateLoadAssetsRequest<SpriteAtlas>(_availableContent.ContentPacks[i].SpriteAtlases[j].GetFullDirectoryPath(), _availableContent.ContentPacks[i].SpriteAtlases[j].GetNameWithVariant(_spriteAtlasVariant));
				yield return loadRequest.StartRequest();
				for (int k = 0; k < loadRequest.AllAssets.Length; k++)
				{
					_loadedCommonSpriteAtlases.Add(loadRequest.AllAssets[k]);
				}
			}
			flag = true;
			break;
		}
		if (!flag)
		{
			throw new UnityException("Common conent was not loaded! Check if Content \"Common\" exists in available content packs.");
		}
		yield return null;
	}

	public IEnumerator LoadScavangeAssets()
	{
		_loadedScavangeAtlases = new List<SpriteAtlas>();
		bool flag = false;
		for (int i = 0; i < _availableContent.ContentPacks.Count; i++)
		{
			if (_availableContent.ContentPacks[i].Name != "Scavenge" || !_availableContent.ContentPacks[i].Enabled)
			{
				continue;
			}
			for (int j = 0; j < _availableContent.ContentPacks[i].SpriteAtlases.Count; j++)
			{
				LoadAssetsRequest<SpriteAtlas> loadRequest = StorageManager.CreateLoadAssetsRequest<SpriteAtlas>(_availableContent.ContentPacks[i].SpriteAtlases[j].GetFullDirectoryPath(), _availableContent.ContentPacks[i].SpriteAtlases[j].GetNameWithVariant(_spriteAtlasVariant));
				yield return loadRequest.StartRequest();
				for (int k = 0; k < loadRequest.AllAssets.Length; k++)
				{
					_loadedScavangeAtlases.Add(loadRequest.AllAssets[k]);
				}
			}
			flag = true;
			break;
		}
		if (!flag)
		{
			throw new UnityException("Scavenge conent was not loaded! Check if Content \"Scavenge\" exists in available content packs.");
		}
		yield return null;
	}

	public IEnumerator UnloadScavangeAssetBundles()
	{
		UnloadRequest unloadRequest = StorageManager.CreateUnloadSceneBundlesRequest("Scavenge_start", unloadAllUnusedObjects: false, unloadAllUnusedAssets: true);
		yield return unloadRequest.StartRequest();
		if (_loadedScavangeAtlases != null)
		{
			_loadedScavangeAtlases.Clear();
		}
	}

	public IEnumerator LoadMenuAssets()
	{
		_loadedMenuAtlases = new List<SpriteAtlas>();
		bool flag = false;
		for (int i = 0; i < _availableContent.ContentPacks.Count; i++)
		{
			if (_availableContent.ContentPacks[i].Name != "Menu" || !_availableContent.ContentPacks[i].Enabled)
			{
				continue;
			}
			for (int j = 0; j < _availableContent.ContentPacks[i].SpriteAtlases.Count; j++)
			{
				LoadAssetsRequest<SpriteAtlas> loadRequest = StorageManager.CreateLoadAssetsRequest<SpriteAtlas>(_availableContent.ContentPacks[i].SpriteAtlases[j].GetFullDirectoryPath(), _availableContent.ContentPacks[i].SpriteAtlases[j].GetNameWithVariant(_spriteAtlasVariant));
				yield return loadRequest.StartRequest();
				for (int k = 0; k < loadRequest.AllAssets.Length; k++)
				{
					_loadedMenuAtlases.Add(loadRequest.AllAssets[k]);
				}
			}
			flag = true;
			break;
		}
		if (!flag)
		{
			throw new UnityException("Menu conent was not loaded! Check if Content \"Menu\" exists in available content packs.");
		}
		yield return null;
	}

	public IEnumerator UnloadMenuAssetBundles()
	{
		UnloadRequest unloadRequest = StorageManager.CreateUnloadSceneBundlesRequest("Menu", unloadAllUnusedObjects: false, unloadAllUnusedAssets: true);
		yield return unloadRequest.StartRequest();
		if (_loadedMenuAtlases != null)
		{
			_loadedMenuAtlases.Clear();
		}
	}

	public static Sprite GetSprite(string name)
	{
		if (_requestedSprites.ContainsKey(name))
		{
			return _requestedSprites[name];
		}
		if (Singleton<GameManager>.Instance.ContentManager._atlasesInBuild != null)
		{
			for (int i = 0; i < Singleton<GameManager>.Instance.ContentManager._atlasesInBuild.Count; i++)
			{
				Sprite sprite = Singleton<GameManager>.Instance.ContentManager._atlasesInBuild[i].GetSprite(name);
				if (sprite != null)
				{
					return sprite;
				}
			}
		}
		if (_loadedSurvivalSpriteAtlases != null)
		{
			for (int j = 0; j < _loadedSurvivalSpriteAtlases.Count; j++)
			{
				Sprite sprite2 = _loadedSurvivalSpriteAtlases[j].GetSprite(name);
				if (sprite2 != null)
				{
					return sprite2;
				}
			}
		}
		if (_loadedCommonSpriteAtlases != null)
		{
			for (int k = 0; k < _loadedCommonSpriteAtlases.Count; k++)
			{
				Sprite sprite3 = _loadedCommonSpriteAtlases[k].GetSprite(name);
				if (sprite3 != null)
				{
					return sprite3;
				}
			}
		}
		if (_loadedScavangeAtlases != null)
		{
			for (int l = 0; l < _loadedScavangeAtlases.Count; l++)
			{
				Sprite sprite4 = _loadedScavangeAtlases[l].GetSprite(name);
				if (sprite4 != null)
				{
					return sprite4;
				}
			}
		}
		if (_loadedMenuAtlases != null)
		{
			for (int m = 0; m < _loadedMenuAtlases.Count; m++)
			{
				Sprite sprite5 = _loadedMenuAtlases[m].GetSprite(name);
				if (sprite5 != null)
				{
					return sprite5;
				}
			}
		}
		return null;
	}
}
