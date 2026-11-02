using System.Collections;
using System.Collections.Generic;
using RG.Parsecs.Common;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class StarlogView : WindowView, IRefreshInteractable
{
	private const float WAIT_TIME_BEFORE_OPEN_ON_NEW_DAY = 0.05f;

	private const float WAIT_TIME_BEFORE_OPEN_ON_FIRST_DAY = 1f;

	[SerializeField]
	private EndGameData _endGameData;

	[SerializeField]
	[Tooltip("Reference for Day Info Controller.")]
	private DayInfoController _dayInfoController;

	[SerializeField]
	[Tooltip("Reference for ReportPanelController.")]
	private ReportPanelController _reportPanelController;

	[SerializeField]
	[Tooltip("Reference for StatsPanelController.")]
	private StatsPanelController _statsPanelController;

	[SerializeField]
	[Tooltip("Reference for GoalsPanelController.")]
	private GoalsPanelController _goalsPanelController;

	[SerializeField]
	[Tooltip("Reference for GoalsPanelController.")]
	private CrewPanelController _crewPanelController;

	[SerializeField]
	private Slider _contentSlider;

	[SerializeField]
	private GameObject _yellowLightEvent;

	[SerializeField]
	private GameObject _greenLightEvent;

	[SerializeField]
	private SurvivalData _survivalData;

	[SerializeField]
	private List<SetScrollRectPosition> _setScrollRectPositions = new List<SetScrollRectPosition>();

	[SerializeField]
	[Tooltip("Button reference for object that will close the Starlog")]
	private Button _closeViewButton;

	public void OnCloseWindowButton()
	{
		_survivalData.StarlogOpened = true;
		Hide();
		FinishWindowClose();
	}

	public void PrepareStarlogView()
	{
		_survivalData.StarlogOpened = false;
		_reportPanelController.ClearContents();
		_contentSlider.value = 0f;
	}

	public void UpdateDayCounter()
	{
		_dayInfoController.UpdateDayCounter();
	}

	public void UpdateStatsAndGoalsLaye()
	{
		_statsPanelController.UpdateStatsDisplay();
		_goalsPanelController.UpdateGoalsPanel();
		_crewPanelController.UpdateCrewPanel();
	}

	public void SetNewDayTexts()
	{
		_dayInfoController.SetDayTextToCurrentDay();
	}

	public void OnDayStarted()
	{
		if (!EndGameManager.Instance.GetEndGameData().RuntimeData.ShouldEndGame)
		{
			EndGameManager.Instance.StartCoroutine(ShowStarlog(0.05f));
		}
		else
		{
			Singleton<GameManager>.Instance.RaycastCatcher.SetCatchingRaycasts(catchingRaycasts: false, null);
		}
	}

	public IEnumerator ShowStarlog(float delay)
	{
		yield return new WaitForSeconds(delay);
		Show();
	}

	private void FinishWindowClose()
	{
		if (_endGameData.RuntimeData.ShouldEndGame)
		{
			Singleton<HandCursor>.Instance.DisablePanels();
			EndGameManager.Instance.LoadEndGameScene();
		}
		_contentSlider.value = 0f;
	}

	protected override void OnClickAway()
	{
		_survivalData.StarlogOpened = true;
		Hide();
		FinishWindowClose();
	}

	public override void OnShow()
	{
		base.OnShow();
		for (int i = 0; i < _setScrollRectPositions.Count; i++)
		{
			_setScrollRectPositions[i].enabled = true;
		}
	}

	public override void OnHide()
	{
		base.OnHide();
		for (int i = 0; i < _setScrollRectPositions.Count; i++)
		{
			_setScrollRectPositions[i].enabled = false;
		}
	}

	public void RefreshInteractable()
	{
		_closeViewButton.interactable = true;
		for (int i = 0; i < _setScrollRectPositions.Count; i++)
		{
			SetScrollRectPosition setScrollRectPosition = _setScrollRectPositions[i];
			if (setScrollRectPosition.gameObject.activeInHierarchy)
			{
				setScrollRectPosition.RefreshScrollBarVisibility();
			}
		}
	}
}
