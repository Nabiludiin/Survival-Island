using System;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class EndDayUITooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	private string _termString;

	public override void HandleContent(TooltipContent content)
	{
		if ((bool)(content as TextTooltipContent))
		{
			HandleItemTooltipContent((TextTooltipContent)content);
		}
	}

	private void HandleItemTooltipContent(TextTooltipContent content)
	{
		if (string.IsNullOrEmpty(content.TTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.TTerm.Term;
		}
		_text.text = $"{content.TextTerm}{Environment.NewLine}{_termString}";
	}
}
