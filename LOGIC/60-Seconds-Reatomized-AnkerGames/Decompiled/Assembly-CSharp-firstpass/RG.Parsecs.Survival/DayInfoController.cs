using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class DayInfoController : MonoBehaviour, IRefreshInteractable
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("Reference to Survival Data scriptable object.")]
	private SurvivalData _survivalData;

	[SerializeField]
	[Tooltip("I2Loc term with \"DAY\" text translation.")]
	private LocalizedString _dayTextTerm;

	[SerializeField]
	[Header("Scene references")]
	[Tooltip("TextMeshPro reference for object that will display DAY text.")]
	private TextMeshProUGUI _dayText;

	[SerializeField]
	[Tooltip("Button reference for object that will change Starlog day view to next one.")]
	private Button _dayForwardButton;

	[SerializeField]
	[Tooltip("Button reference for object that will change Starlog day view to previous one.")]
	private Button _dayBackwardButton;

	[SerializeField]
	[Tooltip("ReportPanelController reference for object that is used for displaying Starlog events content.")]
	private ReportPanelController _reportPanelController;

	private int _dayToDisplay = 1;

	private int _previouslyDisplayedDay = 1;

	private EStarlogPanelType _knobState;

	public int DayToDisplay
	{
		get
		{
			return _dayToDisplay;
		}
		private set
		{
			_dayToDisplay = value;
		}
	}

	private void Awake()
	{
		_dayToDisplay = _survivalData.CurrentDay;
		UpdateDayCounter();
		SetDayTextToCurrentDay();
	}

	public void OnDayForwardButtonPressed()
	{
		_previouslyDisplayedDay = DayToDisplay;
		DayToDisplay++;
		_reportPanelController.DisplayPage(DayToDisplay, _previouslyDisplayedDay);
		UpdateDayText();
		RefreshInteractable();
		if (DayToDisplay >= _survivalData.CurrentDay)
		{
			EventSystem.current.SetSelectedGameObject(_dayForwardButton.gameObject);
		}
	}

	public void OnDayBackwardButtonPressed()
	{
		_previouslyDisplayedDay = DayToDisplay;
		DayToDisplay--;
		_reportPanelController.DisplayPage(DayToDisplay, _previouslyDisplayedDay);
		UpdateDayText();
		RefreshInteractable();
		if (DayToDisplay <= 1)
		{
			EventSystem.current.SetSelectedGameObject(_dayBackwardButton.gameObject);
		}
	}

	private void UpdateDayText()
	{
		_dayText.text = string.Format(_dayTextTerm, DayToDisplay);
	}

	public void SetDayTextToCurrentDay()
	{
		RefreshInteractable();
		UpdateDayText();
	}

	public void UpdateDayCounter()
	{
		DayToDisplay = _survivalData.CurrentDay;
	}

	public void SetKnobState(EStarlogPanelType state)
	{
		_knobState = state;
	}

	public void RefreshInteractable()
	{
		bool interactable = _knobState == EStarlogPanelType.REPORT;
		if (DayToDisplay >= _survivalData.CurrentDay)
		{
			_dayForwardButton.interactable = false;
		}
		else
		{
			_dayForwardButton.interactable = interactable;
		}
		if (DayToDisplay <= 1)
		{
			_dayBackwardButton.interactable = false;
		}
		else
		{
			_dayBackwardButton.interactable = interactable;
		}
	}
}
