using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Common;

[Serializable]
public class ContentPack
{
	[Tooltip("Name of the content pack")]
	public string Name = string.Empty;

	[Tooltip("Is this content pack enabled in game.")]
	public bool Enabled;

	[Tooltip("Events contained in these content pack.")]
	public List<AssetBundleData> Events;

	[Tooltip("Decks contained in these content pack.")]
	public List<AssetBundleData> Decks;

	[Tooltip("Sprite Atlases contained in this content pack.")]
	public List<AssetBundleData> SpriteAtlases;

	public ContentPack(string name, bool enabled)
	{
		Name = name;
		Enabled = enabled;
	}
}
