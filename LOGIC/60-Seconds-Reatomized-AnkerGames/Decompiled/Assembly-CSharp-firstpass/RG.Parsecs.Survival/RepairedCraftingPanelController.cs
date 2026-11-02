using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class RepairedCraftingPanelController : MonoBehaviour, IRefreshInteractable
{
	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private CraftingTitlePanelController _craftingTitlePanelController;

	[SerializeField]
	private CraftingDecisionPanelController _craftingDecisionPanelController;

	[SerializeField]
	private CraftingContentPanelController _craftingContentPanelController;

	[SerializeField]
	private CraftingDisabledPanelController _craftingDisabledPanelController;

	[SerializeField]
	private CraftingSummaryAnimationController _craftingSummaryAnimationController;

	[SerializeField]
	private ResourcesDisplayController _resourcesDisplayController;

	[SerializeField]
	private Button _confirmButton;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private Button _backButton;

	[SerializeField]
	private CraftingView _craftingView;

	[SerializeField]
	private Slider _slider;

	[Tooltip("Crafting paper from EndOfDayView panel")]
	[SerializeField]
	private GameObject _craftingPaperEndOfDayView;

	public void Show()
	{
		_resourcesDisplayController.Show();
		if (_craftingData.RuntimeData.IsOngoing || !_craftingData.RuntimeData.IsActive)
		{
			_craftingTitlePanelController.gameObject.SetActive(value: false);
			_craftingDecisionPanelController.gameObject.SetActive(value: false);
			_craftingContentPanelController.gameObject.SetActive(value: false);
			_craftingDisabledPanelController.gameObject.SetActive(value: true);
			_craftingDisabledPanelController.Show();
			SetConfirmButtonInteractable(interactable: false);
		}
		else
		{
			_craftingTitlePanelController.gameObject.SetActive(value: true);
			_craftingDisabledPanelController.gameObject.SetActive(value: false);
			_craftingTitlePanelController.Show();
			if (_craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction == EPlannedCraftingAction.NONE)
			{
				_craftingDecisionPanelController.SetAllTogglesOff(isVisible: true);
				SetConfirmButtonInteractable(interactable: false);
				_craftingDecisionPanelController.Show();
				_craftingDecisionPanelController.gameObject.SetActive(value: true);
				_craftingContentPanelController.gameObject.SetActive(value: false);
			}
			else
			{
				_craftingDecisionPanelController.gameObject.SetActive(value: false);
				_craftingContentPanelController.gameObject.SetActive(value: true);
				_craftingContentPanelController.Show();
			}
		}
		if (_craftingData.RuntimeData.IsOngoing || !_craftingData.RuntimeData.IsPlanned)
		{
			_craftingSummaryAnimationController.SetVisible(visible: false, force: true);
		}
	}

	public void SetConfirmButtonInteractable(bool interactable)
	{
		_confirmButton.interactable = interactable;
	}

	public void OnDecisionButtonClicked()
	{
		if (_craftingData.RuntimeData.PlannedCraftingActionData.Item == null && _craftingData.RuntimeData.PlannedCraftingActionData.System == null)
		{
			if (_craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction == EPlannedCraftingAction.NONE)
			{
				return;
			}
			_craftingDecisionPanelController.gameObject.SetActive(value: false);
			_craftingContentPanelController.gameObject.SetActive(value: true);
			_craftingTitlePanelController.Show();
			_craftingContentPanelController.Show();
		}
		_slider.value = 0f;
	}

	public void OnConfirmButtonClicked()
	{
		if (_craftingData.RuntimeData.PlannedCraftingActionData.Item != null || _craftingData.RuntimeData.PlannedCraftingActionData.System != null)
		{
			_craftingData.RuntimeData.IsPlanned = true;
			if (_craftingData != null && _craftingData.RuntimeData.PlannedCraftingActionData.Item != null && _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.CRAFT && _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.NONE)
			{
				_craftingData.RuntimeData.PlannedCraftingActionData.Item.Lock();
			}
			_craftingView.Hide();
			_craftingPaperEndOfDayView.SetActive(value: true);
		}
	}

	public void OnBackButtonClicked()
	{
		if (_craftingData.RuntimeData.PlannedCraftingActionData.Item != null || _craftingData.RuntimeData.PlannedCraftingActionData.System != null)
		{
			_craftingSummaryAnimationController.Show(visible: false);
		}
		ResetPanel(isVisible: true);
	}

	private void ResetPlannedCraftingAction()
	{
		if (_craftingData.RuntimeData.PlannedCraftingActionData.Item != null)
		{
			_craftingData.RuntimeData.PlannedCraftingActionData.Item.Unlock();
		}
		_craftingData.RuntimeData.PlannedCraftingActionData.System = null;
		_craftingData.RuntimeData.PlannedCraftingActionData.Item = null;
		_craftingData.RuntimeData.IsPlanned = false;
		if (!_craftingData.RuntimeData.IsOngoing)
		{
			_craftingPaperEndOfDayView.SetActive(value: false);
		}
	}

	public void ResetPanel(bool isVisible)
	{
		if ((_craftingData.RuntimeData.PlannedCraftingActionData.Item != null || _craftingData.RuntimeData.PlannedCraftingActionData.System != null) && _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.RECYCLE && _craftingData.RuntimeData.PlannedCraftingActionData.PlannedCraftingAction != EPlannedCraftingAction.NONE)
		{
			_craftingData.RuntimeData.UnlockCraftingResources();
		}
		_craftingDecisionPanelController.gameObject.SetActive(value: true);
		_craftingDecisionPanelController.Show();
		_craftingDecisionPanelController.SetAllTogglesOff(isVisible);
		_craftingContentPanelController.gameObject.SetActive(value: false);
		_craftingContentPanelController.SetAllTogglesOff();
		_craftingTitlePanelController.Show();
		ResetPlannedCraftingAction();
	}

	public void RefreshInteractable()
	{
		_closeButton.interactable = true;
		_backButton.interactable = true;
	}
}
