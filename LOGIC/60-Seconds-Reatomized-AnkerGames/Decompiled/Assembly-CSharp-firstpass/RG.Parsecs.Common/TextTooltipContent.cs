using I2.Loc;
using UnityEngine;

namespace RG.Parsecs.Common;

public class TextTooltipContent : TooltipContent
{
	[Tooltip("Generic tooltip content.")]
	[SerializeField]
	private LocalizedString _textTerm;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	public LocalizedString TextTerm => _textTerm;

	public TooltipTerm TTerm => _additionalTooltipContent;

	public override bool IsValid()
	{
		if ((string)_textTerm != null)
		{
			return !_textTerm.IsNullOrEmpty();
		}
		return false;
	}
}
