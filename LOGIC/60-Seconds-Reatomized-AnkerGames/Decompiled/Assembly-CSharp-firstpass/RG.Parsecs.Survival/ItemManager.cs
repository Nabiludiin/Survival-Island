using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class ItemManager : Singleton<ItemManager>
{
	[SerializeField]
	private int _itemDurabilityUsage = 1;

	[SerializeField]
	private ItemList _itemList;

	[SerializeField]
	private PlayerResources _playerResources;

	public static int ITEM_DURABAILITY_USAGE => Singleton<ItemManager>.Instance._itemDurabilityUsage;

	public IItem GetItem(string id)
	{
		for (int i = 0; i < _itemList.Items.Count; i++)
		{
			if (_itemList.Items[i].BaseStaticData.ItemId.Equals(id))
			{
				return _itemList.Items[i];
			}
		}
		throw new UnityException($"Item: {id} not found on the item list");
	}

	public PlayerResources GetPlayerResources()
	{
		return _playerResources;
	}

	public ItemList GetItemList()
	{
		return _itemList;
	}
}
