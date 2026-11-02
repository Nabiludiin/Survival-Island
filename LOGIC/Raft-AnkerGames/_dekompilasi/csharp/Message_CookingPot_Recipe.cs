using System;
using Unity.Netcode;

[Serializable]
public class Message_CookingPot_Recipe : Message_NetworkBehaviour_ID
{
	public int recipeIndex;

	protected bool Equals(Message_CookingPot_Recipe other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return recipeIndex == other.recipeIndex;
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
		return Equals((Message_CookingPot_Recipe)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), recipeIndex);
	}

	public static bool operator ==(Message_CookingPot_Recipe left, Message_CookingPot_Recipe right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_CookingPot_Recipe left, Message_CookingPot_Recipe right)
	{
		return !object.Equals(left, right);
	}

	public Message_CookingPot_Recipe()
	{
	}

	public Message_CookingPot_Recipe(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int recipeIndex)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.recipeIndex = recipeIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in recipeIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out recipeIndex, default(FastBufferWriter.ForPrimitives));
	}
}
