using System.Collections.Generic;

public class UI_Cost_Interactable_FishingBait : UI_Cost_Interactable
{
	public static Network_Player player;

	public static List<UI_Cost_Interactable_FishingBait> allFishingBaitCostInteractable = new List<UI_Cost_Interactable_FishingBait>();

	public Item_Base baitToEquip;

	public static UI_Cost_Interactable_FishingBait currentSelected;

	private void Awake()
	{
		allFishingBaitCostInteractable.Add(this);
	}

	private void OnDestroy()
	{
		allFishingBaitCostInteractable.Clear();
	}

	private void OnEnable()
	{
		if (player == null)
		{
			player = ComponentManager<Network_Player>.Value;
		}
		if (player == null)
		{
			return;
		}
		if (baitToEquip != null)
		{
			int itemCount = player.Inventory.GetItemCount(baitToEquip);
			RefreshDisplayInventoryCount(new Cost(baitToEquip, 1), itemCount);
			if (itemCount <= 0 && currentSelected == this)
			{
				currentSelected = null;
				ClearAllHighlights();
				return;
			}
		}
		if (hoverImage != null && player.FishingBaitHandler.CurrentBait == baitToEquip)
		{
			currentSelected = this;
			SetHoverImageActive();
		}
		else
		{
			SetHoverImageInactive();
		}
	}

	private void Start()
	{
		if (player == null)
		{
			player = ComponentManager<Network_Player>.Value;
		}
	}

	protected override void SetHoverImageInactive()
	{
		if (currentSelected != this)
		{
			base.SetHoverImageInactive();
		}
	}

	private void ClearAllHighlights()
	{
		foreach (UI_Cost_Interactable_FishingBait item in allFishingBaitCostInteractable)
		{
			item.SetHoverImageInactive();
		}
	}

	public void OnClick()
	{
		if (player == null)
		{
			player = ComponentManager<Network_Player>.Value;
		}
		if (player == null)
		{
			return;
		}
		if (player.Inventory.GetItemCount(baitToEquip) > 0)
		{
			if (player.FishingBaitHandler != null)
			{
				currentSelected = this;
				player.FishingBaitHandler.EquipBait(baitToEquip);
				ClearAllHighlights();
				SetHoverImageActive();
			}
		}
		else
		{
			if (baitToEquip == null)
			{
				currentSelected = this;
				ClearAllHighlights();
				SetHoverImageActive();
			}
			player.FishingBaitHandler.EquipBait(null);
		}
	}
}
