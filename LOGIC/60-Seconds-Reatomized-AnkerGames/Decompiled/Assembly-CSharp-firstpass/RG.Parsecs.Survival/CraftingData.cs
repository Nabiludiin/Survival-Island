using I2.Loc;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Crafting/Crafting Data")]
public class CraftingData : ShipSystem, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	private const string SYSTEM_ID = "system_crafting";

	[SerializeField]
	private CraftingStaticData _staticData;

	[SerializeField]
	private CraftingRuntimeData _runtimeData;

	public string ID => Guid;

	public CraftingStaticData StaticData => _staticData;

	public CraftingRuntimeData RuntimeData => _runtimeData;

	public override string Id => ID;

	public override int SystemLevel
	{
		get
		{
			return _runtimeData.CraftingLevel;
		}
		set
		{
			_runtimeData.CraftingLevel = value;
		}
	}

	public override bool IsUpgradable => _runtimeData.CraftingLevel < _staticData.MaxLevel;

	public override Sprite Icon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_staticData.Icon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _staticData.Icon;
		}
	}

	public override SystemEvent UpgradeEvent => _staticData.UpgradeEvent;

	public override bool IsAvailable
	{
		get
		{
			return _runtimeData.IsActive;
		}
		set
		{
			_runtimeData.IsActive = value;
		}
	}

	public override bool IsFocusedByDesign => _runtimeData.IsFocusedByDesign;

	public override bool AvailableInCraftingDuringExpedition => _staticData.AvailableInCraftingDuringExpedition;

	public override LocalizedString Name => _staticData.Name;

	public string Serialize()
	{
		CraftingDataWrapper craftingDataWrapper = new CraftingDataWrapper
		{
			IsActive = _runtimeData.IsActive,
			IsRepaired = _runtimeData.IsRepaired,
			IsPlanned = _runtimeData.IsPlanned,
			IsOngoing = _runtimeData.IsOngoing,
			ShouldReactive = _runtimeData.ShouldReactivate,
			IsFocusedByDesign = _runtimeData.IsFocusedByDesign,
			CraftingLevel = _runtimeData.CraftingLevel,
			CraftingTotalTime = _runtimeData.CraftingTotalTime,
			CraftingCurrentTime = _runtimeData.CraftingCurrentTime,
			BlockedPanelTitleTerm = _runtimeData.BlockedPanelTitleTerm,
			BlockedPanelDescriptionTerm = _runtimeData.BlockedPanelDescriptionTerm
		};
		CraftingOperationWrapper plannedAction = new CraftingOperationWrapper
		{
			CraftingAction = _runtimeData.PlannedCraftingActionData.PlannedCraftingAction,
			ItemID = ((_runtimeData.PlannedCraftingActionData.Item != null) ? _runtimeData.PlannedCraftingActionData.Item.BaseStaticData.ItemId : ""),
			SystemID = ((_runtimeData.PlannedCraftingActionData.System != null) ? _runtimeData.PlannedCraftingActionData.System.Id : "")
		};
		craftingDataWrapper.PlannedAction = plannedAction;
		CraftingOperationWrapper ongoingAction = new CraftingOperationWrapper
		{
			CraftingAction = _runtimeData.OngoingCraftingActionData.PlannedCraftingAction,
			ItemID = ((_runtimeData.OngoingCraftingActionData.Item != null) ? _runtimeData.OngoingCraftingActionData.Item.BaseStaticData.ItemId : ""),
			SystemID = ((_runtimeData.OngoingCraftingActionData.System != null) ? _runtimeData.OngoingCraftingActionData.System.Id : "")
		};
		craftingDataWrapper.OngoingAction = ongoingAction;
		return JsonUtility.ToJson(craftingDataWrapper);
	}

	public void Deserialize(string jsonData)
	{
		CraftingDataWrapper craftingDataWrapper = JsonUtility.FromJson<CraftingDataWrapper>(jsonData);
		_runtimeData.IsActive = craftingDataWrapper.IsActive;
		_runtimeData.IsRepaired = craftingDataWrapper.IsRepaired;
		_runtimeData.IsPlanned = craftingDataWrapper.IsPlanned;
		_runtimeData.IsOngoing = craftingDataWrapper.IsOngoing;
		_runtimeData.ShouldReactivate = craftingDataWrapper.ShouldReactive;
		_runtimeData.IsFocusedByDesign = craftingDataWrapper.IsFocusedByDesign;
		_runtimeData.CraftingLevel = craftingDataWrapper.CraftingLevel;
		_runtimeData.CraftingTotalTime = craftingDataWrapper.CraftingTotalTime;
		_runtimeData.CraftingCurrentTime = craftingDataWrapper.CraftingCurrentTime;
		_runtimeData.BlockedPanelTitleTerm = craftingDataWrapper.BlockedPanelTitleTerm;
		_runtimeData.BlockedPanelDescriptionTerm = craftingDataWrapper.BlockedPanelDescriptionTerm;
		if (craftingDataWrapper.IsPlanned)
		{
			_runtimeData.PlannedCraftingActionData.PlannedCraftingAction = craftingDataWrapper.PlannedAction.CraftingAction;
			if (!string.IsNullOrEmpty(craftingDataWrapper.PlannedAction.ItemID))
			{
				_runtimeData.PlannedCraftingActionData.Item = (IItem)_saveEvent.GetReferenceObjectByID(craftingDataWrapper.PlannedAction.ItemID);
			}
			else
			{
				_runtimeData.PlannedCraftingActionData.System = (ShipSystem)_saveEvent.GetReferenceObjectByID(craftingDataWrapper.PlannedAction.SystemID);
			}
		}
		if (craftingDataWrapper.IsOngoing)
		{
			_runtimeData.OngoingCraftingActionData.PlannedCraftingAction = craftingDataWrapper.OngoingAction.CraftingAction;
			if (!string.IsNullOrEmpty(craftingDataWrapper.OngoingAction.ItemID))
			{
				_runtimeData.OngoingCraftingActionData.Item = (IItem)_saveEvent.GetReferenceObjectByID(craftingDataWrapper.OngoingAction.ItemID);
			}
			else
			{
				_runtimeData.OngoingCraftingActionData.System = (ShipSystem)_saveEvent.GetReferenceObjectByID(craftingDataWrapper.OngoingAction.SystemID);
			}
		}
	}

	public void Register()
	{
		_saveEvent.RegisterListener(this);
	}

	public void Unregister()
	{
		_saveEvent.UnregisterListener(this);
	}

	public void ResetData()
	{
		_runtimeData = new CraftingRuntimeData
		{
			IsActive = _staticData.IsActive,
			IsRepaired = _staticData.IsRepaired
		};
	}

	private void OnEnable()
	{
		Register();
	}

	public override CraftingOperation GetUpgradeCostForCurrentLevel()
	{
		return _staticData.UpgradeCosts[_runtimeData.CraftingLevel - 1];
	}

	public override void Upgrade()
	{
		_runtimeData.CraftingLevel++;
	}

	public override void Downgrade()
	{
		_runtimeData.CraftingLevel--;
	}

	public override int MaxLevel()
	{
		return _staticData.MaxLevel;
	}

	public override void SetDisabledReason(LocalizedString title, LocalizedString reason)
	{
		if (string.IsNullOrEmpty(title.mTerm))
		{
			_runtimeData.BlockedPanelTitleTerm = _staticData.BlockedPanelDefaultTitleTerm;
		}
		else
		{
			_runtimeData.BlockedPanelTitleTerm = title;
		}
		if (string.IsNullOrEmpty(reason.mTerm))
		{
			_runtimeData.BlockedPanelDescriptionTerm = _staticData.BlockedPanelDefaultDescriptionTerm;
		}
		else
		{
			_runtimeData.BlockedPanelDescriptionTerm = reason;
		}
	}
}
