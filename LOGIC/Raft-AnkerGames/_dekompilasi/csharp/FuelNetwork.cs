using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
[RequireComponent(typeof(Fuel))]
public class FuelNetwork : MonoBehaviour, IRaycastable
{
	[SerializeField]
	private MonoBehaviour_ID reciever;

	private Fuel fuel;

	private Raft_Network network;

	private Network_Player localPlayer;

	private DisplayTextManager displayText;

	public Fuel Fuel => fuel;

	private void Awake()
	{
		fuel = GetComponent<Fuel>();
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		displayText = ComponentManager<DisplayTextManager>.Value;
		if (Raft_Network.IsHost)
		{
			fuel.OnFuelRemovedEvent += OnFuelRemoved;
		}
	}

	private void OnFuelRemoved()
	{
		Message_Fuel message = new Message_Fuel(Messages.Fuel_Increment, network.NetworkIDManager, reciever.ObjectIndex, localPlayer.steamID, -1);
		network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
	}

	public bool OnFuelMessage(Message_Fuel msgFuel)
	{
		Network_Player playerFromID = network.GetPlayerFromID(msgFuel.SteamID);
		return AddFuel(playerFromID, msgFuel.incrementAmount);
	}

	private bool AddFuel(Network_Player player, int incrementAmount)
	{
		bool flag = incrementAmount > 0;
		if (fuel.HasMaxFuel() & flag)
		{
			return false;
		}
		fuel.AddFuel(incrementAmount);
		if (flag && player != null)
		{
			if (player.IsLocalPlayer)
			{
				player.Inventory.RemoveItem(fuel.fuelItem.UniqueName, incrementAmount);
			}
			player.Animator.SetAnimation(PlayerAnimation.Trigger_Plant, triggering: true);
		}
		return true;
	}

	public void OnIsRayed()
	{
		if (displayText == null)
		{
			displayText = ComponentManager<DisplayTextManager>.Value;
		}
		if (CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance) || fuel.HasMaxFuel() || localPlayer == null)
		{
			displayText.HideDisplayTexts();
		}
		else
		{
			if (fuel == null || fuel.fuelItem == null)
			{
				return;
			}
			int itemCount = localPlayer.Inventory.GetItemCount(fuel.fuelItem);
			if (!fuel.HasMaxFuel() && itemCount > 0)
			{
				InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
				LocalizationParameters.itemX = fuel.fuelItem.settings_Inventory.DisplayName;
				displayText.ShowText(Helper.GetTerm("Game/PlaceItemX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					Message_Fuel message = new Message_Fuel(Messages.Fuel_Increment, network.NetworkIDManager, reciever.ObjectIndex, localPlayer.steamID, 1);
					if (Raft_Network.IsHost)
					{
						AddFuel(localPlayer, 1);
						network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
					else
					{
						network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
			}
			else
			{
				displayText.HideDisplayTexts();
			}
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (displayText != null)
		{
			displayText.HideDisplayTexts();
		}
	}
}
