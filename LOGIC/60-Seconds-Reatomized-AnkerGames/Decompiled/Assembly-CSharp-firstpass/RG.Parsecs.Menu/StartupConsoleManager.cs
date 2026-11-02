using System.Collections;
using I2.Loc;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using Rewired;
using UnityEngine;
using UnityEngine.Events;

namespace RG.Parsecs.Menu;

public class StartupConsoleManager : MonoBehaviour
{
	public UnityEvent onStart;

	public UnityEvent onStartOrFailed;

	public UnityEvent onWait;

	public UnityEvent onComplete;

	private Player _inputPlayer;

	private bool _isLoading;

	private const int FIRST_PLAYER_GAMEPAD_ID = 0;

	private const int NOT_LOGGED_CONSOLE_USER_ID = 0;

	private void Start()
	{
		_inputPlayer = ReInput.players.GetPlayer(0);
		onStart.Invoke();
		onStartOrFailed.Invoke();
	}

	private void Update()
	{
		if (!_isLoading)
		{
			_isLoading = true;
			onWait.Invoke();
			ConsoleUser consoleUser = new ConsoleUser();
			Debug.Log("Steamworks: " + SteamManager.Initialized);
			if (consoleUser.ConsolePlayerId != 0)
			{
				StorageDataManager.TheInstance.Initialize(consoleUser, LoadMenu);
			}
		}
		if (!_isLoading && _inputPlayer.GetButtonDown("DeveloperMenu"))
		{
			_isLoading = true;
			onWait.Invoke();
			ConsoleUser consoleUser2 = new ConsoleUser();
			if (consoleUser2.ConsolePlayerId != 0)
			{
				StorageDataManager.TheInstance.Initialize(consoleUser2, LoadDeveloperMenu);
			}
		}
	}

	private void LoadDeveloperMenu()
	{
		Singleton<GameManager>.Instance.LoadDeveloperMenu();
	}

	private void LoadMenu()
	{
		StartCoroutine(CompleteInitialization());
	}

	private IEnumerator CompleteInitialization()
	{
		LocalizationManager.InitializeIfNeeded();
		onComplete.Invoke();
		yield return null;
	}
}
