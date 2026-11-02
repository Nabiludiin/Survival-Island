using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Common;

public class LoadAssetRequest<T> : LoadRequest where T : Object
{
	private readonly string _assetName;

	public T Asset { get; private set; }

	public override bool IsDone { get; protected set; }

	public LoadAssetRequest(MonoBehaviour owner, string assetBundlePath, string assetBundleName, string assetName)
		: base(owner, assetBundlePath, assetBundleName)
	{
		_assetName = assetName;
	}

	protected override IEnumerator Run()
	{
		yield return LoadAssetBundle();
		AssetBundleRequest loadAssetsRequest = LoadedAssetBundle.LoadAssetAsync<T>(_assetName);
		yield return loadAssetsRequest;
		Asset = (T)loadAssetsRequest.asset;
		AssetsLoaded(loadAssetsRequest.allAssets);
		IsDone = true;
		yield return null;
	}
}
