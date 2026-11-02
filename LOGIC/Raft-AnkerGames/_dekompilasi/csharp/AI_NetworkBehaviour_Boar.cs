public class AI_NetworkBehaviour_Boar : AI_NetworkBehaviour_Animal
{
	public AI_StateMachine_Boar boarSM;

	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		return new RGD_Boar(this);
	}

	public override void OnRestoreFinished(RGD_AI_NetworkBehaviour rgdNetworkBehaviour)
	{
		base.OnRestoreFinished(rgdNetworkBehaviour);
		RGD_Boar rGD_Boar = rgdNetworkBehaviour as RGD_Boar;
		if (!(rGD_Boar == null))
		{
			rGD_Boar.RestoreBoar(this);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Boar_Impale_Player:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			if (!boarSM.attachPlayer.HasPlayerAttached)
			{
				boarSM.attachPlayer.StartCarryingPlayer(Network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
				return true;
			}
			return false;
		}
		case Messages.Boar_Depale_Player:
			if (boarSM.attachPlayer.HasPlayerAttached)
			{
				boarSM.attachPlayer.StopCarryingPlayer(manipulatePosition: true);
				return true;
			}
			return false;
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
