using System.Collections.Generic;
using I2.Loc;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

public class CraftingItemUITooltipContentHandler : TooltipContentHandler
{
	[SerializeField]
	private TextMeshProUGUI _itemNameField;

	[SerializeField]
	private CraftingSummaryController.SummarySlotController[] _resources;

	[SerializeField]
	private LengthsRanges _lengthsRanges;

	[SerializeField]
	private Transform _timeHolder;

	[SerializeField]
	private GameObject _timeIconPrefab;

	[SerializeField]
	private Sprite _timeIcon;

	[SerializeField]
	private PlayerResources _playerResources;

	[SerializeField]
	private ExpeditionData _expeditionData;

	[SerializeField]
	private LocalizedString _operationTimeTerm;

	[SerializeField]
	private LocalizedString _onExpeditionTerm;

	private List<Image> _currentTimesIcons = new List<Image>();

	public override void HandleContent(TooltipContent content)
	{
		CraftingItemUITooltipContent craftingItemUITooltipContent = content as CraftingItemUITooltipContent;
		if (craftingItemUITooltipContent == null)
		{
			Debug.LogErrorFormat("Given content {0} is not a CraftingItemUITooltipContent in {1}", content, name);
			return;
		}
		CraftingOperation operationCost = craftingItemUITooltipContent.GetOperationCost();
		if (operationCost == null)
		{
			Debug.LogError("We're trying to set crafting operation information but craftingOperation is null.");
			return;
		}
		string arg = ((_expeditionData.RuntimeData.IsOngoing && craftingItemUITooltipContent.DisplayOnExpeditonText) ? _onExpeditionTerm.ToString() : "");
		_itemNameField.text = $"{craftingItemUITooltipContent.Name} {arg}";
		SetIcons(_currentTimesIcons, _timeHolder, _timeIcon, _lengthsRanges.GetAmountOfLengthsIconsForRange(operationCost.MinDays, operationCost.MaxDays));
		for (int i = 0; i < _resources.Length; i++)
		{
			if (i < operationCost.Resources.Count)
			{
				_resources[i].SetActive(active: true);
				if (craftingItemUITooltipContent.CraftingAction == EPlannedCraftingAction.RECYCLE)
				{
					if (operationCost.Resources[i].Amount != 0)
					{
						_resources[i].SetActive(active: true);
						_resources[i].SetTextAndIcon(operationCost.Resources[i].Resource.Icon, $"+{operationCost.Resources[i].Amount}");
					}
					else
					{
						_resources[i].SetActive(active: false);
					}
					continue;
				}
				string arg2 = (_playerResources.CanLockForCraftingUI(operationCost.Resources[i]) ? "green" : "red");
				int num = Mathf.Clamp(_playerResources.GetResourceValueForCraftingUI(operationCost.Resources[i].Resource), 0, operationCost.Resources[i].Amount);
				if (operationCost.Resources[i].Amount != 0)
				{
					_resources[i].SetActive(active: true);
					_resources[i].SetTextAndIcon(operationCost.Resources[i].Resource.Icon, string.Format("<color={2}>{0}/{1}</color={2}>", num, operationCost.Resources[i].Amount, arg2));
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
				image = Object.Instantiate(_timeIconPrefab, parent, worldPositionStays: false).GetComponent<Image>();
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
}
