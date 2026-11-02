using System;
using Steamworks;
using UnityEngine;

public class FillCounter : MonoBehaviour
{
	public Action OnIncrement;

	[SerializeField]
	private float maxAmount = 30f;

	private float currentAmount;

	public MonoBehaviour_ID_Network networkReciever;

	private Raft_Network network;

	public float NormalizedAmount => Mathf.Clamp01(currentAmount / maxAmount);

	public bool IsFull => currentAmount >= maxAmount;

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	public void SendOnIncrement()
	{
		OnIncrement?.Invoke();
	}

	public void IncrementNetworked(float amount)
	{
		if (Raft_Network.IsHost)
		{
			IncrementLocal(amount);
		}
		if (networkReciever != null)
		{
			Message_Network_Increment message_Network_Increment = new Message_Network_Increment(Messages.Increment, network.NetworkIDManager, networkReciever.ObjectIndex, amount);
			if (Raft_Network.IsHost)
			{
				message_Network_Increment.amount = currentAmount;
				network.RPC(message_Network_Increment, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message_Network_Increment, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public bool OnCounterModified(Message_Network_Increment msg)
	{
		if (Raft_Network.IsHost)
		{
			IncrementLocal(msg.amount);
			msg.amount = currentAmount;
			return true;
		}
		SetCounterLocal(msg.amount);
		return false;
	}

	public void IncrementLocal(float amount)
	{
		SetCounterLocal(currentAmount + amount);
	}

	public void SetCounterLocal(float amount)
	{
		currentAmount = amount;
		SendOnIncrement();
	}

	public float GetCurrentAmount()
	{
		return currentAmount;
	}
}
