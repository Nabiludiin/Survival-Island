using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Fuel : SerializableBehaviour
{
	public int fuelCount;

	public float fuelTimer;

	protected bool Equals(RGD_Fuel other)
	{
		if (fuelCount == other.fuelCount)
		{
			return fuelTimer.Equals(other.fuelTimer);
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
		return Equals((RGD_Fuel)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(fuelCount, fuelTimer);
	}

	public static bool operator ==(RGD_Fuel left, RGD_Fuel right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Fuel left, RGD_Fuel right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in fuelCount, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in fuelTimer, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out fuelCount, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out fuelTimer, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Fuel()
	{
	}

	public RGD_Fuel(Fuel fuel)
	{
		fuelCount = fuel.GetFuelCount();
		fuelTimer = fuel.GetTimer();
	}

	public void RestoreFuel(Fuel fuel)
	{
		if (fuelCount != -1)
		{
			fuel.AddFuel(fuelCount);
		}
		if (fuelTimer != -1f)
		{
			fuel.ForceSetTimer(fuelTimer);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("FuelCount", fuelCount);
		info.AddValue("FuelTimer", fuelTimer);
	}

	public RGD_Fuel(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "FuelCount"))
			{
				if (name == "FuelTimer")
				{
					fuelTimer = (float)item.Value;
				}
			}
			else
			{
				fuelCount = (int)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		fuelTimer = -1f;
		fuelCount = -1;
	}
}
