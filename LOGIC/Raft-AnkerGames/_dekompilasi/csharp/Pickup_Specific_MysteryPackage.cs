using UnityEngine;

public class Pickup_Specific_MysteryPackage : Pickup_Specific
{
	[SerializeField]
	private float requiredTimePlayed = 30f;

	[SerializeField]
	private Item_Base mysteryBoxItem;

	[SerializeField]
	private SO_MysteryPackageLoot mysteryLoot;

	[SerializeField]
	private float mysteryLootDropChance = 40f;

	public override void PickupSpecific(PlayerInventory inventory)
	{
		base.PickupSpecific(inventory);
		if (!(inventory == null) && (!(requiredTimePlayed > 0f) || !(GameManager.TimePlayed < requiredTimePlayed * 60f)) && mysteryLoot.HasUnlearnedLoot() && (float)Random.Range(0, 100) <= mysteryLootDropChance)
		{
			inventory.AddItem(mysteryBoxItem.UniqueName, 1);
		}
	}
}
