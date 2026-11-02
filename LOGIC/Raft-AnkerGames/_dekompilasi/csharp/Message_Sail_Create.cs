using System;
using Unity.Netcode;

[Serializable]
public class Message_Sail_Create : Message_NetworkBehaviour
{
	public RGD_Sail rgdSail;

	protected bool Equals(Message_Sail_Create other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return object.Equals(rgdSail, other.rgdSail);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((Message_Sail_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdSail);
	}

	public static bool operator ==(Message_Sail_Create left, Message_Sail_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Sail_Create left, Message_Sail_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_Sail_Create()
	{
	}

	public Message_Sail_Create(Messages type, MonoBehaviour_Network behaviour, Sail sail)
		: base(type, behaviour)
	{
		rgdSail = new RGD_Sail(RGDType.None, sail);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdSail.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdSail = new RGD_Sail();
		rgdSail.DeserializeFast(reader);
	}
}
