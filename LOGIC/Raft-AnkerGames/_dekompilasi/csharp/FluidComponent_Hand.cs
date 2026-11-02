public class FluidComponent_Hand : FluidComponent
{
	private PlayerInventory inventory;

	protected override void Start()
	{
		base.Start();
		inventory = ComponentManager<PlayerInventory>.Value;
		enabled = false;
	}

	public void OnSelect()
	{
		if (inventory == null)
		{
			inventory = ComponentManager<PlayerInventory>.Value;
		}
		if (!inventory.hotbar.playerNetwork.IsLocalPlayer)
		{
			return;
		}
		ItemInstance selectedHotbarItem = inventory.GetSelectedHotbarItem();
		if (selectedHotbarItem != null)
		{
			if (selectedHotbarItem.settings_consumeable.FoodType == FoodType.None)
			{
				HideFluid(instantly: true);
			}
			else if (selectedHotbarItem.Uses > 0)
			{
				ShowFluid();
			}
			else
			{
				HideFluid(instantly: true);
			}
		}
	}
}
