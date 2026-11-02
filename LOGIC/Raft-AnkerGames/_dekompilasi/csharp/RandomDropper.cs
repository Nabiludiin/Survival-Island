using UnityEngine;

public class RandomDropper : MonoBehaviour
{
	[SerializeField]
	private SO_RandomDropper randomDropperAsset;

	[SerializeField]
	private Interval_Int amountOfItems;

	private void OnValidate()
	{
		if (amountOfItems != null)
		{
			if (amountOfItems.minValue < 1)
			{
				amountOfItems.minValue = 1;
			}
			if (amountOfItems.maxValue < amountOfItems.minValue)
			{
				amountOfItems.maxValue = amountOfItems.minValue;
			}
		}
	}

	private void DebugLogLoot()
	{
		Item_Base[] randomItems = GetRandomItems();
		foreach (Item_Base item_Base in randomItems)
		{
			if (item_Base == null)
			{
				Debug.Log("Empty");
			}
			else
			{
				Debug.Log(item_Base);
			}
		}
	}

	public Item_Base[] GetRandomItems()
	{
		return randomDropperAsset.GetRandomItems(amountOfItems.GetRandomValue());
	}

	public Item_Base GetRandomItem()
	{
		return randomDropperAsset.GetRandomItem();
	}
}
