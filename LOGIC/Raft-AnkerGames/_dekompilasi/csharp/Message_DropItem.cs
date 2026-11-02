using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_DropItem : Message_NetworkBehaviour
{
	public uint dropObjectIndex;

	[SerializeField]
	private float xPos;

	[SerializeField]
	private float yPos;

	[SerializeField]
	private float zPos;

	[SerializeField]
	private float xDir;

	[SerializeField]
	private float yDir;

	[SerializeField]
	private float zDir;

	public RGD_ItemInstance rgdInstance;

	public bool parentedToRaft;

	public Vector3 DropPosition
	{
		get
		{
			return new Vector3(xPos, yPos, zPos);
		}
		set
		{
			xPos = value.x;
			yPos = value.y;
			zPos = value.z;
		}
	}

	public Vector3 DropDirection
	{
		get
		{
			return new Vector3(xDir, yDir, zDir);
		}
		set
		{
			xDir = value.x;
			yDir = value.y;
			zDir = value.z;
		}
	}

	protected bool Equals(Message_DropItem other)
	{
		if (Equals((Message_NetworkBehaviour)other) && dropObjectIndex == other.dropObjectIndex && xPos.Equals(other.xPos) && yPos.Equals(other.yPos) && zPos.Equals(other.zPos) && xDir.Equals(other.xDir) && yDir.Equals(other.yDir) && zDir.Equals(other.zDir) && object.Equals(rgdInstance, other.rgdInstance))
		{
			return parentedToRaft == other.parentedToRaft;
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
		return Equals((Message_DropItem)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(dropObjectIndex);
		hashCode.Add(xPos);
		hashCode.Add(yPos);
		hashCode.Add(zPos);
		hashCode.Add(xDir);
		hashCode.Add(yDir);
		hashCode.Add(zDir);
		hashCode.Add(rgdInstance);
		hashCode.Add(parentedToRaft);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_DropItem left, Message_DropItem right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_DropItem left, Message_DropItem right)
	{
		return !object.Equals(left, right);
	}

	public Message_DropItem()
	{
	}

	public Message_DropItem(Messages type, MonoBehaviour_Network behaviour, ItemInstance droppedItem, Vector3 dropPosition, Vector3 dropDirection, uint dropObjectIndex, bool parentedToRaft)
		: base(type, behaviour)
	{
		rgdInstance = new RGD_ItemInstance(droppedItem);
		DropPosition = dropPosition;
		DropDirection = dropDirection;
		this.parentedToRaft = parentedToRaft;
		this.dropObjectIndex = dropObjectIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(29))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in dropObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xDir, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yDir, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zDir, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in parentedToRaft, default(FastBufferWriter.ForPrimitives));
		rgdInstance.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out dropObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xDir, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yDir, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zDir, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out parentedToRaft, default(FastBufferWriter.ForPrimitives));
		rgdInstance = new RGD_ItemInstance();
		rgdInstance.DeserializeFast(reader);
	}
}
