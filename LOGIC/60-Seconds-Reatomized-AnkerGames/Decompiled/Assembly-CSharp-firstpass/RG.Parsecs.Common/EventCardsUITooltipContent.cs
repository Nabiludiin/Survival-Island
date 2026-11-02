using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class EventCardsUITooltipContent : TooltipContent
{
	[Tooltip("Current card controller.")]
	[SerializeField]
	private ChoiceCardController _choice;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	public ChoiceCardController Choice => _choice;

	public TooltipTerm EventCardsUITooltipTerm => _additionalTooltipContent;

	public override bool IsValid()
	{
		return _choice != null;
	}
}
