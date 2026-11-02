using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/Characters/New Status", fileName = "New Status")]
public class CharacterStatus : CharacterProperty
{
	[Tooltip("The priority of the status in displaying the character.\nHigh proiority of a status will make the char display system\nprefer using sprites that represent this status visually.")]
	[Range(0f, 100f)]
	[SerializeField]
	private int _priority;

	[Tooltip("If set to false, the character will not be drawn in the ship.")]
	[SerializeField]
	private bool _allowsDrawingInShip = true;

	[Tooltip("If set to true, the game will be over\nwhen the captain enters this status")]
	[SerializeField]
	private bool _endsGameIfCaptainHasIt;

	[Tooltip("If set to true, this status will prevent\nthe character from going on an expedition")]
	[SerializeField]
	private bool _allowsGoingOnExpedition = true;

	[Tooltip("If set to true, this status will prevent\nthe character from going on an expedition")]
	[SerializeField]
	private bool _preventsReturnFromExpedition;

	[Tooltip("If set to true, a character with this status\nwill not be selectable in select character choices.")]
	[SerializeField]
	private bool _allowsSelectionInEventChoice = true;

	[Tooltip("If set to false, the player will not be able\nto give the character any remedies (soup, sock, medkit...)\nif it has this status.")]
	[SerializeField]
	private bool _allowsRationing = true;

	[Tooltip("If a character has this status, the player\nwon't be able to ration these remedies to them.")]
	[SerializeField]
	private IItem[] _disallowedRemedies = new IItem[0];

	[Tooltip("If a character has this status\nthis remedies will be available to ration.")]
	[SerializeField]
	private IItem[] _remediesRequiringStatus = new IItem[0];

	[Tooltip("If set to true, the daily changes of all needs\nwill be ignored for a character with this status.\nThis does not affect changes mades by OnPersists events\ndefined for other statuses!")]
	[SerializeField]
	private bool _ignoresDailyNeedChanges;

	[Tooltip("List of needs that will have their daily changes ignored\nif a character has this status.\nThis does not affect changes mades by OnPersists events\ndefined for other statuses!")]
	[SerializeField]
	private CharacterNeed[] _needsIgnoringDailyChanges = new CharacterNeed[0];

	[Tooltip("Needs that will have all changes ignored")]
	[SerializeField]
	private bool _ignoresAllNeedChanges;

	[Tooltip("List of needs that cant be changedif a character has this status.")]
	[SerializeField]
	private CharacterNeed[] _needsIgnoringAllChanges = new CharacterNeed[0];

	[SerializeField]
	private bool _isVisibleInUI;

	[SerializeField]
	private bool _isPositive;

	[SerializeField]
	private bool _isNegative;

	[SerializeField]
	private bool _isDead;

	public int Priority => _priority;

	public bool EndsGameIfCaptainHasIt => _endsGameIfCaptainHasIt;

	public bool AllowsGoingOnExpedition => _allowsGoingOnExpedition;

	public bool AllowsSelectionInEventChoice => _allowsSelectionInEventChoice;

	public bool IgnoresDailyNeedChanges => _ignoresDailyNeedChanges;

	public bool IgnoresAllNeedChanges => _ignoresAllNeedChanges;

	public bool AllowsDrawingInShip => _allowsDrawingInShip;

	public bool AllowsRationing => _allowsRationing;

	public IItem[] DisallowedRemedies => _disallowedRemedies;

	public CharacterNeed[] NeedsIgnoringDailyChanges => _needsIgnoringDailyChanges;

	public CharacterNeed[] NeedsIgnoringAllChanges => _needsIgnoringAllChanges;

	public bool PreventsReturnFromExpedition => _preventsReturnFromExpedition;

	public bool IsVisibleInUI
	{
		get
		{
			return _isVisibleInUI;
		}
		set
		{
			_isVisibleInUI = value;
		}
	}

	public bool IsPositive => _isPositive;

	public bool IsNegative => _isNegative;

	public bool IsDead => _isDead;

	public IItem[] RemediesRequiringStatus => _remediesRequiringStatus;
}
