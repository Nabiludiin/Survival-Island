using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class ExpeditionItemUITooltipContent : TooltipContent
{
	[Tooltip("Current item controller.")]
	[SerializeField]
	private ItemSlotController _itemSlot;

	[SerializeField]
	private ItemList _itemList;

	[Tooltip("Term containing phrase - No item chosen.")]
	[SerializeField]
	private LocalizedString _noItemName;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	public ItemSlotController ItemSlot => _itemSlot;

	public ItemList ItemList => _itemList;

	public LocalizedString NoItem => _noItemName;

	public TooltipTerm ExpeditionItemUITooltipTerm => _additionalTooltipContent;

	public override bool IsValid()
	{
		if (_itemSlot != null && _itemSlot != null && (string)_noItemName != null)
		{
			return !_noItemName.IsNullOrEmpty();
		}
		return false;
	}
}
