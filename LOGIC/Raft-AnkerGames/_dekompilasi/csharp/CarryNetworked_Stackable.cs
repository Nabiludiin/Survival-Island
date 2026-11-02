using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class CarryNetworked_Stackable : CarryNetworked
{
	[SerializeField]
	private Vector3 localStackOffset;

	[SerializeField]
	private GizmoBox occupyingGizmoBox;

	[SerializeField]
	private Collider stackableCollider;

	private CarryNetworked_Stackable aimedAtStackable;

	protected override void Start()
	{
		base.Start();
		if (!(stackableCollider != null))
		{
			return;
		}
		stackableCollider.enabled = false;
		if (network != null)
		{
			Network_Player localPlayer = network.GetLocalPlayer();
			if (localPlayer != null)
			{
				CarryingComponent carryingComponent = localPlayer.CarryingComponent;
				carryingComponent.OnStartCarry = (Action<Carry>)Delegate.Combine(carryingComponent.OnStartCarry, new Action<Carry>(OnLocalPlayerStartCarryObject));
				CarryingComponent carryingComponent2 = localPlayer.CarryingComponent;
				carryingComponent2.OnStopCarry = (Action<Carry>)Delegate.Combine(carryingComponent2.OnStopCarry, new Action<Carry>(OnLocalPlayerStopCarryObject));
			}
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (stackableCollider != null && network != null)
		{
			Network_Player localPlayer = network.GetLocalPlayer();
			if (localPlayer != null)
			{
				CarryingComponent carryingComponent = localPlayer.CarryingComponent;
				carryingComponent.OnStartCarry = (Action<Carry>)Delegate.Remove(carryingComponent.OnStartCarry, new Action<Carry>(OnLocalPlayerStartCarryObject));
				CarryingComponent carryingComponent2 = localPlayer.CarryingComponent;
				carryingComponent2.OnStopCarry = (Action<Carry>)Delegate.Remove(carryingComponent2.OnStopCarry, new Action<Carry>(OnLocalPlayerStopCarryObject));
			}
		}
	}

	private void OnLocalPlayerStartCarryObject(Carry carryObject)
	{
		if (carryObject != null && carryObject.carryObjectType == carryScript.carryObjectType && !carryScript.IsBeingCarried)
		{
			stackableCollider.enabled = true;
		}
	}

	private void OnLocalPlayerStopCarryObject(Carry carryObject)
	{
		if (carryObject != null && carryObject.carryObjectType == carryScript.carryObjectType)
		{
			stackableCollider.enabled = false;
		}
	}

	public override bool StartCarry(Network_Player player)
	{
		CarryNetworked_Stackable stackedObjectAboveStackableRecursive = GetStackedObjectAboveStackableRecursive(this);
		if (stackedObjectAboveStackableRecursive != null && stackedObjectAboveStackableRecursive != this && !stackedObjectAboveStackableRecursive.carryScript.IsBeingCarried)
		{
			return stackedObjectAboveStackableRecursive.StartCarry(player);
		}
		if (stackableCollider != null)
		{
			stackableCollider.enabled = false;
		}
		return base.StartCarry(player);
	}

	protected override void OnStopCarry(Network_Player player, bool force)
	{
		if (!force && !AllowPlacement())
		{
			return;
		}
		CarryNetworked carryNetworked = null;
		if (player.IsLocalPlayer)
		{
			CarryNetworked_Stackable stackableAtCursor = GetStackableAtCursor();
			if (stackableAtCursor != null && stackableAtCursor.carryScript.carryObjectType == carryScript.carryObjectType)
			{
				CarryNetworked_Stackable stackedObjectAboveStackableRecursive = GetStackedObjectAboveStackableRecursive(stackableAtCursor);
				carryNetworked = ((!(stackedObjectAboveStackableRecursive != null)) ? stackableAtCursor : stackedObjectAboveStackableRecursive);
			}
			if (carryNetworked == null && !player.PersonController.IsConsideredGrounded)
			{
				return;
			}
		}
		if (carryNetworked != null)
		{
			Message_StopCarryStack message = new Message_StopCarryStack(Messages.StopCarryStack, network.NetworkIDManager, ObjectIndex, player.steamID, carryNetworked);
			if (Raft_Network.IsHost)
			{
				StopCarryStack(player, carryNetworked);
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			return;
		}
		Vector3 dropPosition = GetDropPosition(player, force);
		Message_StopCarryPosition message2 = new Message_StopCarryPosition(Messages.StopCarryItem, network.NetworkIDManager, ObjectIndex, player.steamID, dropPosition);
		if (Raft_Network.IsHost)
		{
			StopCarry(player, dropPosition);
			network.RPC(message2, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	protected override Vector3 GetNewGhostPosition(bool force)
	{
		aimedAtStackable = null;
		CarryNetworked_Stackable stackableAtCursor = GetStackableAtCursor();
		if (stackableAtCursor != null && stackableAtCursor.carryScript.carryObjectType == carryScript.carryObjectType)
		{
			CarryNetworked_Stackable stackedObjectAboveStackableRecursive = GetStackedObjectAboveStackableRecursive(stackableAtCursor);
			if (stackedObjectAboveStackableRecursive != null)
			{
				aimedAtStackable = stackedObjectAboveStackableRecursive;
			}
			else
			{
				aimedAtStackable = stackableAtCursor;
			}
		}
		if (aimedAtStackable == null)
		{
			return base.GetNewGhostPosition(force);
		}
		return aimedAtStackable.transform.TransformPoint(localStackOffset);
	}

	protected override void SetGhostRotation()
	{
		if (aimedAtStackable == null)
		{
			base.SetGhostRotation();
		}
		else
		{
			currentGhost.transform.eulerAngles = aimedAtStackable.transform.eulerAngles;
		}
	}

	private bool StopCarryStack(Network_Player player, CarryNetworked other)
	{
		if (carryScript == null || carryScript.carryingPlayer == null)
		{
			return false;
		}
		if (other == null)
		{
			return false;
		}
		if (carryScript.body != null)
		{
			carryScript.body.isKinematic = true;
		}
		if (player.IsLocalPlayer)
		{
			SetGhostActiveStatus(active: false);
			SetShadowMode(castShadows: true);
		}
		carryScript.StopCarrying(player);
		ListenToCharacterPerspectiveChange(player, value: false);
		if (objectToParent != null)
		{
			objectToParent.SetParentSafe(startingParent);
			objectToParent.position = other.transform.TransformPoint(localStackOffset);
			objectToParent.eulerAngles = other.transform.eulerAngles;
			if (changeScale)
			{
				objectToParent.localScale = Vector3.one;
			}
		}
		return true;
	}

	private CarryNetworked_Stackable GetStackedObjectAboveStackableRecursive(CarryNetworked_Stackable stackable)
	{
		CarryNetworked_Stackable carryNetworked_Stackable = GetStackedObjectAboveStackable(stackable);
		CarryNetworked_Stackable carryNetworked_Stackable2 = carryNetworked_Stackable;
		int num = 100;
		while (carryNetworked_Stackable2 != null)
		{
			carryNetworked_Stackable2 = GetStackedObjectAboveStackable(carryNetworked_Stackable2);
			if (carryNetworked_Stackable2 != null)
			{
				carryNetworked_Stackable = carryNetworked_Stackable2;
			}
			if (--num <= 0)
			{
				break;
			}
		}
		return carryNetworked_Stackable;
	}

	private CarryNetworked_Stackable GetStackedObjectAboveStackable(CarryNetworked_Stackable stackable)
	{
		List<CarryNetworked_Stackable> overlaps = stackable.occupyingGizmoBox.GetOverlaps<CarryNetworked_Stackable>(-1, QueryTriggerInteraction.Ignore);
		if (overlaps != null && overlaps.Count > 0)
		{
			return overlaps[0];
		}
		return null;
	}

	private CarryNetworked_Stackable GetStackableAtCursor()
	{
		if (Helper.HitAtCursor(out var hit, Player.UseDistance * 2f, LayerMasks.MASK_RaycastInteractable))
		{
			CarryNetworked_Stackable componentInParent = hit.collider.GetComponentInParent<CarryNetworked_Stackable>();
			if (componentInParent != this)
			{
				return componentInParent;
			}
		}
		return null;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.StopCarryStack)
		{
			Message_StopCarryStack message_StopCarryStack = msg as Message_StopCarryStack;
			CarryNetworked networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex<CarryNetworked>(message_StopCarryStack.otherCarryObjectIndex);
			return StopCarryStack(network.GetPlayerFromID(message_StopCarryStack.SteamID), networkIDFromObjectIndex);
		}
		return base.Deserialize(msg, remoteID);
	}
}
