using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockCollisionMask", menuName = "Raft/Block/BlockCollisionMask", order = 1)]
public class SO_BlockCollisionMask : ScriptableObject
{
	public bool ignoreAll;

	public bool exceptSelf;

	public bool ignoreAllPlaceables;

	public bool ignoreAllNonPlaceables;

	[SerializeField]
	private List<Item_Base> blockTypesToIgnore = new List<Item_Base>();

	private void IgnorePlaceableValueChanged()
	{
		if (ignoreAllPlaceables)
		{
			ignoreAllNonPlaceables = false;
		}
		else if (ignoreAllNonPlaceables)
		{
			ignoreAllPlaceables = false;
		}
	}

	public bool IgnoresBlock(Item_Base itemToCheck)
	{
		if (ignoreAll)
		{
			return true;
		}
		if (ignoreAllNonPlaceables && !itemToCheck.settings_buildable.Placeable)
		{
			return true;
		}
		if (ignoreAllPlaceables && itemToCheck.settings_buildable.Placeable)
		{
			return true;
		}
		foreach (Item_Base item in blockTypesToIgnore)
		{
			if (item.UniqueIndex == itemToCheck.UniqueIndex)
			{
				return true;
			}
		}
		return false;
	}
}
