public class BuildingUI_CostBox_CraftingCost : BuildingUI_CostBox_Disabled, SceneEventInterface
{
	private static PlayerInventory playerInventory;

	public PlayerInventory PlayerInventory => playerInventory;

	private void Start()
	{
		if (playerInventory == null)
		{
			playerInventory = ComponentManager<Network_Player>.Value.Inventory;
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			playerInventory = null;
		}
	}

	public override void PointerEnterSub(BuildingUI_CostBox_Sub sub)
	{
		if (sub != null && playerInventory != null)
		{
			playerInventory.SetItemDescription(sub.item);
		}
	}

	public override void PointerExitSub(BuildingUI_CostBox_Sub sub)
	{
		if (playerInventory != null)
		{
			playerInventory.SetItemDescription(null);
		}
	}
}
