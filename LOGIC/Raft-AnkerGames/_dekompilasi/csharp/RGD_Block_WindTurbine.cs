using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Block_WindTurbine : RGD_Block
{
	public RGD_WindTurbine rgdWindTurbine;

	protected bool Equals(RGD_Block_WindTurbine other)
	{
		if (Equals((RGD_Block)other))
		{
			return object.Equals(rgdWindTurbine, other.rgdWindTurbine);
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
		return Equals((RGD_Block_WindTurbine)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdWindTurbine);
	}

	public static bool operator ==(RGD_Block_WindTurbine left, RGD_Block_WindTurbine right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_WindTurbine left, RGD_Block_WindTurbine right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdWindTurbine.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdWindTurbine = new RGD_WindTurbine();
		rgdWindTurbine.DeserializeFast(reader);
	}

	public RGD_Block_WindTurbine()
	{
	}

	public RGD_Block_WindTurbine(RGDType type, Block block, WindTurbine windTurbine)
		: base(type, block)
	{
		rgdWindTurbine = new RGD_WindTurbine(windTurbine);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDWindTurbine", rgdWindTurbine);
	}

	public RGD_Block_WindTurbine(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDWindTurbine")
			{
				rgdWindTurbine = (RGD_WindTurbine)item.Value;
			}
		}
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		WindTurbine component = block.GetComponent<WindTurbine>();
		if (component != null)
		{
			rgdWindTurbine.Restore(component);
		}
		else
		{
			Debug.Log("Unable to find windturbine on block component transform.");
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdWindTurbine = null;
	}
}
