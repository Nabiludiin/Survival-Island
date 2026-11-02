using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Domestic_Chicken : RGD_Domestic
{
	public float eggTimer;

	protected bool Equals(RGD_Domestic_Chicken other)
	{
		if (Equals((RGD_Domestic)other))
		{
			return eggTimer.Equals(other.eggTimer);
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
		return Equals((RGD_Domestic_Chicken)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), eggTimer);
	}

	public static bool operator ==(RGD_Domestic_Chicken left, RGD_Domestic_Chicken right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Domestic_Chicken left, RGD_Domestic_Chicken right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in eggTimer, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out eggTimer, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Domestic_Chicken()
	{
	}

	public RGD_Domestic_Chicken(AI_NetworkBehaviour_Chicken chickenBehaviour)
		: base(chickenBehaviour)
	{
		eggTimer = chickenBehaviour.EggTimer;
	}

	public void RestoreDomesticBehaviour(AI_NetworkBehaviour_Chicken chickenBehaviour)
	{
		if (eggTimer != -1f)
		{
			chickenBehaviour.EggTimer = eggTimer;
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("EggTimer", eggTimer);
	}

	public RGD_Domestic_Chicken(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "EggTimer")
			{
				eggTimer = Helper.ObjToValue<float>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		eggTimer = -1f;
	}
}
