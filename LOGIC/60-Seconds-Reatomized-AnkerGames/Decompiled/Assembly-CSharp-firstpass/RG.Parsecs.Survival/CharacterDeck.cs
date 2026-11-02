using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(fileName = "New Planet Deck", menuName = "60 Parsecs!/Event Editor/Decks/Character Deck")]
public class CharacterDeck : Deck
{
	[SerializeField]
	[Tooltip("Character that this deck will be checking for statuses.")]
	private Character _character;

	[SerializeField]
	[Tooltip("Should this deck be activated for captain.")]
	private bool _useForCaptain = true;

	[SerializeField]
	[Tooltip("Should this deck be activated for crewmate.")]
	private bool _useForCrewmate = true;

	[SerializeField]
	[Tooltip("Statuses that will activate this deck when characters gets all of them and none of the deactivate statuses")]
	private CharacterStatus[] _activateStatuses;

	[SerializeField]
	[Tooltip("Any of this statuses will deactivate this deck.")]
	private CharacterStatus[] _deactivateStatuses;

	[SerializeField]
	private bool _isOnHold;

	public void RefreshDeckState()
	{
		if (HasAllNeededStatuses() && !HasAnyPreventingStatus() && !_isOnHold)
		{
			if (CurrentState == DeckState.DEACTIVATED && ((_character.RuntimeData.IsCaptain && _useForCaptain) || (!_character.RuntimeData.IsCaptain && _useForCrewmate)))
			{
				SetActive(activate: true);
			}
		}
		else if (CurrentState == DeckState.ACTIVATED)
		{
			SetActive(activate: false);
		}
	}

	private bool HasAllNeededStatuses()
	{
		for (int i = 0; i < _activateStatuses.Length; i++)
		{
			if (_activateStatuses[i] != null && !_character.RuntimeData.HasStatus(_activateStatuses[i].Id))
			{
				return false;
			}
		}
		return true;
	}

	private bool HasAnyPreventingStatus()
	{
		for (int i = 0; i < _deactivateStatuses.Length; i++)
		{
			if (_deactivateStatuses[i] != null && _character.RuntimeData.HasStatus(_deactivateStatuses[i].Id))
			{
				return true;
			}
		}
		return false;
	}

	public CharacterParameter GetCharacterParameter()
	{
		return new CharacterParameter
		{
			Character = _character,
			Name = "StatusCharacter"
		};
	}

	public void SetDeckOnHold(bool onHold)
	{
		_isOnHold = onHold;
	}

	public bool IsDeckOnHold()
	{
		return _isOnHold;
	}

	public override string Serialize()
	{
		return JsonUtility.ToJson(new CharacterDeckCurrentStateWrapper
		{
			CurrentState = CurrentState,
			IsOnHold = _isOnHold
		});
	}

	public override void Deserialize(string jsonData)
	{
		CharacterDeckCurrentStateWrapper characterDeckCurrentStateWrapper = JsonUtility.FromJson<CharacterDeckCurrentStateWrapper>(jsonData);
		CurrentState = characterDeckCurrentStateWrapper.CurrentState;
		_isOnHold = characterDeckCurrentStateWrapper.IsOnHold;
	}
}
