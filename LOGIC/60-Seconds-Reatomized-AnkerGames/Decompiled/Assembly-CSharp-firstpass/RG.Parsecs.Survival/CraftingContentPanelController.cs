using UnityEngine;

namespace RG.Parsecs.Survival;

public class CraftingContentPanelController : MonoBehaviour
{
	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private CraftingCraftableController[] _craftingCraftableControllers;

	[SerializeField]
	private RepairedCraftingPanelController _repairedCraftingPanelController;

	[SerializeField]
	private CraftingSummaryAnimationController _summaryController;

	[SerializeField]
	private CanvasGroup _itemSelectionPanel;

	[SerializeField]
	private SetScrollRectPosition _scrollRect;

	public void Show()
	{
		for (int i = 0; i < _craftingCraftableControllers.Length; i++)
		{
			_craftingCraftableControllers[i].Show(_craftingData.RuntimeData.PlannedCraftingActionData);
		}
		_itemSelectionPanel.interactable = true;
		_summaryController.Reset();
		if (_craftingData.RuntimeData.PlannedCraftingActionData.Item != null)
		{
			_summaryController.SetForceTransition();
		}
		ResetSliderInitialValue();
	}

	private void ResetSliderInitialValue()
	{
		_scrollRect.SetScrollRectPos(Vector2.zero);
		_scrollRect.RefreshScrollBarVisibility();
	}

	public void SetItemForCraftingAction(IItem item)
	{
		if (_summaryController.Ready)
		{
			if ((_craftingData.RuntimeData.PlannedCraftingActionData.Item != null || _craftingData.RuntimeData.PlannedCraftingActionData.System != null) && _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.RECYCLE && _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.NONE)
			{
				_craftingData.RuntimeData.UnlockCraftingResources();
			}
			if ((item == null || !IsCraftingActionPlannedForItem(item)) && IsPlannedActionItemUnlockNecessary())
			{
				_craftingData.RuntimeData.PlannedCraftingActionData.Item.Unlock();
			}
			_craftingData.RuntimeData.PlannedCraftingActionData.Item = item;
			SetCraftingPlanned(item != null);
			if (item != null)
			{
				_craftingData.RuntimeData.LockCraftingResources();
			}
			_summaryController.Show(item != null);
		}
	}

	private bool IsCraftingActionPlannedForItem(IItem item)
	{
		if (_craftingData.RuntimeData.PlannedCraftingActionData.Item != null)
		{
			return item.BaseStaticData.ItemId.Equals(_craftingData.RuntimeData.PlannedCraftingActionData.Item.BaseStaticData.ItemId);
		}
		return false;
	}

	private bool IsPlannedActionItemUnlockNecessary()
	{
		if (_craftingData.RuntimeData.PlannedCraftingActionData.Item != null && _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.CRAFT)
		{
			return _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.NONE;
		}
		return false;
	}

	public void SetSystemForCraftingAction(ShipSystem system)
	{
		if ((_craftingData.RuntimeData.PlannedCraftingActionData.Item != null || _craftingData.RuntimeData.PlannedCraftingActionData.System != null) && _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.RECYCLE && _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.NONE)
		{
			_craftingData.RuntimeData.UnlockCraftingResources();
		}
		_craftingData.RuntimeData.PlannedCraftingActionData.System = system;
		SetCraftingPlanned(system != null);
		if (system != null)
		{
			_craftingData.RuntimeData.LockCraftingResources();
		}
		_summaryController.Show(system != null);
	}

	public void SetCraftingPlanned(bool planned)
	{
		if (!planned)
		{
			_craftingData.RuntimeData.IsPlanned = false;
		}
		_repairedCraftingPanelController.SetConfirmButtonInteractable(planned);
	}

	public void SetAllTogglesOff()
	{
		for (int i = 0; i < _craftingCraftableControllers.Length; i++)
		{
			_craftingCraftableControllers[i].SetIsOnWithoutInvokingOnValueChange(isOn: false);
		}
	}
}
