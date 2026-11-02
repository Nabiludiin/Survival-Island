using System;
using Unity.Netcode;

[Serializable]
public class Message_SteeringWheel_Rotate : Message_NetworkBehaviour_ID
{
	public float rotationDegree;

	protected bool Equals(Message_SteeringWheel_Rotate other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return rotationDegree.Equals(other.rotationDegree);
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
		return Equals((Message_SteeringWheel_Rotate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rotationDegree);
	}

	public static bool operator ==(Message_SteeringWheel_Rotate left, Message_SteeringWheel_Rotate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_SteeringWheel_Rotate left, Message_SteeringWheel_Rotate right)
	{
		return !object.Equals(left, right);
	}

	public Message_SteeringWheel_Rotate()
	{
	}

	public Message_SteeringWheel_Rotate(Messages type, MonoBehaviour_Network behaviour, uint objectIndex, float rotationDegree)
		: base(type, behaviour, objectIndex)
	{
		this.rotationDegree = rotationDegree;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in rotationDegree, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out rotationDegree, default(FastBufferWriter.ForPrimitives));
	}
}
