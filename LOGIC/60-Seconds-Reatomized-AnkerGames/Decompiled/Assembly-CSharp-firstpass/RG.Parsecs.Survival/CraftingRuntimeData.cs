using System;
using I2.Loc;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class CraftingRuntimeData
{
	[SerializeField]
	private bool _isActive;

	[SerializeField]
	private bool _isRepaired;

	[SerializeField]
	private bool _isPlanned;

	[SerializeField]
	private bool _isOngoing;

	[SerializeField]
	private bool _shouldReactivate;

	[SerializeField]
	[Tooltip("Is system currently focused by desig ? Sets lamp color to blinking yellow.")]
	private bool _isFocusedByDesign;

	[SerializeField]
	private OngoingCraftingActionData _plannedCraftingActionData = new OngoingCraftingActionData();

	[SerializeField]
	private OngoingCraftingActionData _ongoingCraftingActionData = new OngoingCraftingActionData();

	[SerializeField]
	private int _craftingLevel = 1;

	[SerializeField]
	private int _craftingTotalTime;

	[SerializeField]
	private int _craftingCurrentTime;

	private bool _lockedCraftingResources;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelTitleTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelDescriptionTerm;

	public bool IsActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			_isActive = value;
		}
	}

	public bool IsOngoing
	{
		get
		{
			return _isOngoing;
		}
		set
		{
			_isOngoing = value;
		}
	}

	public OngoingCraftingActionData PlannedCraftingActionData
	{
		get
		{
			return _plannedCraftingActionData;
		}
		set
		{
			_plannedCraftingActionData = value;
		}
	}

	public OngoingCraftingActionData OngoingCraftingActionData
	{
		get
		{
			return _ongoingCraftingActionData;
		}
		set
		{
			_ongoingCraftingActionData = value;
		}
	}

	public bool IsRepaired
	{
		get
		{
			return _isRepaired;
		}
		set
		{
			_isRepaired = value;
		}
	}

	public int CraftingLevel
	{
		get
		{
			return _craftingLevel;
		}
		set
		{
			_craftingLevel = value;
		}
	}

	public bool IsPlanned
	{
		get
		{
			return _isPlanned;
		}
		set
		{
			_isPlanned = value;
		}
	}

	public bool ShouldReactivate
	{
		get
		{
			return _shouldReactivate;
		}
		set
		{
			_shouldReactivate = value;
		}
	}

	public int CraftingTotalTime
	{
		get
		{
			return _craftingTotalTime;
		}
		set
		{
			_craftingTotalTime = value;
		}
	}

	public int CraftingCurrentTime
	{
		get
		{
			return _craftingCurrentTime;
		}
		set
		{
			_craftingCurrentTime = value;
		}
	}

	public LocalizedString BlockedPanelTitleTerm
	{
		get
		{
			return _blockedPanelTitleTerm;
		}
		set
		{
			_blockedPanelTitleTerm = value;
		}
	}

	public LocalizedString BlockedPanelDescriptionTerm
	{
		get
		{
			return _blockedPanelDescriptionTerm;
		}
		set
		{
			_blockedPanelDescriptionTerm = value;
		}
	}

	public bool IsFocusedByDesign
	{
		get
		{
			return _isFocusedByDesign;
		}
		set
		{
			_isFocusedByDesign = value;
		}
	}

	public void LockCraftingResources()
	{
		if (_lockedCraftingResources)
		{
			return;
		}
		_lockedCraftingResources = true;
		switch (PlannedCraftingActionData.PlannedCraftingAction)
		{
		case EPlannedCraftingAction.CRAFT:
		{
			CraftingOperation createCost = PlannedCraftingActionData.Item.GetCreateCost();
			for (int l = 0; l < createCost.Resources.Count; l++)
			{
				Singleton<ItemManager>.Instance.GetPlayerResources().Lock(createCost.Resources[l]);
			}
			break;
		}
		case EPlannedCraftingAction.REPAIR:
		{
			CraftingOperation repairCostsForCurrentLevel = ((Item)PlannedCraftingActionData.Item).GetRepairCostsForCurrentLevel();
			for (int k = 0; k < repairCostsForCurrentLevel.Resources.Count; k++)
			{
				Singleton<ItemManager>.Instance.GetPlayerResources().Lock(repairCostsForCurrentLevel.Resources[k]);
			}
			break;
		}
		case EPlannedCraftingAction.UPGRADE:
			if (PlannedCraftingActionData.Item != null)
			{
				CraftingOperation upgradeCostForCurrentLevel = PlannedCraftingActionData.Item.GetUpgradeCostForCurrentLevel();
				for (int i = 0; i < upgradeCostForCurrentLevel.Resources.Count; i++)
				{
					Singleton<ItemManager>.Instance.GetPlayerResources().Lock(upgradeCostForCurrentLevel.Resources[i]);
				}
			}
			else if (PlannedCraftingActionData.System != null)
			{
				CraftingOperation upgradeCostForCurrentLevel2 = PlannedCraftingActionData.System.GetUpgradeCostForCurrentLevel();
				for (int j = 0; j < upgradeCostForCurrentLevel2.Resources.Count; j++)
				{
					Singleton<ItemManager>.Instance.GetPlayerResources().Lock(upgradeCostForCurrentLevel2.Resources[j]);
				}
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case EPlannedCraftingAction.NONE:
		case EPlannedCraftingAction.RECYCLE:
			break;
		}
	}

	public CraftingOperation GetCurrentCraftingOperation()
	{
		switch (PlannedCraftingActionData.PlannedCraftingAction)
		{
		case EPlannedCraftingAction.CRAFT:
			if (PlannedCraftingActionData.Item != null)
			{
				return PlannedCraftingActionData.Item.GetCreateCost();
			}
			break;
		case EPlannedCraftingAction.RECYCLE:
			if (PlannedCraftingActionData.Item != null)
			{
				return PlannedCraftingActionData.Item.GetUpgradeCostForCurrentLevel();
			}
			break;
		case EPlannedCraftingAction.REPAIR:
			if (PlannedCraftingActionData.Item != null)
			{
				return ((Item)PlannedCraftingActionData.Item).GetRepairCostsForCurrentLevel();
			}
			break;
		case EPlannedCraftingAction.UPGRADE:
			if (PlannedCraftingActionData.Item != null)
			{
				return PlannedCraftingActionData.Item.GetUpgradeCostForCurrentLevel();
			}
			if (PlannedCraftingActionData.System != null)
			{
				return PlannedCraftingActionData.System.GetUpgradeCostForCurrentLevel();
			}
			break;
		}
		return null;
	}

	public void UnlockCraftingResources()
	{
		if (!_lockedCraftingResources)
		{
			return;
		}
		_lockedCraftingResources = false;
		switch (PlannedCraftingActionData.PlannedCraftingAction)
		{
		case EPlannedCraftingAction.CRAFT:
		{
			CraftingOperation createCost = PlannedCraftingActionData.Item.GetCreateCost();
			for (int l = 0; l < createCost.Resources.Count; l++)
			{
				Singleton<ItemManager>.Instance.GetPlayerResources().Unlock(createCost.Resources[l]);
			}
			break;
		}
		case EPlannedCraftingAction.REPAIR:
		{
			CraftingOperation repairCostsForCurrentLevel = ((Item)PlannedCraftingActionData.Item).GetRepairCostsForCurrentLevel();
			for (int k = 0; k < repairCostsForCurrentLevel.Resources.Count; k++)
			{
				Singleton<ItemManager>.Instance.GetPlayerResources().Unlock(repairCostsForCurrentLevel.Resources[k]);
			}
			break;
		}
		case EPlannedCraftingAction.UPGRADE:
			if (PlannedCraftingActionData.Item != null)
			{
				CraftingOperation upgradeCostForCurrentLevel = PlannedCraftingActionData.Item.GetUpgradeCostForCurrentLevel();
				for (int i = 0; i < upgradeCostForCurrentLevel.Resources.Count; i++)
				{
					Singleton<ItemManager>.Instance.GetPlayerResources().Unlock(upgradeCostForCurrentLevel.Resources[i]);
				}
			}
			else if (PlannedCraftingActionData.System != null)
			{
				CraftingOperation upgradeCostForCurrentLevel2 = PlannedCraftingActionData.System.GetUpgradeCostForCurrentLevel();
				for (int j = 0; j < upgradeCostForCurrentLevel2.Resources.Count; j++)
				{
					Singleton<ItemManager>.Instance.GetPlayerResources().Unlock(upgradeCostForCurrentLevel2.Resources[j]);
				}
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case EPlannedCraftingAction.NONE:
		case EPlannedCraftingAction.RECYCLE:
			break;
		}
	}
}
