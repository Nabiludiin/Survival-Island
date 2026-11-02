using System;
using System.Collections.Generic;
using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.Serialization;

namespace RG.Parsecs.Common;

public class ExpeditionCharacterUITooltipContent : TooltipContent
{
	[Serializable]
	public class StatusTermPair
	{
		[SerializeField]
		private CharacterStatus _status;

		[SerializeField]
		private LocalizedString _term;

		public CharacterStatus Status => _status;

		public LocalizedString Term => _term;
	}

	[Tooltip("Current item controller.")]
	[SerializeField]
	private ExpeditionCharacterSlotController _characterSlot;

	[FormerlySerializedAs("_additionalTooltipContent")]
	[Tooltip("Regular tooltip term.")]
	[SerializeField]
	private LocalizedString _regularTooltipContent;

	[Tooltip("Dead tooltip term.")]
	[SerializeField]
	private LocalizedString _deadTerm;

	[SerializeField]
	private List<StatusTermPair> _termPairs = new List<StatusTermPair>();

	public ExpeditionCharacterSlotController CharacterSlot => _characterSlot;

	public LocalizedString RegularTooltipTerm => _regularTooltipContent;

	public LocalizedString DeadTooltipTerm => _deadTerm;

	public List<StatusTermPair> TermPairs => _termPairs;

	public override bool IsValid()
	{
		return _characterSlot != null;
	}
}
