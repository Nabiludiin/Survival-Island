using System;

[Serializable]
public class CostMultiple
{
	public Item_Base[] items;

	public int amount;

	public CostMultiple()
	{
	}

	public CostMultiple(Item_Base[] items, int amount)
	{
		this.items = items;
		this.amount = amount;
	}

	public bool ContainsItem(Item_Base item)
	{
		Item_Base[] array = items;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].UniqueIndex == item.UniqueIndex)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasEnoughInInventory(Inventory inventory)
	{
		int num = 0;
		for (int i = 0; i < items.Length; i++)
		{
			if (!(items[i] == null))
			{
				num += inventory.GetItemCount(items[i].UniqueName);
			}
		}
		return num >= amount;
	}
}
