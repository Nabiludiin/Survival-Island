using System.Collections.Generic;
using RG.Core.Base;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class RationingManager : RGMonoBehaviour
{
	private static RationingManager _instance;

	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private RationingData _rationingData;

	[SerializeField]
	private RationingListenerList _rationingListenerList;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	private const int PERSIST_EVENTS_LISTENER_LIST_PRIORITY = 15;

	private const int CHARACTER_NEEDS_LISTENER_LIST_PRIORITY = 5;

	private const int CHARACTER_STATUS_UPDATE_LISTENER_LIST_PRIORITY = 10;

	private const int RESET_LISTENER_LIST_PRIORITY = 5;

	public static RationingManager Instance
	{
		get
		{
			if (_instance == null)
			{
				FindInstanceInScene();
			}
			return _instance;
		}
	}

	public RationingData RationingData => _rationingData;

	private static void FindInstanceInScene()
	{
		_instance = Object.FindObjectOfType<RationingManager>();
		if (_instance == null)
		{
			Debug.LogError("No RationingManager in scene!");
		}
	}

	protected override void CustomAwake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	protected override void CustomStart()
	{
		_endOfDayListenerList.RegisterOnEndOfDay(OnEndOfDayReset, "Reset", 5, this);
		_endOfDayListenerList.RegisterOnEndOfDay(OnEndOfDayEvents, "Update", 15, this);
		_endOfDayListenerList.RegisterOnEndOfDay(OnEndOfDaySystems, "Update", 5, this);
		_endOfDayListenerList.RegisterOnEndOfDay(CheckCharactersStatuses, "SystemEvents", 10, this);
		_rationingData.Reset();
	}

	private void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(OnEndOfDayReset, "Reset");
		_endOfDayListenerList.UnregisterOnEndOfDay(OnEndOfDayEvents, "Update");
		_endOfDayListenerList.UnregisterOnEndOfDay(OnEndOfDaySystems, "Update");
		_endOfDayListenerList.UnregisterOnEndOfDay(CheckCharactersStatuses, "SystemEvents");
	}

	public void OnEndOfDayReset()
	{
		_rationingData.ResetRationingData();
	}

	public void OnEndOfDayEvents()
	{
		RunOnPersistStatusEvents();
	}

	public void OnEndOfDaySystems()
	{
		ConsumeRations();
		UpdateCharacterNeedsWithDailyChanges();
	}

	public void CheckCharactersStatuses()
	{
		for (int i = 0; i < _characterList.GetCharacterCount(); i++)
		{
			Character character = _characterList.CharactersInGame[i];
			CheckStatus(character);
		}
	}

	public void CheckStatus(Character character)
	{
		List<CharacterStatus> statusesLost = character.RuntimeData.GetStatusesLost();
		for (int i = 0; i < statusesLost.Count; i++)
		{
			CharacterStatusChangeStatsEntry characterStatusChangeStatsEntry = new CharacterStatusChangeStatsEntry();
			characterStatusChangeStatsEntry.IsCaptain = character.RuntimeData.IsCaptain;
			characterStatusChangeStatsEntry.ChangedStatusId = statusesLost[i].Id;
			characterStatusChangeStatsEntry.CharacterId = character.ID;
			characterStatusChangeStatsEntry.StatusGained = false;
			StatsManager.Instance.AddCharacterStatusChangeStatsEntry(characterStatusChangeStatsEntry);
			CharacterStatusBehaviour behaviourForStatus = character.StaticData.GetBehaviourForStatus(statusesLost[i]);
			if (behaviourForStatus != null && behaviourForStatus.OnStatusExitEvent != null)
			{
				behaviourForStatus.OnStatusExitEvent.SetCharacterParameterByName("StatusCharacter", character);
				behaviourForStatus.OnStatusExitEvent.Run();
			}
		}
		List<CharacterStatus> statusesGained = character.RuntimeData.GetStatusesGained();
		for (int j = 0; j < statusesGained.Count; j++)
		{
			if (statusesGained[j].Id == "status_soulmate")
			{
				StatsManager.Instance.AddGlobalData("SoulmatesFounded", 1);
			}
			CharacterStatusChangeStatsEntry characterStatusChangeStatsEntry2 = new CharacterStatusChangeStatsEntry();
			characterStatusChangeStatsEntry2.IsCaptain = character.RuntimeData.IsCaptain;
			characterStatusChangeStatsEntry2.ChangedStatusId = statusesGained[j].Id;
			characterStatusChangeStatsEntry2.CharacterId = character.ID;
			characterStatusChangeStatsEntry2.StatusGained = true;
			StatsManager.Instance.AddCharacterStatusChangeStatsEntry(characterStatusChangeStatsEntry2);
			CharacterStatusBehaviour behaviourForStatus2 = character.StaticData.GetBehaviourForStatus(statusesGained[j]);
			if (behaviourForStatus2 != null && behaviourForStatus2.OnStatusEnterEvent != null)
			{
				behaviourForStatus2.OnStatusEnterEvent.SetCharacterParameterByName("StatusCharacter", character);
				behaviourForStatus2.OnStatusEnterEvent.Run();
			}
		}
		character.MarkCurrentStatusesAsPrevious();
	}

	public void RunOnPersistStatusEvents()
	{
		for (int i = 0; i < _characterList.GetCharacterCount(); i++)
		{
			Character character = _characterList.CharactersInGame[i];
			for (int j = 0; j < character.StaticData.StatusBehaviours.Length; j++)
			{
				CharacterStatusBehaviour characterStatusBehaviour = character.StaticData.StatusBehaviours[j];
				if (character.RuntimeData.PreviousStatuses.Contains(characterStatusBehaviour.Status) && character.RuntimeData.CurrentStatuses.Contains(characterStatusBehaviour.Status) && character.RuntimeData.HasStatus(characterStatusBehaviour.Status.Id) && characterStatusBehaviour.OnStatusPersistEvent != null)
				{
					characterStatusBehaviour.OnStatusPersistEvent.SetCharacterParameterByName("StatusCharacter", character);
					characterStatusBehaviour.OnStatusPersistEvent.Run();
				}
			}
		}
	}

	public void UpdateCharacterNeedsWithDailyChanges()
	{
		for (int i = 0; i < _characterList.GetCharacterCount(); i++)
		{
			_characterList.CharactersInGame[i].RuntimeData.UpdateNeedsWithDailyChanges();
		}
	}

	protected virtual void ConsumeRation(RationingData rationingData)
	{
	}

	private void ConsumeRations()
	{
		for (int i = 0; i < _rationingData.Rations.Count; i++)
		{
			Ration ration = _rationingData.Rations[i];
			Character character = _characterList.CharactersInGame[ration.CharacterIndex];
			RationingStatsEntry rationingStatsEntry = new RationingStatsEntry();
			rationingStatsEntry.ItemId = ration.RationedItem.BaseStaticData.ItemId;
			rationingStatsEntry.CharacterId = character.ID;
			StatsManager.Instance.AddRationingStatsEntry(rationingStatsEntry);
			if (ration.RationedItem is ConsumableRemedium)
			{
				ConsumableRemedium consumableRemedium = (ConsumableRemedium)ration.RationedItem;
				List<Influence> influences = consumableRemedium.GetInfluencesForCurrentLevel().Influences;
				StatsManager.Instance.AddGlobalData("EatenSoupRations", 1);
				for (int j = 0; j < influences.Count; j++)
				{
					character.RuntimeData.ChangeNeedValue(influences[j].Need.Id, influences[j].Value);
				}
				for (int k = 0; k < consumableRemedium.StaticData.AddStatuses.Count; k++)
				{
					character.RuntimeData.AddStatus(consumableRemedium.StaticData.AddStatuses[k]);
				}
				for (int l = 0; l < consumableRemedium.StaticData.RemoveStatuses.Count; l++)
				{
					character.RuntimeData.RemoveStatus(consumableRemedium.StaticData.RemoveStatuses[l]);
				}
				consumableRemedium.SetAmountBasedOnPlannedConsumption();
			}
			else if (ration.RationedItem is Remedium)
			{
				Remedium remedium = (Remedium)ration.RationedItem;
				List<Influence> influences2 = remedium.GetInfluencesForCurrentLevel().Influences;
				if (remedium.ID == "item_medkit")
				{
					StatsManager.Instance.AddGlobalData("UsedMedkit", 1);
				}
				else if (remedium.ID == "item_sock")
				{
					StatsManager.Instance.AddGlobalData("UsedSockPuppets", 1);
				}
				for (int m = 0; m < influences2.Count; m++)
				{
					character.RuntimeData.ChangeNeedValue(influences2[m].Need.Id, influences2[m].Value);
				}
				for (int n = 0; n < remedium.StaticData.AddStatuses.Count; n++)
				{
					character.RuntimeData.AddStatus(remedium.StaticData.AddStatuses[n]);
				}
				for (int num = 0; num < remedium.StaticData.RemoveStatuses.Count; num++)
				{
					character.RuntimeData.RemoveStatus(remedium.StaticData.RemoveStatuses[num]);
				}
				remedium.Use();
				remedium.Unlock();
			}
		}
		ConsumeRation(_rationingData);
		_rationingData.Rations.Clear();
	}

	public bool AreAllEligibleCharactersRationed(IItem item)
	{
		for (int i = 0; i < _characterList.GetCharacterCount(); i++)
		{
			if (!_rationingData.IsItemRationedForCharacter(i, item.BaseStaticData.ItemId) && CanItemBeRationedToCharacter(item, i))
			{
				return false;
			}
		}
		return true;
	}

	public int GetEligibleCharacterCountForRation(IItem item)
	{
		int num = 0;
		for (int i = 0; i < _characterList.GetCharacterCount(); i++)
		{
			if (CanItemBeRationedToCharacter(item, i))
			{
				num++;
			}
		}
		return num;
	}

	public int GetEligibleCharacterRationedCount(IItem item)
	{
		int num = 0;
		for (int i = 0; i < _characterList.GetCharacterCount(); i++)
		{
			if (CanItemBeRationedToCharacter(item, i) && _rationingData.IsItemRationedForCharacter(i, item.BaseStaticData.ItemId))
			{
				num++;
			}
		}
		return num;
	}

	public void RationAllCharacters(IItem item)
	{
		for (int i = 0; i < _characterList.GetCharacterCount(); i++)
		{
			if (CanItemBeRationedToCharacter(item, i) && !_rationingData.IsItemRationedForCharacter(i, item.BaseStaticData.ItemId) && IsItemAvailableForRationing(item))
			{
				CharacterRationed(i, item);
			}
		}
		_rationingListenerList.RationingChanged();
	}

	public void UnrationAllCharacters(IItem item)
	{
		for (int i = 0; i < _characterList.GetCharacterCount(); i++)
		{
			if (_rationingData.IsItemRationedForCharacter(i, item.BaseStaticData.ItemId))
			{
				CharacterRationed(i, item);
			}
		}
		_rationingListenerList.RationingChanged();
	}

	public bool CanItemBeRationedToCharacter(IItem item, int characterIndex)
	{
		if (!_characterList.CharactersInGame[characterIndex].RuntimeData.HasStatusPreventingRationing())
		{
			return !_characterList.CharactersInGame[characterIndex].RuntimeData.GetDisallowedRemedies().Contains(item);
		}
		return false;
	}

	public bool IsStatusNeededToRationItemToCharacter(IItem item, int characterIndex)
	{
		return _characterList.CharactersInGame[characterIndex].RuntimeData.GetStatusesRequiredByRemedy().Contains(item);
	}

	public bool IsThereEnoughRations(IItem item)
	{
		if (item is ConsumableRemedium)
		{
			ConsumableRemedium consumableRemedium = (ConsumableRemedium)item;
			float num = consumableRemedium.RuntimeData.Amount - consumableRemedium.RuntimeData.PlannedConsumption;
			int num2 = GetEligibleCharacterCountForRation(item) - GetEligibleCharacterRationedCount(item);
			return num >= consumableRemedium.StaticData.ConsumeRatio * (float)num2;
		}
		return false;
	}

	public bool IsItemAvailableForRationing(IItem item)
	{
		if (item is ConsumableRemedium)
		{
			ConsumableRemedium consumableRemedium = (ConsumableRemedium)item;
			return consumableRemedium.RuntimeData.Amount - consumableRemedium.RuntimeData.PlannedConsumption >= consumableRemedium.StaticData.ConsumeRatio;
		}
		if (item is Remedium)
		{
			if (item.BaseRuntimeData.IsAvailable)
			{
				return item.IsLockable();
			}
			return false;
		}
		return false;
	}

	public void CharacterRationed(int characterIndex, IItem item)
	{
		if (_rationingData.IsItemRationedForCharacter(characterIndex, item.BaseStaticData.ItemId))
		{
			_rationingData.UnrationItemFromCharacter(characterIndex, item.BaseStaticData.ItemId);
			if (item is ConsumableRemedium)
			{
				ConsumableRemedium consumableRemedium = (ConsumableRemedium)item;
				consumableRemedium.RuntimeData.PlannedConsumption -= consumableRemedium.StaticData.ConsumeRatio;
			}
			else if (item is Remedium)
			{
				item.Unlock();
			}
		}
		else
		{
			_rationingData.RationItemForCharacter(characterIndex, item);
			if (item is ConsumableRemedium)
			{
				ConsumableRemedium consumableRemedium2 = (ConsumableRemedium)item;
				consumableRemedium2.RuntimeData.PlannedConsumption += consumableRemedium2.StaticData.ConsumeRatio;
			}
			else if (item is Remedium)
			{
				item.Lock();
			}
		}
		_rationingListenerList.RationingChanged();
	}

	public bool IsRemediumRationedToCharacter(Character character, Remedium remedium)
	{
		if (character != null && remedium != null)
		{
			return RationingData.IsItemRationedForCharacter(_characterList.CharactersInGame.IndexOf(character), remedium.BaseStaticData.ItemId);
		}
		return false;
	}
}
