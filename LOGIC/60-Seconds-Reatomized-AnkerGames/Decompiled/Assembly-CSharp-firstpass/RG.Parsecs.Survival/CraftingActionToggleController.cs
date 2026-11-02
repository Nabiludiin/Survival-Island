using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class CraftingActionToggleController : MonoBehaviour
{
	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private EPlannedCraftingAction _craftingAction;

	[SerializeField]
	private CraftingDecisionPanelController _craftingDecisionPanelController;

	[SerializeField]
	private TextMeshProUGUI _itemsCountText;

	[SerializeField]
	private ItemList _itemList;

	[SerializeField]
	private NavigationOverrideToggle _toggle;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TextMeshProUGUI _titleText;

	[SerializeField]
	private Image _availableImage;

	[SerializeField]
	private TextMeshProUGUI _availableText;

	[SerializeField]
	private ShipSystem[] _shipSystems;

	[SerializeField]
	private ExpeditionData _expeditionData;

	[SerializeField]
	private PlayerResources _playerResources;

	[SerializeField]
	private SuitData _suitData;

	private int _currentVisibleItemsCount;

	public bool IsOn
	{
		get
		{
			return _toggle.isOn;
		}
		set
		{
			_toggle.isOn = value;
		}
	}

	public bool Togglable
	{
		get
		{
			return _toggle.Togglable;
		}
		set
		{
			_toggle.Togglable = value;
		}
	}

	public bool Interactable
	{
		get
		{
			return _toggle.interactable;
		}
		set
		{
			_toggle.interactable = value;
		}
	}

	public void Show()
	{
		SetItemsCountText();
		Togglable = _currentVisibleItemsCount > 0;
		Interactable = true;
		SetDisabled();
	}

	private void SetDisabled()
	{
		_icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, Togglable ? 1f : 0.5f);
		_toggle.image.color = new Color(_toggle.image.color.r, _toggle.image.color.g, _toggle.image.color.b, Togglable ? 1f : 0.5f);
		_availableImage.color = new Color(_availableImage.color.r, _availableImage.color.g, _availableImage.color.b, Togglable ? 1f : 0.5f);
		_availableText.color = new Color(_availableText.color.r, _availableText.color.g, _availableText.color.b, Togglable ? 1f : 0.5f);
		_titleText.color = new Color(_titleText.color.r, _titleText.color.g, _titleText.color.b, Togglable ? 1f : 0.5f);
	}

	private void SetItemsCountText()
	{
		_currentVisibleItemsCount = 0;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < _itemList.Items.Count; i++)
		{
			IItem item = _itemList.Items[i];
			switch (_craftingAction)
			{
			case EPlannedCraftingAction.CRAFT:
			{
				CraftingOperation createCost = item.GetCreateCost();
				if (createCost.RequiredSystemLevel > _craftingData.SystemLevel)
				{
					break;
				}
				ConsumableRemedium consumableRemedium = item as ConsumableRemedium;
				if ((!item.BaseRuntimeData.IsAvailable && !item.BaseRuntimeData.IsOnExpedition) || (consumableRemedium != null && consumableRemedium.RuntimeData.Amount <= consumableRemedium.StaticData.MaxAmount - 1f))
				{
					if (_playerResources.CanLockForCraftingUI(createCost.Resources))
					{
						num++;
					}
					num2++;
				}
				break;
			}
			case EPlannedCraftingAction.RECYCLE:
			{
				CraftingOperation recyclingCostForCurrentLevel = item.GetRecyclingCostForCurrentLevel();
				if (item.BaseRuntimeData.IsAvailable && recyclingCostForCurrentLevel.RequiredSystemLevel <= _craftingData.SystemLevel && item.IsLockable())
				{
					num++;
					num2++;
				}
				break;
			}
			case EPlannedCraftingAction.REPAIR:
			{
				Item item2 = item as Item;
				if (!(item2 != null))
				{
					break;
				}
				CraftingOperation repairCostsForCurrentLevel = item2.GetRepairCostsForCurrentLevel();
				if (repairCostsForCurrentLevel.RequiredSystemLevel <= _craftingData.SystemLevel && item2.RuntimeData.IsDamaged && item.BaseRuntimeData.IsAvailable && item.IsLockable())
				{
					if (_playerResources.CanLockForCraftingUI(repairCostsForCurrentLevel.Resources))
					{
						num++;
					}
					num2++;
				}
				break;
			}
			case EPlannedCraftingAction.UPGRADE:
			{
				if (!item.IsUpgradable() || !item.IsLockable())
				{
					break;
				}
				CraftingOperation upgradeCostForCurrentLevel = item.GetUpgradeCostForCurrentLevel();
				if (upgradeCostForCurrentLevel.RequiredSystemLevel <= _craftingData.SystemLevel)
				{
					if (_playerResources.CanLockForCraftingUI(upgradeCostForCurrentLevel.Resources))
					{
						num++;
					}
					num2++;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		if (_shipSystems != null && _craftingAction == EPlannedCraftingAction.UPGRADE)
		{
			for (int j = 0; j < _shipSystems.Length; j++)
			{
				if ((!_shipSystems[j].AvailableInCraftingDuringExpedition && (_expeditionData.RuntimeData.IsOngoing || _expeditionData.RuntimeData.IsPlanned)) || !_shipSystems[j].IsUpgradable || !_shipSystems[j].IsAvailable || ((_shipSystems[j] == _suitData || _shipSystems[j] == _expeditionData) && (_expeditionData.RuntimeData.IsOngoing || _expeditionData.RuntimeData.IsPlanned || _expeditionData.RuntimeData.CurrentPlanetDeck == null)))
				{
					continue;
				}
				CraftingOperation upgradeCostForCurrentLevel2 = _shipSystems[j].GetUpgradeCostForCurrentLevel();
				if (upgradeCostForCurrentLevel2.RequiredSystemLevel <= _craftingData.SystemLevel)
				{
					if (_playerResources.CanLockForCraftingUI(upgradeCostForCurrentLevel2.Resources))
					{
						num++;
					}
					num2++;
				}
			}
		}
		_itemsCountText.text = $"{num}";
		_currentVisibleItemsCount = num2;
	}

	public void SetPlannedCraftingAction(bool toggleValue)
	{
		if (toggleValue)
		{
			_craftingDecisionPanelController.SetPlannedCraftingAction(_craftingAction);
		}
		else
		{
			_craftingDecisionPanelController.SetPlannedCraftingAction(EPlannedCraftingAction.NONE);
		}
	}
}
