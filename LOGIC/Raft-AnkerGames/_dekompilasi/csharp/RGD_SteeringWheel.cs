using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_SteeringWheel : RGD_Block
{
	public float currentRotation;

	protected bool Equals(RGD_SteeringWheel other)
	{
		if (Equals((RGD_Block)other))
		{
			return currentRotation.Equals(other.currentRotation);
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
		return Equals((RGD_SteeringWheel)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), currentRotation);
	}

	public static bool operator ==(RGD_SteeringWheel left, RGD_SteeringWheel right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_SteeringWheel left, RGD_SteeringWheel right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in currentRotation, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out currentRotation, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_SteeringWheel()
	{
	}

	public RGD_SteeringWheel(RGDType type, SteeringWheel steeringWheel)
		: base(type, steeringWheel.GetComponent<Block>())
	{
		currentRotation = steeringWheel.targetLocalEulerZ;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CurrentRotation", currentRotation);
	}

	public RGD_SteeringWheel(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "CurrentRotation")
			{
				currentRotation = Helper.ObjToValue<float>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		currentRotation = 0f;
	}
}
