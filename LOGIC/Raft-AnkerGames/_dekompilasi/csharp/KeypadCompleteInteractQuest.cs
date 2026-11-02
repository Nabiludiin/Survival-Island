using System;
using UnityEngine;

public class KeypadCompleteInteractQuest : MonoBehaviour
{
	[SerializeField]
	private Keypad connectedKeypad;

	[SerializeField]
	private QuestEventBase questEventBase;

	private void Start()
	{
		if (Raft_Network.IsHost)
		{
			Keypad keypad = connectedKeypad;
			keypad.OnCodeComplete = (Action)Delegate.Combine(keypad.OnCodeComplete, new Action(OnKeypadComplete));
		}
	}

	private void OnDestroy()
	{
		if (Raft_Network.IsHost)
		{
			Keypad keypad = connectedKeypad;
			keypad.OnCodeComplete = (Action)Delegate.Remove(keypad.OnCodeComplete, new Action(OnKeypadComplete));
		}
	}

	private void OnKeypadComplete()
	{
		questEventBase.SendOnInteractNetwork(null, forceSuccess: true);
	}
}
