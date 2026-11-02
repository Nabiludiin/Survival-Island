using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class DayDisplayView : DisplayView
{
	[SerializeField]
	private SurvivalData _survivalData;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	[SerializeField]
	private ComputerLampsListenerList _computerLampsListenerList;

	[SerializeField]
	[Tooltip("Image representing the leftmost digit of the day number.")]
	private SpriteRenderer _firstDigit;

	[SerializeField]
	[Tooltip("Image representing the middle digit of the day number.")]
	private SpriteRenderer _secondDigit;

	[SerializeField]
	[Tooltip("Image representing the rightmost digit of the day number.")]
	private SpriteRenderer _thirdDigit;

	[SerializeField]
	[Tooltip("List of images representing numerals 0-9 that will be used as the first (leftmost) digit.")]
	private List<Sprite> _firstDigitImages;

	[SerializeField]
	[Tooltip("List of images representing numerals 0-9 that will be used as the second (middle) digit.")]
	private List<Sprite> _secondDigitImages;

	[SerializeField]
	[Tooltip("List of images representing numerals 0-9 that will be used as the third (rightmost) digit.")]
	private List<Sprite> _thirdDigitImages;

	[SerializeField]
	private Sprite _nullDigit;

	[SerializeField]
	private Animator _leverPanelAnimator;

	[SerializeField]
	private Animator[] _digitsAnimations;

	[SerializeField]
	private SystemView.Lamps _lamps;

	[SerializeField]
	private SystemView.Lamps.ELampState _lampState;

	[SerializeField]
	private GameObject _expeditionDisplayPaper;

	[SerializeField]
	private GameObject _craftingDisplayPaper;

	[SerializeField]
	private ExpeditionData _expeditionData;

	[SerializeField]
	private CraftingData _craftingData;

	private void Awake()
	{
		_endOfDayListenerList.RegisterOnEndOfDay(OnEndOfDay, "Visuals", 1, this);
		_computerLampsListenerList.RegisterOnLampsUpdate(UpdateLampColor);
		UpdateLampColor();
	}

	private void Start()
	{
		UpdateClock(_survivalData.CurrentDay);
		UpdateSystemPapers();
	}

	private void UpdateSystemPapers()
	{
		_expeditionDisplayPaper.SetActive(_expeditionData.RuntimeData.IsOngoing || _expeditionData.RuntimeData.IsPlanned);
		_craftingDisplayPaper.SetActive(_craftingData.RuntimeData.IsOngoing || _craftingData.RuntimeData.IsPlanned);
	}

	private void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(OnEndOfDay, "Visuals");
		_computerLampsListenerList.UnregisterOnLampsUpdate(UpdateLampColor);
	}

	public void UpdateLampColor()
	{
		if (_survivalData.DailyEventResolved && !EndGameManager.Instance.GetEndGameData().RuntimeData.ShouldEndGame)
		{
			_lampState = SystemView.Lamps.ELampState.YELLOW_BLINKING;
		}
		else
		{
			_lampState = SystemView.Lamps.ELampState.RED;
		}
		_lamps.SetLampBasedOnState(_lampState);
	}

	public void UpdateClockBack()
	{
		UpdateClock(_survivalData.CurrentDay - 1);
	}

	public void UpdateClock()
	{
		UpdateClock(_survivalData.CurrentDay);
	}

	private void UpdateClock(int day)
	{
		int num = day / 100;
		int index = day / 10 % 10;
		int index2 = day % 10;
		_firstDigit.sprite = ((num > _firstDigitImages.Count) ? _nullDigit : _firstDigitImages[num]);
		_secondDigit.sprite = ((num > _firstDigitImages.Count) ? _nullDigit : _secondDigitImages[index]);
		_thirdDigit.sprite = ((num > _firstDigitImages.Count) ? _nullDigit : _thirdDigitImages[index2]);
	}

	public void OnEndOfDay()
	{
		UpdateClock(_survivalData.CurrentDay);
		_leverPanelAnimator.SetBool("Open", value: false);
		for (int i = 0; i < _digitsAnimations.Length; i++)
		{
			_digitsAnimations[i].ResetTrigger("Spin");
		}
		UpdateSystemPapers();
	}

	public void SpinClocks()
	{
		for (int i = 0; i < _digitsAnimations.Length; i++)
		{
			_digitsAnimations[i].SetTrigger("Spin");
		}
		Invoke("UpdateClock", 0.5f);
	}
}
