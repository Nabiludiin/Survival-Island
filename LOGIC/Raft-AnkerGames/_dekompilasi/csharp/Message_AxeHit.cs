using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_AxeHit : Message_NetworkBehaviour_SteamID
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

	public int treeObjectIndex = -1;

	public Vector3 HitPoint
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

	public Vector3 HitNormal
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

	protected bool Equals(Message_AxeHit other)
	{
		if (Equals((Message_NetworkBehaviour_SteamID)other) && posX.Equals(other.posX) && posY.Equals(other.posY) && posZ.Equals(other.posZ) && rotX.Equals(other.rotX) && rotY.Equals(other.rotY) && rotZ.Equals(other.rotZ))
		{
			return treeObjectIndex == other.treeObjectIndex;
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
		return Equals((Message_AxeHit)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), posX, posY, posZ, rotX, rotY, rotZ, treeObjectIndex);
	}

	public static bool operator ==(Message_AxeHit left, Message_AxeHit right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_AxeHit left, Message_AxeHit right)
	{
		return !object.Equals(left, right);
	}

	public Message_AxeHit()
	{
	}

	public Message_AxeHit(Messages type, MonoBehaviour_Network behaviour, Network_UserId playerID)
		: base(type, behaviour, playerID)
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(28))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in posX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in rotX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in rotY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in rotZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in treeObjectIndex, default(FastBufferWriter.ForPrimitives));
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
		reader.ReadValue(out treeObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
