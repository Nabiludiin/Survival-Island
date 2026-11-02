using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class CraftingDisabledPanelController : MonoBehaviour
{
	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private Image _itemIcon;

	[SerializeField]
	private TextMeshProUGUI _timeLeft;

	[SerializeField]
	private TextMeshProUGUI _action;

	[SerializeField]
	private TextMeshProUGUI _title;

	[SerializeField]
	private TextMeshProUGUI _description;

	[SerializeField]
	private GameObject _craftingDisabledOngoing;

	[SerializeField]
	private GameObject _craftingDisabledDescription;

	[SerializeField]
	private LocalizedString _currentlyCraftedTerm;

	[SerializeField]
	private LocalizedString _currentlyRecycledTerm;

	[SerializeField]
	private LocalizedString _currentlyRepairedTerm;

	[SerializeField]
	private LocalizedString _currentlyUpgradedTerm;

	public void Show()
	{
		if (_craftingData.RuntimeData.IsOngoing)
		{
			_craftingDisabledOngoing.SetActive(value: true);
			_craftingDisabledDescription.SetActive(value: false);
			if (_craftingData.RuntimeData.OngoingCraftingActionData.Item != null)
			{
				_itemIcon.sprite = _craftingData.RuntimeData.OngoingCraftingActionData.Item.BaseStaticData.IconJournal;
			}
			else if (_craftingData.RuntimeData.OngoingCraftingActionData.System != null)
			{
				_itemIcon.sprite = _craftingData.RuntimeData.OngoingCraftingActionData.System.Icon;
			}
			_timeLeft.text = (_craftingData.RuntimeData.CraftingTotalTime - _craftingData.RuntimeData.CraftingCurrentTime).ToString();
			switch (_craftingData.RuntimeData.OngoingCraftingActionData.PlannedCraftingAction)
			{
			case EPlannedCraftingAction.CRAFT:
				_action.text = _currentlyCraftedTerm;
				break;
			case EPlannedCraftingAction.RECYCLE:
				_action.text = _currentlyRecycledTerm;
				break;
			case EPlannedCraftingAction.REPAIR:
				_action.text = _currentlyRepairedTerm;
				break;
			case EPlannedCraftingAction.UPGRADE:
				_action.text = _currentlyUpgradedTerm;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		else
		{
			_craftingDisabledOngoing.SetActive(value: false);
			_craftingDisabledDescription.SetActive(value: true);
			if (string.IsNullOrEmpty(_craftingData.RuntimeData.BlockedPanelTitleTerm.mTerm))
			{
				_title.text = _craftingData.StaticData.BlockedPanelDefaultTitleTerm;
			}
			else
			{
				_title.text = _craftingData.RuntimeData.BlockedPanelTitleTerm;
			}
			if (string.IsNullOrEmpty(_craftingData.RuntimeData.BlockedPanelDescriptionTerm.mTerm))
			{
				_description.text = _craftingData.StaticData.BlockedPanelDefaultDescriptionTerm;
			}
			else
			{
				_description.text = _craftingData.RuntimeData.BlockedPanelDescriptionTerm;
			}
		}
	}
}
