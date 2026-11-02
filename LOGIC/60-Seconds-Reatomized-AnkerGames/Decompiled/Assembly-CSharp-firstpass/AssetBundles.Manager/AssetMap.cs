using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundles.Manager;

[Serializable]
public class AssetMap
{
	[Serializable]
	public class AssetMapEntry
	{
		public string path;

		public string bundleName;

		public string typeFullName;

		public AssetMapEntry(string p, string b, string t)
		{
			path = p;
			bundleName = b.ToLower();
			typeFullName = t;
		}
	}

	[SerializeField]
	private List<AssetMapEntry> m_assets;

	[SerializeField]
	private string m_manifestName;

	public string ManifestFileName => m_manifestName;

	public string GetAssetBundleName(string assetPath)
	{
		if (m_assets == null)
		{
			return string.Empty;
		}
		foreach (AssetMapEntry asset in m_assets)
		{
			if (assetPath == asset.path)
			{
				return asset.bundleName;
			}
		}
		return string.Empty;
	}

	public AssetMap()
	{
		m_assets = new List<AssetMapEntry>();
	}
}
