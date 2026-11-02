using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Container : MonoBehaviour_ID_Network, IRaycastable
{
	private Raft_Network _network;

	[SerializeField]
	private CarryNetworked objectToSpawn;

	[SerializeField]
	private Transform spawnLocation;

	[SerializeField]
	private int maxItems = 5;

	public List<CarryNetworked> spawnedItems = new List<CarryNetworked>();

	public AreaZone zoneLimitation;

	public EventReference er_takeFromContainer;

	private DisplayTextManager displayText;

	private Network_Player localPlayer;

	private Raft_Network Network
	{
		get
		{
			if (_network == null)
			{
				_network = ComponentManager<Raft_Network>.Value;
			}
			return _network;
		}
	}

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		displayText = ComponentManager<DisplayTextManager>.Value;
	}

	protected virtual void OnEnable()
	{
		foreach (CarryNetworked spawnedItem in spawnedItems)
		{
			if (spawnedItem != null)
			{
				spawnedItem.gameObject.SetActiveSafe(value: true);
			}
		}
	}

	protected virtual void OnDisable()
	{
		foreach (CarryNetworked spawnedItem in spawnedItems)
		{
			if (spawnedItem != null)
			{
				spawnedItem.gameObject.SetActiveSafe(value: false);
			}
		}
	}

	public void OnIsRayed()
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			if (localPlayer == null)
			{
				localPlayer = ComponentManager<Network_Player>.Value;
			}
			if (!localPlayer.CarryingComponent.IsCarrying)
			{
				SpawnObjectNetworked(objectToSpawn.gameObject);
			}
		}
	}

	public void OnRayEnter()
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (!localPlayer.CarryingComponent.IsCarrying)
		{
			displayText.ShowText(Helper.GetTerm("Game/PickUp"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		}
	}

	public void OnRayExit()
	{
		displayText.HideDisplayTexts();
	}

	public void DestroyAllItems()
	{
		for (int i = 0; i < spawnedItems.Count; i++)
		{
			CarryNetworked carryNetworked = spawnedItems[i];
			if (!(carryNetworked == null))
			{
				carryNetworked.OnPreDestroy();
				UnityEngine.Object.Destroy(carryNetworked.gameObject);
			}
		}
		spawnedItems.Clear();
	}

	public virtual CarryNetworked SpawnObjectLocal(Network_Player player, uint spawnedObjectIndex)
	{
		if (Raft_Network.IsHost && maxItems > 0 && spawnedItems.Count >= maxItems)
		{
			for (int i = 0; i < spawnedItems.Count; i++)
			{
				CarryNetworked carryNetworked = spawnedItems[i];
				if (carryNetworked != null)
				{
					DestroyItemNetworked(carryNetworked.ObjectIndex);
					break;
				}
			}
		}
		CarryNetworked newCarryItem = UnityEngine.Object.Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity, transform);
		newCarryItem.ZoneLimitation = zoneLimitation;
		newCarryItem.Initialize(spawnedObjectIndex, transform);
		CarryNetworked carryNetworked2 = newCarryItem;
		carryNetworked2.OnDestroyed = (Action)Delegate.Combine(carryNetworked2.OnDestroyed, (Action)(() =>
		{
			spawnedItems.Remove(newCarryItem);
		}));
		spawnedItems.Add(newCarryItem);
		if (player != null && newCarryItem != null && newCarryItem.carryScript != null && newCarryItem.carryScript.OnStartCarry != null)
		{
			if (!er_takeFromContainer.IsNull)
			{
				RuntimeManager.PlayOneShot(er_takeFromContainer, transform.position);
			}
			newCarryItem.StartCarry(player);
		}
		return newCarryItem;
	}

	protected void SpawnObjectNetworked(GameObject prefab)
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		Message_Container_ObjectIndex message_Container_ObjectIndex = new Message_Container_ObjectIndex(Messages.Container_SpawnObject, Network.NetworkIDManager, ObjectIndex, localPlayer.steamID, 0u);
		if (Raft_Network.IsHost)
		{
			message_Container_ObjectIndex.itemObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			SpawnObjectLocal(localPlayer, message_Container_ObjectIndex.itemObjectIndex);
			Network.RPC(message_Container_ObjectIndex, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			Network.SendP2P(Network.HostID, message_Container_ObjectIndex, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	protected void DestroyItemNetworked(uint objectIndex)
	{
		if (spawnedItems == null)
		{
			return;
		}
		for (int i = 0; i < spawnedItems.Count; i++)
		{
			CarryNetworked carryNetworked = spawnedItems[i];
			if (carryNetworked == null)
			{
				spawnedItems.RemoveAt(i);
				i--;
			}
			else if (carryNetworked.ObjectIndex == objectIndex)
			{
				carryNetworked.OnPreDestroy();
				spawnedItems.RemoveAt(i);
				i--;
				UnityEngine.Object.Destroy(carryNetworked.gameObject);
			}
		}
		if (Raft_Network.IsHost)
		{
			Network.RPC(new Message_Container_ObjectIndex(Messages.Container_DestroyObject, Network.NetworkIDManager, ObjectIndex, 0uL, objectIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Container_SpawnObject:
		{
			Message_Container_ObjectIndex message_Container_ObjectIndex2 = msg as Message_Container_ObjectIndex;
			Network_Player playerFromID = Network.GetPlayerFromID(message_Container_ObjectIndex2.SteamID);
			if (Raft_Network.IsHost)
			{
				message_Container_ObjectIndex2.itemObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
				SpawnObjectLocal(playerFromID, message_Container_ObjectIndex2.itemObjectIndex);
			}
			else
			{
				SpawnObjectLocal(playerFromID, message_Container_ObjectIndex2.itemObjectIndex);
			}
			return true;
		}
		case Messages.Container_DestroyObject:
		{
			Message_Container_ObjectIndex message_Container_ObjectIndex = msg as Message_Container_ObjectIndex;
			DestroyItemNetworked(message_Container_ObjectIndex.itemObjectIndex);
			return false;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
