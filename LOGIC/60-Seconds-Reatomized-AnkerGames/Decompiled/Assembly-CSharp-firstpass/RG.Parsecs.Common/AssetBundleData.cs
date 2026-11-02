using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Common;

[Serializable]
public class AssetBundleData
{
	[SerializeField]
	private string _name = string.Empty;

	public bool StreamingAssets = true;

	public string DirectoryPath = string.Empty;

	public string RelativePath = string.Empty;

	public List<string> Variants;

	public string Name
	{
		get
		{
			if (Variants != null && Variants.Count > 0)
			{
				throw new UnityException("This bundle has variants so you can't use name directly. Use GetNameWithVariant() instead.");
			}
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public string GetFullDirectoryPath()
	{
		if (StreamingAssets)
		{
			DirectoryPath = Application.streamingAssetsPath;
		}
		return (Application.streamingAssetsPath + RelativePath).Replace('\\', '/');
	}

	public string GetNameWithVariant(string variant)
	{
		if (Variants == null || Variants.Count == 0)
		{
			return _name;
		}
		if (!Variants.Contains(variant))
		{
			Debug.LogWarning($"There is no variant {variant} for bundle {_name}. Available variants: {GetVariantsList()}");
		}
		return $"{_name}.{variant}";
	}

	private string GetVariantsList()
	{
		if (Variants == null || Variants.Count == 0)
		{
			return "";
		}
		string text = Variants[0];
		for (int i = 1; i < Variants.Count; i++)
		{
			text += $", {Variants[i]}";
		}
		return text;
	}
}
