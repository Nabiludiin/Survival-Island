using I2.Loc;
using UnityEngine;

namespace RG.Parsecs.Common;

public class UIButtonTooltipContent : TooltipContent
{
	[Tooltip("Button UI term - Active")]
	[SerializeField]
	private LocalizedString _activeButtonTerm;

	[Tooltip("Button UI term - Inactive")]
	[SerializeField]
	private LocalizedString _inactiveButtonTerm;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	public LocalizedString ActiveButtonTerm => _activeButtonTerm;

	public LocalizedString InactiveButtonTerm => _inactiveButtonTerm;

	public TooltipTerm ButtonUITooltipTerm => _additionalTooltipContent;

	public override bool IsValid()
	{
		if ((string)_activeButtonTerm != null && !_activeButtonTerm.IsNullOrEmpty() && (string)_inactiveButtonTerm != null)
		{
			return !_inactiveButtonTerm.IsNullOrEmpty();
		}
		return false;
	}
}
