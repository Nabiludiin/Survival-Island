using System;
using System.Collections.Generic;
using System.Text;
using I2.Loc;
using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(fileName = "New Character", menuName = "60 Parsecs!/Characters/New Character")]
public class Character : RGScriptableObject, ILocalizationParamsManager, ISaveable
{
	[Tooltip("Event to which data will be save and from which be loaded")]
	[SerializeField]
	private SaveEvent _saveEvent;

	[Tooltip("Contains the data defining the character.\nThis should be set up by the designers.")]
	[SerializeField]
	private StaticCharacterData _staticData;

	[Tooltip("Contains the runtime data of the character.\nThis should NOT be modified by the designers.\nIt can be used to view runtime values in the Editor.")]
	[SerializeField]
	private RuntimeCharacterData _runtimeData;

	private const string CHARACTER_PREFIX = "CHARACTER";

	private const string CHARACTER_NAME = "CHARACTER_NAME";

	private const string CHARACTER_SURNAME = "CHARACTER_SURNAME";

	private const string CHARACTER_FULL_NAME = "CHARACTER_FULL_NAME";

	private const string CHARACTER_SEX = "CHARACTER_SEX";

	public StaticCharacterData StaticData => _staticData;

	public RuntimeCharacterData RuntimeData => _runtimeData;

	public string ID => Guid;

	public bool Equals(Character c)
	{
		if (c == null)
		{
			return false;
		}
		return Equals(c.ID);
	}

	public bool Equals(string characterId)
	{
		if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(characterId))
		{
			return ID == characterId;
		}
		return false;
	}

	private string GetOptionBySex(string text)
	{
		string[] array = text.Substring(text.IndexOf("|", StringComparison.Ordinal) + 1).Split('/');
		if (_staticData.Female)
		{
			return array[0];
		}
		return array[1];
	}

	public void UpdateCharacterDecks()
	{
		if (!(_staticData.CharacterDecks == null))
		{
			for (int i = 0; i < _staticData.CharacterDecks.Decks.Count; i++)
			{
				_staticData.CharacterDecks.Decks[i].RefreshDeckState();
			}
		}
	}

	public void Reset()
	{
		ResetLock();
		RuntimeData.CharacterId = ID;
		ResetNeeds();
		ResetAttributes();
		ResetStatuses();
		RuntimeData.CharacterHistory.Clear();
		_runtimeData.IsCaptain = false;
		_runtimeData.ShowsEndOfGameSlide = true;
	}

	public void ResetLock()
	{
		_runtimeData.Locked = false;
	}

	public void ResetAttributes()
	{
		_runtimeData.CurrentAttributes = _staticData.GetMutableAttributes();
	}

	public void ResetNeeds()
	{
		_runtimeData.CurrentNeeds = _staticData.GetMutableNeeds();
	}

	public void ResetStatuses()
	{
		if (_runtimeData.CurrentStatuses == null)
		{
			_runtimeData.CurrentStatuses = new List<CharacterStatus>();
		}
		else
		{
			_runtimeData.CurrentStatuses.Clear();
		}
		for (int i = 0; i < _staticData.StartingStatuses.Length; i++)
		{
			_runtimeData.CurrentStatuses.Add(_staticData.StartingStatuses[i]);
		}
		if (_runtimeData.PreviousStatuses == null)
		{
			_runtimeData.PreviousStatuses = new List<CharacterStatus>();
		}
		else
		{
			_runtimeData.PreviousStatuses.Clear();
		}
		for (int j = 0; j < _staticData.StartingStatuses.Length; j++)
		{
			_runtimeData.PreviousStatuses.Add(_staticData.StartingStatuses[j]);
		}
	}

	public void MarkCurrentStatusesAsPrevious()
	{
		_runtimeData.PreviousStatuses.Clear();
		for (int i = 0; i < _runtimeData.CurrentStatuses.Count; i++)
		{
			_runtimeData.PreviousStatuses.Add(_runtimeData.CurrentStatuses[i]);
		}
	}

	public void ResetNeed(CharacterNeed need)
	{
		for (int i = 0; i < _runtimeData.CurrentNeeds.Count; i++)
		{
			if (!_runtimeData.CurrentNeeds[i].Need.Equals(need))
			{
				continue;
			}
			List<CharacterNeedParameters> mutableNeeds = _staticData.GetMutableNeeds();
			for (int j = 0; j < mutableNeeds.Count; j++)
			{
				if (mutableNeeds[j].Need.Equals(need))
				{
					_runtimeData.CurrentNeeds[i] = mutableNeeds[j];
				}
			}
		}
	}

	public void ResetAttribute(CharacterAttribute attribute)
	{
		for (int i = 0; i < _runtimeData.CurrentAttributes.Count; i++)
		{
			if (!_runtimeData.CurrentAttributes[i].Attribute.Equals(attribute))
			{
				continue;
			}
			List<CharacterAttributeValue> mutableAttributes = _staticData.GetMutableAttributes();
			for (int j = 0; j < mutableAttributes.Count; j++)
			{
				if (mutableAttributes[j].Attribute.Equals(attribute))
				{
					_runtimeData.CurrentAttributes[i] = mutableAttributes[j];
				}
			}
		}
	}

	public override string ToString()
	{
		return ID;
	}

	public void Lock()
	{
		_runtimeData.Locked = true;
	}

	public void Unlock()
	{
		_runtimeData.Locked = false;
	}

	public virtual bool CanLock()
	{
		return !_runtimeData.Locked;
	}

	public void RegisterInI2Loc()
	{
		if (!LocalizationManager.ParamManagers.Contains(this))
		{
			LocalizationManager.ParamManagers.Add(this);
			LocalizationManager.LocalizeAll(Force: true);
		}
	}

	public void UnregisterFromI2Loc()
	{
		LocalizationManager.ParamManagers.Remove(this);
	}

	private void OnDisable()
	{
		LocalizationManager.ParamManagers.Remove(this);
	}

	public string GetParameterValue(string param)
	{
		if (!param.StartsWith("CHARACTER"))
		{
			return null;
		}
		switch (param)
		{
		case "CHARACTER_NAME":
			return _staticData.Name;
		case "CHARACTER_SURNAME":
			return _staticData.Surname;
		case "CHARACTER_FULL_NAME":
			return string.Concat(_staticData.Name, " ", _staticData.Surname);
		default:
			if (param.StartsWith("CHARACTER_SEX"))
			{
				return GetOptionBySex(param);
			}
			return null;
		}
	}

	private void OnEnable()
	{
		_saveEvent.RegisterListener(this);
	}

	public string Serialize()
	{
		CharacterWrapper characterWrapper = new CharacterWrapper
		{
			ID = _runtimeData.CharacterId,
			IsCaptain = _runtimeData.IsCaptain,
			CurrentStatusesID = new List<string>(),
			PreviousStatusesID = new List<string>(),
			Needs = new List<NeedWrapper>(),
			History = _runtimeData.CharacterHistory,
			IsLocked = _runtimeData.Locked,
			ShowsEndOfGameSlide = _runtimeData.ShowsEndOfGameSlide,
			Attributes = new List<AttributeWrapper>()
		};
		for (int i = 0; i < _runtimeData.CurrentStatuses.Count; i++)
		{
			characterWrapper.CurrentStatusesID.Add(_runtimeData.CurrentStatuses[i].Id);
		}
		for (int j = 0; j < _runtimeData.PreviousStatuses.Count; j++)
		{
			characterWrapper.PreviousStatusesID.Add(_runtimeData.PreviousStatuses[j].Id);
		}
		for (int k = 0; k < _runtimeData.CurrentNeeds.Count; k++)
		{
			NeedWrapper item = new NeedWrapper
			{
				NeedStatus = _runtimeData.CurrentNeeds[k].GetNeedStatus(),
				Value = _runtimeData.CurrentNeeds[k].Value
			};
			characterWrapper.Needs.Add(item);
		}
		for (int l = 0; l < _runtimeData.CurrentAttributes.Count; l++)
		{
			characterWrapper.Attributes.Add(new AttributeWrapper
			{
				ID = _runtimeData.CurrentAttributes[l].Attribute.Id,
				Value = _runtimeData.CurrentAttributes[l].Value
			});
		}
		return JsonUtility.ToJson(characterWrapper);
	}

	public void Deserialize(string jsonData)
	{
		CharacterWrapper wrapper = JsonUtility.FromJson<CharacterWrapper>(jsonData);
		_runtimeData.CharacterId = wrapper.ID;
		_runtimeData.IsCaptain = wrapper.IsCaptain;
		_runtimeData.Locked = wrapper.IsLocked;
		_runtimeData.ShowsEndOfGameSlide = wrapper.ShowsEndOfGameSlide;
		_runtimeData.CurrentStatuses.Clear();
		for (int i = 0; i < wrapper.CurrentStatusesID.Count; i++)
		{
			if (!string.IsNullOrEmpty(wrapper.CurrentStatusesID[i]))
			{
				_runtimeData.CurrentStatuses.Add((CharacterStatus)_saveEvent.GetReferenceObjectByID(wrapper.CurrentStatusesID[i]));
				continue;
			}
			Debug.LogWarningFormat("Current status id is null or empty - index {0} character {1} in character deserialization", i, wrapper.ID);
		}
		_runtimeData.PreviousStatuses.Clear();
		for (int j = 0; j < wrapper.PreviousStatusesID.Count; j++)
		{
			if (!string.IsNullOrEmpty(wrapper.PreviousStatusesID[j]))
			{
				_runtimeData.PreviousStatuses.Add((CharacterStatus)_saveEvent.GetReferenceObjectByID(wrapper.PreviousStatusesID[j]));
				continue;
			}
			Debug.LogWarningFormat("Previous status id is null or empty - index {0} character {1} in character deserialization", j, wrapper.ID);
		}
		if (wrapper.Needs.Count > _runtimeData.CurrentNeeds.Count)
		{
			throw new UnityException(GetNeedsMismatchErrorMessage(wrapper));
		}
		for (int k = 0; k < wrapper.Needs.Count; k++)
		{
			NeedWrapper needWrapper = wrapper.Needs[k];
			_runtimeData.CurrentNeeds[k].Value = needWrapper.Value;
			_runtimeData.CurrentNeeds[k].SetNeedStatus(needWrapper.NeedStatus);
		}
		_runtimeData.CharacterHistory = wrapper.History;
		if (wrapper.Attributes != null && wrapper.Attributes.Count > 0)
		{
			for (int l = 0; l < wrapper.Attributes.Count; l++)
			{
				AttributeWrapper attributeWrapper = wrapper.Attributes[l];
				_runtimeData.SetAttributeValue(attributeWrapper.ID, attributeWrapper.Value);
			}
		}
	}

	private string GetNeedsMismatchErrorMessage(CharacterWrapper wrapper)
	{
		StringBuilder stringBuilder = new StringBuilder("Error during character deserialization. The Need count in the serialized wrapper exceeds the need count in the character.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine($"Needs count in wrapper: {wrapper.Needs.Count}");
		stringBuilder.AppendLine("Current character needs:");
		foreach (CharacterNeedParameters currentNeed in _runtimeData.CurrentNeeds)
		{
			stringBuilder.AppendLine(currentNeed.Need.Id);
		}
		return stringBuilder.ToString();
	}

	public void DefaultData()
	{
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
		Reset();
	}
}
