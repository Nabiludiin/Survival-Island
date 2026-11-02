using RG.Core.SaveSystem;
using Rewired;
using UnityEngine;

public class ConsoleGameManager : MonoBehaviour
{
	public static ConsoleGameManager I;

	public RichPresenceManager richPresence = new RichPresenceManager();

	private ConsoleUser _consoleUser;

	public static bool forceNewUser;

	private static bool alreadyInitialized;

	public ConsoleUser ConsoleUser
	{
		get
		{
			return _consoleUser;
		}
		set
		{
			_consoleUser = value;
		}
	}

	private void Awake()
	{
		if (!alreadyInitialized)
		{
			alreadyInitialized = true;
			Object.DontDestroyOnLoad(gameObject);
			I = this;
			ReInput.ControllerConnectedEvent += ReInput_ControllerConnectedEvent;
			ReInput.ControllerPreDisconnectEvent += ReInput_ControllerPreDisconnectEvent;
		}
		else
		{
			Object.Destroy(gameObject);
		}
	}

	private void Update()
	{
	}

	private void ReInput_ControllerConnectedEvent(ControllerStatusChangedEventArgs obj)
	{
	}

	private void ReInput_ControllerPreDisconnectEvent(ControllerStatusChangedEventArgs obj)
	{
	}
}
