using FMODUnity;
using Steamworks;
using UnityEngine;

public class Paddle : UsableTool
{
	[SerializeField]
	private float paddleForce = 5f;

	[SerializeField]
	private Transform particleTransform;

	public EventReference er_paddle;

	private Raft raft;

	protected override void Start()
	{
		base.Start();
		raft = ComponentManager<Raft>.Value;
	}

	public void OnPaddle()
	{
		if (playerNetwork.IsLocalPlayer && playerNetwork.PersonController.HasRaftAsParent && !(playerNetwork.transform.position.y > 2.5f) && playerNetwork.playerPivot.localEulerAngles.x <= 100f)
		{
			Vector3 forward = playerNetwork.transform.forward;
			float force = paddleForce;
			Message_Paddle message = new Message_Paddle(Messages.Paddle, playerNetwork, particleTransform.position, forward, force);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				PaddlePaddle(particleTransform.position, forward, force);
			}
			else
			{
				playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void PaddlePaddle(Vector3 position, Vector3 direction, float force)
	{
		if (GameModeValueManager.GetCurrentGameModeValue().raftSpecificVariables.isRaftAlwaysAnchored)
		{
			return;
		}
		position.y = 0f;
		if (Raft_Network.IsHost && raft != null && playerNetwork != null)
		{
			raft.AddForce(playerNetwork.transform.forward, paddleForce, ForceMode.Impulse, forceThroughAnchor: true);
		}
		if (raft != null)
		{
			GameObject gameObject = ParticleManager.PlaySystem("WaterSplash_Paddle", position);
			if (gameObject != null)
			{
				gameObject.transform.SetParent(raft.transform);
			}
		}
		RuntimeManager.PlayOneShot(er_paddle, position);
		if (playerNetwork != null && playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
		}
	}
}
