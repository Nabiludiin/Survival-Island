using UnityEngine;

public class Pickup_Specific_Blueprint : Pickup_Specific
{
	[SerializeField]
	private float requiredTimePlayed;

	[SerializeField]
	private RandomDropper blueprintDropper;

	public override void PickupSpecific(PlayerInventory inventory)
	{
		base.PickupSpecific(inventory);
		if (inventory == null || (requiredTimePlayed > 0f && GameManager.TimePlayed < requiredTimePlayed * 60f))
		{
			return;
		}
		Item_Base randomItem = blueprintDropper.GetRandomItem();
		if (!(randomItem == null))
		{
			Inventory_ResearchTable value = ComponentManager<Inventory_ResearchTable>.Value;
			if (!(value != null) || !value.HasBeenResearched(randomItem))
			{
				inventory.AddItem(randomItem.UniqueName, 1);
			}
		}
	}
}
