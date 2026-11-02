using System;
using Unity.Netcode;

[Serializable]
public class Message_Snowmobile_Spawn : Message_NetworkBehaviour_ID
{
	public uint mobileObjectIndex;

	public uint mobileBehaviourIndex;

	protected bool Equals(Message_Snowmobile_Spawn other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && mobileObjectIndex == other.mobileObjectIndex)
		{
			return mobileBehaviourIndex == other.mobileBehaviourIndex;
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
		return Equals((Message_Snowmobile_Spawn)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), mobileObjectIndex, mobileBehaviourIndex);
	}

	public static bool operator ==(Message_Snowmobile_Spawn left, Message_Snowmobile_Spawn right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Snowmobile_Spawn left, Message_Snowmobile_Spawn right)
	{
		return !object.Equals(left, right);
	}

	public Message_Snowmobile_Spawn()
	{
	}

	public Message_Snowmobile_Spawn(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, uint mobileObjectIndex, uint mobileBehaviourIndex)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.mobileObjectIndex = mobileObjectIndex;
		this.mobileBehaviourIndex = mobileBehaviourIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in mobileObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in mobileBehaviourIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out mobileObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out mobileBehaviourIndex, default(FastBufferWriter.ForPrimitives));
	}
}
