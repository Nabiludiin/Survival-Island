using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_CookingTable_Pot : RGD_CookingPot
{
	public RGD_Fuel rgdFuel;

	protected bool Equals(RGD_CookingTable_Pot other)
	{
		return object.Equals(rgdFuel, other.rgdFuel);
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
		return Equals((RGD_CookingTable_Pot)obj);
	}

	public override int GetHashCode()
	{
		if (!(rgdFuel != null))
		{
			return 0;
		}
		return rgdFuel.GetHashCode();
	}

	public static bool operator ==(RGD_CookingTable_Pot left, RGD_CookingTable_Pot right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_CookingTable_Pot left, RGD_CookingTable_Pot right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdFuel.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdFuel = new RGD_Fuel();
		rgdFuel.DeserializeFast(reader);
	}

	public RGD_CookingTable_Pot()
	{
	}

	public RGD_CookingTable_Pot(CookingTable_Pot cookingPot)
		: base(cookingPot)
	{
		rgdFuel = new RGD_Fuel(cookingPot.Fuel);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Fuel", rgdFuel);
	}

	public RGD_CookingTable_Pot(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Fuel")
			{
				rgdFuel = (RGD_Fuel)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdFuel = null;
	}
}
