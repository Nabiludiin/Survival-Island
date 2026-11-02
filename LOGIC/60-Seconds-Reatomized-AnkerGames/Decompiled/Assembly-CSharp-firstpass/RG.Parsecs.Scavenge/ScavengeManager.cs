using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Menu;
using RG.Parsecs.Survival;
using Rewired;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace RG.Parsecs.Scavenge;

public class ScavengeManager : MonoBehaviour, IHaveEventHub<ScavengeManager, EventHubArgument<ScavengeManagerEvents>, ScavengeManagerEvents>, IActorBrain
{
	protected EventHub<ScavengeManager, EventHubArgument<ScavengeManagerEvents>, ScavengeManagerEvents> _eventsHub = new DictionaryEventHub<ScavengeManager, EventHubArgument<ScavengeManagerEvents>, ScavengeManagerEvents>(default(ScavengeManagerEventsComparer));

	public ScavengePlayersManager ScavengePlayersManager;

	public GameObject ScavengeSummaryScreen;

	public ScavengeAchievementsDatabase ScavengeAchievementsDatabase;

	public PlayableDirector PlayableIntro;

	public PlayableDirector PlayableWinOutro;

	public PlayableDirector PlayableLoseOutro;

	public AlertPanel AlertPanel;

	public GlobalBoolVariable IsTutorial;

	public GlobalBoolVariable TutorialCompleted;

	public ShipGenerator ShipGenerator;

	public float ScavengeTime = 60f;

	public float BaseExplorationTime;

	public string MusicSoundPath = "event:/Scavenge/SFX/ScavengeTheme";

	public string AlarmSoundPath = "event:/Scavenge/SFX/BaseAlert";

	public ScavengeClock clock;

	public float DropDelay = 0.5f;

	public float PerWeightDelay = 0.2f;

	public float PickUpDelay = 0.5f;

	public float LostStopMovementDelay = 1.2f;

	public CrewList CrewList;

	public int AllowJumpSecondsLeft = 5;

	private ScavengeTutorialDriver _tutorialDriver;

	private bool _loaded;

	private bool _won;

	private Coroutine _theHunt;

	[SerializeField]
	private SaveEvent gameProgressSaveEvent;

	[SerializeField]
	private SaveEvent statsSaveEvent;

	[SerializeField]
	private GlobalBoolVariable isContinueAvailable;

	private const string MASTER_BUS = "bus:/Master";

	private const string SFX_BUS = "bus:/Master/SFX";

	public Transform _positionMarker;

	public virtual EventHub<ScavengeManager, EventHubArgument<ScavengeManagerEvents>, ScavengeManagerEvents> EventHub => _eventsHub;

	public RescurePodEntrance RescurePodEntrance { get; protected set; }

	public bool Loaded => _loaded;

	public int CurrentTime => clock.currentTime;

	public ScavengeState ScavengeState { get; protected set; }

	public bool GameWon => _won;

	public Transform Transform => transform;

	public GameObject GameObject => gameObject;

	public string Name => gameObject.name;

	public Transform PositionMarker => _positionMarker;

	public Player Player => null;

	public WaitForSeconds GetDropDelay()
	{
		return new WaitForSeconds(DropDelay);
	}

	public float GetPerWeightDelay()
	{
		return PerWeightDelay;
	}

	public float GetPickUpDelay()
	{
		return PickUpDelay;
	}

	protected virtual void Awake()
	{
		if (BasePauseMenu.Instance != null)
		{
			BasePauseMenu.Instance.currentGameState = BasePauseMenu.EGameState.Menu;
		}
		GlobalScavengeManagerEventsHub.I.Subscribe(this);
		ScavengeState = ScavengeState.Loading;
		_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.StartedLoading));
		StartCoroutine(Load());
	}

	private IEnumerator Load()
	{
		if (Singleton<HandCursor>.Instance != null)
		{
			Singleton<HandCursor>.Instance.MouseCursorVisible = false;
			Singleton<HandCursor>.Instance.RefreshCaptainCursor();
		}
		bool value = IsTutorial.Value;
		RuntimeManager.LoadBank("SFX_Scavenge", loadSamples: true);
		if (value)
		{
			yield return ShipGenerator.GenerateTutorialShip();
			_tutorialDriver = ShipGenerator.GetTutorialDriver();
			_tutorialDriver.InitTutorial(this);
			ScavengePlayersManager.SpawnPoints[0] = _tutorialDriver.SpawnPoint;
		}
		else
		{
			yield return ShipGenerator.GenerateShip();
		}
		ShipGenerator.DisableItems();
		RescurePodEntrance = ShipGenerator.GetRescurePodEntrance();
		ScavengePlayersManager.Init(this);
		yield return new WaitForSeconds(1f);
		_loaded = true;
		_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.FinishedLoading));
		yield return Intro();
	}

	public void StartGameplay()
	{
		StartCoroutine(GameplayFlow());
	}

	private IEnumerator GameplayFlow()
	{
		float explorationTime = ScavengePlayersManager.GetExplorationTime(BaseExplorationTime);
		ScavengePlayersManager.StartTheHunt();
		if (BasePauseMenu.Instance != null)
		{
			BasePauseMenu.Instance.currentGameState = BasePauseMenu.EGameState.GamePlay;
		}
		yield return Exploration(explorationTime);
		_theHunt = StartCoroutine(TheHunt(ScavengeTime));
		yield return _theHunt;
		yield return Outro();
	}

	private IEnumerator Intro()
	{
		ScavengeState = ScavengeState.Intro;
		_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.StartedIntro));
		PlayableIntro.Play();
		yield return null;
	}

	private IEnumerator Exploration(float explorationTime)
	{
		ScavengeState = ScavengeState.Exploration;
		_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.StartedExploration));
		ScavengePlayersManager.UnpauseGameplay();
		if (_tutorialDriver != null)
		{
			ScavengeState = ScavengeState.Tutorial;
			RescurePodEntrance.SendEvent(RescurePodEntranceEvents.TheHuntStarted);
			yield return StartCoroutine(_tutorialDriver.TutorialExploration());
		}
		else
		{
			ShipGenerator.EnableItems();
			yield return StartCoroutine(clock.StartCount(-1 * (int)explorationTime, (int)explorationTime, withRedIndicator: false));
		}
	}

	public IEnumerator TheHunt(float scavengeTime)
	{
		ScavengeState = ScavengeState.Hunt;
		_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.StartedvHunt));
		RescurePodEntrance.SendEvent(RescurePodEntranceEvents.TheHuntStarted);
		if (!ShipGenerator.I.IsUnloading)
		{
			if (!string.IsNullOrEmpty(MusicSoundPath))
			{
				RuntimeManager.PlayOneShot(MusicSoundPath);
			}
			if (!string.IsNullOrEmpty(AlarmSoundPath))
			{
				RuntimeManager.PlayOneShot(AlarmSoundPath);
			}
		}
		yield return StartCoroutine(clock.StartCount(0, (int)scavengeTime, withRedIndicator: true));
		RescurePodEntrance.SendEvent(RescurePodEntranceEvents.TimeOut);
		yield return null;
	}

	private IEnumerator Outro()
	{
		RuntimeManager.GetBus("bus:/Master/SFX").stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		ScavengeState = ScavengeState.Outro;
		_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.StartedOutro));
		if (ScavengePlayersManager.IsNearRescuePod())
		{
			ScavengePlayersManager.StopMovement();
			_won = true;
			ScavengePlayersManager.Jump(RescurePodEntrance.GetPosition());
			_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.StartedOutroWon));
			AddStats(isSuccessful: true);
			PlayableWinOutro.Play();
		}
		else
		{
			if (_tutorialDriver != null)
			{
				ScavengeState = ScavengeState.Tutorial;
				AlertPanel.Stop();
				clock.Hide();
				RescurePodEntrance.SendEvent(RescurePodEntranceEvents.TheHuntStarted);
				_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.TutorialRestart));
				yield return StartCoroutine(_tutorialDriver.TutorialFailedExploration());
				_theHunt = StartCoroutine(TheHunt(ScavengeTime));
				yield return _theHunt;
				yield return Outro();
				yield break;
			}
			_won = false;
			_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.StartedOutroLost));
			AddStats(isSuccessful: false);
			StartCoroutine(DelayedStopMovement());
			PlayableLoseOutro.Play();
		}
		yield return null;
	}

	private IEnumerator DelayedStopMovement()
	{
		yield return new WaitForSeconds(LostStopMovementDelay);
		ScavengePlayersManager.StopMovement();
	}

	public void PauseGameplay()
	{
		ScavengePlayersManager.PauseGameplay();
	}

	public void UnpauseGameplay()
	{
	}

	public void ShowSummary()
	{
		Debug.Log("before save");
		if (IsTutorial.Value && _won)
		{
			TutorialCompleted.Value = true;
		}
		StorageDataManager.TheInstance.Save(gameProgressSaveEvent.DataTag, () =>
		{
			isContinueAvailable.Value = _won;
			StorageDataManager.TheInstance.Save(statsSaveEvent.DataTag, () =>
			{
				ShowSummaryAfterSave();
			}, () =>
			{
				ShowSummaryAfterSave();
			});
		}, () =>
		{
			ShowSummaryAfterSave();
		});
	}

	private void AddStats(bool isSuccessful)
	{
		if (isSuccessful)
		{
			ScavengeAchievementsDatabase.CheckDontPanic();
		}
		if (!IsTutorial.Value)
		{
			ScavengeEntry entry = new ScavengeEntry
			{
				IsSuccessful = isSuccessful
			};
			ScavangeItemsDatabase itemsDatabase = ShipGenerator.I.itemsDistribute.itemsDatabase;
			if (isSuccessful)
			{
				List<string> list = new List<string>();
				entry.LeftForDead = Mathf.Max(5 - CrewList.crew.GetCharacterCount(), 1);
				entry.TotalItems = itemsDatabase.TotalItemsGathered(list);
				entry.TotalSoups = itemsDatabase.TotalSoupsGathered();
				entry.ItemsIds = list;
			}
			StatsManager.Instance.AddScavengeStats(entry);
			if (isSuccessful)
			{
				ScavengeAchievementsDatabase.CheckHandsFull(StatsManager.Instance.GetGlobalData("ScavengedItemsIDs"));
				ScavengeAchievementsDatabase.CheckSoupOdyssey(StatsManager.Instance.GetGlobalData("ScavengedSoups"));
				ScavengeAchievementsDatabase.CheckAllSeatsTaken(CrewList.crew.GetCharacterCount());
			}
		}
	}

	private void ShowSummaryAfterSave()
	{
		Debug.Log("after save");
		ScavengeState = ScavengeState.Summary;
		if (BasePauseMenu.Instance != null)
		{
			BasePauseMenu.Instance.currentGameState = BasePauseMenu.EGameState.Menu;
		}
		if (_won)
		{
			_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.Won));
			List<Scene> scavangeScenes = ShipGenerator.I.GetScavangeScenes();
			Singleton<GameManager>.Instance.StartSurvival(scavangeScenes);
			return;
		}
		if (Singleton<HandCursor>.Instance != null)
		{
			Singleton<HandCursor>.Instance.MouseCursorVisible = true;
			if (!Singleton<HandCursor>.Instance.isPointerMode)
			{
				Singleton<HandCursor>.Instance.HandSpriteVisible = false;
			}
		}
		_eventsHub.Invoke(this, new EventHubArgument<ScavengeManagerEvents>(ScavengeManagerEvents.Lost));
		ScavengeSummaryScreen.SetActive(value: true);
	}

	internal void SkipToOutro()
	{
		StopCoroutine(_theHunt);
		RescurePodEntrance.SendEvent(RescurePodEntranceEvents.TimeOut);
		RuntimeManager.GetBus("bus:/Master").stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		StartCoroutine(Outro());
	}

	internal bool CanActiveJump()
	{
		if (ScavengeState == ScavengeState.Hunt && clock.currentTime > 0)
		{
			return clock.currentTime <= AllowJumpSecondsLeft;
		}
		return false;
	}

	private void OnDestroy()
	{
		_tutorialDriver = null;
		RescurePodEntrance = null;
	}
}
