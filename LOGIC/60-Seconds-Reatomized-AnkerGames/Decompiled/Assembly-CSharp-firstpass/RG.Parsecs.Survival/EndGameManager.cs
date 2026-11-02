using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using RG.Core.Base;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Menu;
using RG.VirtualInput;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RG.Parsecs.Survival;

public class EndGameManager : RGMonoBehaviour
{
	[SerializeField]
	private string _endOfGameSceneName = "EndOfGame";

	[SerializeField]
	private string _challengeConclusionSceneName = "ChallengeConclusion";

	[SerializeField]
	private ExpeditionData _expeditionData;

	private static EndGameManager _instance;

	[SerializeField]
	private bool _checkCaptainStatusForEndgameCondition = true;

	[SerializeField]
	private bool _checkCustomFunctionForEndgameCondition;

	[SerializeField]
	private bool _stopSurvivalSFXWhileLoadingEndOfGame;

	[SerializeField]
	private bool _checkIfChallenge;

	[SerializeField]
	private GlobalBoolVariable _isSurvivalChallenge;

	[SerializeField]
	private NodeFunction _functionCheckingEndgameCondition;

	[SerializeField]
	private EndGameData _endGameData;

	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	[SerializeField]
	private LocalizedString _theEndDayTitle;

	[SerializeField]
	private List<VirtualInputButton> _virtualInputButtonsToDisableOnEndOfGame = new List<VirtualInputButton>();

	[SerializeField]
	private MusicManager _musicManager;

	public static EndGameManager Instance
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
		_instance = Object.FindObjectOfType<EndGameManager>();
		if (_instance == null)
		{
			Debug.LogError("No EndGameManager in scene!");
		}
	}

	protected override void CustomAwake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Debug.LogWarning("Duplicate instance of EndGameManager detected! Removing " + gameObject.name);
			Object.Destroy(this);
		}
		_endOfDayListenerList.RegisterOnEndOfDay(OnEndDayVisual, "Goals", 4, this);
		_endOfDayListenerList.RegisterOnEndOfDay(CheckDisableVirtualButtonsOnEndOfGame, "Visuals", 11, this);
		_endGameData.Reset();
	}

	protected override void CustomOnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(OnEndDayVisual, "Goals");
		_endOfDayListenerList.UnregisterOnEndOfDay(CheckDisableVirtualButtonsOnEndOfGame, "Visuals");
	}

	private void OnEndDayVisual()
	{
		if (_checkCaptainStatusForEndgameCondition)
		{
			if (_characterList.GetCaptain().RuntimeData.HasEndGameStatus() && !_endGameData.RuntimeData.ShouldEndGame)
			{
				if (_expeditionData.RuntimeData.CurrentPlanetDeck == null)
				{
					StatsManager.Instance.AddGlobalData("DeadInSpace", 1);
				}
				_endGameData.RuntimeData.ShouldEndGame = true;
				_endGameData.RuntimeData.IsGameWon = false;
				EndOfDayManager.Instance.DayTitle = _theEndDayTitle;
			}
		}
		else if (_checkCustomFunctionForEndgameCondition)
		{
			_functionCheckingEndgameCondition.Execute(null);
		}
	}

	private void CheckDisableVirtualButtonsOnEndOfGame()
	{
		if (!_endGameData.RuntimeData.ShouldEndGame || _virtualInputButtonsToDisableOnEndOfGame.Count == 0)
		{
			return;
		}
		foreach (VirtualInputButton item in _virtualInputButtonsToDisableOnEndOfGame)
		{
			if (!(item == null) && !(item.Selectable == null))
			{
				item.Selectable.interactable = false;
			}
		}
	}

	public EndGameData GetEndGameData()
	{
		return _endGameData;
	}

	internal void EndGame(bool result)
	{
		_endGameData.RuntimeData.ShouldEndGame = true;
		_endGameData.RuntimeData.IsGameWon = result;
		EndOfDayManager.Instance.DayTitle = _theEndDayTitle;
	}

	public void LoadEndGameScene()
	{
		Singleton<GameManager>.Instance.RaycastCatcher.SetCatchingRaycasts(catchingRaycasts: true, null);
		if (BasePauseMenu.Instance != null)
		{
			BasePauseMenu.Instance.currentGameState = BasePauseMenu.EGameState.Menu;
		}
		if (!_checkIfChallenge || _isSurvivalChallenge == null || !_isSurvivalChallenge.Value || !_endGameData.RuntimeData.IsGameWon)
		{
			StartCoroutine(LoadEndGameSceneCoroutine());
		}
		else
		{
			StartCoroutine(LoadChallengeConclusionSceneCoroutine());
		}
	}

	private IEnumerator LoadEndGameSceneCoroutine()
	{
		yield return Singleton<GameManager>.Instance.LoadingManager.ObscurerController.ShowObscurer();
		AudioManager.Instance.StopPlayingMusicFadeOut();
		if (_stopSurvivalSFXWhileLoadingEndOfGame)
		{
			AudioManager.Instance.StopPlayingSfxFadeOut();
		}
		if (_musicManager != null)
		{
			_musicManager.StopMusic();
		}
		yield return SceneManager.LoadSceneAsync(_endOfGameSceneName, LoadSceneMode.Additive);
		yield return Singleton<GameManager>.Instance.LoadingManager.ObscurerController.HideObscurer();
		Singleton<GameManager>.Instance.RaycastCatcher.SetCatchingRaycasts(catchingRaycasts: false, null);
		Singleton<GameManager>.Instance.RaycastCatcher.SetClickawayBlocked(block: false);
	}

	private IEnumerator LoadChallengeConclusionSceneCoroutine()
	{
		yield return Singleton<GameManager>.Instance.LoadingManager.ObscurerController.ShowObscurer();
		AudioManager.Instance.StopPlayingMusicFadeOut();
		if (_stopSurvivalSFXWhileLoadingEndOfGame)
		{
			AudioManager.Instance.StopPlayingSfxFadeOut();
		}
		if (_musicManager != null)
		{
			_musicManager.StopMusic();
		}
		yield return SceneManager.LoadSceneAsync(_challengeConclusionSceneName, LoadSceneMode.Additive);
		yield return Singleton<GameManager>.Instance.LoadingManager.ObscurerController.HideObscurer();
		Singleton<GameManager>.Instance.RaycastCatcher.SetCatchingRaycasts(catchingRaycasts: false, null);
		Singleton<GameManager>.Instance.RaycastCatcher.SetClickawayBlocked(block: false);
	}
}
