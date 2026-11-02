using System.Collections;
using System.IO;
using UnityEngine;

namespace RG.Parsecs.Common;

public abstract class LoadRequest : Request
{
	public StorageManager.AssetBundleLoad AssetBundleLoaded;

	public StorageManager.AssetsLoaded AssetsLoaded;

	private readonly string _assetBundlePath;

	private readonly string _assetBundleName;

	protected AssetBundle LoadedAssetBundle { get; private set; }

	protected LoadRequest(MonoBehaviour owner, string assetBundlePath, string assetBundleName)
		: base(owner)
	{
		_assetBundleName = assetBundleName;
		_assetBundlePath = Path.Combine(assetBundlePath, assetBundleName);
	}

	protected IEnumerator LoadAssetBundle()
	{
		LoadedAssetBundle = StorageManager.GetLoadedAssetBundle(_assetBundleName);
		if (LoadedAssetBundle == null)
		{
			AssetBundleCreateRequest assetBundleRequest = AssetBundle.LoadFromFileAsync(_assetBundlePath);
			yield return assetBundleRequest;
			if (AssetBundleLoaded != null)
			{
				AssetBundleLoaded(assetBundleRequest.assetBundle);
			}
			LoadedAssetBundle = assetBundleRequest.assetBundle;
		}
		yield return null;
	}
}
