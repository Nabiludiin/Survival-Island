using RG.Parsecs.Common;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class TextTooltipContentHandler : TooltipContentHandler
{
	[SerializeField]
	private TextMeshProUGUI _text;

	public override void HandleContent(TooltipContent content)
	{
		TextTooltipContent textTooltipContent = content as TextTooltipContent;
		if (textTooltipContent != null)
		{
			_text.text = textTooltipContent.TextTerm;
		}
	}
}
