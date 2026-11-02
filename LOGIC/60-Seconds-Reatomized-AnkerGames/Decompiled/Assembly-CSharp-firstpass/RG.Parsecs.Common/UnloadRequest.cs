using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Common;

public class UnloadRequest : Request
{
	public StorageManager.AssetBundleUnload AssetBundleUnloaded;

	private readonly AssetBundle[] _assetBundlesToUnload;

	private readonly IEnumerator _assetBundlesEnumerator;

	private readonly bool _unloadAllUnusedObjects;

	private readonly bool _unloadAllUnusedAssets;

	private readonly bool _nothingToUnload;

	public override bool IsDone { get; protected set; }

	private UnloadRequest(MonoBehaviour owner, bool unloadAllUnusedObjects, bool unloadAllUnusedAssets)
		: base(owner)
	{
		_nothingToUnload = false;
		_unloadAllUnusedObjects = unloadAllUnusedObjects;
		_unloadAllUnusedAssets = unloadAllUnusedAssets;
		if (_unloadAllUnusedAssets && _unloadAllUnusedObjects)
		{
			Debug.Log("You don't need to perform Resources.UnloadUnusedAssets() if you're unloading Asset Bundles with unloadAllUnusedObjects set to true.");
		}
	}

	public UnloadRequest(MonoBehaviour owner)
		: base(owner)
	{
		_nothingToUnload = true;
	}

	public UnloadRequest(MonoBehaviour owner, AssetBundle[] assetBundlesToUnload, bool unloadAllUnusedObjects, bool unloadAllUnusedAssets)
		: this(owner, unloadAllUnusedObjects, unloadAllUnusedAssets)
	{
		_assetBundlesToUnload = assetBundlesToUnload;
	}

	public UnloadRequest(MonoBehaviour owner, IEnumerator assetBundlesEnumerator, bool unloadAllUnusedObjects, bool unloadAllUnusedAssets)
		: this(owner, unloadAllUnusedObjects, unloadAllUnusedAssets)
	{
		_assetBundlesEnumerator = assetBundlesEnumerator;
	}

	protected override IEnumerator Run()
	{
		if (_nothingToUnload)
		{
			IsDone = true;
			yield break;
		}
		List<string> unloadedBundles = new List<string>();
		if (_assetBundlesEnumerator == null)
		{
			for (int i = 0; i < _assetBundlesToUnload.Length; i++)
			{
				unloadedBundles.Add(_assetBundlesToUnload[i].name);
				Debug.Log(_assetBundlesToUnload[i].name);
				_assetBundlesToUnload[i].Unload(_unloadAllUnusedObjects);
			}
		}
		else
		{
			while (_assetBundlesEnumerator.MoveNext())
			{
				if (_assetBundlesEnumerator.Current != null)
				{
					AssetBundle assetBundle = (AssetBundle)_assetBundlesEnumerator.Current;
					unloadedBundles.Add(assetBundle.name);
					assetBundle.Unload(_unloadAllUnusedObjects);
				}
			}
		}
		if (_unloadAllUnusedAssets)
		{
			yield return Resources.UnloadUnusedAssets();
		}
		AssetBundleUnloaded(unloadedBundles);
		IsDone = true;
		yield return null;
	}
}
