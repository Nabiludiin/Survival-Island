using I2.Loc;
using UnityEngine;

namespace RG.Parsecs.Common;

public class EventButtonUITooltipContent : TooltipContent
{
	[Tooltip("Button UI term - Active")]
	[SerializeField]
	private LocalizedString _eventActiveButtonTerm;

	[Tooltip("Button UI term - Inactive")]
	[SerializeField]
	private LocalizedString _eventInactiveButtonTerm;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	public LocalizedString EventActiveButtonTerm => _eventActiveButtonTerm;

	public LocalizedString EventInactiveButtonTerm => _eventInactiveButtonTerm;

	public TooltipTerm EventButtonUITooltipTerm => _additionalTooltipContent;

	public override bool IsValid()
	{
		if ((string)_eventActiveButtonTerm != null && !_eventActiveButtonTerm.IsNullOrEmpty())
		{
			return ((string)_eventInactiveButtonTerm != null) & !_eventInactiveButtonTerm.IsNullOrEmpty();
		}
		return false;
	}
}
