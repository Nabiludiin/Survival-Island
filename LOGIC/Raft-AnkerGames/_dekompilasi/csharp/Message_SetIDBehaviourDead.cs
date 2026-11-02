using System;
using Unity.Netcode;

[Serializable]
public class Message_SetIDBehaviourDead : Message_NetworkBehaviour
{
	public uint objectIndexValue;

	public string objectTypeName;

	public bool removeInstantly;

	protected bool Equals(Message_SetIDBehaviourDead other)
	{
		if (Equals((Message_NetworkBehaviour)other) && objectIndexValue == other.objectIndexValue && objectTypeName == other.objectTypeName)
		{
			return removeInstantly == other.removeInstantly;
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
		return Equals((Message_SetIDBehaviourDead)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), objectIndexValue, objectTypeName, removeInstantly);
	}

	public static bool operator ==(Message_SetIDBehaviourDead left, Message_SetIDBehaviourDead right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_SetIDBehaviourDead left, Message_SetIDBehaviourDead right)
	{
		return !object.Equals(left, right);
	}

	public Message_SetIDBehaviourDead(Messages type, MonoBehaviour_Network behaviour, uint objectIndexValue, Type objectType, bool removeInstantly = false)
		: base(type, behaviour)
	{
		this.objectIndexValue = objectIndexValue;
		objectTypeName = objectType.FullName;
		this.removeInstantly = removeInstantly;
	}

	public Message_SetIDBehaviourDead()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in objectIndexValue, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(objectTypeName);
		writer.WriteValueSafe(in removeInstantly, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out objectIndexValue, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out objectTypeName, false);
		reader.ReadValue(out removeInstantly, default(FastBufferWriter.ForPrimitives));
	}
}
