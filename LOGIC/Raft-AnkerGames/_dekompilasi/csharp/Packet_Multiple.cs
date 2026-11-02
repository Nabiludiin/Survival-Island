using System;
using Steamworks;

[Serializable]
public class Packet_Multiple : Packet
{
	public byte e;

	public Message[] messages;

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

	public Packet_Multiple(EP2PSend sendType)
	{
		SendType = sendType;
		PacketType = PacketType.Multiple;
	}
}
