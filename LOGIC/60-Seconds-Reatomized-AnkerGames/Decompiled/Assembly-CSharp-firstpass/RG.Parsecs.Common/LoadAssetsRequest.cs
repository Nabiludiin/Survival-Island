using System;
using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Common;

public class LoadAssetsRequest<T> : LoadRequest where T : UnityEngine.Object
{
	public T[] AllAssets { get; private set; }

	public override bool IsDone { get; protected set; }

	public LoadAssetsRequest(MonoBehaviour owner, string assetBundlePath, string assetBundleName)
		: base(owner, assetBundlePath, assetBundleName)
	{
	}

	protected override IEnumerator Run()
	{
		yield return LoadAssetBundle();
		AssetBundleRequest loadAssetsRequest = LoadedAssetBundle.LoadAllAssetsAsync<T>();
		yield return loadAssetsRequest;
		AllAssets = Array.ConvertAll(loadAssetsRequest.allAssets, (UnityEngine.Object asset) => (T)asset);
		if (AssetsLoaded != null)
		{
			AssetsLoaded(loadAssetsRequest.allAssets);
		}
		IsDone = true;
		yield return null;
	}
}
