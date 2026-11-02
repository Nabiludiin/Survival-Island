using UnityEngine;

namespace RG.Parsecs.Survival;

public class CraftingView : WindowView
{
	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private RepairedCraftingPanelController _repairedCraftingPanelController;

	[SerializeField]
	private DamagedCraftingPanelController _damagedCraftingPanelController;

	[SerializeField]
	private CraftingSystemView _craftingSystemView;

	public override void OnShow()
	{
		if (_craftingData.RuntimeData.IsRepaired)
		{
			_repairedCraftingPanelController.gameObject.SetActive(value: true);
			_damagedCraftingPanelController.gameObject.SetActive(value: false);
			_repairedCraftingPanelController.Show();
		}
		else
		{
			_repairedCraftingPanelController.gameObject.SetActive(value: false);
			_damagedCraftingPanelController.gameObject.SetActive(value: true);
			_damagedCraftingPanelController.Show();
		}
	}

	public void ResetCraftingView()
	{
		if (_craftingData.RuntimeData.IsRepaired && _craftingData.RuntimeData.IsActive)
		{
			_repairedCraftingPanelController.ResetPanel(isVisible: false);
		}
	}

	public void RepairCrafting()
	{
		if (_craftingData.RuntimeData.ShouldReactivate && !_craftingData.RuntimeData.IsRepaired)
		{
			_damagedCraftingPanelController.Repair();
		}
	}

	public void OnCloseButtonPressed()
	{
		HideCrafting();
	}

	protected override void OnClickAway()
	{
		HideCrafting();
	}

	private void HideCrafting()
	{
		Hide();
		if (_craftingData.RuntimeData.IsRepaired && _craftingData.RuntimeData.IsActive && !_craftingData.RuntimeData.IsPlanned)
		{
			_repairedCraftingPanelController.ResetPanel(isVisible: false);
		}
	}
}
