using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_StopCarryPosition : Message_NetworkBehaviour_ID_SteamID
{
	public float positionX;

	public float positionY;

	public float positionZ;

	public Vector3 Position
	{
		get
		{
			return new Vector3(positionX, positionY, positionZ);
		}
		set
		{
			positionX = value.x;
			positionY = value.y;
			positionZ = value.z;
		}
	}

	protected bool Equals(Message_StopCarryPosition other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other) && positionX.Equals(other.positionX) && positionY.Equals(other.positionY))
		{
			return positionZ.Equals(other.positionZ);
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
		return Equals((Message_StopCarryPosition)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), positionX, positionY, positionZ);
	}

	public static bool operator ==(Message_StopCarryPosition left, Message_StopCarryPosition right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_StopCarryPosition left, Message_StopCarryPosition right)
	{
		return !object.Equals(left, right);
	}

	public Message_StopCarryPosition()
	{
	}

	public Message_StopCarryPosition(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Network_UserId steamID, Vector3 position)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		Position = position;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(12))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in positionX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in positionY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in positionZ, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out positionX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out positionY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out positionZ, default(FastBufferWriter.ForPrimitives));
	}
}
