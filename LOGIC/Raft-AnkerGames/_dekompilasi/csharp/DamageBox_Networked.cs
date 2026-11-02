using Steamworks;

public class DamageBox_Networked : DamageBox
{
	public MonoBehaviour_ID_Network trackedVia;

	protected override bool HandleDamagePlayer(Network_Player player)
	{
		bool flag = false;
		if (player.IsLocalPlayer)
		{
			flag = base.HandleDamagePlayer(player);
			if (flag && trackedVia != null)
			{
				SendMessageNetworkedHit(player);
				flag = true;
			}
		}
		return flag;
	}

	private void SendMessageNetworkedHit(Network_Player entity)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.DamageBox_Hit, network.NetworkIDManager, trackedVia.ObjectIndex, entity.steamID);
		if (Raft_Network.IsHost)
		{
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}
}
