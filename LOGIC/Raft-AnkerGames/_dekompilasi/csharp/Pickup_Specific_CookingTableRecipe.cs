using UnityEngine;

public class Pickup_Specific_CookingTableRecipe : Pickup_Specific
{
	[SerializeField]
	private RandomDropper[] droppers;

	public override void PickupSpecific(PlayerInventory inventory)
	{
		base.PickupSpecific(inventory);
		if (droppers != null)
		{
			int num = Random.Range(0, droppers.Length);
			if (num >= 0 && num < droppers.Length)
			{
				RandomDropper dropper = droppers[num];
				TryAddingRecipeLoot(dropper, inventory);
			}
		}
	}

	private void TryAddingRecipeLoot(RandomDropper dropper, PlayerInventory inventory)
	{
		if (dropper == null || inventory == null)
		{
			return;
		}
		Item_Base randomItem = dropper.GetRandomItem();
		if (randomItem == null)
		{
			return;
		}
		int num = 20;
		while (randomItem == null || inventory.GetItemCount(randomItem) > 0)
		{
			randomItem = dropper.GetRandomItem();
			num--;
			if (num <= 0)
			{
				return;
			}
		}
		inventory.AddItem(randomItem.UniqueName, 1);
	}
}
