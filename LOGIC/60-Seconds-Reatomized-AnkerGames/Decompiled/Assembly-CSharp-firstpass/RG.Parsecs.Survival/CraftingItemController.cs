using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class CraftingItemController : CraftingCraftableController
{
	[SerializeField]
	private IItem _item;

	private bool _invokeOnValueChange = true;

	protected override Sprite SetSprite()
	{
		return _item.BaseStaticData.IconJournal;
	}

	public override void SetIsOnWithoutInvokingOnValueChange(bool isOn)
	{
		_invokeOnValueChange = false;
		IsOn = isOn;
		_invokeOnValueChange = true;
	}

	protected override void OnShow(OngoingCraftingActionData plannedCraftingAction)
	{
		CraftingOperation craftingOperation = null;
		switch (plannedCraftingAction.PlannedCraftingAction)
		{
		case EPlannedCraftingAction.CRAFT:
		{
			ConsumableRemedium consumableRemedium = _item as ConsumableRemedium;
			if ((!_item.BaseRuntimeData.IsAvailable && !_item.BaseRuntimeData.IsOnExpedition) || (consumableRemedium != null && consumableRemedium.RuntimeData.Amount <= consumableRemedium.StaticData.MaxAmount - 1f))
			{
				gameObject.SetActive(value: true);
				craftingOperation = _item.GetCreateCost();
			}
			else
			{
				gameObject.SetActive(value: false);
			}
			break;
		}
		case EPlannedCraftingAction.RECYCLE:
			if (_item.BaseRuntimeData.IsAvailable && (_item.IsLockable() || (plannedCraftingAction.Item != null && plannedCraftingAction.Item.BaseStaticData.ItemId.Equals(_item.BaseStaticData.ItemId))))
			{
				gameObject.SetActive(value: true);
				craftingOperation = _item.GetRecyclingCostForCurrentLevel();
			}
			else
			{
				gameObject.SetActive(value: false);
			}
			break;
		case EPlannedCraftingAction.REPAIR:
		{
			Item item = _item as Item;
			if (item != null && item.RuntimeData.IsDamaged && (_item.IsLockable() || (plannedCraftingAction.Item != null && plannedCraftingAction.Item.BaseStaticData.ItemId.Equals(_item.BaseStaticData.ItemId))) && item.RuntimeData.IsAvailable)
			{
				gameObject.SetActive(value: true);
				craftingOperation = item.GetRepairCostsForCurrentLevel();
			}
			else
			{
				gameObject.SetActive(value: false);
			}
			break;
		}
		case EPlannedCraftingAction.UPGRADE:
			if (_item.IsUpgradable() && (_item.IsLockable() || (plannedCraftingAction.Item != null && plannedCraftingAction.Item.BaseStaticData.ItemId.Equals(_item.BaseStaticData.ItemId))))
			{
				gameObject.SetActive(value: true);
				craftingOperation = _item.GetUpgradeCostForCurrentLevel();
			}
			else
			{
				gameObject.SetActive(value: false);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException("plannedCraftingAction", plannedCraftingAction, null);
		}
		if (craftingOperation != null)
		{
			if (CraftingData.SystemLevel < craftingOperation.RequiredSystemLevel)
			{
				gameObject.SetActive(value: false);
			}
			else if (CraftingData.SystemLevel > craftingOperation.RequiredSystemLevel && craftingOperation.AvailableOnlyOnRequiredSystemLevel)
			{
				gameObject.SetActive(value: false);
			}
			else if (!AvailableResources.CanLockForCraftingUI(craftingOperation.Resources) && plannedCraftingAction.PlannedCraftingAction != EPlannedCraftingAction.RECYCLE)
			{
				Togglable = false;
				DisabledReason = NoResourcesTerm;
			}
			else
			{
				Togglable = true;
			}
		}
	}

	public override void SetCraftingElement(bool isOn)
	{
		if (_invokeOnValueChange)
		{
			CraftingContentPanelController.SetItemForCraftingAction(isOn ? _item : null);
		}
	}
}
