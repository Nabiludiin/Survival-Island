using RG.Parsecs.Common;
using RG.Parsecs.Menu;
using RG.VirtualInput;
using UnityEngine;

namespace RG.Remaster.Scavenge;

public class RestartPanelController : MonoBehaviour
{
	public void Start()
	{
		Singleton<VirtualInputManager>.Instance.VisualManager.MouseCursorVisible = true;
	}

	private void OnEnable()
	{
		BasePauseMenu.Instance.SetPauseLocked(locked: true);
	}

	private void OnDisable()
	{
		BasePauseMenu.Instance.SetPauseLocked(locked: false);
	}

	public void Restart()
	{
		BasePauseMenu.Instance.SetPauseLocked(locked: false);
		AudioManager.Instance.StopPlayingMusicFadeOut();
		AudioManager.Instance.StopPlayingSfxFadeOut();
		ResetGame.RestartLevel();
	}

	public void Exit()
	{
		BasePauseMenu.Instance.SetPauseLocked(locked: false);
		AudioManager.Instance.StopPlayingMusicFadeOut();
		AudioManager.Instance.StopPlayingSfxFadeOut();
		Singleton<GameManager>.Instance.ResetGame();
	}
}
