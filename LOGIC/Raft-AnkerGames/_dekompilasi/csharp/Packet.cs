using System;

[Serializable]
public class Packet
{
	public byte t;

	public PacketType PacketType
	{
		get
		{
			return (PacketType)t;
		}
		set
		{
			t = (byte)value;
		}
	}
}
