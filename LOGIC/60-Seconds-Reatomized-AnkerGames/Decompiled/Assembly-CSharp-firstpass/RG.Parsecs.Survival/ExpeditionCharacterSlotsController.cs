using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class ExpeditionCharacterSlotsController : MonoBehaviour
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("Character List with data about characters.")]
	private CharacterList _characterList;

	[SerializeField]
	[Header("References")]
	[Tooltip("List of expedition characters visible in on the ship.")]
	private List<ExpeditionCharacterSlotController> _expeditionCharacters;

	[SerializeField]
	[Tooltip("Reference to characters panel.")]
	private CharacterPanelController _characterPanelController;

	public void SetAvailableCharacters()
	{
		int num = 0;
		if (_characterList.CharactersInGame.Count > 4)
		{
			throw new UnityException("There are to many characters! (More than 4)");
		}
		if (_characterList.CharactersInGame.Count < 4)
		{
			num = DisableUnusedSlots();
		}
		if (num >= 3)
		{
			return;
		}
		int i = 0;
		int num2 = num;
		for (; i < _characterList.CharactersInGame.Count; i++)
		{
			if (!_characterList.CharactersInGame[i].RuntimeData.IsCaptain)
			{
				if (num2 == 3)
				{
					throw new UnityException("There are to many characters! Are you sure there is a captain?");
				}
				_expeditionCharacters[num2].SetVisible(visible: true);
				_expeditionCharacters[num2].SetCharacter(_characterList.CharactersInGame[i]);
				num2++;
			}
		}
	}

	private int DisableUnusedSlots()
	{
		int num = 4 - _characterList.CharactersInGame.Count;
		for (int i = 0; i < num; i++)
		{
			_expeditionCharacters[i].SetVisible(visible: false);
		}
		return num;
	}

	public void RefreshCharacters()
	{
		for (int i = 0; i < _expeditionCharacters.Count; i++)
		{
			if (_expeditionCharacters[i].gameObject.activeSelf)
			{
				_expeditionCharacters[i].Interactable = true;
				_expeditionCharacters[i].Togglable = (_expeditionCharacters[i].Character.CanLock() && !_expeditionCharacters[i].Character.RuntimeData.HasStatusPreventingGoingOnExpeditions()) || _expeditionCharacters[i].IsOn;
			}
		}
	}

	public void SetAllTogglesOff()
	{
		for (int i = 0; i < _expeditionCharacters.Count; i++)
		{
			if ((bool)_expeditionCharacters[i].Character)
			{
				_expeditionCharacters[i].Character.Unlock();
			}
			_expeditionCharacters[i].IsOn = false;
		}
	}

	public void SetCurrentCharacter(ExpeditionCharacterSlotController expeditionCharacterSlotController)
	{
		if (!expeditionCharacterSlotController.IsOn)
		{
			_characterPanelController.SetCharacter(null);
			return;
		}
		if (expeditionCharacterSlotController.Character == null)
		{
			throw new UnityException("Character was not set.");
		}
		_characterPanelController.SetCharacter(expeditionCharacterSlotController.Character);
	}
}
