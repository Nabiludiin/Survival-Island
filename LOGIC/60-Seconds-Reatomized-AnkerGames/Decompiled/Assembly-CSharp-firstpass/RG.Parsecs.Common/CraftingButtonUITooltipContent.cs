using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class CraftingButtonUITooltipContent : TooltipContent
{
	[Tooltip("Reference to crafting data scriptable object.")]
	[SerializeField]
	private CraftingData _craftingData;

	[Tooltip("Term containing phrase - Active")]
	[SerializeField]
	private LocalizedString _craftingActiveButtonTerm;

	[Tooltip("Term containing phrase - Inactive")]
	[SerializeField]
	private LocalizedString _craftingInactiveButtonTerm;

	[Tooltip("Term containing phrase - Busy")]
	[SerializeField]
	private LocalizedString _craftingBusyButtonTerm;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	public CraftingData Data => _craftingData;

	public LocalizedString CraftingActiveButtonTerm => _craftingActiveButtonTerm;

	public LocalizedString CraftingInactiveButtonTerm => _craftingInactiveButtonTerm;

	public LocalizedString CraftingBusyButtonTerm => _craftingBusyButtonTerm;

	public TooltipTerm CraftingButtonUITooltipTerm => _additionalTooltipContent;

	public override bool IsValid()
	{
		if (_craftingData != null && (string)_craftingActiveButtonTerm != null && !_craftingActiveButtonTerm.IsNullOrEmpty() && (string)_craftingInactiveButtonTerm != null && !_craftingInactiveButtonTerm.IsNullOrEmpty() && (string)_craftingBusyButtonTerm != null)
		{
			return !_craftingBusyButtonTerm.IsNullOrEmpty();
		}
		return false;
	}
}
