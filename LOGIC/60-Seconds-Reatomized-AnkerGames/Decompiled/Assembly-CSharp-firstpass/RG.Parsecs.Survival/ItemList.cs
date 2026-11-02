using System.Collections.Generic;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Crafting/New Item list", fileName = "New Item list")]
public class ItemList : RGScriptableObject
{
	public List<IItem> Items;

	public IItem GetItemOfId(string id)
	{
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i].BaseStaticData.ItemId.Equals(id))
			{
				return Items[i];
			}
		}
		return null;
	}

	public List<Item> GetItems()
	{
		List<Item> list = new List<Item>();
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i] is Item)
			{
				list.Add((Item)Items[i]);
			}
		}
		return list;
	}

	public List<Remedium> GetRemedies()
	{
		List<Remedium> list = new List<Remedium>();
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i] is Remedium)
			{
				list.Add((Remedium)Items[i]);
			}
		}
		return list;
	}

	public List<ConsumableRemedium> GetConsumableRemedies()
	{
		List<ConsumableRemedium> list = new List<ConsumableRemedium>();
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i] is ConsumableRemedium)
			{
				list.Add((ConsumableRemedium)Items[i]);
			}
		}
		return list;
	}
}
