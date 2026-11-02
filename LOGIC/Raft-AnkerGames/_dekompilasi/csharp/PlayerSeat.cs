using System;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSeat : MonoBehaviour, IRaycastable
{
	private enum PriorityType
	{
		InOrder,
		ByDistance
	}

	private Raft_Network _network;

	[SerializeField]
	private MonoBehaviour_ID_Network receiver;

	[SerializeField]
	private PriorityType takeSeatType;

	[SerializeField]
	private AttachPlayer[] attachPlayerScripts;

	private static CanvasHelper canvas;

	private static Network_Player localPlayer;

	private bool canLeave;

	private bool localPlayerIsSitting;

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

	public AttachPlayer[] Seats => attachPlayerScripts;

	private void Awake()
	{
		AttachPlayer[] array = attachPlayerScripts;
		foreach (AttachPlayer obj in array)
		{
			obj.OnStartCarryPlayer = (Action<Network_Player>)Delegate.Combine(obj.OnStartCarryPlayer, (Action<Network_Player>)((Network_Player player) =>
			{
				if (player.IsLocalPlayer)
				{
					localPlayerIsSitting = true;
				}
			}));
			obj.OnStopCarryPlayer = (Action<Network_Player>)Delegate.Combine(obj.OnStopCarryPlayer, (Action<Network_Player>)((Network_Player player) =>
			{
				if (player.IsLocalPlayer)
				{
					localPlayerIsSitting = false;
				}
			}));
		}
	}

	private void Start()
	{
		if (canvas == null)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
		}
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
	}

	private void Update()
	{
		if (canLeave && localPlayerIsSitting)
		{
			InputAction action = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.GetAction("Cancel", "Crouch");
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.PlayerSeat_Leave, Network.NetworkIDManager, receiver.ObjectIndex, localPlayer.steamID);
				if (Raft_Network.IsHost)
				{
					if (LeaveSeat(localPlayer))
					{
						Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
				else
				{
					Network.SendP2P(Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		canLeave = true;
	}

	void IRaycastable.OnIsRayed()
	{
		bool flag = true;
		AttachPlayer[] array = attachPlayerScripts;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].HasPlayerAttached)
			{
				flag = false;
				break;
			}
		}
		if (!(localPlayer != null) || PlayerItemManager.IsBusy || CanvasHelper.ActiveMenu != MenuType.None || localPlayer.PlayerScript.IsDead || localPlayer.BedComponent.Sleeping || localPlayerIsSitting || flag)
		{
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Use"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			return;
		}
		canvas.displayTextManager.HideDisplayTexts();
		Message_PlayerTakeSeat message_PlayerTakeSeat = new Message_PlayerTakeSeat(Messages.PlayerSeat_Take, Network.NetworkIDManager, receiver.ObjectIndex, localPlayer.steamID);
		if (Raft_Network.IsHost)
		{
			if (TryTakingSeat(localPlayer, ref message_PlayerTakeSeat.seatIndex))
			{
				Network.RPC(message_PlayerTakeSeat, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			Network.SendP2P(Network.HostID, message_PlayerTakeSeat, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		canvas.displayTextManager.HideDisplayTexts();
	}

	private bool LeaveSeat(Network_Player player)
	{
		if (player == null)
		{
			return false;
		}
		AttachPlayer[] array = attachPlayerScripts;
		foreach (AttachPlayer attachPlayer in array)
		{
			if (attachPlayer != null && attachPlayer.carriedPlayer == player)
			{
				return LeaveSeat(attachPlayer);
			}
		}
		return false;
	}

	private bool LeaveSeat(AttachPlayer attachScript)
	{
		if (attachScript == null || attachScript.carriedPlayer == null)
		{
			return false;
		}
		attachScript.StopCarryingPlayer(manipulatePosition: true);
		return true;
	}

	private bool TryTakingSeat(Network_Player player, ref int seatIndex)
	{
		AttachPlayer attachPlayer = null;
		if (Raft_Network.IsHost && seatIndex < 0)
		{
			switch (takeSeatType)
			{
			case PriorityType.InOrder:
			{
				for (int j = 0; j < attachPlayerScripts.Length; j++)
				{
					if (!attachPlayerScripts[j].HasPlayerAttached)
					{
						seatIndex = j;
						break;
					}
				}
				break;
			}
			case PriorityType.ByDistance:
				if (attachPlayerScripts.Length == 1)
				{
					seatIndex = 0;
				}
				else
				{
					if (attachPlayerScripts.Length <= 1)
					{
						break;
					}
					float num = float.MaxValue;
					for (int i = 0; i < attachPlayerScripts.Length; i++)
					{
						AttachPlayer attachPlayer2 = attachPlayerScripts[i];
						if (!(attachPlayer2 == null) && !attachPlayer2.HasPlayerAttached)
						{
							float num2 = Vector3.Distance(attachPlayer2.transform.position, player.transform.position);
							if (num2 < num)
							{
								num = num2;
								seatIndex = i;
							}
						}
					}
				}
				break;
			}
		}
		if (seatIndex >= 0 && seatIndex < attachPlayerScripts.Length)
		{
			AttachPlayer attachPlayer3 = attachPlayerScripts[seatIndex];
			if (!Raft_Network.IsHost || !attachPlayer3.HasPlayerAttached)
			{
				attachPlayer = attachPlayer3;
			}
		}
		if (attachPlayer == null)
		{
			return false;
		}
		attachPlayer.StartCarryingPlayer(player);
		player.PersonController.externalVelocity = Vector3.zero;
		player.PersonController.ResetPlayerPivotToOriginal();
		if (player.IsLocalPlayer)
		{
			player.Animator.SetAnimation(PlayerAnimation.Index_0_Idle);
			canLeave = false;
		}
		return true;
	}

	public bool HandleDeserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.PlayerSeat_Take:
		{
			Message_PlayerTakeSeat message_PlayerTakeSeat = msg as Message_PlayerTakeSeat;
			Network_Player playerFromID = Network.GetPlayerFromID(message_PlayerTakeSeat.SteamID);
			if (playerFromID == null)
			{
				return false;
			}
			return TryTakingSeat(playerFromID, ref message_PlayerTakeSeat.seatIndex);
		}
		case Messages.PlayerSeat_Leave:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			LeaveSeat(Network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
			return true;
		}
		default:
			return false;
		}
	}
}
