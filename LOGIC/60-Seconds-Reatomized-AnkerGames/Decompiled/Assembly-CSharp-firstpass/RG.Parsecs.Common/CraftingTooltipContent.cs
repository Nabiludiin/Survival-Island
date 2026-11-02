using System.Collections.Generic;
using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class CraftingTooltipContent : TooltipContent
{
	[Tooltip("Reference to crafting data scriptable object.")]
	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private ItemList _itemList;

	[Tooltip("List of names depending on Crafting system level.")]
	[SerializeField]
	private List<LocalizedString> _craftingNamesWithLevel;

	[Tooltip("Term containing phrase - Status.")]
	[SerializeField]
	private LocalizedString _statusName;

	[Tooltip("Term containing phrase - Craftable Items.")]
	[SerializeField]
	private LocalizedString _craftableItemsTerm;

	[Tooltip("Term containing phrase - Available items to craft.")]
	[SerializeField]
	private LocalizedString _upgradeableItemsTerm;

	[Tooltip("Term containing phrase - Available items to craft.")]
	[SerializeField]
	private LocalizedString _repairableItemsTerm;

	[Tooltip("Term containing phrase - Available items to craft.")]
	[SerializeField]
	private LocalizedString _recyclableItemsTerm;

	[Tooltip("Term containing phrase - Damaged.")]
	[SerializeField]
	private LocalizedString _damagedStatus;

	[Tooltip("Term containing phrase - Busy.")]
	[SerializeField]
	private LocalizedString _busyStatus;

	[Tooltip("Term containing phrase - Unavailable.")]
	[SerializeField]
	private LocalizedString _unavailableStatus;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	public CraftingData CraftingSystem => _craftingData;

	public List<LocalizedString> CraftingNamesWithLevel => _craftingNamesWithLevel;

	public LocalizedString Status => _statusName;

	public LocalizedString Damaged => _damagedStatus;

	public LocalizedString RecyclableItemsTerm => _recyclableItemsTerm;

	public LocalizedString Busy => _busyStatus;

	public LocalizedString Unavailable => _unavailableStatus;

	public ItemList ItemList => _itemList;

	public TooltipTerm CraftingTooltipTerm => _additionalTooltipContent;

	public LocalizedString CraftableItemsTerm => _craftableItemsTerm;

	public LocalizedString UpgradeableItemsTerm => _upgradeableItemsTerm;

	public LocalizedString RepairableItemsTerm => _repairableItemsTerm;

	public override bool IsValid()
	{
		return true;
	}

	private void OnValidate()
	{
		if (_craftingData.StaticData.MaxLevel != _craftingNamesWithLevel.Count)
		{
			Debug.LogWarning("Inappropriate number of set term names. " + name);
		}
	}
}
