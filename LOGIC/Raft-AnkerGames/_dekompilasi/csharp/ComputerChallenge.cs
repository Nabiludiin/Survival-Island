using System;
using Steamworks;
using UnityEngine;

public class ComputerChallenge : MonoBehaviour_ID_Network
{
	[SerializeField]
	private float maxTimer = 15f;

	[SerializeField]
	private QuestInteractable lockedDoor;

	[SerializeField]
	private Keypad_ComputerChallenge[] computerKeypads;

	private Raft_Network network;

	private float timer;

	private bool isComplete;

	public float ChallengeTimer => timer;

	public bool IsComplete => isComplete;

	private void Awake()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Keypad_ComputerChallenge[] array = computerKeypads;
		foreach (Keypad_ComputerChallenge keypad in array)
		{
			Keypad_ComputerChallenge keypad_ComputerChallenge = keypad;
			keypad_ComputerChallenge.OnCodeComplete = (Action)Delegate.Combine(keypad_ComputerChallenge.OnCodeComplete, (Action)(() =>
			{
				OnKeyPadComplete(keypad);
			}));
			keypad.AssignComputerChallengeReference(this);
		}
	}

	private new void OnDestroy()
	{
		network = null;
		computerKeypads = null;
	}

	private void Update()
	{
		if (!(timer > 0f) || IsComplete)
		{
			return;
		}
		timer -= Time.deltaTime;
		if (timer <= 0f && Raft_Network.IsHost)
		{
			if (network == null)
			{
				network = ComponentManager<Raft_Network>.Value;
			}
			network.RPC(new Message_NetworkBehaviour_ID(Messages.ComputerChallenge_ResetTimer, network.NetworkIDManager, ObjectIndex), Target.All, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			ResetTimer();
		}
	}

	private void OnKeyPadComplete(Keypad_ComputerChallenge keypad)
	{
		if (keypad == null || isComplete)
		{
			return;
		}
		keypad.SetCodeComplete();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		bool flag = true;
		Keypad_ComputerChallenge[] array = computerKeypads;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].CodeCompleted)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			if (timer <= 0f)
			{
				if (network == null)
				{
					network = ComponentManager<Raft_Network>.Value;
				}
				network.RPC(new Message_NetworkBehaviour_ID(Messages.ComputerChallenge_StartTimer, network.NetworkIDManager, ObjectIndex), Target.All, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				StartTimer();
			}
		}
		else
		{
			if (network == null)
			{
				network = ComponentManager<Raft_Network>.Value;
			}
			network.RPC(new Message_NetworkBehaviour_ID(Messages.ComputerChallenge_Complete, network.NetworkIDManager, ObjectIndex), Target.All, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			CompleteChallenge();
		}
	}

	private void StartTimer()
	{
		timer = maxTimer;
	}

	private void ResetTimer()
	{
		timer = 0f;
		Keypad_ComputerChallenge[] array = computerKeypads;
		foreach (Keypad_ComputerChallenge keypad_ComputerChallenge in array)
		{
			if (keypad_ComputerChallenge.CodeCompleted)
			{
				keypad_ComputerChallenge.ResetCodeCompleted();
			}
		}
	}

	private void CompleteChallenge()
	{
		isComplete = true;
		timer = 0f;
		Keypad_ComputerChallenge[] array = computerKeypads;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnCompleteChallenge();
		}
		lockedDoor.SetNewObjectStateIndex(1);
	}

	public RGD_LandmarkItem_ComputerChallenge GetRGDSave()
	{
		return new RGD_LandmarkItem_ComputerChallenge(GetComponent<LandmarkItem>())
		{
			complete = isComplete
		};
	}

	public void RestoreRGDSave(RGD_LandmarkItem_ComputerChallenge rgd)
	{
		if (rgd.complete)
		{
			CompleteChallenge();
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.ComputerChallenge_StartTimer:
			StartTimer();
			return false;
		case Messages.ComputerChallenge_ResetTimer:
			ResetTimer();
			return false;
		case Messages.ComputerChallenge_Complete:
			CompleteChallenge();
			return false;
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
