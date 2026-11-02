using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_MotorWheel : RGD_Block
{
	public bool isEngineTurnedOn;

	public RGD_Tank rgdTank;

	public bool isRotatingForward;

	protected bool Equals(RGD_MotorWheel other)
	{
		if (Equals((RGD_Block)other) && isEngineTurnedOn == other.isEngineTurnedOn && object.Equals(rgdTank, other.rgdTank))
		{
			return isRotatingForward == other.isRotatingForward;
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
		return Equals((RGD_MotorWheel)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), isEngineTurnedOn, rgdTank, isRotatingForward);
	}

	public static bool operator ==(RGD_MotorWheel left, RGD_MotorWheel right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_MotorWheel left, RGD_MotorWheel right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in isEngineTurnedOn, default(FastBufferWriter.ForPrimitives));
		rgdTank.SerializeFast(writer);
		writer.WriteValueSafe(in isRotatingForward, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out isEngineTurnedOn, default(FastBufferWriter.ForPrimitives));
		rgdTank = new RGD_Tank();
		rgdTank.DeserializeFast(reader);
		reader.ReadValue(out isRotatingForward, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_MotorWheel()
	{
	}

	public RGD_MotorWheel(RGDType type, MotorWheel motor)
		: base(type, motor.GetComponent<Block>())
	{
		rgdTank = new RGD_Tank(motor.FuelTank);
		isEngineTurnedOn = motor.engineSwitchOn;
		isRotatingForward = motor.rotatesForward;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("IsEngineTurnedOn", isEngineTurnedOn);
		info.AddValue("Tank", rgdTank);
		info.AddValue("RotatesForward", isRotatingForward);
	}

	public RGD_MotorWheel(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "IsEngineTurnedOn":
				isEngineTurnedOn = (bool)item.Value;
				break;
			case "Tank":
				rgdTank = (RGD_Tank)item.Value;
				break;
			case "RotatesForward":
				isRotatingForward = (bool)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		isEngineTurnedOn = false;
		rgdTank = null;
		isRotatingForward = false;
	}
}
