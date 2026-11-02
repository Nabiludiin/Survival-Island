using Steamworks;
using UnityEngine;

[RequireComponent(typeof(LandmarkItem_ChallengePipe))]
[RequireComponent(typeof(Challenge_Pipe))]
public class RotationComponent_Auto : RotationComponent
{
	private enum RotationMode
	{
		Linear,
		AnimationCurve
	}

	private enum NetworkUpdateMode
	{
		Automatic,
		Parameters
	}

	public float automaticRotationSpeed = 1f;

	[SerializeField]
	private NetworkUpdateMode networkUpdateMode;

	[SerializeField]
	private float networkSendRateSeconds = 1f;

	[SerializeField]
	private float clientSideMinimumPlayerDistanceToSnapToHostRotation;

	private float lastSendTimeStamp;

	private void Update()
	{
		float num = GetRotationSpeed();
		if (invertRotation)
		{
			num *= -1f;
		}
		if (networkUpdateMode == NetworkUpdateMode.Automatic && Raft_Network.IsHost)
		{
			RotateNetworked(num * Time.deltaTime);
		}
		else
		{
			Rotate(num * Time.deltaTime);
		}
		if (Raft_Network.IsHost && networkUpdateMode == NetworkUpdateMode.Parameters)
		{
			HandleHostSending();
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.RotationComponentRequestRotation)
		{
			Message_RotationComponentSetRotation msg2 = msg as Message_RotationComponentSetRotation;
			HandleClientSideRotationRequest(msg2);
		}
		return base.Deserialize(msg, remoteID);
	}

	private void HandleHostSending()
	{
		if (Time.time - lastSendTimeStamp > networkSendRateSeconds)
		{
			lastSendTimeStamp = Time.time;
			Message_RotationComponentSetRotation message = new Message_RotationComponentSetRotation(Messages.RotationComponentRequestRotation, network.NetworkIDManager, targetTrackedNetworkID.ObjectIndex, rotationTransform.localEulerAngles);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private float GetRotationSpeed()
	{
		return automaticRotationSpeed;
	}

	private void HandleClientSideRotationRequest(Message_RotationComponentSetRotation msg)
	{
		if (networkUpdateMode == NetworkUpdateMode.Parameters)
		{
			if (!(localPlayer == null) && Vector3.Distance(localPlayer.FeetPosition, rotationTransform.position) > clientSideMinimumPlayerDistanceToSnapToHostRotation)
			{
				SetRotation(msg.Euler);
			}
		}
		else
		{
			Debug.LogError("Somehow rotation compnent set to update reciever, rotation request message, this is not intented", gameObject);
		}
	}
}
