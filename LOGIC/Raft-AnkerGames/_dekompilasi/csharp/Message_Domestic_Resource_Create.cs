using System;
using Unity.Netcode;

[Serializable]
public class Message_Domestic_Resource_Create : Message_NetworkBehaviour_ID
{
	public bool isResourceReady;

	protected bool Equals(Message_Domestic_Resource_Create other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return isResourceReady == other.isResourceReady;
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
		return Equals((Message_Domestic_Resource_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), isResourceReady);
	}

	public static bool operator ==(Message_Domestic_Resource_Create left, Message_Domestic_Resource_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Domestic_Resource_Create left, Message_Domestic_Resource_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_Domestic_Resource_Create()
	{
	}

	public Message_Domestic_Resource_Create(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, AI_NetworkBehaviour_Domestic_Resource resourceBehaviour)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		isResourceReady = resourceBehaviour.Resource.IsReady;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in isResourceReady, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out isResourceReady, default(FastBufferWriter.ForPrimitives));
	}
}
