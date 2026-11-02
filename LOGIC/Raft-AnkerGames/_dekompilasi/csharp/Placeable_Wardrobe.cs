using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Placeable_Wardrobe : MonoBehaviour_ID_Network, IBlockPlaced
{
	[HideInInspector]
	public Raft_Network network;

	[SerializeField]
	private Animator animator;

	public EventReference er_open;

	public EventReference er_close;

	private HashSet<Network_Player> playersUsingIt = new HashSet<Network_Player>();

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this, typeof(Placeable_Wardrobe));
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		NetworkIDManager.AddNetworkID(this, typeof(Placeable_Wardrobe));
	}

	private void UpdateOpen()
	{
		bool flag = playersUsingIt.Count > 0;
		bool flag2 = animator.GetBool("Open");
		animator.SetBool("Open", flag);
		if (!flag2 & flag)
		{
			RuntimeManager.PlayOneShot(er_open, transform.position);
		}
		else if (flag2 && !flag)
		{
			RuntimeManager.PlayOneShot(er_close, transform.position);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Wardrobe_Open:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID2 = msg as Message_NetworkBehaviour_ID_SteamID;
			Network_Player playerFromID2 = network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID2.SteamID);
			playersUsingIt.Add(playerFromID2);
			UpdateOpen();
			return true;
		}
		case Messages.Wardrobe_Close:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			Network_Player playerFromID = network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID);
			playersUsingIt.Remove(playerFromID);
			UpdateOpen();
			return true;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
