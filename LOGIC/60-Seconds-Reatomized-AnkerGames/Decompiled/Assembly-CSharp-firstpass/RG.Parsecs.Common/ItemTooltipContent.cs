using System.Collections.Generic;
using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class ItemTooltipContent : TooltipContent
{
	[Tooltip("Term containing phrase - Damaged.")]
	[SerializeField]
	private LocalizedString _damagedItemName;

	[Tooltip("List of names depending on Item system level.")]
	[SerializeField]
	private List<LocalizedString> _itemNamesWithLevel;

	[Tooltip("Reference to item data scriptable object.")]
	[SerializeField]
	private IItem _item;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	public IItem Item => _item;

	public TooltipTerm TooltipTerm => _additionalTooltipContent;

	public List<LocalizedString> ItemNamesWithLevel => _itemNamesWithLevel;

	public LocalizedString DamagedItemName => _damagedItemName;

	public override bool IsValid()
	{
		if (_itemNamesWithLevel == null)
		{
			return false;
		}
		for (int i = 0; i < _itemNamesWithLevel.Count; i++)
		{
			if ((string)_itemNamesWithLevel[i] == null || _itemNamesWithLevel[i].IsNullOrEmpty())
			{
				return false;
			}
		}
		return true;
	}

	private void OnValidate()
	{
		if (_item.BaseStaticData.MaxLevel != _itemNamesWithLevel.Count)
		{
			Debug.LogWarning("Inappropriate number of set term names. " + name);
		}
	}
}
