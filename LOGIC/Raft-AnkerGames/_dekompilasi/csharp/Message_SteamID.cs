using Steamworks;

public class Message_SteamID : Message
{
	public ulong id;

	public Message_SteamID(Messages type, Network_UserId id)
	{
		Type = type;
		this.id = id;
	}

	public CSteamID GetCSteamID()
	{
		return (CSteamID)id;
	}
}
