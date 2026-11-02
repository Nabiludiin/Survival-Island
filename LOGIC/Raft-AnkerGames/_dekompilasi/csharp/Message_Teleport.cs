using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Teleport : Message_NetworkBehaviour_SteamID
{
	[SerializeField]
	private float posX;

	[SerializeField]
	private float posY;

	[SerializeField]
	private float posZ;

	public Vector3 Position
	{
		get
		{
			return new Vector3(posX, posY, posZ);
		}
		private set
		{
			posX = value.x;
			posY = value.y;
			posZ = value.z;
		}
	}

	protected bool Equals(Message_Teleport other)
	{
		if (Equals((Message_NetworkBehaviour_SteamID)other) && posX.Equals(other.posX) && posY.Equals(other.posY))
		{
			return posZ.Equals(other.posZ);
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
		return Equals((Message_Teleport)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), posX, posY, posZ);
	}

	public static bool operator ==(Message_Teleport left, Message_Teleport right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Teleport left, Message_Teleport right)
	{
		return !object.Equals(left, right);
	}

	public Message_Teleport()
	{
	}

	public Message_Teleport(Messages type, MonoBehaviour_Network behaviour, Network_Player playerToTeleport, Vector3 position)
		: base(type, behaviour, playerToTeleport.steamID)
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
		writer.WriteValue(in posX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posZ, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out posX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out posY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out posZ, default(FastBufferWriter.ForPrimitives));
	}
}
