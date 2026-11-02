using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class EngineControls : MonoBehaviour_ID_Network, SceneEventInterface
{
	public static List<EngineControls> AllEngineControls = new List<EngineControls>();

	public static bool AnchorsAreDown = false;

	private bool ownAnchorsAreDown;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private InteractableButton_Networked button_reversal;

	[SerializeField]
	private InteractableButton_Networked button_invertPower;

	[SerializeField]
	private InteractableButton_Networked button_anchorRemote;

	public EventReference ref_soundAnchorsDown;

	public EventReference ref_soundAnchorsUp;

	private InteractableButton_Network_Forwarder buttonForwarder;

	private Raft raft;

	private Raft_Network network;

	private bool hasBeenPlaced;

	public void Start()
	{
		raft = ComponentManager<Raft>.Value;
		network = ComponentManager<Raft_Network>.Value;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (hasBeenPlaced)
		{
			NetworkIDManager.RemoveNetworkID(this, typeof(EngineControls));
			AllEngineControls.Remove(this);
		}
	}

	public void Update()
	{
		if (hasBeenPlaced && AnchorsAreDown != ownAnchorsAreDown)
		{
			SetAnchorState(AnchorsAreDown);
		}
	}

	public void OnBlockPlaced()
	{
		hasBeenPlaced = true;
		AllEngineControls.Add(this);
		InteractableButton_Networked interactableButton_Networked = button_reversal;
		interactableButton_Networked.OnButtonPressed = (Action)Delegate.Combine(interactableButton_Networked.OnButtonPressed, new Action(ReverseDirection));
		InteractableButton_Networked interactableButton_Networked2 = button_invertPower;
		interactableButton_Networked2.OnButtonPressed = (Action)Delegate.Combine(interactableButton_Networked2.OnButtonPressed, new Action(InvertPower));
		InteractableButton_Networked interactableButton_Networked3 = button_anchorRemote;
		interactableButton_Networked3.OnButtonPressed = (Action)Delegate.Combine(interactableButton_Networked3.OnButtonPressed, new Action(ToggleRemoteAnchorsNetworked));
		buttonForwarder = new InteractableButton_Network_Forwarder(new InteractableButton_Networked[3] { button_reversal, button_invertPower, button_anchorRemote });
		NetworkIDManager.AddNetworkID(this, typeof(EngineControls));
		SetStateAnchorDownStateToAnchors();
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			if (AllEngineControls != null)
			{
				AllEngineControls.Clear();
			}
			AnchorsAreDown = false;
		}
	}

	private void SetStateAnchorDownStateToAnchors()
	{
		if (raft == null)
		{
			raft = ComponentManager<Raft>.Value;
		}
		if (AllEngineControls.Count <= 1 && raft.remoteAnchors.Count > 0)
		{
			if (raft.remoteAnchors[0].DroppingAnchor || raft.remoteAnchors[0].AtBottom)
			{
				SetAnchorState(state: true);
			}
			else
			{
				SetAnchorState(state: false);
			}
		}
	}

	private void ReverseDirection()
	{
		List<MotorWheel> motors = RaftVelocityManager.Motors;
		if (motors == null)
		{
			return;
		}
		foreach (MotorWheel item in motors)
		{
			item.OnButtonPressed_ChangeDirection();
		}
	}

	private void InvertPower()
	{
		List<MotorWheel> motors = RaftVelocityManager.Motors;
		if (motors == null)
		{
			return;
		}
		foreach (MotorWheel item in motors)
		{
			item.OnButtonPressed_ToggleEngine();
		}
	}

	private void ToggleRemoteAnchorsNetworked()
	{
		Message_NetworkBehaviour_ID_Int message = new Message_NetworkBehaviour_ID_Int(Messages.RemoteAnchorsDown, network.NetworkIDManager, ObjectIndex, (!AnchorsAreDown) ? 1 : 0);
		if (Raft_Network.IsHost)
		{
			SetAnchorState(!AnchorsAreDown);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void SetAnchorState(bool state)
	{
		AnchorsAreDown = state;
		ownAnchorsAreDown = AnchorsAreDown;
		if (hasBeenPlaced)
		{
			if (AnchorsAreDown)
			{
				RuntimeManager.PlayOneShot(ref_soundAnchorsDown, transform.position);
			}
			else
			{
				RuntimeManager.PlayOneShot(ref_soundAnchorsUp, transform.position);
			}
		}
		animator.SetBool("AnchorDown", AnchorsAreDown);
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block(RGDType.Block_EngineControls, GetComponent<Block>());
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.InteractableButtonPress:
		case Messages.InteractableButtonPress_Int:
		{
			Message_InteractableButtonPress msg2 = msg as Message_InteractableButtonPress;
			buttonForwarder.HandleMessage(msg2);
			return true;
		}
		case Messages.RemoteAnchorsDown:
			if (Raft_Network.IsHost)
			{
				ToggleRemoteAnchorsNetworked();
			}
			else
			{
				if ((msg as Message_NetworkBehaviour_ID_Int).IntegerValue == 0)
				{
					AnchorsAreDown = false;
				}
				else
				{
					AnchorsAreDown = true;
				}
				SetAnchorState(AnchorsAreDown);
			}
			return true;
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
