using System;
using Unity.Netcode;

[Serializable]
public class Message_TreasurePointManager_Spawn : Message_NetworkBehaviour_ID
{
	public SerializableVector3 position;

	public uint tpObjectIndex;

	public uint tpPickupObjectIndex;

	public int tpParentObjectIndex;

	public int uniqueTreasureIndex;

	protected bool Equals(Message_TreasurePointManager_Spawn other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && object.Equals(position, other.position) && tpObjectIndex == other.tpObjectIndex && tpPickupObjectIndex == other.tpPickupObjectIndex && tpParentObjectIndex == other.tpParentObjectIndex)
		{
			return uniqueTreasureIndex == other.uniqueTreasureIndex;
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
		return Equals((Message_TreasurePointManager_Spawn)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), position, tpObjectIndex, tpPickupObjectIndex, tpParentObjectIndex, uniqueTreasureIndex);
	}

	public static bool operator ==(Message_TreasurePointManager_Spawn left, Message_TreasurePointManager_Spawn right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_TreasurePointManager_Spawn left, Message_TreasurePointManager_Spawn right)
	{
		return !object.Equals(left, right);
	}

	public Message_TreasurePointManager_Spawn()
	{
	}

	public Message_TreasurePointManager_Spawn(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, TreasurePoint tp)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		position = tp.transform.localPosition;
		tpObjectIndex = tp.ObjectIndex;
		tpPickupObjectIndex = tp.pickupNetworked.ObjectIndex;
		tpParentObjectIndex = tp.GetParentObjectIndex();
		uniqueTreasureIndex = tp.uniqueTreasureIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(28))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in position.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in tpObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in tpPickupObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in tpParentObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in uniqueTreasureIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		position = new SerializableVector3();
		reader.ReadValue(out position.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out tpObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out tpPickupObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out tpParentObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out uniqueTreasureIndex, default(FastBufferWriter.ForPrimitives));
	}
}
