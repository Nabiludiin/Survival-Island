using System;

[Serializable]
public class Message_DisconnectNotify : Message_SteamID
{
	public DisconnectReason disconnectReason;

	public bool saveGame;

	public Message_DisconnectNotify(Messages type, Network_UserId id, DisconnectReason disconnectReason, bool saveGame)
		: base(type, id)
	{
		this.disconnectReason = disconnectReason;
		this.saveGame = saveGame;
	}
}
