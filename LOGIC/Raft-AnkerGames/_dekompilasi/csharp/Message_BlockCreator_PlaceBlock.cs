using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_BlockCreator_PlaceBlock : Message_NetworkBehaviour
{
	[SerializeField]
	private float posX;

	[SerializeField]
	private float posY;

	[SerializeField]
	private float posZ;

	[SerializeField]
	private float rotX;

	[SerializeField]
	private float rotY;

	[SerializeField]
	private float rotZ;

	public int uniqueBlockIndex;

	public uint blockObjectIndex;

	public uint networkedObjectIndex;

	public uint networkedBehaviourIndex;

	public DPS dpsType;

	public int hotSlotIndex;

	public Vector3 LocalPosition
	{
		get
		{
			return new Vector3(posX, posY, posZ);
		}
		set
		{
			posX = value.x;
			posY = value.y;
			posZ = value.z;
		}
	}

	public Vector3 LocalEuler
	{
		get
		{
			return new Vector3(rotX, rotY, rotZ);
		}
		set
		{
			rotX = value.x;
			rotY = value.y;
			rotZ = value.z;
		}
	}

	protected bool Equals(Message_BlockCreator_PlaceBlock other)
	{
		if (Equals((Message_NetworkBehaviour)other) && posX.Equals(other.posX) && posY.Equals(other.posY) && posZ.Equals(other.posZ) && rotX.Equals(other.rotX) && rotY.Equals(other.rotY) && rotZ.Equals(other.rotZ) && uniqueBlockIndex == other.uniqueBlockIndex && blockObjectIndex == other.blockObjectIndex && networkedObjectIndex == other.networkedObjectIndex && networkedBehaviourIndex == other.networkedBehaviourIndex && dpsType == other.dpsType)
		{
			return hotSlotIndex == other.hotSlotIndex;
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
		return Equals((Message_BlockCreator_PlaceBlock)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(posX);
		hashCode.Add(posY);
		hashCode.Add(posZ);
		hashCode.Add(rotX);
		hashCode.Add(rotY);
		hashCode.Add(rotZ);
		hashCode.Add(uniqueBlockIndex);
		hashCode.Add(blockObjectIndex);
		hashCode.Add(networkedObjectIndex);
		hashCode.Add(networkedBehaviourIndex);
		hashCode.Add((int)dpsType);
		hashCode.Add(hotSlotIndex);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_BlockCreator_PlaceBlock left, Message_BlockCreator_PlaceBlock right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BlockCreator_PlaceBlock left, Message_BlockCreator_PlaceBlock right)
	{
		return !object.Equals(left, right);
	}

	public Message_BlockCreator_PlaceBlock(Messages type, MonoBehaviour_Network behaviour, int buildableUniqueIndex, uint blockObjectIndex, uint networkedObjectIndex, uint networkedBehaviourIndex, Vector3 localPosition, Vector3 localEuler, int hotSlotIndex, DPS dpsType)
		: base(type, behaviour)
	{
		uniqueBlockIndex = buildableUniqueIndex;
		this.blockObjectIndex = blockObjectIndex;
		LocalPosition = localPosition;
		LocalEuler = localEuler;
		this.dpsType = dpsType;
		this.hotSlotIndex = hotSlotIndex;
		this.networkedObjectIndex = networkedObjectIndex;
		this.networkedBehaviourIndex = networkedBehaviourIndex;
	}

	public Message_BlockCreator_PlaceBlock()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(48))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in posX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in rotX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in rotY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in rotZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in uniqueBlockIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in blockObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in networkedObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in networkedBehaviourIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in dpsType, default(FastBufferWriter.ForEnums));
		writer.WriteValue(in hotSlotIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out posX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out posY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out posZ, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out rotX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out rotY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out rotZ, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out uniqueBlockIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out blockObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out networkedObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out networkedBehaviourIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out dpsType, default(FastBufferWriter.ForEnums));
		reader.ReadValue(out hotSlotIndex, default(FastBufferWriter.ForPrimitives));
	}
}
