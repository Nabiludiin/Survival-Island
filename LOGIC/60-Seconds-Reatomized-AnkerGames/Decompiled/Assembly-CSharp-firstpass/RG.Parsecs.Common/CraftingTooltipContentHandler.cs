using System;
using System.Text;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class CraftingTooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private PlayerResources _playerResources;

	[SerializeField]
	private ItemList _itemList;

	[SerializeField]
	private ExpeditionData _expeditionData;

	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private ShipSystem[] _shipSystems;

	private string _systemStatusString;

	private string _levelNameString;

	private string _termString;

	[SerializeField]
	private SuitData _suitData;

	public override void HandleContent(TooltipContent content)
	{
		if ((bool)(content as CraftingTooltipContent))
		{
			HandleItemTooltipContent((CraftingTooltipContent)content);
		}
	}

	private void HandleItemTooltipContent(CraftingTooltipContent content)
	{
		int num = 0;
		for (int i = 0; i < content.ItemList.Items.Count; i++)
		{
			IItem item = content.ItemList.Items[i];
			ConsumableRemedium consumableRemedium = item as ConsumableRemedium;
			if (!item.BaseRuntimeData.IsAvailable || (consumableRemedium != null && consumableRemedium.RuntimeData.Amount < consumableRemedium.StaticData.MaxAmount - 1f))
			{
				num++;
			}
		}
		if (content.CraftingSystem.RuntimeData.IsRepaired && content.CraftingSystem.IsAvailable && !content.CraftingSystem.RuntimeData.IsOngoing)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(Environment.NewLine);
			stringBuilder.AppendLine($"{content.CraftableItemsTerm} <color=green>{GetItemsCountText(EPlannedCraftingAction.CRAFT).ToString()}</color=green>");
			stringBuilder.AppendLine($"{content.RecyclableItemsTerm} <color=green>{GetItemsCountText(EPlannedCraftingAction.RECYCLE).ToString()}</color=green>");
			stringBuilder.AppendLine($"{content.UpgradeableItemsTerm} <color=green>{GetItemsCountText(EPlannedCraftingAction.UPGRADE).ToString()}</color=green>");
			stringBuilder.AppendLine($"{content.RepairableItemsTerm} <color=green>{GetItemsCountText(EPlannedCraftingAction.REPAIR).ToString()}</color=green>");
			_systemStatusString = stringBuilder.ToString();
		}
		else if (!content.CraftingSystem.RuntimeData.IsRepaired)
		{
			_systemStatusString = $"{Environment.NewLine}{content.Status}: <color=red>{content.Damaged}</color=red>";
		}
		else if (content.CraftingSystem.RuntimeData.IsOngoing)
		{
			_systemStatusString = $"{Environment.NewLine}{content.Status}: <color=yellow>{content.Busy}</color=yellow>";
		}
		else if (content.CraftingSystem.RuntimeData.IsRepaired && !content.CraftingSystem.IsAvailable)
		{
			_systemStatusString = $"{Environment.NewLine}{content.Status}: <color=yellow>{content.Unavailable}</color=yellow>";
		}
		if (content.CraftingNamesWithLevel != null && content.CraftingSystem.SystemLevel - 1 < content.CraftingNamesWithLevel.Count)
		{
			_levelNameString = content.CraftingNamesWithLevel[content.CraftingSystem.SystemLevel - 1];
		}
		if (string.IsNullOrEmpty(content.CraftingTooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.CraftingTooltipTerm.Term;
		}
		_text.text = $"{_levelNameString}{_systemStatusString}{Environment.NewLine}{_termString}";
	}

	private int GetItemsCountText(EPlannedCraftingAction craftingAction)
	{
		int num = 0;
		for (int i = 0; i < _itemList.Items.Count; i++)
		{
			IItem item = _itemList.Items[i];
			switch (craftingAction)
			{
			case EPlannedCraftingAction.CRAFT:
			{
				CraftingOperation createCost = item.GetCreateCost();
				if (createCost.RequiredSystemLevel <= _craftingData.SystemLevel)
				{
					ConsumableRemedium consumableRemedium = item as ConsumableRemedium;
					if (((!item.BaseRuntimeData.IsAvailable && !item.BaseRuntimeData.IsOnExpedition) || (consumableRemedium != null && consumableRemedium.RuntimeData.Amount <= consumableRemedium.StaticData.MaxAmount - 1f)) && _playerResources.CanLockForCraftingUI(createCost.Resources))
					{
						num++;
					}
				}
				break;
			}
			case EPlannedCraftingAction.RECYCLE:
			{
				CraftingOperation recyclingCostForCurrentLevel = item.GetRecyclingCostForCurrentLevel();
				if (item.BaseRuntimeData.IsAvailable && item.IsLockable() && recyclingCostForCurrentLevel.RequiredSystemLevel <= _craftingData.SystemLevel)
				{
					num++;
				}
				break;
			}
			case EPlannedCraftingAction.REPAIR:
			{
				Item item2 = item as Item;
				if (item2 != null)
				{
					CraftingOperation repairCostsForCurrentLevel = item2.GetRepairCostsForCurrentLevel();
					if (repairCostsForCurrentLevel.RequiredSystemLevel <= _craftingData.SystemLevel && item2.RuntimeData.IsDamaged && _playerResources.CanLockForCraftingUI(repairCostsForCurrentLevel.Resources) && item2.IsLockable() && item.BaseRuntimeData.IsAvailable)
					{
						num++;
					}
				}
				break;
			}
			case EPlannedCraftingAction.UPGRADE:
				if (item.IsUpgradable())
				{
					CraftingOperation upgradeCostForCurrentLevel = item.GetUpgradeCostForCurrentLevel();
					if (upgradeCostForCurrentLevel.RequiredSystemLevel <= _craftingData.SystemLevel && _playerResources.CanLockForCraftingUI(upgradeCostForCurrentLevel.Resources) && item.IsLockable())
					{
						num++;
					}
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		if (_shipSystems != null && craftingAction == EPlannedCraftingAction.UPGRADE)
		{
			for (int j = 0; j < _shipSystems.Length; j++)
			{
				if ((_shipSystems[j].AvailableInCraftingDuringExpedition || (!_expeditionData.RuntimeData.IsOngoing && !_expeditionData.RuntimeData.IsPlanned)) && _shipSystems[j].IsUpgradable && _shipSystems[j].IsAvailable && ((!(_shipSystems[j] == _suitData) && !(_shipSystems[j] == _expeditionData)) || (!_expeditionData.RuntimeData.IsOngoing && !_expeditionData.RuntimeData.IsPlanned && !(_expeditionData.RuntimeData.CurrentPlanetDeck == null))))
				{
					CraftingOperation upgradeCostForCurrentLevel2 = _shipSystems[j].GetUpgradeCostForCurrentLevel();
					if (upgradeCostForCurrentLevel2.RequiredSystemLevel <= _craftingData.SystemLevel && _playerResources.CanLockForCraftingUI(upgradeCostForCurrentLevel2.Resources))
					{
						num++;
					}
				}
			}
		}
		return num;
	}
}
