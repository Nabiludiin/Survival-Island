using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/Characters/New Character List", fileName = "New Character List")]
public class CharacterList : RGScriptableObject, ISaveable, ILocalizationParamsManager
{
	public delegate void OnCharacterListChanged();

	public const int MAX_CHARACTER_COUNT = 4;

	private const string SAVE_ID = "character_CharacterList";

	[Tooltip("Event to which data will be save and from which be loaded")]
	[SerializeField]
	private SaveEvent _saveEvent;

	private List<OnCharacterListChanged> _onCharacterListChanged = new List<OnCharacterListChanged>();

	[SerializeField]
	[Header("Assets")]
	[Space]
	[Tooltip("Set containing all characters playable in Survival mode.")]
	private CharacterSet _allCharacters;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	[Header("Gameplay")]
	[Space]
	[SerializeField]
	private List<Character> _characters = new List<Character>();

	private int _startingCrew;

	private const string CREW_PREFIX = "CREW";

	private const string CREW_COUNT_STARTING = "CREW_COUNT_STARTING";

	private const string CREW_COUNT_LIVE_ABOARD = "CREW_COUNT_LIVE_ABOARD";

	private const string CREW_COUNT_LIVE_ALL = "CREW_COUNT_LIVE_ALL";

	private const string CAPTAIN_PREFIX = "CAPTAIN";

	private const string CAPTAIN_NAME = "CAPTAIN_NAME";

	private const string CAPTAIN_SURNAME = "CAPTAIN_SURNAME";

	private const string CAPTAIN_FULL_NAME = "CAPTAIN_FULL_NAME";

	private const string CAPTAIN_SEX = "CAPTAIN_SEX";

	private const string CAPTAIN_IS_ALONE = "CAPTAIN_IS_ALONE";

	private const int REFRESH_CHARACTER_DECKS_LISTENER_LIST_PRIORITY = 15;

	public CharacterSet GetCharacterSet => _allCharacters;

	public List<Character> CharactersInGame => _characters;

	public List<Character> AllCharacters => _allCharacters.AllCharacters.ToList();

	public string ID => Guid;

	public int StartingCrew => _startingCrew;

	public void RegisterCharacterListChangedListener(OnCharacterListChanged listener)
	{
		if (!_onCharacterListChanged.Contains(listener))
		{
			_onCharacterListChanged.Add(listener);
		}
	}

	public void UnregisterCharacterListChangedListener(OnCharacterListChanged listener)
	{
		if (_onCharacterListChanged.Contains(listener))
		{
			_onCharacterListChanged.Remove(listener);
		}
	}

	public void NotifyListenersCharacterListChanged()
	{
		for (int i = 0; i < _onCharacterListChanged.Count; i++)
		{
			_onCharacterListChanged[i]();
		}
	}

	public IEnumerator NotifyListenersCharacterListChangedCoroutine()
	{
		NotifyListenersCharacterListChanged();
		yield return null;
	}

	private void OnEnable()
	{
		_characters.Clear();
		Register();
		_onCharacterListChanged = new List<OnCharacterListChanged>();
		_endOfDayListenerList.RegisterOnEndOfDay(RefreshCharacterDecks, "SystemEvents", 15, this);
	}

	private void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(RefreshCharacterDecks, "SystemEvents");
		Unregister();
	}

	private void OnDisable()
	{
		LocalizationManager.ParamManagers.Remove(this);
	}

	private void RefreshCharacterDecks()
	{
		for (int i = 0; i < _characters.Count; i++)
		{
			_characters[i].UpdateCharacterDecks();
		}
	}

	public Character GetCaptain()
	{
		foreach (Character character in _characters)
		{
			if (character.RuntimeData.IsCaptain)
			{
				return character;
			}
		}
		Debug.LogWarning("Character list is empty!");
		return null;
	}

	public int GetCharacterCount()
	{
		return _characters.Count;
	}

	public void AddCharToList(Character character)
	{
		_characters.Add(character);
		NotifyListenersCharacterListChanged();
	}

	public void RemoveCharFromList(Character character)
	{
		_characters.Remove(character);
		NotifyListenersCharacterListChanged();
	}

	public void ResetCharFromList()
	{
		_characters.Clear();
		NotifyListenersCharacterListChanged();
	}

	public List<Character> GetCharactersWithStatus(string statusId)
	{
		List<Character> list = new List<Character>();
		for (int i = 0; i < _characters.Count; i++)
		{
			if (_characters[i].RuntimeData.HasStatus(statusId))
			{
				list.Add(_characters[i]);
			}
		}
		return list;
	}

	public List<Character> GetCharactersWithStatus(CharacterStatus status)
	{
		return GetCharactersWithStatus(status.Id);
	}

	public List<Character> GetCharactersWithoutStatus(CharacterStatus status)
	{
		return GetCharactersWithoutStatus(status.Id);
	}

	public List<Character> GetCharactersWithoutStatus(string statusId)
	{
		List<Character> list = new List<Character>();
		for (int i = 0; i < _characters.Count; i++)
		{
			if (!_characters[i].RuntimeData.HasStatus(statusId))
			{
				list.Add(_characters[i]);
			}
		}
		return list;
	}

	public Character GetAvailableCharacterOfId(string id)
	{
		for (int i = 0; i < _characters.Count; i++)
		{
			if (_characters[i].Equals(id))
			{
				return _characters[i];
			}
		}
		return null;
	}

	public List<Character> GetLiveCharacters()
	{
		List<Character> list = new List<Character>();
		for (int i = 0; i < _characters.Count; i++)
		{
			if (_characters[i].RuntimeData.IsAlive())
			{
				list.Add(_characters[i]);
			}
		}
		return list;
	}

	public int GetLiveCharactersCount()
	{
		return GetLiveCharacters().Count;
	}

	public Character GetCharacter(string id)
	{
		for (int i = 0; i < _characters.Count; i++)
		{
			if (_characters[i].Equals(id))
			{
				return _characters[i];
			}
		}
		Debug.LogError("No character found for id " + id);
		return null;
	}

	public string Serialize()
	{
		CharacterListWrapper characterListWrapper = new CharacterListWrapper
		{
			StartCrewMember = _startingCrew,
			CharactersList = new List<string>()
		};
		for (int i = 0; i < _characters.Count; i++)
		{
			characterListWrapper.CharactersList.Add(_characters[i].ID);
		}
		return JsonUtility.ToJson(characterListWrapper);
	}

	public void Deserialize(string jsonData)
	{
		CharacterListWrapper characterListWrapper = JsonUtility.FromJson<CharacterListWrapper>(jsonData);
		_startingCrew = characterListWrapper.StartCrewMember;
		for (int i = 0; i < characterListWrapper.CharactersList.Count; i++)
		{
			_characters.Add(_allCharacters.GetCharacter(characterListWrapper.CharactersList[i]));
		}
		NotifyListenersCharacterListChanged();
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
		_characters.Clear();
	}

	public void RegisterInI2Loc()
	{
		if (!LocalizationManager.ParamManagers.Contains(this))
		{
			LocalizationManager.ParamManagers.Add(this);
			LocalizationManager.LocalizeAll(Force: true);
		}
	}

	public string GetParameterValue(string param)
	{
		if (!param.StartsWith("CREW") && !param.StartsWith("CAPTAIN"))
		{
			return null;
		}
		if (param.StartsWith("CAPTAIN"))
		{
			switch (param)
			{
			case "CAPTAIN_NAME":
				return GetCaptain().StaticData.Name;
			case "CAPTAIN_SURNAME":
				return GetCaptain().StaticData.Surname;
			case "CAPTAIN_FULL_NAME":
				return string.Concat(GetCaptain().StaticData.Name, " ", GetCaptain().StaticData.Surname);
			}
			if (param.StartsWith("CAPTAIN_SEX"))
			{
				return GetOptionBySex(param);
			}
			if (param.StartsWith("CAPTAIN_IS_ALONE"))
			{
				return GetOptionByCaptainAlone(param);
			}
		}
		else if (param.StartsWith("CREW"))
		{
			int num = -1;
			if (param.StartsWith("CREW_COUNT_STARTING"))
			{
				num = _startingCrew;
			}
			if (param.StartsWith("CREW_COUNT_LIVE_ALL"))
			{
				num = GetLiveCharactersCount();
			}
			if (param.StartsWith("CREW_COUNT_LIVE_ABOARD"))
			{
				int num2 = 0;
				for (int i = 0; i < _characters.Count; i++)
				{
					if (_characters[i].RuntimeData.IsAlive() && !_characters[i].RuntimeData.IsOnExpedition())
					{
						num2++;
					}
				}
				num = num2;
			}
			if (num == -1)
			{
				return null;
			}
			return param.Substring(param.IndexOf("|", StringComparison.Ordinal) + 1).Split('/')[num - 1];
		}
		return null;
	}

	private string GetOptionByCaptainAlone(string text)
	{
		string[] array = text.Substring(text.IndexOf("|", StringComparison.Ordinal) + 1).Split('/');
		if (GetLiveCharactersCount() == 1)
		{
			return array[0];
		}
		return array[1];
	}

	private string GetOptionBySex(string text)
	{
		string[] array = text.Substring(text.IndexOf("|", StringComparison.Ordinal) + 1).Split('/');
		if (GetCaptain().StaticData.Female)
		{
			return array[0];
		}
		return array[1];
	}

	public void ResetInGameCharacters()
	{
		for (int i = 0; i < _characters.Count; i++)
		{
			_characters[i].Reset();
		}
	}

	public void SetStartingData()
	{
		_startingCrew = GetCharacterCount();
	}

	public void RemoveForbiddenNeedsFromCaptain()
	{
		if (!IsCharacterListValid())
		{
			return;
		}
		List<CharacterNeedParameters> list = new List<CharacterNeedParameters>();
		for (int i = 0; i < _characters[0].RuntimeData.CurrentNeeds.Count; i++)
		{
			if (_characters[0].RuntimeData.CurrentNeeds[i].Need.CanBeHadByCaptain)
			{
				list.Add(_characters[0].RuntimeData.CurrentNeeds[i]);
			}
		}
		_characters[0].RuntimeData.CurrentNeeds = list;
	}

	private bool IsCharacterListValid()
	{
		if (_characters == null || _characters.Count < 1)
		{
			Debug.LogWarning("Character list is null or empty!");
			return false;
		}
		if (!_characters[0].RuntimeData.IsCaptain)
		{
			Debug.LogWarning("First character on list is not marked as captain!");
			return false;
		}
		for (int i = 1; i < _characters.Count; i++)
		{
			if (_characters[i].RuntimeData.IsCaptain)
			{
				Debug.Log("Character not first on the list marked as captain!");
				return false;
			}
		}
		return true;
	}
}
