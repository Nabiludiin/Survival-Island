using System;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class CraftingManager : MonoBehaviour
{
	private static CraftingManager _instance;

	[Header("References")]
	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	[SerializeField]
	private Scheduler _scheduler;

	[SerializeField]
	private PlayerResources _availableResources;

	[SerializeField]
	private CraftingView _craftingView;

	[SerializeField]
	private GameObject _craftingDisplayPaper;

	[SerializeField]
	private ItemList _itemList;

	[SerializeField]
	private ShipSystem[] _systems;

	[SerializeField]
	private PlayerResources _playerResources;

	private static int RESET_LISTENER_LIST_PRIORITY = 10;

	[SerializeField]
	[Tooltip("Reference to Survival Data.")]
	private SurvivalData _survivalData;

	[SerializeField]
	private EPlannedCraftingAction _interruptedCraftingOperation;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		if (!(_craftingView == null))
		{
			_endOfDayListenerList.RegisterOnEndOfDay(SetCraftingActionEvent, "PreEventSystems", 5, this);
			_endOfDayListenerList.RegisterOnEndOfDay(_craftingView.RepairCrafting, "PreEventSystems", 6, this);
			_endOfDayListenerList.RegisterOnEndOfDay(_craftingView.ResetCraftingView, "Reset", RESET_LISTENER_LIST_PRIORITY, this);
			_endOfDayListenerList.RegisterOnEndOfDay(FinishCrafting, "PreEventSystems", 7, this);
		}
	}

	private void OnDestroy()
	{
		if (!(_craftingView == null))
		{
			_endOfDayListenerList.UnregisterOnEndOfDay(SetCraftingActionEvent, "PreEventSystems");
			_endOfDayListenerList.UnregisterOnEndOfDay(_craftingView.RepairCrafting, "PreEventSystems");
			_endOfDayListenerList.UnregisterOnEndOfDay(_craftingView.ResetCraftingView, "Reset");
			_endOfDayListenerList.UnregisterOnEndOfDay(FinishCrafting, "PreEventSystems");
		}
	}

	public static bool IsCraftingOngoing()
	{
		return _instance._craftingData.RuntimeData.IsOngoing;
	}

	public static CraftingData GetCraftingSystem()
	{
		return _instance._craftingData;
	}

	public static EPlannedCraftingAction GetInterruptedOperation()
	{
		return _instance._interruptedCraftingOperation;
	}

	public static bool InterruptCraftingIfOngoing(IItem item)
	{
		if (_instance == null || !_instance._craftingData.RuntimeData.IsOngoing || _instance._craftingData.RuntimeData.OngoingCraftingActionData.Item != item)
		{
			return false;
		}
		_instance._interruptedCraftingOperation = _instance._craftingData.RuntimeData.OngoingCraftingActionData.PlannedCraftingAction;
		_instance._scheduler.ScheduleEvent(item.BaseStaticData.InterruptCraftingEvent, -1, -1);
		item.BaseStaticData.InterruptCraftingEvent.SetItemParameterByName("CraftItem", item);
		_instance._craftingData.RuntimeData.IsOngoing = false;
		_instance._craftingData.RuntimeData.CraftingCurrentTime = 0;
		_instance._craftingData.RuntimeData.CraftingTotalTime = 0;
		if (_instance._craftingData.RuntimeData.OngoingCraftingActionData.Item != null)
		{
			_instance._craftingData.RuntimeData.OngoingCraftingActionData.Item.Unlock();
		}
		_instance._craftingData.RuntimeData.OngoingCraftingActionData = new OngoingCraftingActionData();
		return true;
	}

	public void SetCraftingActionEvent()
	{
		if (!_craftingData.RuntimeData.IsPlanned)
		{
			return;
		}
		CraftingOperation craftingOperationForPlannedCraftingAction = GetCraftingOperationForPlannedCraftingAction();
		_craftingData.RuntimeData.CraftingTotalTime = UnityEngine.Random.Range(craftingOperationForPlannedCraftingAction.MinDays, craftingOperationForPlannedCraftingAction.MaxDays);
		if (_craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.RECYCLE)
		{
			for (int i = 0; i < _availableResources.RuntimeResources.Count; i++)
			{
				for (int j = 0; j < craftingOperationForPlannedCraftingAction.Resources.Count; j++)
				{
					_availableResources.Unlock(craftingOperationForPlannedCraftingAction.Resources[j]);
					if (_availableResources.RuntimeResources[i].Resource.Equals(craftingOperationForPlannedCraftingAction.Resources[j].Resource))
					{
						_availableResources.RuntimeResources[i] -= craftingOperationForPlannedCraftingAction.Resources[j];
					}
				}
			}
		}
		_craftingData.RuntimeData.OngoingCraftingActionData = _craftingData.RuntimeData.PlannedCraftingActionData.Clone();
		_craftingData.RuntimeData.PlannedCraftingActionData = new OngoingCraftingActionData();
		_craftingData.RuntimeData.IsOngoing = true;
		_craftingData.RuntimeData.IsPlanned = false;
	}

	public void FinishCrafting()
	{
		if (!_craftingData.RuntimeData.IsOngoing)
		{
			return;
		}
		if (_craftingData.RuntimeData.CraftingCurrentTime >= _craftingData.RuntimeData.CraftingTotalTime - 1)
		{
			SystemEvent craftingActionEventForPlannedCraftingAction = GetCraftingActionEventForPlannedCraftingAction();
			if (_craftingData.RuntimeData.OngoingCraftingActionData.Item != null)
			{
				craftingActionEventForPlannedCraftingAction.SetItemParameterByName("CraftItem", _craftingData.RuntimeData.OngoingCraftingActionData.Item);
			}
			_scheduler.ScheduleEvent(craftingActionEventForPlannedCraftingAction, 0, 0);
			_craftingData.RuntimeData.IsOngoing = false;
			_craftingData.RuntimeData.CraftingCurrentTime = 0;
			_craftingData.RuntimeData.CraftingTotalTime = 0;
			if (_craftingData.RuntimeData.OngoingCraftingActionData.Item != null && (!(_craftingData.RuntimeData.OngoingCraftingActionData.Item is ConsumableRemedium) || _craftingData.RuntimeData.OngoingCraftingActionData.PlannedCraftingAction == EPlannedCraftingAction.RECYCLE))
			{
				_craftingData.RuntimeData.OngoingCraftingActionData.Item.Unlock();
			}
			if (_craftingData.RuntimeData.OngoingCraftingActionData.Item != null)
			{
				CraftingStatsEntry craftingStatsEntry = new CraftingStatsEntry();
				craftingStatsEntry.Day = _survivalData.CurrentDay;
				craftingStatsEntry.Action = _craftingData.RuntimeData.OngoingCraftingActionData.PlannedCraftingAction;
				craftingStatsEntry.CraftedItemId = _craftingData.RuntimeData.OngoingCraftingActionData.Item.BaseStaticData.ItemId;
				craftingStatsEntry.CraftedItemLevel = _craftingData.RuntimeData.OngoingCraftingActionData.Item.BaseRuntimeData.Level;
				StatsManager.Instance.AddCraftingStatsEntry(craftingStatsEntry);
			}
			else if (_craftingData.RuntimeData.OngoingCraftingActionData.System != null)
			{
				ShipSystemUpgradeStatsEntry shipSystemUpgradeStatsEntry = new ShipSystemUpgradeStatsEntry();
				shipSystemUpgradeStatsEntry.Action = _craftingData.RuntimeData.OngoingCraftingActionData.PlannedCraftingAction;
				shipSystemUpgradeStatsEntry.Day = _survivalData.CurrentDay;
				shipSystemUpgradeStatsEntry.ShipSystemId = _craftingData.RuntimeData.OngoingCraftingActionData.System.Id;
				shipSystemUpgradeStatsEntry.SystemLevel = _craftingData.RuntimeData.OngoingCraftingActionData.System.SystemLevel + 1;
				StatsManager.Instance.AddSystemUpgradeStatsEntry(shipSystemUpgradeStatsEntry);
			}
			_craftingData.RuntimeData.OngoingCraftingActionData = new OngoingCraftingActionData();
			_craftingDisplayPaper.SetActive(value: false);
		}
		else
		{
			_craftingData.RuntimeData.CraftingCurrentTime++;
		}
	}

	private CraftingOperation GetCraftingOperationForPlannedCraftingAction(EPlannedCraftingAction operationType = EPlannedCraftingAction.NONE)
	{
		if (operationType == EPlannedCraftingAction.NONE)
		{
			operationType = _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction;
		}
		switch (operationType)
		{
		case EPlannedCraftingAction.CRAFT:
			if (_craftingData.RuntimeData.PlannedCraftingActionData.Item != null)
			{
				return _craftingData.RuntimeData.PlannedCraftingActionData.Item.GetCreateCost();
			}
			break;
		case EPlannedCraftingAction.RECYCLE:
			if (_craftingData.RuntimeData.PlannedCraftingActionData.Item != null)
			{
				return _craftingData.RuntimeData.PlannedCraftingActionData.Item.GetRecyclingCostForCurrentLevel();
			}
			break;
		case EPlannedCraftingAction.REPAIR:
		{
			Item item = _craftingData.RuntimeData.PlannedCraftingActionData.Item as Item;
			if (item != null)
			{
				return item.GetRepairCostsForCurrentLevel();
			}
			break;
		}
		case EPlannedCraftingAction.UPGRADE:
			if (_craftingData.RuntimeData.PlannedCraftingActionData.Item != null)
			{
				return _craftingData.RuntimeData.PlannedCraftingActionData.Item.GetUpgradeCostForCurrentLevel();
			}
			if (_craftingData.RuntimeData.PlannedCraftingActionData.System != null)
			{
				return _craftingData.RuntimeData.PlannedCraftingActionData.System.GetUpgradeCostForCurrentLevel();
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return null;
	}

	private SystemEvent GetCraftingActionEventForPlannedCraftingAction()
	{
		switch (_craftingData.RuntimeData.OngoingCraftingActionData.PlannedCraftingAction)
		{
		case EPlannedCraftingAction.CRAFT:
			if (_craftingData.RuntimeData.OngoingCraftingActionData.Item != null)
			{
				return _craftingData.RuntimeData.OngoingCraftingActionData.Item.BaseStaticData.CreateEvent;
			}
			break;
		case EPlannedCraftingAction.RECYCLE:
			if (_craftingData.RuntimeData.OngoingCraftingActionData.Item != null)
			{
				return _craftingData.RuntimeData.OngoingCraftingActionData.Item.BaseStaticData.RecyclingEvent;
			}
			break;
		case EPlannedCraftingAction.REPAIR:
		{
			Item item = _craftingData.RuntimeData.OngoingCraftingActionData.Item as Item;
			if (item != null)
			{
				return item.StaticData.RepairEvent;
			}
			break;
		}
		case EPlannedCraftingAction.UPGRADE:
			if (_craftingData.RuntimeData.OngoingCraftingActionData.Item != null)
			{
				return _craftingData.RuntimeData.OngoingCraftingActionData.Item.BaseStaticData.UpgradeEvent;
			}
			if (_craftingData.RuntimeData.OngoingCraftingActionData.System != null)
			{
				return _craftingData.RuntimeData.OngoingCraftingActionData.System.UpgradeEvent;
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return null;
	}
}
