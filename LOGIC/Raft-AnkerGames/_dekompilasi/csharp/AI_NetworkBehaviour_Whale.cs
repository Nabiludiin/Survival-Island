using FMODUnity;
using Steamworks;
using UnityEngine;

public class AI_NetworkBehaviour_Whale : AI_NetworkBehaviour_Animal
{
	[SerializeField]
	private ParticleSystem waterParticle;

	[SerializeField]
	private AI_State diveState;

	public EventReference er_shootWater;

	private static Transform raft;

	private bool hasPlayedWater;

	protected override void UpdateHost()
	{
		base.UpdateHost();
		if (!hasPlayedWater && !(stateMachineAnimal.currentState != diveState))
		{
			ShootWater();
			network.RPC(new Message_NetworkBehaviour_ID(Messages.Whale_ShootWater, network.NetworkIDManager, ObjectIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void ShootWater()
	{
		hasPlayedWater = true;
		if (waterParticle != null)
		{
			waterParticle.Play();
		}
		RuntimeManager.PlayOneShot(er_shootWater, transform.position);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Whale_ShootWater)
		{
			ShootWater();
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
