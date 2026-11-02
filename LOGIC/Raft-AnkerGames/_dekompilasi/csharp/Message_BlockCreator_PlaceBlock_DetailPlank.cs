using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_BlockCreator_PlaceBlock_DetailPlank : Message_BlockCreator_PlaceBlock
{
	public float length;

	protected bool Equals(Message_BlockCreator_PlaceBlock_DetailPlank other)
	{
		if (Equals((Message_BlockCreator_PlaceBlock)other))
		{
			return length.Equals(other.length);
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
		return Equals((Message_BlockCreator_PlaceBlock_DetailPlank)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), length);
	}

	public static bool operator ==(Message_BlockCreator_PlaceBlock_DetailPlank left, Message_BlockCreator_PlaceBlock_DetailPlank right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BlockCreator_PlaceBlock_DetailPlank left, Message_BlockCreator_PlaceBlock_DetailPlank right)
	{
		return !object.Equals(left, right);
	}

	public Message_BlockCreator_PlaceBlock_DetailPlank(Messages type, MonoBehaviour_Network behaviour, int buildableUniqueIndex, uint blockObjectIndex, uint networkedObjectIndex, uint networkedBehaviourIndex, Vector3 localPosition, Vector3 localEuler, int hotSlotIndex, DPS dpsType, float length)
		: base(type, behaviour, buildableUniqueIndex, blockObjectIndex, networkedObjectIndex, networkedBehaviourIndex, localPosition, localEuler, hotSlotIndex, dpsType)
	{
		this.length = length;
	}

	public Message_BlockCreator_PlaceBlock_DetailPlank()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in length, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out length, default(FastBufferWriter.ForPrimitives));
	}
}
