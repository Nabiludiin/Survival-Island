using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;

public class WorldEventManager : MonoBehaviour_ID_Network
{
	public const int MAXIMUMACTIVEEVENTS = 2;

	public static SO_WorldEvent[] so_worldEvents;

	public static WorldEvent[] worldEvents;

	public static Network_Player localPlayer;

	public static Network_Host_Entities networkHostEntities;

	private static Raft_Network network;

	public void Awake()
	{
		ComponentManager<WorldEventManager>.Value = this;
		InitEvents();
	}

	public void Start()
	{
		localPlayer = ComponentManager<Network_Player>.Value;
		networkHostEntities = ComponentManager<Network_Host_Entities>.Value;
		network = ComponentManager<Raft_Network>.Value;
	}

	public void Update()
	{
		WorldEvent[] array = worldEvents;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateEvent();
		}
	}

	public static bool CanAddActiveEvent()
	{
		int num = 0;
		WorldEvent[] array = worldEvents;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].isActive)
			{
				num++;
			}
		}
		if (num < 2)
		{
			return true;
		}
		return false;
	}

	public static WorldEvent GetWorldEventFromType(SO_WorldEvent.WorldEventType weType)
	{
		return worldEvents.FirstOrDefault((WorldEvent we) => we.eventData.worldEventType == weType);
	}

	public void InitEvents()
	{
		so_worldEvents = Resources.LoadAll<SO_WorldEvent>("SO_WorldEvents");
		List<WorldEvent> list = new List<WorldEvent>();
		SO_WorldEvent[] array = so_worldEvents;
		foreach (SO_WorldEvent data in array)
		{
			list.Add(WorldEventFromData(data));
		}
		worldEvents = list.ToArray();
	}

	public static RGD_WorldEvent[] GetSaveDataForEvents()
	{
		List<RGD_WorldEvent> list = new List<RGD_WorldEvent>();
		WorldEvent[] array = worldEvents;
		for (int i = 0; i < array.Length; i++)
		{
			RGD_WorldEvent item = new RGD_WorldEvent(array[i]);
			list.Add(item);
		}
		return list.ToArray();
	}

	public void PrintActiveEvents()
	{
		string text = "Active Events Are: ";
		WorldEvent[] array = worldEvents;
		foreach (WorldEvent worldEvent in array)
		{
			if (worldEvent.isActive)
			{
				text = text + worldEvent.eventData.worldEventType.ToString() + " / ";
			}
		}
		Debug.Log(text);
	}

	public void StartEvent(SO_WorldEvent.WorldEventType type)
	{
		WorldEvent worldEventFromType = GetWorldEventFromType(type);
		StartEvent(worldEventFromType);
	}

	public void StartEvent(WorldEvent we)
	{
		if (we != null && !we.isActive)
		{
			we.StartEvent();
			if (Raft_Network.IsHost)
			{
				RPCStartEvent(we);
			}
		}
	}

	public void StopEvent(SO_WorldEvent.WorldEventType type)
	{
		WorldEvent worldEventFromType = GetWorldEventFromType(type);
		StopEvent(worldEventFromType);
	}

	public void StopEvent(WorldEvent we)
	{
		if (we != null)
		{
			we.StopEvent();
			if (Raft_Network.IsHost)
			{
				RPCStopEvent(we);
			}
		}
	}

	public void RPCStartEvent(WorldEvent we)
	{
		if (Raft_Network.IsHost)
		{
			Message_WorldEventToggle_On message = new Message_WorldEventToggle_On(Messages.WorldEventToggle_On, network.NetworkIDManager, ObjectIndex, value: true, we.spawnedEntites, we.eventData.worldEventType);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void RPCStopEvent(WorldEvent we)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (Raft_Network.IsHost)
		{
			Message_WorldEventToggle_Off message = new Message_WorldEventToggle_Off(Messages.WorldEventToggle_Off, network.NetworkIDManager, ObjectIndex, we.eventData.worldEventType);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private WorldEvent WorldEventFromData(SO_WorldEvent data)
	{
		return data.worldEventType switch
		{
			SO_WorldEvent.WorldEventType.Dolphins => (WorldEvent)new WorldEvent_Dolphins(data, this), 
			SO_WorldEvent.WorldEventType.Whale => new WorldEvent_Whale(data, this), 
			SO_WorldEvent.WorldEventType.Birdswarm => new WorldEvent_Birds(data, this), 
			SO_WorldEvent.WorldEventType.Turtle => new WorldEvent_Turtle(data, this), 
			SO_WorldEvent.WorldEventType.Stingray => new WorldEvent_Stingray(data, this), 
			_ => null, 
		};
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.WorldEventToggle_On:
			if (!Raft_Network.IsHost)
			{
				Message_WorldEventToggle_On obj = msg as Message_WorldEventToggle_On;
				WorldEvent worldEventFromType = GetWorldEventFromType((SO_WorldEvent.WorldEventType)obj.worldEventTypeIndex);
				StartEvent(worldEventFromType);
				foreach (uint spawnedEntitiesIndex in obj.spawnedEntitiesIndexes)
				{
					AI_NetworkBehaviour networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex<AI_NetworkBehaviour>(spawnedEntitiesIndex);
					if (networkIDFromObjectIndex != null)
					{
						worldEventFromType.AddSpawnedEntity(networkIDFromObjectIndex);
					}
				}
			}
			return false;
		case Messages.WorldEventToggle_Off:
		{
			Message_WorldEventToggle_Off message_WorldEventToggle_Off = msg as Message_WorldEventToggle_Off;
			if (!Raft_Network.IsHost)
			{
				StopEvent((SO_WorldEvent.WorldEventType)message_WorldEventToggle_Off.worldEventTypeIndex);
			}
			return false;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
