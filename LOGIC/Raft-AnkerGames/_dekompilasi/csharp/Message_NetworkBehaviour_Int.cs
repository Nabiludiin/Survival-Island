using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_NetworkBehaviour_Int : Message_NetworkBehaviour
{
	[SerializeField]
	private int i;

	public int IntegerValue => i;

	protected bool Equals(Message_NetworkBehaviour_Int other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return i == other.i;
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
		return Equals((Message_NetworkBehaviour_Int)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), i);
	}

	public static bool operator ==(Message_NetworkBehaviour_Int left, Message_NetworkBehaviour_Int right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NetworkBehaviour_Int left, Message_NetworkBehaviour_Int right)
	{
		return !object.Equals(left, right);
	}

	public Message_NetworkBehaviour_Int()
	{
	}

	public Message_NetworkBehaviour_Int(Messages type, MonoBehaviour_Network behaviour, int integerValue)
		: base(type, behaviour)
	{
		i = integerValue;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in i, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out i, default(FastBufferWriter.ForPrimitives));
	}
}
