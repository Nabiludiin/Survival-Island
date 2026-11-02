using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class NetworkUpdateManager : MonoBehaviour, SceneEventInterface
{
	public static SortedDictionary<uint, MonoBehaviour_Network> NetworkedBehaviours = new SortedDictionary<uint, MonoBehaviour_Network>();

	private static Raft_Network network;

	private static uint globalBehaviourIndex;

	private static List<MonoBehaviour_Network> NetworkedBehaviours2 = new List<MonoBehaviour_Network>();

	private List<Message> reliableMessages = new List<Message>();

	private List<Message> unreliableMessages = new List<Message>();

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			NetworkedBehaviours.Clear();
			NetworkedBehaviours2.Clear();
			SetBehaviourIndex(0);
		}
	}

	private void LateUpdate()
	{
		reliableMessages.Clear();
		unreliableMessages.Clear();
		for (int i = 0; i < NetworkedBehaviours2.Count; i++)
		{
			MonoBehaviour_Network monoBehaviour_Network = NetworkedBehaviours2[i];
			if (monoBehaviour_Network.IsKilled && Raft_Network.IsHost)
			{
				uint behaviourIndex = monoBehaviour_Network.BehaviourIndex;
				monoBehaviour_Network.Revive();
				Message_NetworkBehaviour message_NetworkBehaviour = new Message_NetworkBehaviour(Messages.RemoveNetworkBehaviour, monoBehaviour_Network);
				reliableMessages.Add(message_NetworkBehaviour);
				monoBehaviour_Network.Deserialize(message_NetworkBehaviour, 0uL);
				NetworkedBehaviours.Remove(behaviourIndex);
				NetworkedBehaviours2.Remove(monoBehaviour_Network);
				i--;
			}
			else if (monoBehaviour_Network.IsDirty)
			{
				monoBehaviour_Network.Serialize_Update();
				unreliableMessages.AddRange(monoBehaviour_Network.MessagesStack);
				monoBehaviour_Network.Clean();
			}
		}
		if (reliableMessages.Count > 0)
		{
			if (Raft_Network.IsHost)
			{
				network.RPC(reliableMessages, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, reliableMessages, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		if (unreliableMessages.Count > 0)
		{
			if (Raft_Network.IsHost)
			{
				network.RPC(unreliableMessages, Target.Other, EP2PSend.k_EP2PSendUnreliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, unreliableMessages, EP2PSend.k_EP2PSendUnreliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public static void AddBehaviour(MonoBehaviour_Network behaviour)
	{
		if (!(behaviour == null))
		{
			if (NetworkedBehaviours.ContainsKey(behaviour.BehaviourIndex))
			{
				Debug.LogError("Key already present: " + behaviour.transform.name + " conflict with " + NetworkedBehaviours[behaviour.BehaviourIndex].transform.name, behaviour);
				return;
			}
			NetworkedBehaviours.Add(behaviour.BehaviourIndex, behaviour);
			NetworkedBehaviours2.Add(behaviour);
		}
	}

	public static void SetBehaviourDead(MonoBehaviour_Network behaviour)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (Raft_Network.IsHost)
		{
			if (NetworkedBehaviours.ContainsKey(behaviour.BehaviourIndex))
			{
				NetworkedBehaviours[behaviour.BehaviourIndex].Kill();
			}
		}
		else
		{
			network.SendP2P(network.HostID, new Message_NetworkBehaviour(Messages.RemoveNetworkBehaviour, behaviour), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static void SetIndexForBehaviour(MonoBehaviour_Network behaviour)
	{
		behaviour.ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
		behaviour.BehaviourIndex = GetUniqueBehaviourIndex();
	}

	public static void RemoveBehaviour(MonoBehaviour_Network behaviour)
	{
		if (NetworkedBehaviours.ContainsKey(behaviour.BehaviourIndex))
		{
			NetworkedBehaviours.Remove(behaviour.BehaviourIndex);
			NetworkedBehaviours2.Remove(behaviour);
		}
	}

	public static void DeserializeSingleMessage(Message msg, Network_UserId remoteID)
	{
		bool flag = false;
		if (msg is Message_NetworkBehaviour)
		{
			Message_NetworkBehaviour message_NetworkBehaviour = msg as Message_NetworkBehaviour;
			if (NetworkedBehaviours.ContainsKey(message_NetworkBehaviour.BehaviourIndex))
			{
				MonoBehaviour_Network monoBehaviour_Network = NetworkedBehaviours[message_NetworkBehaviour.BehaviourIndex];
				if (message_NetworkBehaviour.Type == Messages.RemoveNetworkBehaviour)
				{
					if (Raft_Network.IsHost)
					{
						SetBehaviourDead(monoBehaviour_Network);
						flag = true;
					}
					else
					{
						monoBehaviour_Network.Revive();
						bool flag2 = monoBehaviour_Network.Deserialize(message_NetworkBehaviour, remoteID);
						NetworkedBehaviours.Remove(monoBehaviour_Network.BehaviourIndex);
						NetworkedBehaviours2.Remove(monoBehaviour_Network);
						if (Raft_Network.IsHost && !flag2)
						{
							flag = true;
						}
					}
				}
				else
				{
					bool flag3 = monoBehaviour_Network.Deserialize(message_NetworkBehaviour, remoteID);
					if (Raft_Network.IsHost && !flag3)
					{
						flag = true;
					}
				}
			}
		}
		else
		{
			flag = true;
		}
		if (!flag && (bool)network && Raft_Network.IsHost)
		{
			network.RPC(msg, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static MonoBehaviour_Network GetBehaviourFromIndex(uint behaviourIndex)
	{
		if (NetworkedBehaviours.ContainsKey(behaviourIndex))
		{
			return NetworkedBehaviours[behaviourIndex];
		}
		return null;
	}

	public static uint GetUniqueBehaviourIndex()
	{
		return ++globalBehaviourIndex;
	}

	public static uint GetCurrentBehaviourIndex()
	{
		return globalBehaviourIndex;
	}

	public static void SetBehaviourIndex(int newIndex)
	{
		globalBehaviourIndex = (uint)newIndex;
	}
}
