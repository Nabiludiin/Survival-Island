using System;
using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class CraftingItemUITooltipContent : TooltipContent
{
	[Tooltip("Only one of the Item and System fields needs to have a value - this tooltip content is reused for both item and system tooltips in crafting UI")]
	[SerializeField]
	private IItem _item;

	[Tooltip("Only one of the Item and System fields needs to have a value - this tooltip content is reused for both item and system tooltips in crafting UI")]
	[SerializeField]
	private ShipSystem _system;

	private EPlannedCraftingAction _craftingAction;

	public LocalizedString Name
	{
		get
		{
			if (_item != null)
			{
				return _item.BaseStaticData.Name;
			}
			if (_system != null)
			{
				return _system.Name;
			}
			Debug.LogErrorFormat("Returning empty localized string!");
			return new LocalizedString("NULL");
		}
	}

	public bool DisplayOnExpeditonText
	{
		get
		{
			if (_system == null)
			{
				return false;
			}
			if (!_system.AvailableInCraftingDuringExpedition)
			{
				return _craftingAction == EPlannedCraftingAction.UPGRADE;
			}
			return false;
		}
	}

	public EPlannedCraftingAction CraftingAction => _craftingAction;

	public void SetCraftingAction(EPlannedCraftingAction craftingAction)
	{
		_craftingAction = craftingAction;
	}

	public CraftingOperation GetOperationCost()
	{
		if (_item != null)
		{
			switch (_craftingAction)
			{
			case EPlannedCraftingAction.CRAFT:
				return _item.GetCreateCost();
			case EPlannedCraftingAction.RECYCLE:
				return _item.GetRecyclingCostForCurrentLevel();
			case EPlannedCraftingAction.REPAIR:
			{
				Item item = _item as Item;
				if (item != null)
				{
					return item.GetRepairCostsForCurrentLevel();
				}
				Debug.LogErrorFormat("Crafting operation was trying to get Repair cost for tooltip, but item was null. Returning default CraftingOperation object.");
				return new CraftingOperation();
			}
			case EPlannedCraftingAction.UPGRADE:
				return _item.GetUpgradeCostForCurrentLevel();
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		if (_system != null && _craftingAction == EPlannedCraftingAction.UPGRADE)
		{
			return _system.GetUpgradeCostForCurrentLevel();
		}
		Debug.LogErrorFormat("Tooltip couldn't get correct crafting operation. Returning default CraftingOperation object.");
		return new CraftingOperation();
	}

	public override bool IsValid()
	{
		if (!(_item != null))
		{
			return _system != null;
		}
		return true;
	}
}
