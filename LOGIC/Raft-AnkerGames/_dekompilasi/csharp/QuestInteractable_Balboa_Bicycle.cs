using FMODUnity;
using Steamworks;
using UnityEngine;

public class QuestInteractable_Balboa_Bicycle : QuestInteractable
{
	[SerializeField]
	private Interval_Float plingInterval;

	[SerializeField]
	public StudioEventEmitter plingSoundEmitter;

	[SerializeField]
	private float currentPingInterval = 1f;

	[SerializeField]
	private float currentPlingTimer;

	private void Update()
	{
		if (Raft_Network.IsHost && CurrentObjectStateIndex == 0)
		{
			currentPlingTimer += Time.deltaTime;
			if (currentPlingTimer >= currentPingInterval)
			{
				Pling();
			}
		}
	}

	private void Pling()
	{
		if (plingSoundEmitter != null)
		{
			plingSoundEmitter.PlaySafe();
		}
		if (Raft_Network.IsHost)
		{
			ResetTimer();
			Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.Balboa_BicyclePling, Network.NetworkIDManager, ObjectIndex);
			Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void ResetTimer()
	{
		currentPingInterval = plingInterval.GetRandomValue();
		currentPlingTimer = 0f;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Balboa_BicyclePling)
		{
			Pling();
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
