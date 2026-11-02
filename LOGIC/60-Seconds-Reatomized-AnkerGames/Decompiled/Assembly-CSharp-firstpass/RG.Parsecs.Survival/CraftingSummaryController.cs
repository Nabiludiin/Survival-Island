using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class CraftingSummaryController : MonoBehaviour
{
	[Serializable]
	public class SummarySlotController
	{
		[SerializeField]
		private Transform _slotTransform;

		[SerializeField]
		private Image _icon;

		[SerializeField]
		private TextMeshProUGUI _text;

		public void SetText(string text)
		{
			_text.text = text;
		}

		public void SetTextAndIcon(Sprite icon, string text)
		{
			_icon.sprite = icon;
			_text.text = text;
		}

		public void SetActive(bool active)
		{
			_slotTransform.gameObject.SetActive(active);
		}
	}

	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private LengthsRanges _lengthsRanges;

	[SerializeField]
	private PlayerResources _playerResources;

	[SerializeField]
	private Image _itemIcon;

	[SerializeField]
	private Sprite _timeIcon;

	[SerializeField]
	private GameObject _timeIconPrefab;

	[SerializeField]
	private Transform _timeHolder;

	[SerializeField]
	private SummarySlotController[] _resources;

	private List<Image> _currentTimesIcons = new List<Image>();

	public void SetActivationContent()
	{
		SetIcons(_currentTimesIcons, _timeHolder, _timeIcon, _lengthsRanges.GetAmountOfLengthsIconsForRange(1, 1));
		for (int i = 0; i < _resources.Length; i++)
		{
			if (i < _craftingData.StaticData.RequiredResources.Length)
			{
				if (_craftingData.StaticData.RequiredResources[i].Amount > 0)
				{
					_resources[i].SetActive(active: true);
					_resources[i].SetTextAndIcon(_craftingData.StaticData.RequiredResources[i].Resource.Icon, $"{_craftingData.StaticData.RequiredResources[i].Amount}/{_playerResources.GetResourceValue(_craftingData.StaticData.RequiredResources[i].Resource)}");
				}
				else
				{
					_resources[i].SetActive(active: false);
				}
			}
			else
			{
				_resources[i].SetActive(active: false);
			}
		}
	}

	private void SetIcons(List<Image> currentIcons, Transform parent, Sprite newSprite, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Image image;
			if (currentIcons.Count <= i)
			{
				image = UnityEngine.Object.Instantiate(_timeIconPrefab, parent, worldPositionStays: false).GetComponent<Image>();
				currentIcons.Add(image);
			}
			else
			{
				image = currentIcons[i];
				image.gameObject.SetActive(value: true);
			}
			image.sprite = newSprite;
		}
		if (currentIcons.Count > amount)
		{
			for (int j = amount; j < currentIcons.Count; j++)
			{
				currentIcons[j].gameObject.SetActive(value: false);
			}
		}
	}

	public void SetContent(bool temporaryCard = false)
	{
		OngoingCraftingActionData ongoingCraftingActionData = null;
		if (_craftingData.RuntimeData.IsPlanned | temporaryCard)
		{
			ongoingCraftingActionData = _craftingData.RuntimeData.PlannedCraftingActionData;
		}
		else if (_craftingData.RuntimeData.IsOngoing)
		{
			ongoingCraftingActionData = _craftingData.RuntimeData.OngoingCraftingActionData;
		}
		if (ongoingCraftingActionData == null)
		{
			throw new UnityException("Current crafting data is null but SetContent() method was invoked! That's an error,");
		}
		if (ongoingCraftingActionData.Item == null && ongoingCraftingActionData.System == null)
		{
			throw new UnityException("Both System and Item are null so there is nothing to set in SetContent() for summary paper.");
		}
		CraftingOperation craftingOperation = null;
		if (ongoingCraftingActionData.Item != null)
		{
			_itemIcon.sprite = ongoingCraftingActionData.Item.BaseStaticData.IconJournal;
		}
		else if (ongoingCraftingActionData.System != null)
		{
			_itemIcon.sprite = ongoingCraftingActionData.System.Icon;
		}
		switch (ongoingCraftingActionData.PlannedCraftingAction)
		{
		case EPlannedCraftingAction.CRAFT:
			craftingOperation = ongoingCraftingActionData.Item.GetCreateCost();
			break;
		case EPlannedCraftingAction.RECYCLE:
			craftingOperation = ongoingCraftingActionData.Item.GetRecyclingCostForCurrentLevel();
			break;
		case EPlannedCraftingAction.REPAIR:
		{
			Item item = ongoingCraftingActionData.Item as Item;
			if (item != null)
			{
				craftingOperation = item.GetRepairCostsForCurrentLevel();
			}
			break;
		}
		case EPlannedCraftingAction.UPGRADE:
			if (ongoingCraftingActionData.Item != null)
			{
				craftingOperation = ongoingCraftingActionData.Item.GetUpgradeCostForCurrentLevel();
			}
			else if (ongoingCraftingActionData.System != null)
			{
				craftingOperation = ongoingCraftingActionData.System.GetUpgradeCostForCurrentLevel();
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (craftingOperation == null)
		{
			throw new UnityException("We're trying to set crafting operation information but craftingOperation is null.");
		}
		SetIcons(_currentTimesIcons, _timeHolder, _timeIcon, _lengthsRanges.GetAmountOfLengthsIconsForRange(craftingOperation.MinDays, craftingOperation.MaxDays));
		for (int i = 0; i < _resources.Length; i++)
		{
			if (i < craftingOperation.Resources.Count)
			{
				if (craftingOperation.Resources[i] == null)
				{
					throw new UnityException("There is no resource on specified index when setting CraftingSummaryController. Index: " + i);
				}
				if (craftingOperation.Resources[i].Amount <= 0)
				{
					_resources[i].SetActive(active: false);
					continue;
				}
				_resources[i].SetActive(active: true);
				if (ongoingCraftingActionData.PlannedCraftingAction == EPlannedCraftingAction.RECYCLE)
				{
					_resources[i].SetTextAndIcon(craftingOperation.Resources[i].Resource.Icon, $"+ {craftingOperation.Resources[i].Amount} ");
					continue;
				}
				int num = (_craftingData.RuntimeData.IsOngoing ? craftingOperation.Resources[i].Amount : Mathf.Clamp(_playerResources.GetResourceValue(craftingOperation.Resources[i].Resource), 0, craftingOperation.Resources[i].Amount));
				_resources[i].SetTextAndIcon(craftingOperation.Resources[i].Resource.Icon, $"{num}/{craftingOperation.Resources[i].Amount}");
			}
			else
			{
				_resources[i].SetActive(active: false);
			}
		}
	}
}
