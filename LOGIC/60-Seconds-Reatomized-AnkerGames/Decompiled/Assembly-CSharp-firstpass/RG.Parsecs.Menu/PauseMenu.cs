using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using RG.Parsecs.Common;
using RG.Parsecs.Scavenge;
using RG.Parsecs.Survival;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RG.Parsecs.Menu;

public class PauseMenu : BasePauseMenu
{
	public GamepadPanelPause GamepadPanelPause;

	private bool _paused;

	public GameObject restartBtn;

	public Canvas pausePanel;

	public Canvas popupPanel;

	private Bus _animationBus;

	[SerializeField]
	private string ANIMATION_BUS = "bus:/Master/Animation";

	private Bus _musicBus;

	[SerializeField]
	private string MUSIC_BUS = "bus:/Master/Music";

	private Bus _sfxBus;

	[SerializeField]
	private string SFX_BUS = "bus:/Master/SFX";

	[SerializeField]
	private bool _hasFocus = true;

	protected new void Awake()
	{
		base.Awake();
		_musicBus = RuntimeManager.GetBus(MUSIC_BUS);
		_sfxBus = RuntimeManager.GetBus(SFX_BUS);
		_animationBus = RuntimeManager.GetBus(ANIMATION_BUS);
	}

	private void Update()
	{
		Player player = ReInput.players.GetPlayer(0);
		if (currentGameState == EGameState.GamePlay && player.GetButtonDown(12) && !_locked)
		{
			TogglePauseGame();
		}
		if (!_hasFocus && currentGameState == EGameState.GamePlay && !_paused && !_locked)
		{
			_hasFocus = true;
			TogglePauseGame();
		}
	}

	private void OnEnable()
	{
	}

	public override void SetPause(bool pause)
	{
		Debug.Log("This method does nothing in 60Parsecs!");
	}

	public override void TogglePauseGame()
	{
		if (!_paused)
		{
			MCTime.PauseTimeScale = 0f;
			_paused = true;
		}
		else
		{
			MCTime.PauseTimeScale = 1f;
			_paused = false;
		}
		if (currentGameState == EGameState.GamePlay)
		{
			if (ShipGenerator.I != null && Singleton<HandCursor>.Instance != null)
			{
				Singleton<HandCursor>.Instance.MouseCursorVisible = _paused;
				if (!Singleton<HandCursor>.Instance.isPointerMode)
				{
					Singleton<HandCursor>.Instance.HandSpriteVisible = false;
				}
			}
			pausePanel.gameObject.SetActive(_paused);
			restartBtn.SetActive(ShipGenerator.I != null);
			if (GamepadPanelPause != null)
			{
				if (_paused)
				{
					GamepadPanelPause.Show();
				}
				else
				{
					GamepadPanelPause.Hide();
				}
			}
		}
		_musicBus.setPaused(_paused);
		_animationBus.setPaused(_paused);
		_sfxBus.setPaused(_paused);
		AudioListener.pause = _paused;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		_hasFocus = hasFocus;
	}

	public void QuitToMenu()
	{
		if (_paused)
		{
			TogglePauseGame();
		}
		if (!_musicBus.isValid())
		{
			_musicBus = RuntimeManager.GetBus(MUSIC_BUS);
		}
		if (!_animationBus.isValid())
		{
			_animationBus = RuntimeManager.GetBus(ANIMATION_BUS);
		}
		if (!_sfxBus.isValid())
		{
			_sfxBus = RuntimeManager.GetBus(SFX_BUS);
		}
		_animationBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		_musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		_sfxBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (Singleton<GameManager>.Instance != null && Singleton<GameManager>.Instance.RaycastCatcher != null)
		{
			Singleton<GameManager>.Instance.RaycastCatcher.SetCatchingRaycasts(catchingRaycasts: false, null);
		}
		if (ShipGenerator.I != null)
		{
			ShipGenerator.I.IsUnloading = true;
			List<Scene> scavangeScenes = ShipGenerator.I.GetScavangeScenes();
			Singleton<GameManager>.Instance.LoadMenu(scavangeScenes);
		}
		else
		{
			Singleton<GameManager>.Instance.LoadMenu();
		}
	}

	public void Restart()
	{
		if (_paused)
		{
			TogglePauseGame();
		}
		if (!_animationBus.isValid())
		{
			_animationBus = RuntimeManager.GetBus(ANIMATION_BUS);
		}
		_animationBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (!_musicBus.isValid())
		{
			_musicBus = RuntimeManager.GetBus(MUSIC_BUS);
		}
		_musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (!_sfxBus.isValid())
		{
			_sfxBus = RuntimeManager.GetBus(SFX_BUS);
		}
		_sfxBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (ShipGenerator.I != null)
		{
			ShipGenerator.I.IsUnloading = true;
			List<Scene> scavangeScenes = ShipGenerator.I.GetScavangeScenes();
			Singleton<GameManager>.Instance.StartScavenge(scavangeScenes);
		}
		else
		{
			Singleton<GameManager>.Instance.StartScavenge();
		}
	}
}
