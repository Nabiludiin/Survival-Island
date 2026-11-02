using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class NetworkIDManager : MonoBehaviour_Network, SceneEventInterface
{
	private static Dictionary<Type, Dictionary<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>>> networkIDs = new Dictionary<Type, Dictionary<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>>>();

	private static HashSet<MonoBehaviour_NetworkTick> networkTickIDs = new HashSet<MonoBehaviour_NetworkTick>();

	private List<Message> dirtyTickMessages = new List<Message>();

	private static Raft_Network network;

	private void Awake()
	{
		ComponentManager<NetworkIDManager>.Value = this;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		SoundManager value = ComponentManager<SoundManager>.Value;
		if (value != null)
		{
			if (Raft_Network.IsHost)
			{
				value.ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			}
			AddNetworkID(value);
		}
		if (Raft_Network.IsHost && GameManager.IsInNewGame)
		{
			NetworkUpdateManager.SetIndexForBehaviour(this);
			NetworkUpdateManager.AddBehaviour(this);
		}
	}

	private void LateUpdate()
	{
		if (networkTickIDs == null || networkTickIDs.Count <= 0)
		{
			return;
		}
		foreach (MonoBehaviour_NetworkTick networkTickID in networkTickIDs)
		{
			if (networkTickID != null && networkTickID.IsDirty)
			{
				networkTickID.Serialize_Update();
				dirtyTickMessages.AddRange(networkTickID.MessagesStack);
				networkTickID.Clean();
			}
		}
		if (dirtyTickMessages != null && dirtyTickMessages.Count > 0)
		{
			if (Raft_Network.IsHost)
			{
				network.RPC(dirtyTickMessages, Target.Other, EP2PSend.k_EP2PSendUnreliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, dirtyTickMessages, EP2PSend.k_EP2PSendUnreliable, NetworkChannel.Channel_Game);
			}
		}
		dirtyTickMessages.Clear();
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			networkIDs.Clear();
			networkTickIDs.Clear();
			network = null;
		}
	}

	public static void SendIDBehaviourDead(uint objectIndex, Type type, bool removeInstantly = false)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Message_SetIDBehaviourDead message_SetIDBehaviourDead = new Message_SetIDBehaviourDead(Messages.SetIDBehaviourDead, network.NetworkIDManager, objectIndex, type, removeInstantly);
		if (Raft_Network.IsHost)
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex = GetNetworkIDFromObjectIndex(objectIndex, type);
			if (networkIDFromObjectIndex != null && networkIDFromObjectIndex.Deserialize(message_SetIDBehaviourDead, network.HostID))
			{
				network.RPC(message_SetIDBehaviourDead, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			network.SendP2P(network.HostID, message_SetIDBehaviourDead, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static void AddNetworkIDTick(MonoBehaviour_NetworkTick networkTickID)
	{
		if (!(networkTickID == null) && !networkTickIDs.Contains(networkTickID))
		{
			networkTickIDs.Add(networkTickID);
		}
	}

	public static void AddNetworkID(MonoBehaviour_ID_Network networkID)
	{
		AddNetworkID(networkID, networkID.GetType(), NetworkIDTag.None);
	}

	public static void AddNetworkID(MonoBehaviour_ID_Network networkID, NetworkIDTag tag)
	{
		AddNetworkID(networkID, networkID.GetType(), tag);
	}

	public static void AddNetworkID(MonoBehaviour_ID_Network networkID, Type type)
	{
		AddNetworkID(networkID, type, NetworkIDTag.None);
	}

	public static void AddNetworkID(MonoBehaviour_ID_Network networkID, Type type, NetworkIDTag tag)
	{
		if (!(type == null))
		{
			if (!networkIDs.ContainsKey(type))
			{
				networkIDs.Add(type, new Dictionary<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>>());
			}
			Dictionary<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>> dictionary = networkIDs[type];
			if (!dictionary.ContainsKey(tag))
			{
				dictionary.Add(tag, new HashSet<MonoBehaviour_ID_Network>());
			}
			dictionary[tag].Add(networkID);
		}
	}

	public static void RemoveNetworkTickID(MonoBehaviour_NetworkTick networkTickID)
	{
		if (!(networkTickID == null) && networkTickIDs.Contains(networkTickID))
		{
			networkTickIDs.Remove(networkTickID);
		}
	}

	public static void RemoveNetworkID(MonoBehaviour_ID_Network networkID)
	{
		Type type = networkID.GetType();
		RemoveNetworkID(networkID, type, NetworkIDTag.None);
	}

	public static void RemoveNetworkID(MonoBehaviour_ID_Network networkID, Type type)
	{
		RemoveNetworkID(networkID, type, NetworkIDTag.None);
	}

	public static void RemoveNetworkID(MonoBehaviour_ID_Network networkID, Type type, NetworkIDTag tag)
	{
		if (!(type == null) && networkIDs.ContainsKey(type))
		{
			Dictionary<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>> dictionary = networkIDs[type];
			if (dictionary.ContainsKey(tag))
			{
				dictionary[tag].Remove(networkID);
			}
		}
	}

	public static MonoBehaviour_ID_Network GetNetworkIDFromObjectIndex(uint objectIndex, Type type)
	{
		if (!networkIDs.ContainsKey(type))
		{
			return null;
		}
		Dictionary<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>> dictionary = networkIDs[type];
		if (dictionary != null)
		{
			foreach (HashSet<MonoBehaviour_ID_Network> value in dictionary.Values)
			{
				foreach (MonoBehaviour_ID_Network item in value)
				{
					if (item.ObjectIndex == objectIndex)
					{
						return item;
					}
				}
			}
		}
		return null;
	}

	public static MonoBehaviour_ID_Network GetNetworkIDFromObjectIndex(uint objectIndex)
	{
		foreach (Dictionary<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>> value in networkIDs.Values)
		{
			foreach (HashSet<MonoBehaviour_ID_Network> value2 in value.Values)
			{
				foreach (MonoBehaviour_ID_Network item in value2)
				{
					if (item.ObjectIndex == objectIndex)
					{
						return item;
					}
				}
			}
		}
		return null;
	}

	public static T GetNetworkIDFromObjectIndex<T>(uint objectIndex)
	{
		Type typeFromHandle = typeof(T);
		if (networkIDs.ContainsKey(typeFromHandle))
		{
			foreach (HashSet<MonoBehaviour_ID_Network> value in networkIDs[typeFromHandle].Values)
			{
				foreach (MonoBehaviour_ID_Network item in value)
				{
					if (item.ObjectIndex == objectIndex)
					{
						return (T)(object)item;
					}
				}
			}
		}
		return default;
	}

	public static T GetNetworkIDFromObjectIndex<T>(uint objectIndex, NetworkIDTag tag)
	{
		Type typeFromHandle = typeof(T);
		if (networkIDs.ContainsKey(typeFromHandle))
		{
			Dictionary<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>> dictionary = networkIDs[typeFromHandle];
			if (dictionary.ContainsKey(tag))
			{
				foreach (MonoBehaviour_ID_Network item in dictionary[tag])
				{
					if (item.ObjectIndex == objectIndex)
					{
						return (T)(object)item;
					}
				}
			}
		}
		return default;
	}

	public static T[] GetNetworkdIDs<T>()
	{
		Type typeFromHandle = typeof(T);
		if (networkIDs.ContainsKey(typeFromHandle))
		{
			List<T> list = new List<T>();
			foreach (KeyValuePair<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>> item in networkIDs[typeFromHandle])
			{
				foreach (MonoBehaviour_ID_Network item2 in item.Value)
				{
					if (item2 != null)
					{
						list.Add(item2.GetComponentInChildren<T>());
						continue;
					}
					Debug.Log("MonoBehaviour_ID_Network is null. Type[" + typeFromHandle?.ToString() + "], NetworkIDTag[" + item.Key.ToString() + "]");
				}
			}
			return list.ToArray();
		}
		return null;
	}

	public static T[] GetNetworkdIDs<T>(NetworkIDTag tag)
	{
		Type typeFromHandle = typeof(T);
		if (networkIDs.ContainsKey(typeFromHandle))
		{
			Dictionary<NetworkIDTag, HashSet<MonoBehaviour_ID_Network>> dictionary = networkIDs[typeFromHandle];
			if (dictionary.ContainsKey(tag))
			{
				HashSet<MonoBehaviour_ID_Network> hashSet = dictionary[tag];
				T[] array = new T[hashSet.Count];
				int num = 0;
				{
					foreach (MonoBehaviour_ID_Network item in hashSet)
					{
						array[num++] = item.GetComponentInChildren<T>();
					}
					return array;
				}
			}
		}
		return null;
	}

	public override List<RGD> Serialize_SaveMultiple()
	{
		List<RGD> list = new List<RGD>();
		list.Add(new RGD_NetworkBehaviour(RGDType.NetworkBehaviour_NetworkIDManager, this));
		PickupItem_Networked[] networkdIDs = GetNetworkdIDs<PickupItem_Networked>(NetworkIDTag.ChickenEgg);
		if (networkdIDs.ContainsItems())
		{
			list.Add(new RGD_Chicken_Eggs(networkdIDs));
		}
		return list;
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_NetworkBehaviour(Messages.CreateNetworkIDManager, this));
		Message_RestoreQuests msg = new Message_RestoreQuests(Messages.QuestRestore, this, QuestProgressTracker.GetUnlockedQuestsIndexes());
		AddMessage(msg);
		Network_Host_Entities value = ComponentManager<Network_Host_Entities>.Value;
		if (value != null)
		{
			List<Message_NetworkBehaviour> msgs = new List<Message_NetworkBehaviour>();
			value.Serialize_CreateFromIDManager(msgs);
			AddMessage(msgs);
		}
		ChunkManager value2 = ComponentManager<ChunkManager>.Value;
		if (value2 != null)
		{
			List<ChunkPoint> allChunkPointsList = value2.GetAllChunkPointsList();
			if (allChunkPointsList.ContainsItems())
			{
				AddMessage(new Message_ChunkManager_ChunkPoints(Messages.ChunkManager_Create, this, value2.ObjectIndex, allChunkPointsList));
			}
		}
		QuestItemManager value3 = ComponentManager<QuestItemManager>.Value;
		if (value3 != null)
		{
			AddMessage(new Message_Quest_Create(Messages.QuestCreate, this, value3.ObjectIndex, QuestItemManager.QuestItems));
		}
		else
		{
			Debug.Log("Unable to find landmarkQuest script via componentmanager");
		}
		NoteBook value4 = ComponentManager<NoteBook>.Value;
		if (value4 != null)
		{
			AddMessage(new Message_NoteBook_Create(Messages.NoteBook_Create, this, value4.ObjectIndex, value4));
		}
		TrophyHolder[] networkdIDs = GetNetworkdIDs<TrophyHolder>();
		if (networkdIDs.ContainsItems())
		{
			AddMessage(new Message_TrophyBoardCreate(Messages.TrophyHolder_Create, this, networkdIDs));
		}
		PickupItem_Networked[] networkdIDs2 = GetNetworkdIDs<PickupItem_Networked>(NetworkIDTag.ChickenEgg);
		if (networkdIDs2.ContainsItems())
		{
			AddMessage(new Message_ChickenEggs(Messages.Chicken_CreateEggs, this, networkdIDs2));
		}
		Sprinkler[] networkdIDs3 = GetNetworkdIDs<Sprinkler>();
		if (networkdIDs3.ContainsItems())
		{
			AddMessage(new Message_Sprinkler_Create(Messages.Sprinkler_Create, this, networkdIDs3));
		}
		Reciever[] networkdIDs4 = GetNetworkdIDs<Reciever>();
		if (networkdIDs4.ContainsItems())
		{
			AddMessage(new Message_Reciever_Create(Messages.Reciever_Create, this, networkdIDs4));
		}
		RadioPlayer[] networkdIDs5 = GetNetworkdIDs<RadioPlayer>();
		if (networkdIDs5.ContainsItems())
		{
			AddMessage(new Message_RadioPlayer_Create(Messages.Radio_Create, this, networkdIDs5));
		}
		MotorWheel[] networkdIDs6 = GetNetworkdIDs<MotorWheel>();
		if (networkdIDs6.ContainsItems())
		{
			AddMessage(new Message_MotorWheelCreate(Messages.MotorWheelCreate, this, networkdIDs6));
		}
		SteeringWheel[] networkdIDs7 = GetNetworkdIDs<SteeringWheel>();
		if (networkdIDs7.ContainsItems())
		{
			AddMessage(new Message_SteeringWheel_Create(Messages.SteeringWheelCreate, this, networkdIDs7));
		}
		Placeable_Extractor[] networkdIDs8 = GetNetworkdIDs<Placeable_Extractor>();
		if (networkdIDs8.ContainsItems())
		{
			AddMessage(new Message_Extractor_Create(Messages.Extractor_Create, this, networkdIDs8));
		}
		Scarecrow[] networkdIDs9 = GetNetworkdIDs<Scarecrow>();
		if (networkdIDs9.ContainsItems())
		{
			AddMessage(new Message_Scarecrow_Create(Messages.Scarecrow_Create, this, networkdIDs9));
		}
		ItemCollector[] networkdIDs10 = GetNetworkdIDs<ItemCollector>();
		if (networkdIDs10.ContainsItems())
		{
			AddMessage(new Message_ItemCollectorCreate(Messages.ItemCollectorCreate, this, networkdIDs10));
		}
		AI_NetworkBehaviour[] networkdIDs11 = GetNetworkdIDs<AI_NetworkBehaviour>();
		if (networkdIDs11.ContainsItems())
		{
			List<Message_NetworkBehaviour> list = new List<Message_NetworkBehaviour>();
			foreach (AI_NetworkBehaviour aI_NetworkBehaviour in networkdIDs11)
			{
				if (aI_NetworkBehaviour != null)
				{
					aI_NetworkBehaviour.Serialize_CreateFromIDManager(list);
				}
			}
			if (list.ContainsItems())
			{
				Message_AINetworkBehaviours_Create message_AINetworkBehaviours_Create = new Message_AINetworkBehaviours_Create(Messages.AINetworkBehaviourCreate, this);
				message_AINetworkBehaviours_Create.createMessages = list.ToArray();
				AddMessage(message_AINetworkBehaviours_Create);
			}
		}
		TextWriterObject[] networkdIDs12 = GetNetworkdIDs<TextWriterObject>();
		if (networkdIDs12.ContainsItems())
		{
			AddMessage(new Message_Sign_Create(Messages.Sign_Create, this, networkdIDs12));
		}
		BatteryCharger[] networkdIDs13 = GetNetworkdIDs<BatteryCharger>();
		if (networkdIDs13.ContainsItems())
		{
			AddMessage(new Message_BatteryCharger_Create(Messages.BatteryCharger_Create, this, networkdIDs13));
		}
		TreasurePoint[] networkdIDs14 = GetNetworkdIDs<TreasurePoint>();
		if (networkdIDs14.ContainsItems())
		{
			AddMessage(new Message_TreasurePoint_Create(Messages.TreasurePointManager_Create, this, networkdIDs14));
		}
		BeeHive[] networkdIDs15 = GetNetworkdIDs<BeeHive>();
		if (networkdIDs15.ContainsItems())
		{
			AddMessage(new Message_BeeHive_Create(Messages.BeeHiveCreate, this, networkdIDs15));
		}
		SoundManager value5 = ComponentManager<SoundManager>.Value;
		if (value5 != null)
		{
			AddMessage(new Message_NetworkBehaviour_ID(Messages.SoundManager_Create, network.NetworkIDManager, value5.ObjectIndex));
		}
		if (MeshPath.PathConnections != null && MeshPath.PathConnections.Count > 0)
		{
			AddMessage(new Message_MeshPath_CreateAll(Messages.MeshPath_CreateAll, this));
		}
		AddMessage(new Message_WorldEvent_Create(Messages.WorldEvent_Create, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Message_NetworkBehaviour_ID message_NetworkBehaviour_ID = msg as Message_NetworkBehaviour_ID;
		switch (msg.Type)
		{
		case Messages.SetIDBehaviourDead:
		{
			Message_SetIDBehaviourDead obj = msg as Message_SetIDBehaviourDead;
			MonoBehaviour_ID_Network monoBehaviour_ID_Network = GetNetworkIDFromObjectIndex(type: Type.GetType(obj.objectTypeName), objectIndex: obj.objectIndexValue);
			if (monoBehaviour_ID_Network != null)
			{
				return monoBehaviour_ID_Network.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.ChunkManager_Create:
			ComponentManager<ChunkManager>.Value.Deserialize(msg, remoteID);
			break;
		case Messages.ChunkManager_RefreshChunkPoints:
		case Messages.ChunkManager_ClearChunkPoints:
			return GetNetworkIDFromObjectIndex<ChunkManager>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex).Deserialize(msg, remoteID);
		case Messages.QuestCreate:
			return ComponentManager<QuestItemManager>.Value.Deserialize(msg, remoteID);
		case Messages.QuestItemModifyCount:
			return GetNetworkIDFromObjectIndex<QuestItemManager>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex).Deserialize(msg, remoteID);
		case Messages.NoteBook_Create:
			ComponentManager<NoteBook>.Value.Deserialize(msg, remoteID);
			break;
		case Messages.NoteBook_SetNoteUnlockState:
		case Messages.NoteBook_FlipPage:
		case Messages.NoteBook_SetBookVisibleState:
			return GetNetworkIDFromObjectIndex<NoteBook>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex).Deserialize(msg, remoteID);
		case Messages.CreateHostNetworkEntities:
		{
			Network_Host_Entities value = ComponentManager<Network_Host_Entities>.Value;
			if (value != null)
			{
				value.ObjectIndex = message_NetworkBehaviour_ID.NetworkID_ObjectIndex;
				AddNetworkID(value);
			}
			return false;
		}
		case Messages.Shark_Create:
		case Messages.DamageEntity:
		case Messages.Seagull_Create:
		case Messages.CreateAINetworkBehaviour:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex3 = GetNetworkIDFromObjectIndex<Network_Host_Entities>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex3 != null)
			{
				return networkIDFromObjectIndex3.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.QuestRestore:
		case Messages.QuestFinish:
		case Messages.QuestProgress:
			QuestProgressTracker.Deserialize_NetworkMessage(msg);
			break;
		case Messages.TrophyHolder_Create:
			(msg as Message_TrophyBoardCreate).RestoreTrophyHolders(GetNetworkdIDs<TrophyHolder>());
			break;
		case Messages.TrophyHolder_Pickup:
		case Messages.TrophyHolder_Place:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex39 = GetNetworkIDFromObjectIndex<TrophyHolder>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex39 != null)
			{
				return networkIDFromObjectIndex39.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Sign_Create:
			(msg as Message_Sign_Create).RestoreSigns(GetNetworkdIDs<TextWriterObject>());
			break;
		case Messages.Sign_StartEdit:
		case Messages.Sign_EndEdit:
		case Messages.Sign_OnEdit:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex30 = GetNetworkIDFromObjectIndex<TextWriterObject>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex30 != null)
			{
				return networkIDFromObjectIndex30.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.CookingPot_Pickup:
		case Messages.CookingPot_Insert:
		case Messages.CookingPot_PickupFood:
		case Messages.CookingPot_Start:
		case Messages.CookingPot_Finish:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex26 = GetNetworkIDFromObjectIndex<CookingTable>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex26 != null)
			{
				return networkIDFromObjectIndex26.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Fuel_Increment:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex23 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex23 != null)
			{
				return networkIDFromObjectIndex23.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Tank:
		case Messages.Tank_SetValue:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex24 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex24 != null)
			{
				return networkIDFromObjectIndex24.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.ExtractorProcessTanks:
		case Messages.ExtractorHarvestOutput:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex17 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex17 != null)
			{
				return networkIDFromObjectIndex17.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Sprinkler_Create:
			(msg as Message_Sprinkler_Create).RestoreSprinklers(GetNetworkdIDs<Sprinkler>());
			break;
		case Messages.Sprinkler_Water:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex8 = GetNetworkIDFromObjectIndex<Sprinkler>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex8 != null)
			{
				return networkIDFromObjectIndex8.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Battery_Insert:
		case Messages.Battery_Take:
		case Messages.Battery_Update:
		case Messages.Battery_OnOff:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex5 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex5 != null)
			{
				return networkIDFromObjectIndex5.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.ThrowableComponent_Throw:
		case Messages.ThrowableComponent_Collision:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex11 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex11 != null)
			{
				return networkIDFromObjectIndex11.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Chicken_CreateEggs:
			(msg as Message_ChickenEggs).RestoreEggs();
			break;
		case Messages.Reciever_Create:
			(msg as Message_Reciever_Create).RestoreRecievers(GetNetworkdIDs<Reciever>(), this);
			break;
		case Messages.Reciever_Antenna_Connect:
		case Messages.Reciever_FrequencyUpdate:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex46 = GetNetworkIDFromObjectIndex<Reciever>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex46 != null)
			{
				return networkIDFromObjectIndex46.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Radio_Create:
			(msg as Message_RadioPlayer_Create).RestoreRadios(GetNetworkdIDs<RadioPlayer>(), this);
			break;
		case Messages.Radio_Toggle:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex43 = GetNetworkIDFromObjectIndex<RadioPlayer>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex43 != null)
			{
				return networkIDFromObjectIndex43.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.MotorWheelCreate:
			(msg as Message_MotorWheelCreate).RestoreMotorWheels(GetNetworkdIDs<MotorWheel>());
			break;
		case Messages.MotorWheel_Inverse:
		case Messages.MotorWheel_PowerButton:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex40 = GetNetworkIDFromObjectIndex<MotorWheel>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex40 != null)
			{
				return networkIDFromObjectIndex40.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.SteeringWheelCreate:
			(msg as Message_SteeringWheel_Create).RestoreSteeringWheels(GetNetworkdIDs<SteeringWheel>());
			break;
		case Messages.SteeringWheelRotate:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex37 = GetNetworkIDFromObjectIndex<SteeringWheel>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex37 != null)
			{
				return networkIDFromObjectIndex37.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Extractor_Create:
			(msg as Message_Extractor_Create).RestoreExtractors(GetNetworkdIDs<Placeable_Extractor>());
			break;
		case Messages.InteractableButtonPress:
		case Messages.InteractableButtonPress_Int:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex35 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex35 != null)
			{
				return networkIDFromObjectIndex35.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.RemoteAnchorsDown:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex32 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex32 != null)
			{
				return networkIDFromObjectIndex32.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.PickupChannel:
		{
			PickupItem_Networked networkIDFromObjectIndex28 = GetNetworkIDFromObjectIndex<PickupItem_Networked>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex28 != null)
			{
				return networkIDFromObjectIndex28.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Scarecrow_Create:
			(msg as Message_Scarecrow_Create).RestoreScareCrows(GetNetworkdIDs<Scarecrow>(), this);
			break;
		case Messages.Scarecrow_Damage:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex27 = GetNetworkIDFromObjectIndex<Scarecrow>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex27 != null)
			{
				return networkIDFromObjectIndex27.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.ItemCollectorCreate:
			(msg as Message_ItemCollectorCreate).RestoreItemCollectors(GetNetworkdIDs<ItemCollector>());
			break;
		case Messages.ItemCollectorCollect:
		case Messages.ItemCollectorClear:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex25 = GetNetworkIDFromObjectIndex<ItemCollector>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex25 != null)
			{
				return networkIDFromObjectIndex25.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.AINetworkBehaviourCreate:
		{
			Message_AINetworkBehaviours_Create message_AINetworkBehaviours_Create = msg as Message_AINetworkBehaviours_Create;
			if (message_AINetworkBehaviours_Create.createMessages.ContainsItems())
			{
				for (int j = 0; j < message_AINetworkBehaviours_Create.createMessages.Length; j++)
				{
					Deserialize(message_AINetworkBehaviours_Create.createMessages[j], remoteID);
				}
			}
			return false;
		}
		case Messages.StoneBird_DropStone:
		case Messages.PufferFish_Explode:
		case Messages.ResourceRegenerativeChange:
		case Messages.AnimalCaptured:
		case Messages.Boar_Impale_Player:
		case Messages.Boar_Depale_Player:
		case Messages.StartCarryAIBehaviour:
		case Messages.StopCarryAIBehaviour:
		case Messages.CreateDomestic:
		case Messages.CreateDomesticResource:
		case Messages.Chicken_LayEgg:
		case Messages.RareMaterial:
		case Messages.QuickTameAnimal:
		case Messages.AINetworkBehaviourUpdate:
		case Messages.AINetworkAnimatorUpdate:
		case Messages.Charge_Hit:
		case Messages.AI_Protect:
		case Messages.Whale_ShootWater:
		case Messages.AttachPlayerMeleeAttack:
		case Messages.HyenaBoss_StartPin:
		case Messages.HyenaBoss_StopPin:
		case Messages.HyenaBoss_SpawnStone:
		case Messages.HyenaBoss_ThrowStone:
		case Messages.HyenaBoss_DigStoneMultiplier:
		case Messages.HyenaBoss_LandJump:
		case Messages.HyenaBoss_StartCharge:
		case Messages.AINetworkBehaviourUpdate_Shark:
		case Messages.AINetworkBehaviourUpdate_Animal:
		case Messages.AINetworkBehaviourUpdate_Domestic:
		case Messages.AINetworkBehaviourUpdate_NPC:
		case Messages.AINetworkBehaviourUpdate_PufferFish:
		case Messages.AINetworkBehaviourUpdate_StoneBird:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex22 = GetNetworkIDFromObjectIndex<AI_NetworkBehaviour>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex22 != null)
			{
				return networkIDFromObjectIndex22.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Seagull_Update:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex19 = GetNetworkIDFromObjectIndex<Seagull>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex19 != null)
			{
				return networkIDFromObjectIndex19.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.QuestEventInteract:
		case Messages.Balboa_BicyclePling:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex15 = GetNetworkIDFromObjectIndex<QuestEventBase>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex15 != null)
			{
				return networkIDFromObjectIndex15.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.StartSinkingJumpObstacle:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex13 = GetNetworkIDFromObjectIndex<SinkingObstacle>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex13 != null)
			{
				return networkIDFromObjectIndex13.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Teleport:
		{
			Message_Teleport message_Teleport = msg as Message_Teleport;
			Network_Player playerFromID = network.GetPlayerFromID(message_Teleport.SteamID);
			if (playerFromID != null && !playerFromID.BedComponent.Sleeping && !playerFromID.PlayerNetworkManager.IsAttached && !playerFromID.RessurectComponent.IsCarrying && !playerFromID.RessurectComponent.BeingCarried)
			{
				playerFromID.transform.SetParentSafe(null);
				playerFromID.PersonController.SwitchControllerType(ControllerType.Ground);
				playerFromID.transform.position = message_Teleport.Position;
			}
			return true;
		}
		case Messages.BeeHiveCreate:
			(msg as Message_BeeHive_Create).RestoreBeeHives(GetNetworkdIDs<BeeHive>());
			break;
		case Messages.BeeHiveHarvestOutput:
		case Messages.BeeHiveFillYield:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex48 = GetNetworkIDFromObjectIndex<BeeHive>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex48 != null)
			{
				return networkIDFromObjectIndex48.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.TreasurePoint_Excevate:
		case Messages.TreasurePoint_Reveal:
		case Messages.TreasurePoint_Move:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex47 = GetNetworkIDFromObjectIndex<TreasurePoint>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex47 != null)
			{
				return networkIDFromObjectIndex47.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.TreasurePointManager_Spawn:
		case Messages.TreasurePointManager_Create:
			return ComponentManager<TreasurePointManager>.Value.Deserialize(msg, remoteID);
		case Messages.BatteryCharger_Create:
			(msg as Message_BatteryCharger_Create).RestoreBatteryChargers(GetNetworkdIDs<BatteryCharger>());
			break;
		case Messages.RotationComponentRotate:
		case Messages.RotationComponentSetRotation:
		case Messages.RotationComponentRequestRotation:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex45 = GetNetworkIDFromObjectIndex<Challenge_Pipe>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex45 != null)
			{
				return networkIDFromObjectIndex45.Deserialize(msg, remoteID);
			}
			networkIDFromObjectIndex45 = GetNetworkIDFromObjectIndex<PlaceableFirework>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex45 != null)
			{
				return networkIDFromObjectIndex45.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Tangaroa_CockpitLaunch:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex44 = GetNetworkIDFromObjectIndex<TangaroaCockpitLaunch>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex44 != null)
			{
				return networkIDFromObjectIndex44.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.FireworkBlock_LightFuse:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex42 = GetNetworkIDFromObjectIndex<PlaceableFirework>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex42 != null)
			{
				return networkIDFromObjectIndex42.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Firework_Explode:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex41 = GetNetworkIDFromObjectIndex<Firework>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex41 != null)
			{
				return networkIDFromObjectIndex41.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.TikiPoleCompleted:
		{
			uint[] pieceBlockIndexes = (msg as TikiPole_Head.Message_TikiPoleComplete).pieceBlockIndexes;
			TikiPole_Head tikiPole_Head = null;
			List<TikiPole_Piece> list = new List<TikiPole_Piece>();
			uint[] array = pieceBlockIndexes;
			for (int k = 0; k < array.Length; k++)
			{
				uint num = array[k];
				Block blockByObjectIndex = BlockCreator.GetBlockByObjectIndex(num);
				if (blockByObjectIndex != null)
				{
					TikiPole_Piece component = blockByObjectIndex.GetComponent<TikiPole_Piece>();
					if (component != null)
					{
						if (component is TikiPole_Head)
						{
							tikiPole_Head = component as TikiPole_Head;
						}
						list.Add(component);
					}
				}
				else
				{
					Debug.Log("Unable to find block at index" + num + "when trying to complete tikipole for client.");
				}
			}
			if (tikiPole_Head != null)
			{
				tikiPole_Head.polePieces = list.ToArray();
				tikiPole_Head.StartSpinningPieces();
			}
			break;
		}
		case Messages.DamageBox_Hit:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex38 = GetNetworkIDFromObjectIndex<AI_NetworkBehaviour>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex38 != null)
			{
				return networkIDFromObjectIndex38.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.SoundManager_Create:
		{
			SoundManager value3 = ComponentManager<SoundManager>.Value;
			if (value3 != null)
			{
				value3.ObjectIndex = message_NetworkBehaviour_ID.NetworkID_ObjectIndex;
			}
			return false;
		}
		case Messages.SoundManager_PlayOneShot:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex36 = GetNetworkIDFromObjectIndex<SoundManager>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex36 != null)
			{
				return networkIDFromObjectIndex36.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Instrument_KeyModified:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex34 = GetNetworkIDFromObjectIndex<Instrument>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex34 != null)
			{
				return networkIDFromObjectIndex34.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.StartCarryItem:
		case Messages.StopCarryItem:
		case Messages.StopCarryItemInterest:
		case Messages.StopCarryStack:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex33 = GetNetworkIDFromObjectIndex<CarryNetworked>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex33 != null)
			{
				return networkIDFromObjectIndex33.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Container_SpawnObject:
		case Messages.Container_DestroyObject:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex31 = GetNetworkIDFromObjectIndex<Container>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex31 != null)
			{
				return networkIDFromObjectIndex31.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.ComputerChallenge_StartTimer:
		case Messages.ComputerChallenge_ResetTimer:
		case Messages.ComputerChallenge_Complete:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex29 = GetNetworkIDFromObjectIndex<ComputerChallenge>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex29 != null)
			{
				return networkIDFromObjectIndex29.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.MeshPath_Create:
		{
			Message_MeshPath message_MeshPath2 = msg as Message_MeshPath;
			if (message_MeshPath2 != null && message_MeshPath2.rgdMeshPath != null)
			{
				return message_MeshPath2.rgdMeshPath.Restore(replicating: false);
			}
			return false;
		}
		case Messages.MeshPath_Remove:
		{
			Message_MeshPath message_MeshPath = msg as Message_MeshPath;
			if (message_MeshPath != null && message_MeshPath.rgdMeshPath != null)
			{
				MeshPathConnection meshPathConnection = message_MeshPath.rgdMeshPath.GetMeshPathConnection();
				if (meshPathConnection != null && meshPathConnection.path != null)
				{
					return MeshPath.RemovePath(meshPathConnection.path);
				}
			}
			return false;
		}
		case Messages.MeshPath_CreateAll:
		{
			Message_MeshPath_CreateAll message_MeshPath_CreateAll = msg as Message_MeshPath_CreateAll;
			if (message_MeshPath_CreateAll != null && message_MeshPath_CreateAll.rgdMeshPaths.ContainsItems())
			{
				for (int i = 0; i < message_MeshPath_CreateAll.rgdMeshPaths.Length; i++)
				{
					RGD_MeshPath rGD_MeshPath = message_MeshPath_CreateAll.rgdMeshPaths[i];
					if (rGD_MeshPath != null)
					{
						rGD_MeshPath.Restore(replicating: true);
					}
				}
			}
			return false;
		}
		case Messages.TradingPost_Purchase:
		case Messages.TradingPost_Reputation:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex21 = GetNetworkIDFromObjectIndex<TradingPost>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex21 != null)
			{
				return networkIDFromObjectIndex21.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Snowmobile_TakeSeat:
		case Messages.Snowmobile_LeaveSeat:
		case Messages.Snowmobile_Update:
		case Messages.Snowmobile_Impact:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex20 = GetNetworkIDFromObjectIndex<Snowmobile>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex20 != null)
			{
				return networkIDFromObjectIndex20.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.SnowmobileShed_Spawn:
		case Messages.SnowmobileShed_Reset:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex18 = GetNetworkIDFromObjectIndex<SnowmobileShed>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex18 != null)
			{
				return networkIDFromObjectIndex18.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.PlayerSeat_Take:
		case Messages.PlayerSeat_Leave:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex16 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex16 != null)
			{
				return networkIDFromObjectIndex16.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.WorldEventToggle_On:
		case Messages.WorldEventToggle_Off:
		{
			MonoBehaviour_ID_Network value2 = ComponentManager<WorldEventManager>.Value;
			if (value2 != null)
			{
				return value2.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.WorldEvent_Create:
			(msg as Message_WorldEvent_Create).RestoreWorldEvents();
			break;
		case Messages.Increment:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex14 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex14 != null)
			{
				return networkIDFromObjectIndex14.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Toggle_Held:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex12 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex12 != null)
			{
				return networkIDFromObjectIndex12.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Dialogue:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex10 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex10 != null)
			{
				return networkIDFromObjectIndex10.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.ThrowExplosive:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex9 = GetNetworkIDFromObjectIndex<ExplosiveThrower>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex9 != null)
			{
				return networkIDFromObjectIndex9.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.OlofThrowMeat:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex7 = GetNetworkIDFromObjectIndex<Boss_OlofPart2>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex7 != null)
			{
				return networkIDFromObjectIndex7.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.FinalRoomOlofFall:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex6 = GetNetworkIDFromObjectIndex(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex6 != null)
			{
				return networkIDFromObjectIndex6.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.CookingStand_InsertItem:
		case Messages.CookingStand_CollectItem:
		case Messages.CookingStand_IncrementFuel:
		case Messages.CookingStand_SlotComplete:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex4 = GetNetworkIDFromObjectIndex<CookingStand>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex4 != null)
			{
				return networkIDFromObjectIndex4.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.Wardrobe_Open:
		case Messages.Wardrobe_Close:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex2 = GetNetworkIDFromObjectIndex<Placeable_Wardrobe>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex2 != null)
			{
				return networkIDFromObjectIndex2.Deserialize(msg, remoteID);
			}
			break;
		}
		case Messages.CharacterUnlock_Interact:
		{
			MonoBehaviour_ID_Network networkIDFromObjectIndex = GetNetworkIDFromObjectIndex<CharacterUnlock>(message_NetworkBehaviour_ID.NetworkID_ObjectIndex);
			if (networkIDFromObjectIndex != null)
			{
				return networkIDFromObjectIndex.Deserialize(msg, remoteID);
			}
			break;
		}
		}
		return false;
	}
}
