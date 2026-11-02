using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ExpeditionRuntimeData
{
	[Serializable]
	public class OngoingExpData
	{
		[SerializeField]
		private ExpeditionDestination _chosenDestination;

		[SerializeField]
		private Character _expeditionCharacter;

		[SerializeField]
		private List<IItem> _expeditionItems = new List<IItem>();

		public ExpeditionDestination ChosenDestination
		{
			get
			{
				return _chosenDestination;
			}
			set
			{
				_chosenDestination = value;
			}
		}

		public Character ExpeditionCharacter
		{
			get
			{
				return _expeditionCharacter;
			}
			set
			{
				_expeditionCharacter = value;
			}
		}

		public List<IItem> ExpeditionItems
		{
			get
			{
				return _expeditionItems;
			}
			set
			{
				_expeditionItems = value;
			}
		}

		public OngoingExpData Clone()
		{
			OngoingExpData ongoingExpData = new OngoingExpData();
			ongoingExpData._expeditionItems = new List<IItem>();
			for (int i = 0; i < _expeditionItems.Count; i++)
			{
				ongoingExpData._expeditionItems.Add(_expeditionItems[i]);
			}
			ongoingExpData._chosenDestination = _chosenDestination;
			ongoingExpData._expeditionCharacter = _expeditionCharacter;
			return ongoingExpData;
		}
	}

	[SerializeField]
	[Tooltip("Is Expedition System currently active.")]
	private bool _isActive;

	[SerializeField]
	[Tooltip("Is Expedition planned?")]
	private bool _isPlanned;

	[SerializeField]
	[Tooltip("Is Expedition ongoing?")]
	private bool _isOngoing;

	[SerializeField]
	[Tooltip("Is expedition system repaired?")]
	private bool _isRepaired = true;

	[SerializeField]
	[Tooltip("Is system currently focused by desig ? Sets lamp color to blinking yellow.")]
	private bool _isFocusedByDesign;

	[SerializeField]
	[Tooltip("Current time spend on expedition.")]
	private int _elapsedExpeditionTime;

	[SerializeField]
	[Tooltip("Expedition System level.")]
	private int _expeditionSystemLevel = 1;

	[SerializeField]
	[Tooltip("Should expedition system be reactivated?")]
	private bool _shouldReactivate;

	[SerializeField]
	[Tooltip("Planet Deck from the planet we have landed on.")]
	private PlanetDeck _currentPlanetDeck;

	[SerializeField]
	[Tooltip("Data that is set when expedition is set from Survival scene.")]
	private OngoingExpData _plannedExpeditionData = new OngoingExpData();

	[SerializeField]
	[Tooltip("Data that is set when expedition is sent")]
	private OngoingExpData _ongoingExpeditionData = new OngoingExpData();

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelTitleTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelDescriptionTerm;

	[NonSerialized]
	private List<ExpeditionDestination> _expeditionDestinations;

	public bool IsActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			_isActive = value;
		}
	}

	public int ExpeditionSystemLevel
	{
		get
		{
			return _expeditionSystemLevel;
		}
		set
		{
			_expeditionSystemLevel = value;
		}
	}

	public bool IsPlanned
	{
		get
		{
			return _isPlanned;
		}
		set
		{
			_isPlanned = value;
		}
	}

	public bool IsOngoing
	{
		get
		{
			return _isOngoing;
		}
		set
		{
			_isOngoing = value;
		}
	}

	public int ElapsedExpeditionTime
	{
		get
		{
			return _elapsedExpeditionTime;
		}
		set
		{
			_elapsedExpeditionTime = value;
		}
	}

	public bool ShouldReactivate
	{
		get
		{
			return _shouldReactivate;
		}
		set
		{
			_shouldReactivate = value;
		}
	}

	public PlanetDeck CurrentPlanetDeck
	{
		get
		{
			return _currentPlanetDeck;
		}
		set
		{
			_currentPlanetDeck = value;
		}
	}

	public OngoingExpData PlannedExpeditionData
	{
		get
		{
			return _plannedExpeditionData;
		}
		set
		{
			_plannedExpeditionData = value;
		}
	}

	public OngoingExpData OngoingExpeditionData
	{
		get
		{
			return _ongoingExpeditionData;
		}
		set
		{
			_ongoingExpeditionData = value;
		}
	}

	public List<ExpeditionDestination> ExpeditionDestinations
	{
		get
		{
			if (_expeditionDestinations == null && _currentPlanetDeck != null)
			{
				_expeditionDestinations = _currentPlanetDeck.Destinations;
			}
			return _expeditionDestinations;
		}
	}

	public bool IsRepaired
	{
		get
		{
			return _isRepaired;
		}
		set
		{
			_isRepaired = value;
		}
	}

	public LocalizedString BlockedPanelTitleTerm
	{
		get
		{
			return _blockedPanelTitleTerm;
		}
		set
		{
			_blockedPanelTitleTerm = value;
		}
	}

	public LocalizedString BlockedPanelDescriptionTerm
	{
		get
		{
			return _blockedPanelDescriptionTerm;
		}
		set
		{
			_blockedPanelDescriptionTerm = value;
		}
	}

	public bool IsFocusedByDesign
	{
		get
		{
			return _isFocusedByDesign;
		}
		set
		{
			_isFocusedByDesign = value;
		}
	}

	public bool CanAnyoneGoToExpedition()
	{
		List<Character> liveCharacters = CharacterManager.Instance.GetCharacterList().GetLiveCharacters();
		for (int i = 0; i < liveCharacters.Count; i++)
		{
			if (!liveCharacters[i].RuntimeData.IsCaptain && !liveCharacters[i].RuntimeData.HasStatusPreventingGoingOnExpeditions())
			{
				return true;
			}
		}
		return false;
	}
}
