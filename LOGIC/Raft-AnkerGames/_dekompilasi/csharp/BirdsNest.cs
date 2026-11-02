using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class BirdsNest : MonoBehaviour_Network, IRaycastable
{
	public static List<BirdsNest> AllNests = new List<BirdsNest>();

	public GameObject[] eggs;

	public GameObject[] feathers;

	[SerializeField]
	private Item_Base eggItem;

	[SerializeField]
	private Item_Base featherItem;

	private Raft_Network network;

	private bool occupied;

	private DisplayTextManager displayTextManager;

	public bool Occuppied
	{
		get
		{
			if (!occupied)
			{
				return HasEgg;
			}
			return true;
		}
		set
		{
			occupied = value;
		}
	}

	public bool HasEgg => EggCount > 0;

	public int EggCount
	{
		get
		{
			int num = 0;
			GameObject[] array = eggs;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].activeInHierarchy)
				{
					num++;
				}
			}
			return num;
		}
	}

	public bool HasFeather => FeatherCount > 0;

	public int FeatherCount
	{
		get
		{
			int num = 0;
			GameObject[] array = feathers;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].activeInHierarchy)
				{
					num++;
				}
			}
			return num;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (AllNests.Contains(this))
		{
			AllNests.Remove(this);
		}
	}

	public void OnBlockPlaced()
	{
		if (!AllNests.Contains(this))
		{
			AllNests.Add(this);
		}
		network = ComponentManager<Raft_Network>.Value;
		displayTextManager = ComponentManager<DisplayTextManager>.Value;
	}

	public void LayFeather(int numberOfFeathers)
	{
		GameObject[] array = feathers;
		foreach (GameObject gameObject in array)
		{
			if (numberOfFeathers > 0)
			{
				gameObject.SetActive(value: true);
				numberOfFeathers--;
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	public bool PickupFeather(Network_Player player)
	{
		GameObject[] array = feathers;
		foreach (GameObject gameObject in array)
		{
			if (gameObject.activeInHierarchy)
			{
				gameObject.SetActive(value: false);
				if (player != null && player.IsLocalPlayer)
				{
					player.Inventory.AddItem(featherItem.UniqueName, 1);
				}
				return true;
			}
		}
		return false;
	}

	public void LayEgg(int numberOfEggs)
	{
		GameObject[] array = eggs;
		foreach (GameObject gameObject in array)
		{
			if (numberOfEggs > 0)
			{
				gameObject.SetActive(value: true);
				numberOfEggs--;
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	public bool PickupEgg(Network_Player player)
	{
		GameObject[] array = eggs;
		foreach (GameObject gameObject in array)
		{
			if (gameObject.activeInHierarchy)
			{
				gameObject.SetActive(value: false);
				if (player != null && player.IsLocalPlayer)
				{
					player.Inventory.AddItem(eggItem.UniqueName, 1);
				}
				return true;
			}
		}
		return false;
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Birdsnest(RGDType.Block_BirdsNest, GetComponent<Block>(), this);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_Birdnest_Create(Messages.BirdsNest_Create, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.BirdsNest_Create:
		{
			Message_Birdnest_Create message_Birdnest_Create = msg as Message_Birdnest_Create;
			for (int i = 0; i < eggs.Length; i++)
			{
				if (i < message_Birdnest_Create.activeEggs.Length)
				{
					eggs[i].SetActive(message_Birdnest_Create.activeEggs[i]);
				}
			}
			for (int j = 0; j < feathers.Length; j++)
			{
				if (j < message_Birdnest_Create.activeFeathers.Length)
				{
					feathers[j].SetActive(message_Birdnest_Create.activeFeathers[j]);
				}
			}
			return false;
		}
		case Messages.BirdsNest_LayEgg:
		{
			Message_Birdnest_Lay message_Birdnest_Lay2 = msg as Message_Birdnest_Lay;
			LayEgg(message_Birdnest_Lay2.numberOfItems);
			return true;
		}
		case Messages.BirdsNest_PickupEgg:
		{
			Message_NetworkBehaviour_SteamID message_NetworkBehaviour_SteamID2 = msg as Message_NetworkBehaviour_SteamID;
			return PickupEgg(network.GetPlayerFromID(message_NetworkBehaviour_SteamID2.SteamID));
		}
		case Messages.BirdsNest_LayFeather:
		{
			Message_Birdnest_Lay message_Birdnest_Lay = msg as Message_Birdnest_Lay;
			LayFeather(message_Birdnest_Lay.numberOfItems);
			return true;
		}
		case Messages.BirdsNest_PickupFeather:
		{
			Message_NetworkBehaviour_SteamID message_NetworkBehaviour_SteamID = msg as Message_NetworkBehaviour_SteamID;
			return PickupFeather(network.GetPlayerFromID(message_NetworkBehaviour_SteamID.SteamID));
		}
		default:
			return false;
		}
	}

	public void OnIsRayed()
	{
		if (displayTextManager == null)
		{
			return;
		}
		if ((!HasEgg && !HasFeather) || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			displayTextManager.HideDisplayTexts();
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		LocalizationParameters.itemX = (HasEgg ? eggItem.settings_Inventory.DisplayName : featherItem.settings_Inventory.DisplayName);
		displayTextManager.ShowText(Helper.GetTerm("Game/PickUpX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			return;
		}
		displayTextManager.HideDisplayTexts();
		if (HasEgg)
		{
			Message_NetworkBehaviour_SteamID message = new Message_NetworkBehaviour_SteamID(Messages.BirdsNest_PickupEgg, this, network.GetLocalPlayer().steamID);
			if (Raft_Network.IsHost)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				PickupEgg(network.GetLocalPlayer());
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		if (HasFeather)
		{
			Message_NetworkBehaviour_SteamID message2 = new Message_NetworkBehaviour_SteamID(Messages.BirdsNest_PickupFeather, this, network.GetLocalPlayer().steamID);
			if (Raft_Network.IsHost)
			{
				network.RPC(message2, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				PickupFeather(network.GetLocalPlayer());
			}
			else
			{
				network.SendP2P(network.HostID, message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (displayTextManager != null)
		{
			displayTextManager.HideDisplayTexts();
		}
	}
}
