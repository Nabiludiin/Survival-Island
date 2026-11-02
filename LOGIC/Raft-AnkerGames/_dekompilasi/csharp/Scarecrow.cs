using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class Scarecrow : MonoBehaviour_ID_Network, IBlockPlaced
{
	public bool invurnerable;

	public BoxCollider mainCollider;

	[SerializeField]
	private List<Rigidbody> parts = new List<Rigidbody>();

	public EventReference er_break;

	[HideInInspector]
	public Block block;

	private Raft_Network network;

	private bool occupied;

	public bool Destroyed => Health <= 0;

	public int Health
	{
		get
		{
			if (!invurnerable)
			{
				return parts.Count;
			}
			return 1;
		}
	}

	public bool Occupied
	{
		get
		{
			return occupied;
		}
		set
		{
			occupied = value;
		}
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		block = GetComponent<Block>();
	}

	protected override void OnDestroy()
	{
		NetworkIDManager.RemoveNetworkID(this);
		base.OnDestroy();
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		NetworkIDManager.AddNetworkID(this);
	}

	private void DestroyScarecrowLate()
	{
		BlockCreator.RemoveBlockNetwork(block, null, updateRaftBounds: false);
	}

	public void DamageScarecrow()
	{
		if (!invurnerable && !Destroyed)
		{
			Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.Scarecrow_Damage, network.NetworkIDManager, ObjectIndex);
			if (Raft_Network.IsHost)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				RemovePart();
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void DestroyPartInstantly()
	{
		if (!invurnerable && Health > 0)
		{
			if (block == null)
			{
				block = GetComponent<Block>();
			}
			block.itemToReturnOnDestroy = null;
			Rigidbody rigidbody = parts[0];
			if (rigidbody != null)
			{
				parts.RemoveAt(0);
				Object.Destroy(rigidbody.gameObject);
			}
		}
	}

	private void RemovePart()
	{
		if (invurnerable)
		{
			return;
		}
		if (Health > 0)
		{
			block.itemToReturnOnDestroy = null;
			Rigidbody rigidbody = parts[0];
			if (rigidbody != null)
			{
				RuntimeManager.PlayOneShotAttached(er_break, rigidbody.gameObject);
				rigidbody.transform.SetParentSafe(transform.parent);
				rigidbody.isKinematic = false;
				rigidbody.GetComponent<OnCollisionPlaySound>().active = true;
				rigidbody.GetComponent<WaterFloatSemih_Rigidbody>().enabled = true;
				parts.RemoveAt(0);
				Object.Destroy(rigidbody.gameObject, 10f);
				OccupyingComponent component = GetComponent<OccupyingComponent>();
				if (component != null)
				{
					component.FindRenderers();
				}
			}
		}
		if (Raft_Network.IsHost && Destroyed)
		{
			Invoke("DestroyScarecrowLate", 3f);
		}
	}

	public override RGD Serialize_Save()
	{
		if (!Destroyed)
		{
			return new RGD_Scarecrow(RGDType.Block_Scarecrow, GetComponent<Block>(), this);
		}
		return null;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Scarecrow_Damage)
		{
			RemovePart();
			return true;
		}
		return false;
	}
}
