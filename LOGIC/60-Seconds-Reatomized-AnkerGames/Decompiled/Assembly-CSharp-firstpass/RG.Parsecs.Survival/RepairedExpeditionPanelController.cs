using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class RepairedExpeditionPanelController : MonoBehaviour, IRefreshInteractable
{
	public enum EExpeditionPanels
	{
		NONE,
		DESTINATION,
		CHARACTER,
		EQUIPMENT,
		LOCKED
	}

	[SerializeField]
	[Header("Data")]
	[Tooltip("Expedition Data scriptable object holding informations about expedition system.")]
	private ExpeditionData _expeditionData;

	[SerializeField]
	[Header("References")]
	[Tooltip("Reference to back button that is used to go to previous panel.")]
	private Button _backButton;

	[SerializeField]
	[Tooltip("Reference to accept button that is used to go to next panel.")]
	private Button _acceptButton;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	[Tooltip("Reference to top panel.")]
	private GameObject _topPanel;

	[SerializeField]
	[Tooltip("Locked expedition panel.")]
	private LockedPanelController _lockedPanel;

	[SerializeField]
	[Tooltip("Destination Panel for choosing destination for the expedition.")]
	private DestinationPanelController _destinationPanel;

	[SerializeField]
	[Tooltip("Characters Panel for choosing character for the expedition.")]
	private CharacterPanelController _characterPanel;

	[SerializeField]
	[Tooltip("Equipment Panel for choosing equipment for the expedition.")]
	private EquipmentPanelController _equipmentPanel;

	[SerializeField]
	[Tooltip("Summary prined paper for showing made choices for the expedition.")]
	private ExpeditionSummaryAnimationController _summary;

	[SerializeField]
	[Tooltip("Destination progress orb.")]
	private GameObject _progressOrbDestination;

	[SerializeField]
	[Tooltip("Character progress orb.")]
	private GameObject _progressOrbCharacter;

	[SerializeField]
	[Tooltip("Equipment progress orb.")]
	private GameObject _progressOrbEquipment;

	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private SuitData _suitData;

	private EExpeditionPanels _currentPanel;

	private bool _lastAcceptButtonInteractableState;

	public bool CanShowNextPanel => _currentPanel != EExpeditionPanels.EQUIPMENT;

	public EExpeditionPanels GetPanelStatus()
	{
		return _currentPanel;
	}

	public void Initialize()
	{
		SetStartingPage();
	}

	public void ResetPanel()
	{
		_destinationPanel.ResetDestination();
		_characterPanel.ResetCharacters();
		_equipmentPanel.ResetEquipment();
		_summary.ResetSummary();
	}

	private void SetStartingPage()
	{
		if (_expeditionData.RuntimeData.IsOngoing || (!_expeditionData.RuntimeData.IsActive && _expeditionData.RuntimeData.IsRepaired) || _expeditionData.RuntimeData.CurrentPlanetDeck == null || (_craftingData.RuntimeData.IsOngoing && (_craftingData.RuntimeData.OngoingCraftingActionData.System == _expeditionData || _craftingData.RuntimeData.OngoingCraftingActionData.System == _suitData)) || (_craftingData.RuntimeData.IsPlanned && (_craftingData.RuntimeData.PlannedCraftingActionData.System == _expeditionData || _craftingData.RuntimeData.PlannedCraftingActionData.System == _suitData)) || CharacterManager.Instance.GetCharacterList().GetLiveCharactersCount() == 1)
		{
			_currentPanel = EExpeditionPanels.LOCKED;
		}
		else if (_expeditionData.RuntimeData.IsPlanned)
		{
			_currentPanel = EExpeditionPanels.EQUIPMENT;
		}
		else
		{
			_currentPanel = EExpeditionPanels.DESTINATION;
		}
		SwitchPanel();
	}

	public void ShowNextPanel()
	{
		switch (_currentPanel)
		{
		default:
			return;
		case EExpeditionPanels.DESTINATION:
			_currentPanel = EExpeditionPanels.CHARACTER;
			break;
		case EExpeditionPanels.CHARACTER:
			_currentPanel = EExpeditionPanels.EQUIPMENT;
			break;
		}
		SwitchPanel();
	}

	public void ShowPreviousPanel()
	{
		switch (_currentPanel)
		{
		default:
			return;
		case EExpeditionPanels.CHARACTER:
			_currentPanel = EExpeditionPanels.DESTINATION;
			break;
		case EExpeditionPanels.EQUIPMENT:
			_currentPanel = EExpeditionPanels.CHARACTER;
			break;
		}
		SwitchPanel();
	}

	private void SwitchPanel()
	{
		switch (_currentPanel)
		{
		case EExpeditionPanels.DESTINATION:
			ShowDestinationPanel();
			break;
		case EExpeditionPanels.CHARACTER:
			ShowCharacterPanel();
			break;
		case EExpeditionPanels.EQUIPMENT:
			ShowEquipmentPanel();
			break;
		case EExpeditionPanels.LOCKED:
			ShowLockedPanel();
			break;
		}
	}

	private void ShowLockedPanel()
	{
		_summary.SetSummaryState(ExpeditionSummaryAnimationController.ESummaryState.HIDDEN);
		_acceptButton.interactable = false;
		_lockedPanel.Initialize();
		_destinationPanel.gameObject.SetActive(value: false);
		_characterPanel.gameObject.SetActive(value: false);
		_equipmentPanel.gameObject.SetActive(value: false);
		_topPanel.gameObject.SetActive(value: false);
		_lockedPanel.gameObject.SetActive(value: true);
	}

	private void ShowDestinationPanel()
	{
		if (_expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination == null)
		{
			_summary.SetSummaryState(ExpeditionSummaryAnimationController.ESummaryState.HIDDEN);
		}
		_destinationPanel.Initialize();
		_destinationPanel.gameObject.SetActive(value: true);
		_backButton.interactable = false;
		_progressOrbDestination.SetActive(value: true);
		_progressOrbCharacter.SetActive(value: false);
		_progressOrbEquipment.SetActive(value: false);
		_characterPanel.gameObject.SetActive(value: false);
		_equipmentPanel.gameObject.SetActive(value: false);
		_topPanel.gameObject.SetActive(value: true);
		_lockedPanel.gameObject.SetActive(value: false);
	}

	private void ShowCharacterPanel()
	{
		_summary.SetSummaryState(ExpeditionSummaryAnimationController.ESummaryState.DESTINATION);
		_characterPanel.Initialize();
		_characterPanel.gameObject.SetActive(value: true);
		_backButton.interactable = true;
		_progressOrbDestination.SetActive(value: true);
		_progressOrbCharacter.SetActive(value: true);
		_progressOrbEquipment.SetActive(value: false);
		_destinationPanel.gameObject.SetActive(value: false);
		_equipmentPanel.gameObject.SetActive(value: false);
		_topPanel.gameObject.SetActive(value: true);
		_lockedPanel.gameObject.SetActive(value: false);
	}

	private void ShowEquipmentPanel()
	{
		_summary.SetSummaryState(ExpeditionSummaryAnimationController.ESummaryState.CHARACTER);
		_equipmentPanel.Initialize();
		_equipmentPanel.gameObject.SetActive(value: true);
		_backButton.interactable = true;
		_progressOrbDestination.SetActive(value: true);
		_progressOrbCharacter.SetActive(value: true);
		_progressOrbEquipment.SetActive(value: true);
		_characterPanel.gameObject.SetActive(value: false);
		_destinationPanel.gameObject.SetActive(value: false);
		_topPanel.gameObject.SetActive(value: true);
		_lockedPanel.gameObject.SetActive(value: false);
	}

	public void PrintLastSummary()
	{
		_summary.SetSummaryState(ExpeditionSummaryAnimationController.ESummaryState.EQUIPMENT);
	}

	public void SetAcceptButtonInteractable(bool interactable)
	{
		_acceptButton.interactable = interactable;
		_lastAcceptButtonInteractableState = interactable;
	}

	public void OnBackButtonPresed()
	{
		ShowPreviousPanel();
	}

	public void RefreshInteractable()
	{
		_closeButton.interactable = true;
		_acceptButton.interactable = _lastAcceptButtonInteractableState;
	}
}
