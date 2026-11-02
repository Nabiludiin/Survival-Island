using I2.Loc;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class VisualsTooltipContentHandler : TooltipContentHandler
{
	[SerializeField]
	private TextMeshProUGUI _text;

	public override void HandleContent(TooltipContent content)
	{
		if (content is VisualsTooltipContent)
		{
			HandleVisitorTooltipContent((VisualsTooltipContent)content);
		}
	}

	private void HandleVisitorTooltipContent(VisualsTooltipContent content)
	{
		if (_text != null)
		{
			if (content.TooltipTerm == null)
			{
				_text.text = string.Empty;
				return;
			}
			_text.text = content.TooltipTerm.Term;
			_text.gameObject.GetComponent<Localize>().OnLocalize();
		}
	}
}
