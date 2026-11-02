using UnityEngine;

namespace AssetBundles.Manager;

public abstract class AssetBundleLoadAssetOperation : AssetBundleLoadOperation
{
	public abstract T GetAsset<T>() where T : Object;
}
