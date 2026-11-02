using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Tank : SerializableBehaviour
{
	public float tankValue;

	protected bool Equals(RGD_Tank other)
	{
		return tankValue.Equals(other.tankValue);
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
		return Equals((RGD_Tank)obj);
	}

	public override int GetHashCode()
	{
		return tankValue.GetHashCode();
	}

	public static bool operator ==(RGD_Tank left, RGD_Tank right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Tank left, RGD_Tank right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Tank()
	{
	}

	public RGD_Tank(Tank tank)
	{
		tankValue = tank.CurrentTankAmount;
	}

	public void RestoreTank(Tank tank)
	{
		if (tank == null)
		{
			Debug.Log("Attempted to load in tank, but it was null");
		}
		else
		{
			tank.SetTankAmount(tankValue);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("TankValue", tankValue);
	}

	public RGD_Tank(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "TankValue")
			{
				tankValue = Helper.ObjToValue<float>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		tankValue = 0f;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in tankValue, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out tankValue, default(FastBufferWriter.ForPrimitives));
	}
}
