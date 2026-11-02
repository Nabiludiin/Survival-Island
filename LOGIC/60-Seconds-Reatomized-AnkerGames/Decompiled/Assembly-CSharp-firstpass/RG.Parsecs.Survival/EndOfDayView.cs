using System.Collections.Generic;
using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class EndOfDayView : WindowView, IRefreshInteractable
{
	public SurvivalData Data;

	public Button EndDayButton;

	public Button CloseEndDayPanelButton;

	public EndOfDayManager Manager;

	public GameObject EndOfDayButtonGlass;

	public List<EndOfDayRationingBarView> RationingBars;

	[SerializeField]
	private GameObject _expeditionSummaryPaper;

	[SerializeField]
	private ExpeditionData _expeditionData;

	[SerializeField]
	private ExpeditionSummaryController _expeditionSummary;

	[SerializeField]
	private GameObject _craftingSummaryPaper;

	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private CraftingSummaryController _craftingSummary;

	[Tooltip("A delay between pulling the lever and ending the day, so that the lever return animation can play.")]
	[SerializeField]
	private float _endOfDayButtonOffset = 2f;

	[SerializeField]
	private GameObject _choiceCardPaper;

	[SerializeField]
	private Image _choiceImage;

	[SerializeField]
	private ChoiceCardsController _choiceCardsController;

	[SerializeField]
	private RationingManager _rationingManager;

	public RationingManager RationingManager => _rationingManager;

	public void RefreshInteractable()
	{
		EndDayButton.interactable = Data.DailyEventResolved;
	}

	public override void OnShow()
	{
		if (!Data.DailyEventResolved)
		{
			_choiceCardPaper.SetActive(value: false);
			EndOfDayButtonGlass.SetActive(value: true);
		}
		else
		{
			if (_choiceCardsController != null && _choiceCardsController.GetPlayerChoice() != null)
			{
				_choiceImage.sprite = _choiceCardsController.GetPlayerChoice().GetCurrentImage();
				_choiceCardPaper.SetActive(value: true);
			}
			else
			{
				_choiceCardPaper.SetActive(value: false);
			}
			EndOfDayButtonGlass.SetActive(value: false);
		}
		RefreshInteractable();
		for (int i = 0; i < RationingBars.Count; i++)
		{
			RationingBars[i].RefreshRationingBar();
		}
		EndDayButton.interactable = true;
		CloseEndDayPanelButton.interactable = true;
		if (_expeditionData.RuntimeData.IsPlanned || _expeditionData.RuntimeData.IsOngoing)
		{
			_expeditionSummaryPaper.SetActive(value: true);
		}
		else
		{
			_expeditionSummaryPaper.SetActive(value: false);
		}
		if (_expeditionData.RuntimeData.IsPlanned || _expeditionData.RuntimeData.IsOngoing)
		{
			_expeditionSummary.SetExpeditionCharacter();
			_expeditionSummary.SetExpeditionEquipment();
			_expeditionSummary.SetExpeditionDestination();
		}
		if (_craftingData.RuntimeData.IsPlanned || _craftingData.RuntimeData.IsOngoing)
		{
			_craftingSummaryPaper.SetActive(value: true);
		}
		else
		{
			_craftingSummaryPaper.SetActive(_craftingData.RuntimeData.ShouldReactivate);
		}
		if (_craftingData.RuntimeData.IsPlanned || _craftingData.RuntimeData.IsOngoing)
		{
			_craftingSummary.SetContent();
		}
		else if (_craftingData.RuntimeData.ShouldReactivate)
		{
			_craftingSummary.SetActivationContent();
		}
	}

	public void OnEndDayButtonPressed()
	{
		EndDayButton.interactable = false;
		CloseEndDayPanelButton.interactable = false;
		Singleton<GameManager>.Instance.RaycastCatcher.SetClickawayBlocked(block: true);
		Invoke("EndDayOffSet", _endOfDayButtonOffset);
	}

	public void EndDayOffSet()
	{
		Hide(disableRaycast: false);
		Manager.EndOfDay();
	}

	public void OnCloseWindowButtonPressed()
	{
		Hide();
	}
}
