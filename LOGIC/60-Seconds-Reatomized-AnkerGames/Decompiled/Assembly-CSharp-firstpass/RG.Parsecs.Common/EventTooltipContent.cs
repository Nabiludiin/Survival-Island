using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class EventTooltipContent : TooltipContent
{
	[Tooltip("Reference to survival data scriptable object.")]
	[SerializeField]
	private SurvivalData _data;

	[Tooltip("Term containing phrase - Event.")]
	[SerializeField]
	private LocalizedString _eventName;

	[Tooltip("Term containing phrase - Decision made.")]
	[SerializeField]
	private LocalizedString _decisionMade;

	[Tooltip("Term containing phrase - Need decision.")]
	[SerializeField]
	private LocalizedString _decisionAwaits;

	[Tooltip("Term containing phrase - Status.")]
	[SerializeField]
	private LocalizedString _statusName;

	[Tooltip("Term containing phrase - Open starlog.")]
	[SerializeField]
	private LocalizedString _openStarlog;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipTerm;

	public LocalizedString EventName => _eventName;

	public LocalizedString StatusName => _statusName;

	public LocalizedString DecisionMade => _decisionMade;

	public LocalizedString DecisionAwaits => _decisionAwaits;

	public SurvivalData Data => _data;

	public TooltipTerm EventTooltipTerm => _additionalTooltipTerm;

	public LocalizedString OpenStarlog => _openStarlog;

	public override bool IsValid()
	{
		if (_data != null && (string)_eventName != null)
		{
			return !_eventName.IsNullOrEmpty();
		}
		return false;
	}
}
