using System;
using System.Collections.Generic;
using I2.Loc;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class RuntimeCharacterData
{
	private const float THRESHOLD_SAFETY_MARGIN = 0.001f;

	[SerializeField]
	private string _characterId;

	[SerializeField]
	private bool _isCaptain;

	[SerializeField]
	private List<CharacterStatus> _currentStatuses = new List<CharacterStatus>();

	[SerializeField]
	private List<CharacterStatus> _previousStatuses = new List<CharacterStatus>();

	[SerializeField]
	private List<CharacterAttributeValue> _currentAttributes = new List<CharacterAttributeValue>();

	[SerializeField]
	private List<CharacterNeedParameters> _currentNeeds = new List<CharacterNeedParameters>();

	[SerializeField]
	private bool _locked;

	[SerializeField]
	private bool _showsEndOfGameSlide = true;

	[SerializeField]
	private List<LocalizedString> _characterHistory = new List<LocalizedString>();

	private List<CharacterStatus> statusesGained = new List<CharacterStatus>();

	private List<CharacterStatus> statusesLost = new List<CharacterStatus>();

	private List<CharacterNeed> needsIgnoring = new List<CharacterNeed>();

	private List<IItem> dissalowedRemedies = new List<IItem>();

	private List<IItem> remediesRequiringStatus = new List<IItem>();

	private List<CharacterNeed> needsIgnoringDailyChanges = new List<CharacterNeed>();

	public string CharacterId
	{
		get
		{
			return _characterId;
		}
		set
		{
			_characterId = value;
		}
	}

	public bool IsCaptain
	{
		get
		{
			return _isCaptain;
		}
		set
		{
			_isCaptain = value;
			if (!_isCaptain)
			{
				return;
			}
			for (int num = _currentNeeds.Count - 1; num >= 0; num--)
			{
				if (!_currentNeeds[num].Need.CanBeHadByCaptain)
				{
					_currentNeeds.RemoveAt(num);
				}
			}
		}
	}

	public bool ShowsEndOfGameSlide
	{
		get
		{
			return _showsEndOfGameSlide;
		}
		set
		{
			_showsEndOfGameSlide = value;
		}
	}

	public List<CharacterStatus> CurrentStatuses
	{
		get
		{
			return _currentStatuses;
		}
		set
		{
			_currentStatuses = value;
		}
	}

	public List<CharacterAttributeValue> CurrentAttributes
	{
		get
		{
			return _currentAttributes;
		}
		set
		{
			_currentAttributes = value;
		}
	}

	public List<CharacterNeedParameters> CurrentNeeds
	{
		get
		{
			return _currentNeeds;
		}
		set
		{
			_currentNeeds = value;
		}
	}

	public bool Locked
	{
		get
		{
			return _locked;
		}
		set
		{
			_locked = value;
		}
	}

	public List<CharacterStatus> PreviousStatuses
	{
		get
		{
			return _previousStatuses;
		}
		set
		{
			_previousStatuses = value;
		}
	}

	public List<LocalizedString> CharacterHistory
	{
		get
		{
			return _characterHistory;
		}
		set
		{
			_characterHistory = value;
		}
	}

	public void AddTextUpdate(LocalizedString update)
	{
		_characterHistory.Add(update);
	}

	public void ChangeNeedValue(string needId, float valueChange)
	{
		CharacterNeedParameters needById = GetNeedById(needId);
		if (needById == null || HasStatusIgnoringAllNeedChanges() || GetNeedsIgnoringAllChanges().Contains(needById.Need))
		{
			return;
		}
		CharacterNeedParameters.ECharacterNeedStatus needStatus = needById.GetNeedStatus();
		needById.Value += valueChange;
		needById.Value = Mathf.Clamp(needById.Value, 0f, 1f);
		NeedStatsEntry needStatsEntry = new NeedStatsEntry();
		needStatsEntry.CharacterId = CharacterId;
		needStatsEntry.NeedId = needId;
		needStatsEntry.Value = needById.Value;
		StatsManager.Instance.AddNeedStatsEntry(needStatsEntry);
		if (needStatus == needById.GetNeedStatus())
		{
			return;
		}
		if (needById.AllowStatusRemoval(needStatus))
		{
			if (needById.CachedStatus != null)
			{
				RemoveStatus(needById.CachedStatus);
				needById.CachedStatus = null;
			}
			RemoveStatus(needById.GetCharacterStatusForNeedStatus(needStatus));
		}
		else
		{
			needById.CachedStatus = needById.GetCharacterStatusForNeedStatus(needStatus);
		}
		AddStatus(needById.GetCharacterStatusForNeedStatus(needById.GetNeedStatus()));
	}

	public void ChangeNeedValueWithLimit(string needId, float valueChange)
	{
		CharacterNeedParameters needById = GetNeedById(needId);
		if (needById == null || HasStatusIgnoringAllNeedChanges() || GetNeedsIgnoringAllChanges().Contains(needById.Need))
		{
			return;
		}
		if (CanChangesToNeedPreventCharacterFromReturningFromExpedition(needById))
		{
			ChangeNeedValue(needId, valueChange);
			return;
		}
		float num = needById.Value + valueChange;
		List<CharacterNeedParameters.ECharacterNeedStatus> characterNeedStatusesPreventingReturningFromExpedition = needById.GetCharacterNeedStatusesPreventingReturningFromExpedition();
		if (valueChange > 0f)
		{
			if (characterNeedStatusesPreventingReturningFromExpedition.Contains(CharacterNeedParameters.ECharacterNeedStatus.MAX) && num >= 1f)
			{
				num = 0.999f;
			}
			if (characterNeedStatusesPreventingReturningFromExpedition.Contains(CharacterNeedParameters.ECharacterNeedStatus.HIGH) && num >= needById.HighValueThreshold)
			{
				num = needById.HighValueThreshold - 0.001f;
			}
			if (characterNeedStatusesPreventingReturningFromExpedition.Contains(CharacterNeedParameters.ECharacterNeedStatus.LOW) && num >= needById.RiseToLowValueThreshold)
			{
				num = needById.RiseToLowValueThreshold - 0.001f;
			}
		}
		else
		{
			if (characterNeedStatusesPreventingReturningFromExpedition.Contains(CharacterNeedParameters.ECharacterNeedStatus.HIGH) && num <= needById.FallToHighValueThreshold)
			{
				num = needById.FallToHighValueThreshold + 0.001f;
			}
			if (characterNeedStatusesPreventingReturningFromExpedition.Contains(CharacterNeedParameters.ECharacterNeedStatus.LOW) && num <= needById.LowValueThreshold)
			{
				num = needById.LowValueThreshold + 0.001f;
			}
			if (characterNeedStatusesPreventingReturningFromExpedition.Contains(CharacterNeedParameters.ECharacterNeedStatus.MIN) && num <= 0f)
			{
				num = 0.001f;
			}
		}
		SetNeedValue(needId, num);
	}

	private bool CanChangesToNeedPreventCharacterFromReturningFromExpedition(CharacterNeedParameters need)
	{
		return need.GetCharacterNeedStatusesPreventingReturningFromExpedition().Count == 0;
	}

	public void ChangeAttributeValue(string attributeId, float valueChange)
	{
		CharacterAttributeValue attributeById = GetAttributeById(attributeId);
		if (attributeById != null)
		{
			attributeById.Value += valueChange;
			attributeById.Value = Mathf.Clamp(attributeById.Value, 0f, 1f);
		}
	}

	public void SetNeedValue(string needId, float value)
	{
		CharacterNeedParameters needById = GetNeedById(needId);
		if (needById != null)
		{
			CharacterNeedParameters.ECharacterNeedStatus needStatus = needById.GetNeedStatus();
			needById.Value = Mathf.Clamp(value, 0f, 1f);
			NeedStatsEntry needStatsEntry = new NeedStatsEntry();
			needStatsEntry.CharacterId = CharacterId;
			needStatsEntry.NeedId = needId;
			needStatsEntry.Value = needById.Value;
			StatsManager.Instance.AddNeedStatsEntry(needStatsEntry);
			if (needStatus != needById.GetNeedStatus())
			{
				RemoveStatus(needById.GetCharacterStatusForNeedStatus(needStatus));
				AddStatus(needById.GetCharacterStatusForNeedStatus(needById.GetNeedStatus()));
			}
		}
	}

	public void SetAttributeValue(string attributeId, float value)
	{
		CharacterAttributeValue attributeById = GetAttributeById(attributeId);
		if (attributeById != null)
		{
			attributeById.Value = value;
			attributeById.Value = Mathf.Clamp(attributeById.Value, 0f, 1f);
		}
	}

	public void UpdateNeedsWithDailyChanges()
	{
		if (IgnoresDailyNeedChanges())
		{
			return;
		}
		List<CharacterNeed> list = GetNeedsIgnoringDailyChanges();
		for (int i = 0; i < CurrentNeeds.Count; i++)
		{
			if ((list == null || !list.Contains(CurrentNeeds[i].Need)) && Mathf.Abs(CurrentNeeds[i].DailyValueChange) > 0.001f)
			{
				ChangeNeedValue(CurrentNeeds[i].Need.Id, CurrentNeeds[i].DailyValueChange);
			}
		}
	}

	public void AddStatus(CharacterStatus status)
	{
		if (status != null && !_currentStatuses.Contains(status))
		{
			_currentStatuses.Add(status);
		}
	}

	public void RemoveStatus(CharacterStatus status)
	{
		if (status != null && _currentStatuses.Contains(status))
		{
			_currentStatuses.Remove(status);
		}
	}

	public bool IsOnExpedition()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (_currentStatuses[i].Is("status_onexpedition"))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsAlive()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (_currentStatuses[i].IsDead)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsDrawnOnShip()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (!_currentStatuses[i].AllowsDrawingInShip)
			{
				return false;
			}
		}
		return true;
	}

	public bool IgnoresDailyNeedChanges()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (_currentStatuses[i].IgnoresDailyNeedChanges)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasEndGameStatus()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (_currentStatuses[i].EndsGameIfCaptainHasIt)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasStatusPreventingGoingOnExpeditions()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (!_currentStatuses[i].AllowsGoingOnExpedition)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasStatusPreventingSelectionInEventChoices()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (!_currentStatuses[i].AllowsSelectionInEventChoice)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasStatusPreventingRationing()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (!_currentStatuses[i].AllowsRationing)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasStatusIgnoringAllNeedChanges()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (_currentStatuses[i].IgnoresAllNeedChanges)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasStatusPreventingReturnFromExpedition()
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (_currentStatuses[i].PreventsReturnFromExpedition)
			{
				return true;
			}
		}
		return false;
	}

	public List<CharacterStatus> GetStatusesGained()
	{
		statusesGained.Clear();
		for (int i = 0; i < CurrentStatuses.Count; i++)
		{
			if (!PreviousStatuses.Contains(CurrentStatuses[i]))
			{
				statusesGained.Add(CurrentStatuses[i]);
			}
		}
		return statusesGained;
	}

	public List<CharacterStatus> GetStatusesLost()
	{
		statusesLost.Clear();
		for (int i = 0; i < PreviousStatuses.Count; i++)
		{
			if (!CurrentStatuses.Contains(PreviousStatuses[i]))
			{
				statusesLost.Add(PreviousStatuses[i]);
			}
		}
		return statusesLost;
	}

	public List<CharacterNeed> GetNeedsIgnoringAllChanges()
	{
		needsIgnoring.Clear();
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (_currentStatuses[i].NeedsIgnoringAllChanges == null)
			{
				continue;
			}
			for (int j = 0; j < _currentStatuses[i].NeedsIgnoringAllChanges.Length; j++)
			{
				if (!needsIgnoring.Contains(_currentStatuses[i].NeedsIgnoringAllChanges[j]))
				{
					needsIgnoring.Add(_currentStatuses[i].NeedsIgnoringAllChanges[j]);
				}
			}
		}
		return needsIgnoring;
	}

	public CharacterNeedParameters GetNeedById(string id)
	{
		for (int i = 0; i < _currentNeeds.Count; i++)
		{
			if (_currentNeeds[i].Need.Is(id))
			{
				return _currentNeeds[i];
			}
		}
		return null;
	}

	public CharacterAttributeValue GetAttributeByObject(CharacterAttribute obj)
	{
		for (int i = 0; i < _currentAttributes.Count; i++)
		{
			if (_currentAttributes[i].Attribute == obj)
			{
				return _currentAttributes[i];
			}
		}
		return null;
	}

	public CharacterAttributeValue GetAttributeById(string id)
	{
		for (int i = 0; i < _currentAttributes.Count; i++)
		{
			if (_currentAttributes[i].Attribute.Is(id))
			{
				return _currentAttributes[i];
			}
		}
		return null;
	}

	public bool HasStatus(string id)
	{
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			if (_currentStatuses[i].Is(id))
			{
				return true;
			}
		}
		return false;
	}

	public List<IItem> GetDisallowedRemedies()
	{
		dissalowedRemedies.Clear();
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			for (int j = 0; j < _currentStatuses[i].DisallowedRemedies.Length; j++)
			{
				if (!dissalowedRemedies.Contains(_currentStatuses[i].DisallowedRemedies[j]))
				{
					dissalowedRemedies.Add(_currentStatuses[i].DisallowedRemedies[j]);
				}
			}
		}
		return dissalowedRemedies;
	}

	public List<IItem> GetStatusesRequiredByRemedy()
	{
		remediesRequiringStatus.Clear();
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			for (int j = 0; j < _currentStatuses[i].RemediesRequiringStatus.Length; j++)
			{
				if (!remediesRequiringStatus.Contains(_currentStatuses[i].RemediesRequiringStatus[j]))
				{
					remediesRequiringStatus.Add(_currentStatuses[i].RemediesRequiringStatus[j]);
				}
			}
		}
		return remediesRequiringStatus;
	}

	public string GetCharacterRuntimeState()
	{
		return string.Empty;
	}

	private List<CharacterNeed> GetNeedsIgnoringDailyChanges()
	{
		needsIgnoringDailyChanges.Clear();
		for (int i = 0; i < _currentStatuses.Count; i++)
		{
			CharacterStatus characterStatus = _currentStatuses[i];
			if (characterStatus.NeedsIgnoringDailyChanges == null || characterStatus.NeedsIgnoringDailyChanges.Length == 0)
			{
				continue;
			}
			for (int j = 0; j < characterStatus.NeedsIgnoringDailyChanges.Length; j++)
			{
				if (!needsIgnoringDailyChanges.Contains(characterStatus.NeedsIgnoringDailyChanges[j]))
				{
					needsIgnoringDailyChanges.Add(characterStatus.NeedsIgnoringDailyChanges[j]);
				}
			}
		}
		return needsIgnoringDailyChanges;
	}
}
