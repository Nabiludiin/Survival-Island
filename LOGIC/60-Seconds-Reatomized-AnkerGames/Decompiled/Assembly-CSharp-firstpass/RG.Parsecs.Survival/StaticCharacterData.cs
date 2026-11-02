using System;
using System.Collections.Generic;
using I2.Loc;
using RG.Parsecs.Common;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class StaticCharacterData
{
	[SerializeField]
	private LocalizedString _name = null;

	[SerializeField]
	private LocalizedString _surname = null;

	[SerializeField]
	private LocalizedString _description = null;

	[Tooltip("A prefab with CharacterBody component and the necessary children.")]
	[SerializeField]
	private GameObject _bodyPrefab;

	[SerializeField]
	private Goal _personalGoal;

	[Tooltip("Character epilogue")]
	[SerializeField]
	private EndGameCanvas _personalEpilogue;

	[Tooltip("Captain epilogue")]
	[SerializeField]
	private EndGameCanvas _captainEpilogue;

	[Tooltip("The personality profiles the character responds positively to.")]
	[SerializeField]
	private PersonalityProfile[] _positiveResponse;

	[Tooltip("The personality profiles the character responds negatively to.")]
	[SerializeField]
	private PersonalityProfile[] _negativeResponse;

	[SerializeField]
	private bool _female = true;

	[Tooltip("The captains gender must fit this value\nto unlock the romance deck with this character.\nIf set to NONE, the romance deck cannot be unlocked.")]
	[SerializeField]
	private ERomanticInterests _romanticInterest;

	[Tooltip("The decks that should be unlocked when the designer decides that the characters enter a relationship.")]
	[SerializeField]
	private Deck[] _romanceDecks;

	[Tooltip("The decks that should be unlocked in the event run\nwhen the morale of the character rises above the threshold.")]
	[SerializeField]
	private Deck[] _frienshipDecks;

	[Tooltip("The decks that should be unlocked in the event run\nwhen the morale of the character falls below the threshold.")]
	[SerializeField]
	private Deck[] _adversityDecks;

	[SerializeField]
	private CharacterDeckList _characterDecks;

	[Tooltip("A list defining what events should be run\nwhen the character enters/exits/remains in various statuses.\nShould contain entries for all statuses.")]
	[SerializeField]
	private CharacterStatusBehaviour[] _statusBehaviours;

	[SerializeField]
	private CharacterStatus[] _startingStatuses;

	[Tooltip("A list defining the initial values of the characters attributes.\nShould contain entries for all attributes.")]
	[SerializeField]
	private CharacterAttributeValue[] _attributes;

	[Tooltip("A list defining the needs of the character and related parameters.\nShould contain entries for all needs.")]
	[SerializeField]
	private CharacterNeedParameters[] _needs;

	[SerializeField]
	private CharacterIcons _icons;

	[SerializeField]
	private LocalizedString _iconTerm;

	[Tooltip("The sound event set in this SoundSlot will be played back on click by this script. If null is set, the sound will not be played.")]
	[SerializeField]
	private SoundSlot _soundSlot;

	public LocalizedString Name => _name;

	public LocalizedString Surname => _surname;

	public SoundSlot VoiceOver => _soundSlot;

	public string FullName => $"{_name} {_surname}";

	public Goal PersonalGoal => _personalGoal;

	public EndGameCanvas CaptainEpilogue => _captainEpilogue;

	public EndGameCanvas PersonalEpilogue => _personalEpilogue;

	public PersonalityProfile[] PositiveResponse => _positiveResponse;

	public PersonalityProfile[] NegativeResponse => _negativeResponse;

	public bool Female => _female;

	public ERomanticInterests RomanticInterest => _romanticInterest;

	public Deck[] RomanceDecks => _romanceDecks;

	public Deck[] FriendshipDecks => _frienshipDecks;

	public Deck[] AdversityDecks => _adversityDecks;

	public CharacterStatusBehaviour[] StatusBehaviours => _statusBehaviours;

	public CharacterStatus[] StartingStatuses => _startingStatuses;

	public CharacterAttributeValue[] Attributes => _attributes;

	public CharacterNeedParameters[] Needs => _needs;

	public CharacterIcons Icons => _icons;

	public GameObject BodyPrefab => _bodyPrefab;

	public CharacterDeckList CharacterDecks => _characterDecks;

	public LocalizedString Description => _description;

	public LocalizedString IconTerm => _iconTerm;

	public CharacterAttributeValue GetAttributeById(string id)
	{
		for (int i = 0; i < _attributes.Length; i++)
		{
			if (_attributes[i].Attribute.Is(id))
			{
				return _attributes[i];
			}
		}
		return null;
	}

	public CharacterStatusBehaviour GetBehaviourForStatus(CharacterStatus status)
	{
		return GetBehaviourForStatus(status.Id);
	}

	public CharacterStatusBehaviour GetBehaviourForStatus(string statusId)
	{
		for (int i = 0; i < _statusBehaviours.Length; i++)
		{
			if (_statusBehaviours[i].Status.Is(statusId))
			{
				return _statusBehaviours[i];
			}
		}
		Debug.LogWarning($"No behaviour found for status with id {statusId}!");
		return null;
	}

	public List<CharacterAttributeValue> GetMutableAttributes()
	{
		List<CharacterAttributeValue> list = new List<CharacterAttributeValue>();
		for (int i = 0; i < _attributes.Length; i++)
		{
			list.Add(_attributes[i].GetMutableCopy());
		}
		return list;
	}

	public List<CharacterNeedParameters> GetMutableNeeds()
	{
		List<CharacterNeedParameters> list = new List<CharacterNeedParameters>();
		for (int i = 0; i < _needs.Length; i++)
		{
			list.Add(_needs[i].GetMutableCopy());
		}
		return list;
	}

	public AttitudeTowardsPersonalityProfile GetAttitudeTowardsPersonalityProfile(PersonalityProfile profile)
	{
		for (int i = 0; i < _positiveResponse.Length; i++)
		{
			if (_positiveResponse[i].Is(profile.Id))
			{
				return AttitudeTowardsPersonalityProfile.POSITIVE;
			}
		}
		for (int j = 0; j < _negativeResponse.Length; j++)
		{
			if (_negativeResponse[j].Is(profile.Id))
			{
				return AttitudeTowardsPersonalityProfile.NEGATIVE;
			}
		}
		return AttitudeTowardsPersonalityProfile.NEUTRAL;
	}
}
