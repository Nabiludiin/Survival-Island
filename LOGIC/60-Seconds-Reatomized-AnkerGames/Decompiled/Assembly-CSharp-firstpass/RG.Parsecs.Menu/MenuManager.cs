using FMODUnity;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityEngine.Events;

namespace RG.Parsecs.Menu;

public class MenuManager : MonoBehaviour
{
	[SerializeField]
	private string MASTER_BUS = "bus:/Master";

	public GlobalBoolVariable itsTutorial;

	public GlobalBoolVariable tutorialCompleted;

	public GlobalBoolVariable continueAvailable;

	public GlobalBoolVariable onlySurvival;

	public GameObject[] computerScreens;

	public SaveEvent gameSaveEvent;

	public UnityEvent onComplete;

	public UnityEvent startTutorial;

	public UnityEvent normalMenu;

	public IconManager continueIconManager;

	public IconManager newGameIconManager;

	public OptionsManager optionsManager;

	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private Mission _survivalMission;

	public GlobalBoolVariable menuFirstTime;

	[EventRef]
	[SerializeField]
	private string mainMenuTheme = "event:/Menu/MainTheme";

	public string MainMenuTheme => mainMenuTheme;

	protected CharacterList CharacterList => _characterList;

	protected Mission SurvivalMission => _survivalMission;

	private void Start()
	{
		InitComplete();
	}

	public void RefreshInteractable()
	{
		if (!continueAvailable.Value)
		{
			continueIconManager.SetNotInteractable();
		}
	}

	public virtual void InitComplete()
	{
		optionsManager.InitializeFmodBus();
		optionsManager.SetVolumes();
		if (!continueAvailable.Value)
		{
			continueIconManager.SetNotInteractable();
		}
		ResetData();
		SetMouseCursorVisible(visible: true);
		if (!Singleton<HandCursor>.Instance.isPointerMode)
		{
			Singleton<HandCursor>.Instance.HandSpriteVisible = false;
			if (continueAvailable.Value)
			{
				continueIconManager.Select();
			}
			else
			{
				newGameIconManager.Select();
			}
		}
		RefreshRaptainCursor();
		onComplete.Invoke();
		if (!tutorialCompleted.Value)
		{
			startTutorial.Invoke();
		}
		else
		{
			normalMenu.Invoke();
		}
		if (menuFirstTime.Value)
		{
			menuFirstTime.Value = false;
		}
		AudioManager.Instance.PlayMusicFadeOut(mainMenuTheme);
	}

	public virtual void ResetData()
	{
		gameSaveEvent.ResetGame();
		itsTutorial.ResetData();
		onlySurvival.ResetData();
	}

	public void LoadTutorial()
	{
		StopMusic();
		itsTutorial.Value = true;
		MissionManager.Instance.SetActualMission(_survivalMission);
		Singleton<GameManager>.Instance.StartScavenge();
	}

	public virtual void StartNewGame()
	{
		StopMusic();
		MissionManager.Instance.SetActualMission(_survivalMission);
		Singleton<GameManager>.Instance.StartScavenge();
	}

	public virtual void StartSurvival()
	{
		StopMusic();
		onlySurvival.Value = true;
		Character captain = _characterList.GetCharacterSet.GetCaptain();
		_characterList.AddCharToList(captain);
		MissionManager.Instance.SetActualMission(_survivalMission);
		Singleton<GameManager>.Instance.StartSurvival();
	}

	public void ContinueGame()
	{
		StopMusic();
		Singleton<GameManager>.Instance.StartSurvival(gameSaveEvent.DataTag);
	}

	public void SaveSettings()
	{
		StorageDataManager.TheInstance.Save("settings", () =>
		{
			Debug.Log("done");
		}, null);
	}

	public void ShowComputerScreen(int id)
	{
		for (int i = 0; i < computerScreens.Length; i++)
		{
			if (i == id)
			{
				computerScreens[i].SetActive(value: true);
			}
			else
			{
				computerScreens[i].SetActive(value: false);
			}
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	protected void StopMusic()
	{
		AudioManager.Instance.StopPlayingMusicFadeOut();
	}

	public void PlayAudio(string MusicSoundPath)
	{
		AudioManager.PlaySound(MusicSoundPath);
	}

	public void SetMouseCursorVisible(bool visible)
	{
		if (Singleton<HandCursor>.Instance != null)
		{
			Singleton<HandCursor>.Instance.MouseCursorVisible = visible;
		}
	}

	public void RefreshRaptainCursor()
	{
		if (Singleton<HandCursor>.Instance != null)
		{
			Singleton<HandCursor>.Instance.RefreshCaptainCursor();
		}
	}

	public void InstantlyHideObscurer()
	{
		if (!(Singleton<GameManager>.Instance == null))
		{
			Singleton<GameManager>.Instance.LoadingManager.ObscurerController.InstantlyHideObscurer();
		}
	}

	public void HideObscurer()
	{
		if (!(Singleton<GameManager>.Instance == null))
		{
			StartCoroutine(Singleton<GameManager>.Instance.LoadingManager.ObscurerController.HideObscurer());
		}
	}

	public void InstantlyShowObscurer()
	{
		if (!(Singleton<GameManager>.Instance == null))
		{
			Singleton<GameManager>.Instance.LoadingManager.ObscurerController.InstantlyShowObscurer();
		}
	}
}
