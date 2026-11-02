using UnityEngine;

namespace RG.Parsecs.Menu;

public abstract class BasePauseMenu : MonoBehaviour
{
	public enum EGameState
	{
		Menu,
		GamePlay
	}

	private static BasePauseMenu _instance;

	protected bool _locked;

	public EGameState currentGameState;

	public static BasePauseMenu Instance => _instance;

	protected void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			Object.DontDestroyOnLoad(this);
		}
		else
		{
			Debug.LogError("Other pause menu on scene");
			Object.Destroy(this);
		}
	}

	public void SetPauseLocked(bool locked)
	{
		_locked = locked;
	}

	public abstract void TogglePauseGame();

	public abstract void SetPause(bool paused);
}
