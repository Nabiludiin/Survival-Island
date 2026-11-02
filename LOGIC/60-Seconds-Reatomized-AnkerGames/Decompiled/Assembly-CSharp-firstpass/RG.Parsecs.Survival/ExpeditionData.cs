using System.Collections.Generic;
using I2.Loc;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Expedition/Expedition Data")]
public class ExpeditionData : ShipSystem, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	public const string SYSTEM_ID = "system_expedition";

	[SerializeField]
	private ExpeditionStaticData _staticData;

	[SerializeField]
	private ExpeditionRuntimeData _runtimeData;

	public string ID => Guid;

	public ExpeditionStaticData StaticData => _staticData;

	public ExpeditionRuntimeData RuntimeData => _runtimeData;

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

	public override int SystemLevel
	{
		get
		{
			return _runtimeData.ExpeditionSystemLevel;
		}
		set
		{
			_runtimeData.ExpeditionSystemLevel = value;
		}
	}

	public override bool IsUpgradable => _runtimeData.ExpeditionSystemLevel < _staticData.MaxLevel;

	public override SystemEvent UpgradeEvent => _staticData.UpgradeEvent;

	public override string Id => ID;

	public override bool IsFocusedByDesign => _runtimeData.IsFocusedByDesign;

	public override bool AvailableInCraftingDuringExpedition => _staticData.AvailableForCraftingDuringExpedition;

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

	public override LocalizedString Name => _staticData.Name;

	public string Serialize()
	{
		ExpeditionWrapper expeditionWrapper = new ExpeditionWrapper
		{
			IsActive = _runtimeData.IsActive,
			IsPlanned = _runtimeData.IsPlanned,
			IsOngoing = _runtimeData.IsOngoing,
			IsRepaired = _runtimeData.IsRepaired,
			IsFocusedByDesign = _runtimeData.IsFocusedByDesign,
			ElapsedExpeditionTime = _runtimeData.ElapsedExpeditionTime,
			Level = _runtimeData.ExpeditionSystemLevel,
			ShouldReactivate = _runtimeData.ShouldReactivate,
			BlockedPanelTitleTerm = _runtimeData.BlockedPanelTitleTerm,
			BlockedPanelDescriptionTerm = _runtimeData.BlockedPanelDescriptionTerm
		};
		expeditionWrapper.CurrentPlanetDeck = ((_runtimeData.CurrentPlanetDeck != null) ? _runtimeData.CurrentPlanetDeck.ID : string.Empty);
		ExpeditionDataWrapper plannedExpedition = new ExpeditionDataWrapper
		{
			Destination = ((_runtimeData.PlannedExpeditionData.ChosenDestination != null) ? _runtimeData.PlannedExpeditionData.ChosenDestination.ID : string.Empty),
			Character = ((_runtimeData.PlannedExpeditionData.ExpeditionCharacter != null) ? _runtimeData.PlannedExpeditionData.ExpeditionCharacter.ID : string.Empty),
			Items = new List<string>()
		};
		for (int i = 0; i < _runtimeData.PlannedExpeditionData.ExpeditionItems.Count; i++)
		{
			plannedExpedition.Items.Add(_runtimeData.PlannedExpeditionData.ExpeditionItems[i].BaseStaticData.ItemId);
		}
		expeditionWrapper.PlannedExpedition = plannedExpedition;
		ExpeditionDataWrapper ongoingExpedition = new ExpeditionDataWrapper
		{
			Destination = ((_runtimeData.OngoingExpeditionData.ChosenDestination != null) ? _runtimeData.OngoingExpeditionData.ChosenDestination.ID : string.Empty),
			Character = ((_runtimeData.OngoingExpeditionData.ExpeditionCharacter != null) ? _runtimeData.OngoingExpeditionData.ExpeditionCharacter.ID : string.Empty),
			Items = new List<string>()
		};
		for (int j = 0; j < _runtimeData.OngoingExpeditionData.ExpeditionItems.Count; j++)
		{
			ongoingExpedition.Items.Add(_runtimeData.OngoingExpeditionData.ExpeditionItems[j].BaseStaticData.ItemId);
		}
		expeditionWrapper.OngoingExpedition = ongoingExpedition;
		return JsonUtility.ToJson(expeditionWrapper);
	}

	public void Deserialize(string jsonData)
	{
		ExpeditionWrapper expeditionWrapper = JsonUtility.FromJson<ExpeditionWrapper>(jsonData);
		_runtimeData.IsActive = expeditionWrapper.IsActive;
		_runtimeData.IsPlanned = expeditionWrapper.IsPlanned;
		_runtimeData.IsOngoing = expeditionWrapper.IsOngoing;
		_runtimeData.IsRepaired = expeditionWrapper.IsRepaired;
		_runtimeData.IsFocusedByDesign = expeditionWrapper.IsFocusedByDesign;
		_runtimeData.ElapsedExpeditionTime = expeditionWrapper.ElapsedExpeditionTime;
		_runtimeData.ExpeditionSystemLevel = expeditionWrapper.Level;
		_runtimeData.ShouldReactivate = expeditionWrapper.ShouldReactivate;
		if (!string.IsNullOrEmpty(expeditionWrapper.CurrentPlanetDeck))
		{
			_runtimeData.CurrentPlanetDeck = (PlanetDeck)_saveEvent.GetReferenceObjectByID(expeditionWrapper.CurrentPlanetDeck);
		}
		_runtimeData.BlockedPanelTitleTerm = expeditionWrapper.BlockedPanelTitleTerm;
		_runtimeData.BlockedPanelDescriptionTerm = expeditionWrapper.BlockedPanelDescriptionTerm;
		ExpeditionDataWrapper plannedExpedition = expeditionWrapper.PlannedExpedition;
		if (!string.IsNullOrEmpty(plannedExpedition.Destination))
		{
			_runtimeData.PlannedExpeditionData.ChosenDestination = (ExpeditionDestination)_saveEvent.GetReferenceObjectByID(plannedExpedition.Destination);
		}
		if (!string.IsNullOrEmpty(plannedExpedition.Character))
		{
			_runtimeData.PlannedExpeditionData.ExpeditionCharacter = (Character)_saveEvent.GetReferenceObjectByID(plannedExpedition.Character);
		}
		_runtimeData.PlannedExpeditionData.ExpeditionItems.Clear();
		for (int i = 0; i < plannedExpedition.Items.Count; i++)
		{
			_runtimeData.PlannedExpeditionData.ExpeditionItems.Add((IItem)_saveEvent.GetReferenceObjectByID(plannedExpedition.Items[i]));
		}
		ExpeditionDataWrapper ongoingExpedition = expeditionWrapper.OngoingExpedition;
		if (!string.IsNullOrEmpty(ongoingExpedition.Destination))
		{
			_runtimeData.OngoingExpeditionData.ChosenDestination = (ExpeditionDestination)_saveEvent.GetReferenceObjectByID(ongoingExpedition.Destination);
		}
		if (!string.IsNullOrEmpty(ongoingExpedition.Character))
		{
			_runtimeData.OngoingExpeditionData.ExpeditionCharacter = (Character)_saveEvent.GetReferenceObjectByID(ongoingExpedition.Character);
		}
		_runtimeData.OngoingExpeditionData.ExpeditionItems.Clear();
		for (int j = 0; j < ongoingExpedition.Items.Count; j++)
		{
			_runtimeData.OngoingExpeditionData.ExpeditionItems.Add((IItem)_saveEvent.GetReferenceObjectByID(ongoingExpedition.Items[j]));
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
		_runtimeData = new ExpeditionRuntimeData();
	}

	private void OnEnable()
	{
		Register();
	}

	public void SetOngoingExpeditionRuntimeData()
	{
		_runtimeData.OngoingExpeditionData = _runtimeData.PlannedExpeditionData.Clone();
	}

	public override CraftingOperation GetUpgradeCostForCurrentLevel()
	{
		return _staticData.UpgradeCosts[_runtimeData.ExpeditionSystemLevel - 1];
	}

	public override void Upgrade()
	{
		_runtimeData.ExpeditionSystemLevel++;
	}

	public override void Downgrade()
	{
		_runtimeData.ExpeditionSystemLevel--;
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
