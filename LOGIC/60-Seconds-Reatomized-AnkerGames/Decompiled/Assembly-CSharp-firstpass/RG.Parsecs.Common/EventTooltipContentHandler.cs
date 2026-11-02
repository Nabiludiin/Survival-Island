using System;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class EventTooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	private string _eventStatus;

	private string _termString;

	public override void HandleContent(TooltipContent content)
	{
		if ((bool)(content as EventTooltipContent))
		{
			HandleItemTooltipContent((EventTooltipContent)content);
		}
	}

	private void HandleItemTooltipContent(EventTooltipContent content)
	{
		if (!content.Data.StarlogOpenedOnFirstDay)
		{
			_eventStatus = $"{content.StatusName}: <color=red>{content.OpenStarlog}</color=red>";
		}
		else if (!content.Data.DailyEventResolved)
		{
			_eventStatus = $"{content.StatusName}: <color=red>{content.DecisionAwaits}</color=red>";
		}
		else
		{
			_eventStatus = $"{content.StatusName}: <color=green>{content.DecisionMade}</color=green>";
		}
		if (string.IsNullOrEmpty(content.EventTooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.EventTooltipTerm.Term;
		}
		_text.text = $"{content.EventName}{Environment.NewLine}{_eventStatus}{Environment.NewLine}{_termString}";
	}
}
