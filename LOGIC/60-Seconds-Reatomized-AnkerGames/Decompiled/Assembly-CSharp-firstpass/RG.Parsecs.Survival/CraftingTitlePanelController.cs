using System;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class CraftingTitlePanelController : MonoBehaviour
{
	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private GameObject _backButton;

	[SerializeField]
	private TextMeshProUGUI _titleText;

	[SerializeField]
	private LocalizedString _whatToDoTerm;

	[SerializeField]
	private LocalizedString _craftModeTerm;

	[SerializeField]
	private LocalizedString _recycleModeTerm;

	[SerializeField]
	private LocalizedString _repairModeDoTerm;

	[SerializeField]
	private LocalizedString _upgradeModeTerm;

	public void Show()
	{
		switch (_craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction)
		{
		case EPlannedCraftingAction.NONE:
			_backButton.SetActive(value: false);
			_titleText.text = _whatToDoTerm;
			break;
		case EPlannedCraftingAction.CRAFT:
			_backButton.SetActive(value: true);
			_titleText.text = _craftModeTerm;
			break;
		case EPlannedCraftingAction.RECYCLE:
			_backButton.SetActive(value: true);
			_titleText.text = _recycleModeTerm;
			break;
		case EPlannedCraftingAction.REPAIR:
			_backButton.SetActive(value: true);
			_titleText.text = _repairModeDoTerm;
			break;
		case EPlannedCraftingAction.UPGRADE:
			_backButton.SetActive(value: true);
			_titleText.text = _upgradeModeTerm;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
