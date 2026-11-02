using System.Collections;
using System.Collections.Generic;
using RG.Core.Base;
using RG.Parsecs.Common;
using RG.VirtualInput;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class CharacterSlot : RGMonoBehaviour, IRefreshInteractable
{
	private const string DEAD_ON_EXPEDITION_STATUS = "CharacterStatus_DeadExpedition";

	[SerializeField]
	[Range(0f, 3f)]
	private int _characterIndex;

	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private string _sortingLayerNameForBody;

	[SerializeField]
	private bool _useForcedCharacter;

	[Tooltip("If Use Forced Character is true, this characters body will be chosenfor this slot if it is present in the Character List. In that case it can't be null.")]
	[SerializeField]
	private Character _forcedCharacter;

	[Tooltip("This is auto-filled on start\nbasing on chosen Character Index or Forced Character and Character List, with or without the use of Character Body Prefab List.")]
	[SerializeField]
	private CharacterBody _characterBody;

	[SerializeField]
	private CharacterBodyPrefabList _characterBodyPrefabList;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	[SerializeField]
	private List<CharacterPosition> _posForCharacters;

	private bool _isSetupRequired = true;

	[SerializeField]
	private WindowView _rationingWindowView;

	[SerializeField]
	private bool _useGamepadButton = true;

	[SerializeField]
	private bool _canBeSelectedWhenDead;

	[SerializeField]
	private EndGameData _endGameData;

	private VirtualInputButton _gamepadButton;

	private void OnMouseUpAsButton()
	{
		ShowRationing();
	}

	public void ShowRationing()
	{
		if (!(_rationingWindowView == null) && !(_characterList == null) && !_characterList.CharactersInGame[_characterIndex].RuntimeData.HasStatusPreventingRationing() && !EndGameManager.Instance.GetEndGameData().RuntimeData.ShouldEndGame && !Singleton<HandCursor>.Instance.IsPointerOverGameObject())
		{
			_rationingWindowView.Show();
		}
	}

	protected override void CustomStart()
	{
		_endOfDayListenerList.RegisterOnEndOfDay(OnEndOfDay, "Visuals", 1, this, forceOrder: true);
		_characterList.RegisterCharacterListChangedListener(OnCharacterListChanged);
		SetupCharacterBody();
		StartCoroutine(RefreshInteractableAfterSecond());
	}

	public void OnCharacterListChanged()
	{
		_isSetupRequired = true;
		if (_useGamepadButton)
		{
			RefreshInteractable();
		}
	}

	private void SetupCharacterBody()
	{
		if (_characterBody != null)
		{
			Object.Destroy(_characterBody.gameObject);
		}
		if (_useForcedCharacter)
		{
			if (_characterList.CharactersInGame.Contains(_forcedCharacter))
			{
				Vector3 offsetForCharacters = GetOffsetForCharacters(_forcedCharacter);
				if (_forcedCharacter.StaticData.BodyPrefab != null)
				{
					_characterBody = Object.Instantiate(_forcedCharacter.StaticData.BodyPrefab, transform).GetComponent<CharacterBody>();
				}
				else
				{
					for (int i = 0; i < _characterBodyPrefabList.CharacterBodyPrefabs.Count; i++)
					{
						if (_characterBodyPrefabList.CharacterBodyPrefabs[i].Character.Equals(_forcedCharacter))
						{
							_characterBody = Object.Instantiate(_characterBodyPrefabList.CharacterBodyPrefabs[i].BodyPrefab, transform).GetComponent<CharacterBody>();
						}
					}
				}
				_characterBody.transform.localPosition = new Vector3(offsetForCharacters.x, offsetForCharacters.y, offsetForCharacters.z);
				if (!string.IsNullOrEmpty(_sortingLayerNameForBody))
				{
					_characterBody.SetBodySortingLayer(_sortingLayerNameForBody);
				}
				_characterBody.UpdateDisplayedCharacter();
				SetSoundSlot();
			}
		}
		else if (_characterIndex < _characterList.GetCharacterCount())
		{
			Vector3 offsetForCharacters2 = GetOffsetForCharacters(_characterList.CharactersInGame[_characterIndex]);
			if (_characterList.CharactersInGame[_characterIndex].StaticData.BodyPrefab != null)
			{
				_characterBody = Object.Instantiate(_characterList.CharactersInGame[_characterIndex].StaticData.BodyPrefab, transform).GetComponent<CharacterBody>();
			}
			else
			{
				for (int j = 0; j < _characterBodyPrefabList.CharacterBodyPrefabs.Count; j++)
				{
					if (_characterBodyPrefabList.CharacterBodyPrefabs[j].Character.Equals(_characterList.CharactersInGame[_characterIndex]))
					{
						_characterBody = Object.Instantiate(_characterBodyPrefabList.CharacterBodyPrefabs[j].BodyPrefab, transform).GetComponent<CharacterBody>();
					}
				}
			}
			_characterBody.transform.localPosition = new Vector3(offsetForCharacters2.x, offsetForCharacters2.y, offsetForCharacters2.z);
			if (!string.IsNullOrEmpty(_sortingLayerNameForBody))
			{
				_characterBody.SetBodySortingLayer(_sortingLayerNameForBody);
			}
			_characterBody.UpdateDisplayedCharacter();
			SetSoundSlot();
		}
		_isSetupRequired = false;
	}

	private void SetSoundSlot()
	{
		OnSpriteClickedSoundPlayer component = GetComponent<OnSpriteClickedSoundPlayer>();
		if (component != null)
		{
			if (_useForcedCharacter)
			{
				component.SoundSlot = _forcedCharacter.StaticData.VoiceOver;
			}
			else
			{
				component.SoundSlot = _characterList.CharactersInGame[_characterIndex].StaticData.VoiceOver;
			}
		}
	}

	private void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(OnEndOfDay, "Visuals");
		_characterList.UnregisterCharacterListChangedListener(OnCharacterListChanged);
	}

	public void OnEndOfDay()
	{
		if (_isSetupRequired)
		{
			SetupCharacterBody();
		}
		if (_characterBody != null)
		{
			_characterBody.UpdateDisplayedCharacter();
		}
		StartCoroutine(RefreshInteractableAfterSecond());
	}

	private Vector3 GetOffsetForCharacters(Character character)
	{
		if (_posForCharacters == null)
		{
			return Vector3.zero;
		}
		for (int i = 0; i < _posForCharacters.Count; i++)
		{
			if (_posForCharacters[i].Character == character)
			{
				return _posForCharacters[i].Position;
			}
		}
		return Vector3.zero;
	}

	public void RefreshInteractable()
	{
		if (_gamepadButton == null)
		{
			_gamepadButton = GetComponentInChildren<VirtualInputButton>();
		}
		bool interactable = _characterList.CharactersInGame.Contains(_forcedCharacter) && !_forcedCharacter.RuntimeData.IsOnExpedition() && (_forcedCharacter.RuntimeData.IsAlive() || CanCharacterBeSelectedWhenDead()) && !_endGameData.RuntimeData.ShouldEndGame;
		if (_gamepadButton != null)
		{
			_gamepadButton.Selectable.interactable = interactable;
		}
	}

	private bool CanCharacterBeSelectedWhenDead()
	{
		if (!_canBeSelectedWhenDead)
		{
			return false;
		}
		Debug.Log("Does character has status dead on expedition? " + _forcedCharacter.RuntimeData.HasStatus("CharacterStatus_DeadExpedition"));
		return !_forcedCharacter.RuntimeData.HasStatus("CharacterStatus_DeadExpedition");
	}

	private IEnumerator RefreshInteractableAfterSecond()
	{
		yield return new WaitForSeconds(1f);
		RefreshInteractable();
	}

	public Character GetCharacter()
	{
		if (_useForcedCharacter)
		{
			return _forcedCharacter;
		}
		if (_characterList != null && _characterIndex >= 0 && _characterList.GetCharacterCount() >= _characterIndex)
		{
			return _characterList.CharactersInGame[_characterIndex];
		}
		return null;
	}
}
