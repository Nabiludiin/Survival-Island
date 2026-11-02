using UnityEngine;

namespace AssetBundles.Manager;

public class LoadedAssetBundle
{
	public AssetBundle m_AssetBundle;

	public int m_ReferencedCount;

	public LoadedAssetBundle(AssetBundle assetBundle)
	{
		m_AssetBundle = assetBundle;
		m_ReferencedCount = 1;
	}
}
