using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class CarryNetworked_InterestPoint : CarryNetworked
{
	[HideInInspector]
	public ObjectOfInterestType currentInterestType;

	[HideInInspector]
	public ObjectOfInterestTracker aimedAtInterestPoint;

	[SerializeField]
	private List<ObjectOfInterestType> validDropTypes = new List<ObjectOfInterestType>();

	protected override void OnStartCarry(Network_Player player)
	{
		base.OnStartCarry(player);
		currentInterestType = ObjectOfInterestType.None;
	}

	protected override void OnStopCarry(Network_Player player, bool force)
	{
		ObjectOfInterestTracker objectOfInterestTracker = RaycastForInterestPoint();
		if (objectOfInterestTracker != null && objectOfInterestTracker.transform.childCount == 0)
		{
			if (!er_stopCarry.IsNull)
			{
				PlayOneShot(er_stopCarry, objectOfInterestTracker.transform.position);
			}
			Message_StopCarryObjectInterest message = new Message_StopCarryObjectInterest(Messages.StopCarryItemInterest, network.NetworkIDManager, ObjectIndex, player.steamID, objectOfInterestTracker.trackType);
			if (Raft_Network.IsHost)
			{
				if (!StopCarryInterest(player, objectOfInterestTracker.trackType))
				{
					return;
				}
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			Vector3 dropPosition = GetDropPosition(player, force);
			Message_StopCarryPosition message2 = new Message_StopCarryPosition(Messages.StopCarryItem, network.NetworkIDManager, ObjectIndex, player.steamID, dropPosition);
			if (Raft_Network.IsHost)
			{
				if (!StopCarry(player, dropPosition))
				{
					return;
				}
				network.RPC(message2, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		if (player.IsLocalPlayer)
		{
			SetGhostActiveStatus(active: false);
			SetShadowMode(castShadows: true);
		}
	}

	protected override Vector3 GetNewGhostPosition(bool force)
	{
		aimedAtInterestPoint = RaycastForInterestPoint();
		if (aimedAtInterestPoint != null)
		{
			return aimedAtInterestPoint.transform.position;
		}
		return base.GetNewGhostPosition(force);
	}

	protected override void SetGhostRotation()
	{
		if (aimedAtInterestPoint != null)
		{
			currentGhost.transform.eulerAngles = aimedAtInterestPoint.transform.eulerAngles;
		}
		else
		{
			base.SetGhostRotation();
		}
	}

	public void AttachToInterest(ObjectOfInterestType interestType)
	{
		if (carryScript == null)
		{
			return;
		}
		Transform objectOfInterest = ObjectManager.GetObjectOfInterest(interestType);
		if (!(objectOfInterest == null))
		{
			currentInterestType = interestType;
			if (carryScript.body != null)
			{
				carryScript.body.isKinematic = true;
			}
			if (objectToParent != null)
			{
				objectToParent.SetParentSafe(objectOfInterest);
				Transform transform = objectToParent;
				Vector3 localPosition = (objectToParent.localEulerAngles = Vector3.zero);
				transform.localPosition = localPosition;
			}
		}
	}

	private bool StopCarryInterest(Network_Player player, ObjectOfInterestType interestType)
	{
		if (carryScript == null || carryScript.carryingPlayer == null)
		{
			return false;
		}
		Transform objectOfInterest = ObjectManager.GetObjectOfInterest(interestType);
		if (objectOfInterest == null)
		{
			return false;
		}
		currentInterestType = interestType;
		if (carryScript.body != null)
		{
			carryScript.body.isKinematic = true;
		}
		carryScript.StopCarrying(player);
		ListenToCharacterPerspectiveChange(player, value: false);
		if (objectToParent != null)
		{
			objectToParent.SetParentSafe(objectOfInterest);
			Transform transform = objectToParent;
			Vector3 localPosition = (objectToParent.localEulerAngles = Vector3.zero);
			transform.localPosition = localPosition;
		}
		return true;
	}

	private ObjectOfInterestTracker RaycastForInterestPoint()
	{
		if (Helper.HitAtCursor(out var hit, Player.UseDistance * 2f, LayerMasks.MASK_RaycastInteractable))
		{
			ObjectOfInterestTracker component = hit.collider.GetComponent<ObjectOfInterestTracker>();
			if (component != null && validDropTypes.Contains(component.trackType))
			{
				if (component.maxYDiffToInteract != 0f && Mathf.Abs(network.GetLocalPlayer().transform.position.y - hit.point.y) > component.maxYDiffToInteract)
				{
					return null;
				}
				return component;
			}
		}
		return null;
	}

	public override RGD_CarryNetwork GetSaveData()
	{
		return new RGD_CarryNetworkInterestPoint(this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.StopCarryItemInterest)
		{
			Message_StopCarryObjectInterest message_StopCarryObjectInterest = msg as Message_StopCarryObjectInterest;
			return StopCarryInterest(network.GetPlayerFromID(message_StopCarryObjectInterest.SteamID), message_StopCarryObjectInterest.interestType);
		}
		return base.Deserialize(msg, remoteID);
	}
}
