using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_WorldShiftManager_WorldShift : Message_NetworkBehaviour
{
	public float offsetX;

	public float offsetZ;

	public int newShiftCount;

	protected bool Equals(Message_WorldShiftManager_WorldShift other)
	{
		if (Equals((Message_NetworkBehaviour)other) && offsetX.Equals(other.offsetX) && offsetZ.Equals(other.offsetZ))
		{
			return newShiftCount == other.newShiftCount;
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
		return Equals((Message_WorldShiftManager_WorldShift)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), offsetX, offsetZ, newShiftCount);
	}

	public static bool operator ==(Message_WorldShiftManager_WorldShift left, Message_WorldShiftManager_WorldShift right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WorldShiftManager_WorldShift left, Message_WorldShiftManager_WorldShift right)
	{
		return !object.Equals(left, right);
	}

	public Message_WorldShiftManager_WorldShift()
	{
	}

	public Message_WorldShiftManager_WorldShift(Messages type, MonoBehaviour_Network behaviour, Vector3 worldOffset, int shiftCount)
		: base(type, behaviour)
	{
		offsetX = worldOffset.x;
		offsetZ = worldOffset.z;
		newShiftCount = shiftCount;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in offsetX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in offsetZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in newShiftCount, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out offsetX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out offsetZ, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out newShiftCount, default(FastBufferWriter.ForPrimitives));
	}
}
