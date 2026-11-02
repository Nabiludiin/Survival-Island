using System;
using Unity.Netcode;

[Serializable]
public class Message_RareMaterial : Message_NetworkBehaviour_ID
{
	public int rareMaterialIndex;

	protected bool Equals(Message_RareMaterial other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return rareMaterialIndex == other.rareMaterialIndex;
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
		return Equals((Message_RareMaterial)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rareMaterialIndex);
	}

	public static bool operator ==(Message_RareMaterial left, Message_RareMaterial right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_RareMaterial left, Message_RareMaterial right)
	{
		return !object.Equals(left, right);
	}

	public Message_RareMaterial()
	{
	}

	public Message_RareMaterial(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, RareMaterial rareMaterial)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		if (rareMaterial != null)
		{
			rareMaterialIndex = (int)rareMaterial.randomizer.lastRandomizedIndex;
		}
		else
		{
			rareMaterialIndex = -1;
		}
	}

	public void RestoreMaterial(RareMaterial rareMaterial)
	{
		if (rareMaterialIndex != -1 && rareMaterial != null)
		{
			rareMaterial.AssignSpecificMaterial((uint)rareMaterialIndex);
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in rareMaterialIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out rareMaterialIndex, default(FastBufferWriter.ForPrimitives));
	}
}
