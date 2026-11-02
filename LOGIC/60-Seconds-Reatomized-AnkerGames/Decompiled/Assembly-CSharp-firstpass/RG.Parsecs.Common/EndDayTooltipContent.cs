using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class EndDayTooltipContent : TooltipContent
{
	[Tooltip("Reference to survival data scriptable object.")]
	[SerializeField]
	private SurvivalData _data;

	[Tooltip("Term containing phrase - End of day.")]
	[SerializeField]
	private LocalizedString _endDayName;

	[Tooltip("Term containing phrase - Status.")]
	[SerializeField]
	private LocalizedString _statusName;

	[Tooltip("Term containing phrase - Ready.")]
	[SerializeField]
	private LocalizedString _statusReady;

	[Tooltip("Term containing phrase - Not ready.")]
	[SerializeField]
	private LocalizedString _statusNotReady;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipTerm;

	public LocalizedString EndDayName => _endDayName;

	public LocalizedString StatusName => _statusName;

	public LocalizedString Ready => _statusReady;

	public LocalizedString NotReady => _statusNotReady;

	public SurvivalData Data => _data;

	public TooltipTerm EndDayTooltipTerm => _additionalTooltipTerm;

	public override bool IsValid()
	{
		if (_data != null && (string)_endDayName != null)
		{
			return !_endDayName.IsNullOrEmpty();
		}
		return false;
	}
}
