using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_StationaryAnchorCreate : Message_NetworkBehaviour
{
	public bool atBottom;

	public float jointDistance;

	[SerializeField]
	private float xPos;

	[SerializeField]
	private float yPos;

	[SerializeField]
	private float zPos;

	[SerializeField]
	private float xRot;

	[SerializeField]
	private float yRot;

	[SerializeField]
	private float zRot;

	public Vector3 Position
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

	public Vector3 Rotation
	{
		get
		{
			return new Vector3(xRot, yRot, zRot);
		}
		set
		{
			xRot = value.x;
			yRot = value.y;
			zRot = value.z;
		}
	}

	protected bool Equals(Message_StationaryAnchorCreate other)
	{
		if (Equals((Message_NetworkBehaviour)other) && atBottom == other.atBottom && jointDistance.Equals(other.jointDistance) && xPos.Equals(other.xPos) && yPos.Equals(other.yPos) && zPos.Equals(other.zPos) && xRot.Equals(other.xRot) && yRot.Equals(other.yRot))
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
		return Equals((Message_StationaryAnchorCreate)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(atBottom);
		hashCode.Add(jointDistance);
		hashCode.Add(xPos);
		hashCode.Add(yPos);
		hashCode.Add(zPos);
		hashCode.Add(xRot);
		hashCode.Add(yRot);
		hashCode.Add(zRot);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_StationaryAnchorCreate left, Message_StationaryAnchorCreate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_StationaryAnchorCreate left, Message_StationaryAnchorCreate right)
	{
		return !object.Equals(left, right);
	}

	public Message_StationaryAnchorCreate()
	{
	}

	public Message_StationaryAnchorCreate(Messages type, MonoBehaviour_Network behaviour, Transform anchor, bool atBottom, float jointDistance)
		: base(type, behaviour)
	{
		Position = anchor.position;
		Rotation = anchor.eulerAngles;
		this.atBottom = atBottom;
		this.jointDistance = jointDistance;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(29))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in atBottom, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in jointDistance, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xRot, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yRot, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zRot, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out atBottom, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out jointDistance, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xRot, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yRot, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zRot, default(FastBufferWriter.ForPrimitives));
	}
}
