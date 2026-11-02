using System;
using Steamworks;

[Serializable]
public class Packet_Single : Packet
{
	public byte e;

	public Message message;

	public EP2PSend SendType
	{
		get
		{
			return (EP2PSend)e;
		}
		set
		{
			e = (byte)value;
		}
	}

	public Packet_Single(EP2PSend sendType)
	{
		SendType = sendType;
		PacketType = PacketType.Single;
	}

	public Packet_Single(EP2PSend sendType, Message message)
	{
		SendType = sendType;
		this.message = message;
		PacketType = PacketType.Single;
	}
}
