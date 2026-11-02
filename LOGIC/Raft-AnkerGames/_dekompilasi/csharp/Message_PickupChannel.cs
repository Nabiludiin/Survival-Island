using System;

[Serializable]
public class Message_PickupChannel : Message_NetworkBehaviour_ID_SteamID
{
	public Message_PickupChannel()
	{
	}

	public Message_PickupChannel(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, uint networkID_ObjectIndex)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
	}
}
