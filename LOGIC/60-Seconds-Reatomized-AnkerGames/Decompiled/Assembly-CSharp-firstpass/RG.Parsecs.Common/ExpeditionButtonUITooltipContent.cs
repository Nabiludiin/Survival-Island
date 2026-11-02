using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class ExpeditionButtonUITooltipContent : TooltipContent
{
	[SerializeField]
	private RepairedExpeditionPanelController _repairedExpeditionPanel;

	[Tooltip("Term containing phrase - Choose destination to proceed.")]
	[SerializeField]
	private LocalizedString _destinationInactiveConfirmButtonTerm;

	[Tooltip("Term containing phrase - Choose character to proceed.")]
	[SerializeField]
	private LocalizedString _characterInactiveConfirmButtonTerm;

	[Tooltip("Term containing phrase - Send expedition.")]
	[SerializeField]
	private LocalizedString _sendExpeditionConfirmButtonTerm;

	[Tooltip("Term containing phrase - Inactive")]
	[SerializeField]
	private LocalizedString _lockedInactiveConfirmButtonTerm;

	[Tooltip("Term containing phrase - Active")]
	[SerializeField]
	private LocalizedString _activeConfirmButtonTerm;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	public RepairedExpeditionPanelController ExpeditionPanel => _repairedExpeditionPanel;

	public LocalizedString ChooseDestiantion => _destinationInactiveConfirmButtonTerm;

	public LocalizedString ChooseCharacter => _characterInactiveConfirmButtonTerm;

	public LocalizedString SendExpedition => _sendExpeditionConfirmButtonTerm;

	public LocalizedString Locked => _lockedInactiveConfirmButtonTerm;

	public LocalizedString ActiveConfirmButton => _activeConfirmButtonTerm;

	public TooltipTerm ExpeditionButtonUITooltipTerm => _additionalTooltipContent;

	public override bool IsValid()
	{
		if ((string)_destinationInactiveConfirmButtonTerm != null && !_destinationInactiveConfirmButtonTerm.IsNullOrEmpty() && (string)_characterInactiveConfirmButtonTerm != null && !_characterInactiveConfirmButtonTerm.IsNullOrEmpty() && (string)_activeConfirmButtonTerm != null && !_activeConfirmButtonTerm.IsNullOrEmpty() && (string)_lockedInactiveConfirmButtonTerm != null)
		{
			return !_lockedInactiveConfirmButtonTerm.IsNullOrEmpty();
		}
		return false;
	}
}
