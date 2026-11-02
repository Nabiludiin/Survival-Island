using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using RG.Core.Base;
using RG.Parsecs.Common;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.VirtualInput;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class EndOfGameViewManager : RGMonoBehaviour
{
	private static EndOfGameViewManager _instance;

	[Header("Settings")]
	[Tooltip("If this is set to true, slideshows will be displayed if the game is won. If this is false, no slideshows will ever be displayed.")]
	[SerializeField]
	private bool _slideshowsEnabled = true;

	[SerializeField]
	private bool _daysSurvivedPanelEnabled = true;

	[SerializeField]
	private bool _medalsEnabled = true;

	[SerializeField]
	private bool _playWinJingle = true;

	[SerializeField]
	private bool _playLoseJingle = true;

	[Header("Game data")]
	[SerializeField]
	private EndGameData _endGameData;

	[SerializeField]
	private SurvivalData _survivalData;

	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private List<EndGameCanvas> _slideList = new List<EndGameCanvas>();

	[Tooltip("The functions in this list will be run in order, before the slide list canvases are evaluated.")]
	[SerializeField]
	private List<NodeFunction> _runOnStartup = new List<NodeFunction>();

	[SerializeField]
	private SoundSlot _soundSlotForGameWonJingle;

	[SerializeField]
	private SoundSlot _soundSlotForGameLostJingle;

	[Header("Ending UI elements")]
	[SerializeField]
	private GameObject _previousButton;

	[SerializeField]
	private GameObject _nextButton;

	[SerializeField]
	private GameObject _hideEndingButton;

	[SerializeField]
	private GameObject _exitToMenuButton;

	[SerializeField]
	private VirtualInputButton _exitToMenuVirtualButton;

	[SerializeField]
	private GameObject _shareToSocialButton;

	[SerializeField]
	private VirtualInputButton _shareToSocialVirtualButton;

	[SerializeField]
	private LoadingObscurerController _obscurerController;

	[SerializeField]
	private CanvasGroup _endingUiCanvasGroup;

	[SerializeField]
	private Button _showTextButton;

	[SerializeField]
	private float _textFadeInTime = 1f;

	private float _buttonsFadeInTime = 5.5f;

	private bool _showTextButtonClicked;

	private int _currentSlide;

	[Header("Panels")]
	[SerializeField]
	private GameObject _endingPanel;

	[SerializeField]
	private GameObject _winPanel;

	[SerializeField]
	private GameObject _lostPanel;

	[SerializeField]
	private GameObject _statsPanel;

	[SerializeField]
	private GameObject _dayCountPanel;

	[SerializeField]
	private GameObject _medalPanel;

	[SerializeField]
	private Image _questSlideTextBackground;

	private Dictionary<EndGameTextId, EndGameTextController> textControllers = new Dictionary<EndGameTextId, EndGameTextController>();

	public static EndOfGameViewManager Instance
	{
		get
		{
			if (_instance == null)
			{
				FindInstanceInScene();
			}
			return _instance;
		}
	}

	private static void FindInstanceInScene()
	{
		_instance = Object.FindObjectOfType<EndOfGameViewManager>();
		if (_instance == null)
		{
			Debug.LogError("No EndOfGameViewManager in scene!");
		}
	}

	protected override void CustomAwake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		EndGameTextController[] componentsInChildren = GetComponentsInChildren<EndGameTextController>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			textControllers.Add(componentsInChildren[i].textId, componentsInChildren[i]);
			Debug.LogFormat("Added text with id {0} to dictionary", componentsInChildren[i].textId);
		}
		if (_runOnStartup != null && _runOnStartup.Count > 0)
		{
			for (int j = 0; j < _runOnStartup.Count; j++)
			{
				if (_runOnStartup[j] != null)
				{
					_runOnStartup[j].Execute(null);
				}
			}
		}
		if (_endGameData.RuntimeData.IsGameWon)
		{
			if (_slideshowsEnabled)
			{
				SetupSlideList();
				_endingPanel.SetActive(value: true);
				StartCoroutine(RefreshEndingSlideshowDisplay());
				return;
			}
			TurnOnWinLosePanel();
			if (_playWinJingle && IsSoundSlotValid(_soundSlotForGameWonJingle))
			{
				PlayJingle(_soundSlotForGameWonJingle);
			}
		}
		else
		{
			TurnOnWinLosePanel();
			if (_playLoseJingle && IsSoundSlotValid(_soundSlotForGameLostJingle))
			{
				PlayJingle(_soundSlotForGameLostJingle);
			}
		}
	}

	private bool IsSoundSlotValid(SoundSlot slot)
	{
		if (slot != null)
		{
			return !string.IsNullOrEmpty(slot.SoundEventName);
		}
		return false;
	}

	private void PlayJingle(SoundSlot slot)
	{
		RuntimeManager.CreateInstance(slot.SoundEventName).start();
	}

	private IEnumerator ShowWinLoseSummary()
	{
		yield return _obscurerController.HideObscurer();
		if (_endGameData.RuntimeData.IsGameWon)
		{
			_winPanel.SetActive(value: true);
			_lostPanel.SetActive(value: false);
			SummaryPanelAnimationController component = _winPanel.GetComponent<SummaryPanelAnimationController>();
			if (component != null)
			{
				yield return component.RunShowAnimation();
			}
		}
		else
		{
			_winPanel.SetActive(value: false);
			_lostPanel.SetActive(value: true);
			SummaryPanelAnimationController component = _lostPanel.GetComponent<SummaryPanelAnimationController>();
			if (component != null)
			{
				yield return component.RunShowAnimation();
			}
		}
		if (_daysSurvivedPanelEnabled)
		{
			_dayCountPanel.SetActive(value: true);
			SummaryPanelAnimationController component = _dayCountPanel.GetComponent<SummaryPanelAnimationController>();
			if (component != null)
			{
				yield return component.RunShowAnimation();
			}
		}
		if (_medalsEnabled)
		{
			if (MissionManager.Instance.IsMissionOngoing())
			{
				_medalPanel.SetActive(value: true);
				SummaryPanelAnimationController component = _medalPanel.GetComponent<SummaryPanelAnimationController>();
				if (component != null)
				{
					yield return component.RunShowAnimation();
				}
			}
			else
			{
				_medalPanel.SetActive(value: false);
			}
		}
		_exitToMenuButton.SetActive(value: true);
		if (!DemoManager.IS_DEMO_VERSION)
		{
			_shareToSocialButton.SetActive(_survivalData.CurrentDay > 1);
		}
		if (SteamManager.IsRunningOnSteamDeck())
		{
			_shareToSocialButton.SetActive(value: false);
		}
		_statsPanel.SetActive(value: true);
		StartCoroutine(SetButtonsInteractableOnVisible());
		yield return null;
	}

	private IEnumerator SetButtonsInteractableOnVisible()
	{
		yield return new WaitForSeconds(_buttonsFadeInTime);
		OnUIClickedSoundPlayer component = _exitToMenuButton.GetComponent<OnUIClickedSoundPlayer>();
		if (component != null)
		{
			component.Activate();
		}
		component = _shareToSocialVirtualButton.GetComponent<OnUIClickedSoundPlayer>();
		if (component != null)
		{
			component.Activate();
		}
		if (_exitToMenuVirtualButton.Selectable != null)
		{
			_exitToMenuVirtualButton.Selectable.interactable = true;
		}
		if (_shareToSocialVirtualButton.Selectable != null)
		{
			_shareToSocialVirtualButton.Selectable.interactable = true;
		}
		_exitToMenuVirtualButton.SelectThisSelectable();
	}

	public void TurnOnWinLosePanel()
	{
		StartCoroutine(ShowWinLoseSummary());
	}

	private void SetupSlideList()
	{
		_slideList.Clear();
		if (_endGameData.RuntimeData.MainQuestSummary != null)
		{
			_slideList.Add(_endGameData.RuntimeData.MainQuestSummary);
		}
		for (int i = 0; i < _characterList.CharactersInGame.Count; i++)
		{
			if (_characterList.CharactersInGame[i].RuntimeData.ShowsEndOfGameSlide && !_characterList.CharactersInGame[i].RuntimeData.IsCaptain && _characterList.CharactersInGame[i].StaticData.PersonalEpilogue != null)
			{
				_slideList.Add(_characterList.CharactersInGame[i].StaticData.PersonalEpilogue);
			}
		}
		if (_characterList.GetCaptain().RuntimeData.ShowsEndOfGameSlide)
		{
			if (_endGameData.RuntimeData.CaptainsFate == null)
			{
				_slideList.Add(_characterList.GetCaptain().StaticData.CaptainEpilogue);
			}
			else
			{
				_slideList.Add(_endGameData.RuntimeData.CaptainsFate);
			}
		}
		if (_endGameData.RuntimeData.SessionSummary != null)
		{
			_slideList.Add(_endGameData.RuntimeData.SessionSummary);
		}
	}

	public void OnPreviousButtonClick()
	{
		_previousButton.SetActive(value: false);
		_nextButton.SetActive(value: false);
		if (_currentSlide > 0)
		{
			_currentSlide--;
		}
		StartCoroutine(RefreshEndingSlideshowDisplay());
	}

	public void OnNextButtonClick()
	{
		_previousButton.SetActive(value: false);
		_nextButton.SetActive(value: false);
		if (_currentSlide < _slideList.Count - 1)
		{
			_currentSlide++;
		}
		StartCoroutine(RefreshEndingSlideshowDisplay());
	}

	private IEnumerator RefreshEndingSlideshowDisplay()
	{
		yield return _obscurerController.ShowObscurer();
		if (_currentSlide == 0)
		{
			_showTextButton.interactable = true;
			_showTextButton.GetComponent<Image>().raycastTarget = true;
			_showTextButtonClicked = false;
			_endingUiCanvasGroup.alpha = 0f;
			if (_questSlideTextBackground != null)
			{
				Color color = _questSlideTextBackground.color;
				color.a = 0.55f;
				_questSlideTextBackground.color = color;
			}
		}
		else if (_questSlideTextBackground != null)
		{
			Color color2 = _questSlideTextBackground.color;
			color2.a = 0f;
			_questSlideTextBackground.color = color2;
		}
		foreach (KeyValuePair<EndGameTextId, EndGameTextController> textController in textControllers)
		{
			textController.Value.SetText(string.Empty);
		}
		VisualsManager.Instance.ResetAllVisuals();
		if (_slideList.Count > 0)
		{
			yield return _slideList[_currentSlide].Run();
		}
		VisualsManager.Instance.UpdateAllVisuals();
		RefreshEndingButtons();
		yield return _obscurerController.HideObscurer();
	}

	public void DisplayText(EndGameTextId textId, string localizedText)
	{
		if (textControllers.ContainsKey(textId))
		{
			textControllers[textId].SetText(localizedText);
			return;
		}
		Debug.LogWarningFormat("Trying to display text for EndGameTextId {0} thats not registered in dictionary", textId);
	}

	private void RefreshEndingButtons()
	{
		_previousButton.SetActive(_currentSlide > 0);
		_nextButton.SetActive(_currentSlide < _slideList.Count - 1);
		_hideEndingButton.SetActive(_currentSlide == _slideList.Count - 1);
		_hideEndingButton.GetComponent<Button>().interactable = _currentSlide == _slideList.Count - 1;
	}

	public void OnShowTextButtonClicked()
	{
		if (!_showTextButtonClicked)
		{
			_showTextButtonClicked = true;
			StartCoroutine(OnShowTextButtonClickedCoroutine());
		}
	}

	public IEnumerator OnShowTextButtonClickedCoroutine()
	{
		while ((double)_endingUiCanvasGroup.alpha < 0.99)
		{
			_endingUiCanvasGroup.alpha += Time.deltaTime / _textFadeInTime;
			yield return new WaitForEndOfFrame();
		}
		_endingUiCanvasGroup.alpha = 1f;
		_showTextButton.interactable = false;
		_showTextButton.GetComponent<Image>().raycastTarget = false;
		_showTextButtonClicked = false;
		yield return null;
	}

	public void OnHideEndingButtonClick()
	{
		StartCoroutine(OnHideEndingButtonClickCoroutine());
	}

	public IEnumerator OnHideEndingButtonClickCoroutine()
	{
		yield return _obscurerController.ShowObscurer();
		_endingPanel.SetActive(value: false);
		VisualsManager.Instance.ResetAllVisuals();
		VisualsManager.Instance.UpdateAllVisuals();
		yield return ShowWinLoseSummary();
	}

	public void OnReturnToMenuButtonClick()
	{
		List<Scene> list = new List<Scene>();
		list.Add(SceneManager.GetSceneByName("Survival"));
		list.Add(SceneManager.GetSceneByName("EndOfGame"));
		Singleton<GameManager>.Instance.LoadMenu(list);
	}
}
