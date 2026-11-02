public class AI_NetworkBehaviour_Pig : AI_NetworkBehaviour_Boar
{
	public DamageBox_Networked netBox;

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.DamageBox_Hit)
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			netBox.OnDamage?.Invoke(Network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
			return true;
		}
		return base.Deserialize(msg, remoteID);
	}
}
