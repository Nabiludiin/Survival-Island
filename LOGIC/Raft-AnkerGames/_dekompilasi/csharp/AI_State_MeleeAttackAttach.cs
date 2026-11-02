using Steamworks;

public class AI_State_MeleeAttackAttach : AI_State_MeleeAttack
{
	public AttachPlayer attachPlayer;

	public bool manipulatePosition = true;

	public AI_State successfullAttachState;

	public AI_State playerAlreadyAttachedState;

	private AI_NetworkBehaviour networkBehaviour;

	private Raft_Network network;

	protected override void Start()
	{
		base.Start();
		networkBehaviour = stateMachine.GetComponent<AI_NetworkBehaviour>();
		network = ComponentManager<Raft_Network>.Value;
	}

	protected override void StartAttack(Network_Player player)
	{
		if (Raft_Network.IsHost && player.PlayerNetworkManager.IsAttached)
		{
			stateMachine.ChangeState(playerAlreadyAttachedState);
		}
		else
		{
			base.StartAttack(player);
		}
	}

	protected override void SuccessFullAttackCall(Network_Player player)
	{
		base.SuccessFullAttackCall(player);
		if (Raft_Network.IsHost && attachPlayer != null && !player.PlayerNetworkManager.IsAttached)
		{
			StartCarryingPlayer(player);
			stateMachine.ChangeState(successfullAttachState);
			player.Network.RPC(new Message_NetworkBehaviour_ID_SteamID(Messages.AttachPlayerMeleeAttack, player.Network.NetworkIDManager, networkBehaviour.ObjectIndex, player.steamID), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void StartCarryingPlayer(Network_Player player)
	{
		attachPlayer.StartCarryingPlayer(player, manipulatePosition);
	}

	public bool FakeDeserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.AttachPlayerMeleeAttack)
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			Network_Player playerFromID = network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID);
			StartCarryingPlayer(playerFromID);
			return false;
		}
		return false;
	}
}
