using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_PickupItem : SerializableBehaviour
{
	[OptionalField(VersionAdded = 2)]
	public uint itemObjectIndex;

	[OptionalField(VersionAdded = 2)]
	public RGD_ItemInstance rgdInstance;

	[OptionalField(VersionAdded = 2)]
	[SerializeField]
	private float xPos;

	[OptionalField(VersionAdded = 2)]
	[SerializeField]
	private float yPos;

	[OptionalField(VersionAdded = 2)]
	[SerializeField]
	private float zPos;

	[OptionalField(VersionAdded = 2)]
	[SerializeField]
	private float xRot;

	[OptionalField(VersionAdded = 2)]
	[SerializeField]
	private float yRot;

	[OptionalField(VersionAdded = 2)]
	[SerializeField]
	private float zRot;

	public Vector3 Position
	{
		get
		{
			return new Vector3(xPos, yPos, zPos);
		}
		private set
		{
			xPos = value.x;
			yPos = value.y;
			zPos = value.z;
		}
	}

	public Vector3 Rotation
	{
		get
		{
			return new Vector3(xRot, yRot, zRot);
		}
		private set
		{
			xRot = value.x;
			yRot = value.y;
			zRot = value.z;
		}
	}

	protected bool Equals(RGD_PickupItem other)
	{
		if (itemObjectIndex == other.itemObjectIndex && object.Equals(rgdInstance, other.rgdInstance) && xPos.Equals(other.xPos) && yPos.Equals(other.yPos) && zPos.Equals(other.zPos) && xRot.Equals(other.xRot) && yRot.Equals(other.yRot))
		{
			return zRot.Equals(other.zRot);
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
		return Equals((RGD_PickupItem)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(itemObjectIndex, rgdInstance, xPos, yPos, zPos, xRot, yRot, zRot);
	}

	public static bool operator ==(RGD_PickupItem left, RGD_PickupItem right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_PickupItem left, RGD_PickupItem right)
	{
		return !object.Equals(left, right);
	}

	public RGD_PickupItem()
	{
	}

	public RGD_PickupItem(PickupItem pickupItem)
	{
		if (!(pickupItem == null))
		{
			rgdInstance = new RGD_ItemInstance(pickupItem.itemInstance);
			Position = pickupItem.transform.position;
			Rotation = pickupItem.transform.eulerAngles;
			itemObjectIndex = pickupItem.networkID.ObjectIndex;
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(28))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in itemObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xRot, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yRot, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zRot, default(FastBufferWriter.ForPrimitives));
		rgdInstance.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out itemObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xRot, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yRot, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zRot, default(FastBufferWriter.ForPrimitives));
		rgdInstance = new RGD_ItemInstance();
		rgdInstance.DeserializeFast(reader);
	}
}
