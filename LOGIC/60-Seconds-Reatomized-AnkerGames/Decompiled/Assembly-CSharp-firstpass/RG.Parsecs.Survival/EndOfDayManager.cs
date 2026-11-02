using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using I2.Loc;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.VirtualInput;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class EndOfDayManager : MonoBehaviour
{
	private const float WAIT_TIME_BEFORE_OPEN_ON_FIRST_DAY = 1f;

	[Header("Basic Data")]
	public SurvivalData Data;

	public EndOfDayListenerList ListenerList;

	public ComputerLampsListenerList ComputerLampsListenerList;

	public RationingListenerList RationingListenerList;

	public PlanetLandingController PlanetLandingController;

	[Header("Display")]
	public TextMeshProUGUI ObscurerText;

	public LoadingObscurerController Obscurer;

	public AlphaFadeController Text;

	public StarlogView StarlogView;

	public DayDisplayView DayDisplayView;

	public LocalizedString DayTitle = null;

	public bool DayTitleReplacesDayNumber;

	public float PostLoadingObscurerFadeoutDelay = 4f;

	public float TextDisplayTime = 1f;

	[SerializeField]
	private LocalizedString _textDay;

	[SerializeField]
	private ExpeditionData _expeditionData;

	[SerializeField]
	private bool _showDaySkipAnimation;

	[SerializeField]
	private float _timeBetweenNumbersDuringDaySkipAnimation;

	[EventRef]
	[SerializeField]
	private string _skipDaySound;

	[Header("Custom systems")]
	[Tooltip("This allows the designers to run chosen ReportEvents (without displaying text, obviously) during different moments of the end of day processing.\nEvents marked as Update will be run after most report events but before system events. Thats when rationing happened in Parsecs.\nEvents marked as System are happening when SystemEvents are run.\nEvents marked as Reset are run last in the processing, as cleanup. During that time the rationing decisions of the player from the previous day are reset in Parsecs.\n\nCAUTION! The end of day processing order is complex and fragile, use with care and test thoroughly.")]
	public List<CustomEndOfDayProcessingRegistrationData> EventRegistrationData = new List<CustomEndOfDayProcessingRegistrationData>();

	[SerializeField]
	private LocalizedString _endGameText;

	[SerializeField]
	private GlobalBoolVariable _isSurvivalInteractable;

	[SerializeField]
	private bool _storeSpacePlanetDaysStats = true;

	private const string SFX_BUS = "bus:/Master/SFX";

	private static EndOfDayManager _instance;

	private int _previousDay;

	private WaitForSeconds _beforeTextDelay = new WaitForSeconds(1f);

	private WaitForSeconds _obscurerShowTime = new WaitForSeconds(1.5f);

	private WaitForSeconds _textDisplayTime = new WaitForSeconds(1f);

	public static EndOfDayManager Instance => _instance;

	public int AcutalDay => Data.CurrentDay;

	private void Awake()
	{
		_instance = this;
		ListenerList.RegisterOnEndOfDay(ComputerLampsListenerList.UpdateLampsOnEndOfDay, "Visuals", 2, this);
		ListenerList.RegisterOnEndOfDay(RationingListenerList.UpdateRationingOnEndOfDay, "Visuals", 3, this);
		if (EventRegistrationData == null || EventRegistrationData.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < EventRegistrationData.Count; i++)
		{
			if (EventRegistrationData[i] != null && EventRegistrationData[i].IsValid() && ListenerList != null)
			{
				EventRegistrationData[i].RegisterInEndOfDayListenerList(ListenerList);
			}
		}
	}

	private void OnDestroy()
	{
		ListenerList.UnregisterOnEndOfDay(ComputerLampsListenerList.UpdateLampsOnEndOfDay, "Visuals");
		ListenerList.UnregisterOnEndOfDay(RationingListenerList.UpdateRationingOnEndOfDay, "Visuals");
		if (EventRegistrationData == null || EventRegistrationData.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < EventRegistrationData.Count; i++)
		{
			if (EventRegistrationData[i] != null && EventRegistrationData[i].IsValid() && ListenerList != null)
			{
				EventRegistrationData[i].UnregisterFromEndOfDayListenerList(ListenerList);
			}
		}
	}

	private void Start()
	{
		Obscurer.InstantlyShowObscurer();
		StartCoroutine(InitializeEndOfDay());
	}

	public void EndOfDay()
	{
		StartCoroutine(ProcessEndOfDay());
	}

	private IEnumerator ProcessEndOfDay()
	{
		Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked = true;
		Singleton<VirtualInputManager>.Instance.VisualManager.MouseCursorVisible = false;
		StatsManager.Instance.AddGlobalData("SurvivedDays", 1);
		PlanetDeck currentPlanetDeck = _expeditionData.RuntimeData.CurrentPlanetDeck;
		if (_storeSpacePlanetDaysStats)
		{
			if (currentPlanetDeck == null)
			{
				StatsManager.Instance.AddGlobalData("DaysInSpace", 1);
			}
			else
			{
				StatsManager.Instance.AddGlobalData("DaysOnAllPlanet", 1);
				if (currentPlanetDeck.name == "Planet1")
				{
					StatsManager.Instance.AddGlobalData("DaysOnPlanet1", 1);
				}
				else if (currentPlanetDeck.name == "Planet2")
				{
					StatsManager.Instance.AddGlobalData("DaysOnPlanet2", 1);
				}
				else if (currentPlanetDeck.name == "Planet3")
				{
					StatsManager.Instance.AddGlobalData("DaysOnPlanet3", 1);
				}
			}
		}
		yield return Obscurer.ShowObscurer();
		if (Data.DaysToSkip == 0)
		{
			Data.CurrentDay++;
		}
		yield return ListenerList.NotifyEndOfDay();
		ObscurerText.text = string.Format(_textDay, Data.DisplayDay);
		currentPlanetDeck = _expeditionData.RuntimeData.CurrentPlanetDeck;
		if (Data.ShouldLandOnPlanet)
		{
			Bus bus = RuntimeManager.GetBus("bus:/Master/SFX");
			PlanetLandingController.gameObject.SetActive(value: true);
			Obscurer.gameObject.SetActive(value: false);
			bus.setMute(mute: true);
			yield return PlanetLandingController.LandOnPlanet(currentPlanetDeck);
			Obscurer.gameObject.SetActive(value: true);
			yield return Obscurer.ShowObscurer();
			PlanetLandingController.gameObject.SetActive(value: false);
			bus.setMute(mute: false);
			Data.ShouldLandOnPlanet = false;
		}
		if (DayTitleReplacesDayNumber && (string)DayTitle != null && DayTitle.mTerm != null && !string.IsNullOrEmpty(DayTitle.ToString()))
		{
			ObscurerText.text = DayTitle.ToString();
			DayTitle = null;
		}
		yield return _beforeTextDelay;
		int skippedDays = Data.DisplayDay - _previousDay;
		if (_showDaySkipAnimation && skippedDays > 1)
		{
			int num = _previousDay + 1;
			ObscurerText.text = string.Format(_textDay, num);
		}
		yield return Text.FadeIn();
		if (_showDaySkipAnimation && skippedDays > 1)
		{
			for (int i = 2; i <= skippedDays; i++)
			{
				int num2 = _previousDay + i;
				ObscurerText.text = string.Format(_textDay, num2);
				AudioManager.PlaySound(_skipDaySound);
				yield return new WaitForSeconds(_timeBetweenNumbersDuringDaySkipAnimation);
			}
		}
		yield return _obscurerShowTime;
		if (DayDisplayView != null)
		{
			DayDisplayView.UpdateClockBack();
		}
		yield return Obscurer.HideObscurer();
		yield return Text.FadeOut();
		if (DayDisplayView != null)
		{
			DayDisplayView.SpinClocks();
		}
		if (!DayTitleReplacesDayNumber && (string)DayTitle != null && DayTitle.mTerm != null && !string.IsNullOrEmpty(DayTitle.ToString()))
		{
			ObscurerText.text = DayTitle.ToString();
			yield return Text.FadeIn();
			yield return _textDisplayTime;
			yield return Text.FadeOut();
			DayTitle = null;
		}
		ListenerList.NotifyDayStarted();
		_previousDay = Data.DisplayDay;
		Singleton<GameManager>.Instance.RaycastCatcher.SetClickawayBlocked(block: false);
		Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked = false;
		Singleton<VirtualInputManager>.Instance.VisualManager.MouseCursorVisible = true;
	}

	private IEnumerator InitializeEndOfDay()
	{
		Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked = true;
		if (_isSurvivalInteractable != null && !_isSurvivalInteractable.Value && !string.IsNullOrEmpty(_endGameText))
		{
			ObscurerText.text = _endGameText;
		}
		else
		{
			ObscurerText.text = string.Format(_textDay, Data.DisplayDay);
		}
		_previousDay = Data.DisplayDay;
		yield return new WaitForSeconds(PostLoadingObscurerFadeoutDelay);
		yield return Text.FadeIn();
		yield return new WaitForSeconds(TextDisplayTime);
		yield return Obscurer.HideObscurer();
		yield return Text.FadeOut();
		Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked = false;
		Singleton<VirtualInputManager>.Instance.VisualManager.MouseCursorVisible = true;
	}
}
