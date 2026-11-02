using System;
using System.Collections;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Anchor_Throwable_Stand : MonoBehaviour_Network
{
	public Block block;

	[SerializeField]
	private GameObject pickupModel;

	[SerializeField]
	private Item_Base usableItemOnPickup;

	[SerializeField]
	private Rope rope;

	[SerializeField]
	private Transform ropeConnectionTransform;

	private static Raft raft;

	private Anchor_Throwable anchor_Throwable;

	private Anchor thrownAnchor;

	private bool fullyAnchored;

	private bool hasThrownAnchor;

	private bool hasBeenPlaced;

	private Item_Base previousItemInHand;

	private Raft_Network network;

	private Network_Player playerNetwork;

	public GameObject PickupModel => pickupModel;

	public Rope Rope => rope;

	public Transform RopeConnectionTransform => ropeConnectionTransform;

	public bool FullyAnchored
	{
		get
		{
			return fullyAnchored;
		}
		set
		{
			fullyAnchored = value;
		}
	}

	public bool HasThrownAnchor
	{
		get
		{
			return hasThrownAnchor;
		}
		set
		{
			hasThrownAnchor = value;
		}
	}

	private void Start()
	{
		block = GetComponent<Block>();
		raft = ComponentManager<Raft>.Value;
		network = ComponentManager<Raft_Network>.Value;
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void Update()
	{
		if (FullyAnchored)
		{
			if (!rope.gameObject.activeInHierarchy)
			{
				rope.gameObject.SetActive(value: true);
			}
			return;
		}
		SetRopePositionAndLayer();
		if (IsConnected())
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Fire"];
			InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Context"];
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "LMB"))
			{
				playerNetwork.Animator.anim.SetBool("HookThrow", value: true);
			}
			else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action2, "RMB") && !SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(action, "LMB"))
			{
				if (Raft_Network.IsHost)
				{
					network.RPC(new Message_NetworkBehaviour(Messages.DisconnectThrowableAnchorStand, this), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					DisconnectThrowableWithStand();
				}
				else
				{
					network.SendP2P(network.HostID, new Message_NetworkBehaviour(Messages.DisconnectThrowableAnchorStand, this), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		else if (thrownAnchor != null && !FullyAnchored && thrownAnchor.transform.position.y < GameManager.OceanBottom)
		{
			OnAnchorHit();
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (fullyAnchored)
		{
			if (thrownAnchor != null)
			{
				raft.RemoveAnchor();
			}
			else if (PickupModel != null)
			{
				raft.RemoveAnchor();
			}
		}
		DisconnectThrowableWithStand();
		if (thrownAnchor != null)
		{
			UnityEngine.Object.Destroy(thrownAnchor.gameObject);
		}
		else if (fullyAnchored)
		{
			UnityEngine.Object.Destroy(PickupModel.gameObject);
		}
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void OnBlockPlaced()
	{
		rope.autoTileMaterial = true;
	}

	public bool IsBusy()
	{
		return anchor_Throwable != null;
	}

	public void OnThrowableThrow(Anchor thrownAnchor)
	{
		block.itemToReturnOnDestroy = null;
		block.canBeRemoved = false;
		this.thrownAnchor = thrownAnchor;
		this.thrownAnchor.SetReciever(transform);
		this.thrownAnchor.transform.SetParent(null);
		this.thrownAnchor.gameObject.SetLayerSafe(LayerMask.NameToLayer("Anchor"));
		HasThrownAnchor = true;
		playerNetwork.Animator.anim.SetBool("HookThrow", value: false);
		playerNetwork.Animator.anim.SetBool("HookInHand", value: false);
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.PlayerItemManager.SelectUsable(null);
			StartCoroutine(ReselectItem(0.5f));
		}
	}

	private IEnumerator ReselectItem(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		if (playerNetwork != null)
		{
			if (playerNetwork.PlayerItemManager != null)
			{
				playerNetwork.PlayerItemManager.SelectUsable(previousItemInHand);
			}
			if (playerNetwork.Inventory == null)
			{
				playerNetwork.Inventory.hotbar.ReselectCurrentSlot();
			}
			if (playerNetwork.IsLocalPlayer)
			{
				PlayerItemManager.IsBusy = false;
			}
		}
	}

	public void OnAnchorHit()
	{
		if (!FullyAnchored && Raft_Network.IsHost)
		{
			thrownAnchor.GetComponent<Rigidbody>().isKinematic = true;
			fullyAnchored = true;
			network.RPC(new Message_ThrowableStandCreate(Messages.ThrowableStandCreate, this, thrownAnchor.transform), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			raft.AddAnchor(playSound: true, gameObject);
		}
	}

	public void DestroyStand()
	{
		BlockCreator.RemoveBlockNetwork(GetComponentInParent<Block>(), null, updateRaftBounds: true);
	}

	public bool ConnectStandWithThrowable(Network_Player player)
	{
		if (playerNetwork == player)
		{
			return false;
		}
		if (IsConnected())
		{
			return false;
		}
		block.canBeRemoved = false;
		pickupModel.SetActive(value: false);
		anchor_Throwable = player.GetComponentInChildren<Anchor_Throwable>();
		anchor_Throwable.anchor_stand = this;
		playerNetwork = player;
		playerNetwork.Animator.SetAnimation(PlayerAnimation.Index_2_Hook);
		if (player.IsLocalPlayer)
		{
			previousItemInHand = playerNetwork.PlayerItemManager.useItemController.GetCurrentItemInHand();
			playerNetwork.PlayerItemManager.SelectUsable(usableItemOnPickup);
			playerNetwork.Animator.anim.SetBool("HookInHand", value: true);
			PlayerItemManager.IsBusy = true;
		}
		return true;
	}

	public void DisconnectThrowableWithStand()
	{
		if (IsConnected())
		{
			if (block != null)
			{
				block.canBeRemoved = true;
			}
			anchor_Throwable.anchor_stand = null;
			pickupModel.SetActive(value: true);
			if (playerNetwork.IsLocalPlayer)
			{
				playerNetwork.Animator.anim.SetBool("HookInHand", value: false);
				playerNetwork.PlayerItemManager.SelectUsable(previousItemInHand);
				PlayerItemManager.IsBusy = false;
			}
			rope.ResetParent();
			rope.gameObject.SetActive(value: false);
			playerNetwork = null;
			anchor_Throwable = null;
		}
	}

	public Transform GetThrownAnchor()
	{
		if (FullyAnchored && thrownAnchor == null)
		{
			return PickupModel.transform;
		}
		return thrownAnchor.transform;
	}

	public Anchor GetAnchor()
	{
		return thrownAnchor;
	}

	private void SetRopePositionAndLayer()
	{
		if (IsConnected())
		{
			if (playerNetwork.IsLocalPlayer && !playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel)
			{
				rope.SetPosition(0, anchor_Throwable.anchorParent, Helper.WorldPointToFOVPointLocal(anchor_Throwable.anchorParent.position, anchor_Throwable.anchorParent));
				rope.SetPosition(1, RopeConnectionTransform);
			}
			else
			{
				rope.SetPosition(0, anchor_Throwable.anchorParent);
				rope.SetPosition(1, RopeConnectionTransform);
			}
		}
		else if (thrownAnchor != null)
		{
			rope.SetPosition(0, thrownAnchor.transform);
			rope.SetPosition(1, ropeConnectionTransform.position);
		}
		else
		{
			rope.ResetParent();
			rope.SetPosition(0, ropeConnectionTransform.position);
			rope.SetPosition(1, pickupModel.transform.position);
		}
	}

	private bool IsConnected()
	{
		if (anchor_Throwable == null)
		{
			return false;
		}
		return anchor_Throwable.anchor_stand == this;
	}

	private void OnWorldShift(Vector3 shift)
	{
		if (thrownAnchor != null)
		{
			thrownAnchor.transform.position -= shift;
		}
		else if (FullyAnchored)
		{
			PickupModel.transform.position -= shift;
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_AnchorThrowable(RGDType.Block_AnchorThrowable, GetComponent<Block>(), this);
	}

	public override void Serialize_Create()
	{
		if (HasThrownAnchor && FullyAnchored)
		{
			Transform anchor = ((thrownAnchor == null) ? PickupModel.transform : thrownAnchor.transform);
			AddMessage(new Message_ThrowableStandCreate(Messages.ThrowableStandCreate, this, anchor));
		}
		if (IsConnected())
		{
			AddMessage(new Message_NetworkBehaviour_SteamID(Messages.ConnectThrowableAnchorStand, this, network.GetIDFromPlayer(playerNetwork)));
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.ThrowableStandCreateInAir:
			pickupModel.SetActive(value: false);
			rope.gameObject.SetActive(value: false);
			return false;
		case Messages.ThrowableStandCreate:
		{
			if (thrownAnchor != null)
			{
				UnityEngine.Object.Destroy(thrownAnchor);
			}
			Message_ThrowableStandCreate message_ThrowableStandCreate = msg as Message_ThrowableStandCreate;
			pickupModel.SetActive(value: true);
			pickupModel.transform.SetParent(null);
			pickupModel.transform.position = message_ThrowableStandCreate.Position;
			pickupModel.transform.eulerAngles = message_ThrowableStandCreate.Rotation;
			fullyAnchored = true;
			if (block == null)
			{
				block = GetComponent<Block>();
			}
			if (block != null)
			{
				block.canBeRemoved = false;
			}
			rope.SetPosition(0, ropeConnectionTransform);
			rope.SetPosition(1, pickupModel.transform);
			if (raft == null)
			{
				raft = ComponentManager<Raft>.Value;
			}
			raft.AddAnchor(playSound: true, gameObject);
			return false;
		}
		case Messages.ConnectThrowableAnchorStand:
		{
			Message_NetworkBehaviour_SteamID message_NetworkBehaviour_SteamID = msg as Message_NetworkBehaviour_SteamID;
			return ConnectStandWithThrowable(network.GetPlayerFromID(message_NetworkBehaviour_SteamID.SteamID));
		}
		case Messages.DisconnectThrowableAnchorStand:
			DisconnectThrowableWithStand();
			return true;
		default:
			return false;
		}
	}
}
