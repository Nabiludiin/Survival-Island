using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_ResearchTable : RGD_Block
{
	public RGD_Slot rgdSlot;

	protected bool Equals(RGD_ResearchTable other)
	{
		if (Equals((RGD_Block)other))
		{
			return object.Equals(rgdSlot, other.rgdSlot);
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
		return Equals((RGD_ResearchTable)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdSlot);
	}

	public static bool operator ==(RGD_ResearchTable left, RGD_ResearchTable right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_ResearchTable left, RGD_ResearchTable right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdSlot.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdSlot = new RGD_Slot();
		rgdSlot.DeserializeFast(reader);
	}

	public RGD_ResearchTable()
	{
	}

	public RGD_ResearchTable(RGDType type, Block block, ResearchTable table)
		: base(type, block)
	{
		if (table != null)
		{
			rgdSlot = new RGD_Slot(table.Slot, 0);
		}
	}

	public void RestoreResearchTable(ResearchTable table)
	{
		if (rgdSlot != null)
		{
			rgdSlot.RestoreSlot(table.Slot);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Slot", rgdSlot);
	}

	public RGD_ResearchTable(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Slot")
			{
				rgdSlot = (RGD_Slot)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdSlot = null;
	}
}
