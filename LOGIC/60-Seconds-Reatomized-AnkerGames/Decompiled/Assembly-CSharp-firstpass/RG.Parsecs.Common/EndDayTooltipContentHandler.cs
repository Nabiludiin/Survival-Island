using System;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class EndDayTooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	private string _endDayStatus;

	private string _termString;

	public override void HandleContent(TooltipContent content)
	{
		if (content is EndDayTooltipContent)
		{
			HandleItemTooltipContent((EndDayTooltipContent)content);
		}
	}

	private void HandleItemTooltipContent(EndDayTooltipContent content)
	{
		if (!content.Data.DailyEventResolved)
		{
			_endDayStatus = $"{content.StatusName}: <color=red>{content.NotReady}</color=red>";
		}
		else
		{
			_endDayStatus = $"{content.StatusName}: <color=green>{content.Ready}</color=green>";
		}
		if (string.IsNullOrEmpty(content.EndDayTooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.EndDayTooltipTerm.Term;
		}
		_text.text = $"{content.EndDayName}{Environment.NewLine}{_endDayStatus}{Environment.NewLine}{_termString}";
	}
}
