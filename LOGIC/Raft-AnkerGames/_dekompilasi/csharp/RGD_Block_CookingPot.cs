using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_CookingPot : RGD_Block
{
	public RGD_CookingPot rgdCookingTable;

	protected bool Equals(RGD_Block_CookingPot other)
	{
		if (Equals((RGD_Block)other))
		{
			return object.Equals(rgdCookingTable, other.rgdCookingTable);
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
		return Equals((RGD_Block_CookingPot)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdCookingTable);
	}

	public static bool operator ==(RGD_Block_CookingPot left, RGD_Block_CookingPot right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_CookingPot left, RGD_Block_CookingPot right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (rgdCookingTable is RGD_CookingTable_Pot)
		{
			writer.WriteValueSafe<CookingPotType>(CookingPotType.Pot, default(FastBufferWriter.ForEnums));
		}
		else if (rgdCookingTable is RGD_CookingTable_Juicer)
		{
			writer.WriteValueSafe<CookingPotType>(CookingPotType.Juicer, default(FastBufferWriter.ForEnums));
		}
		else
		{
			writer.WriteValueSafe<CookingPotType>(CookingPotType.Normal, default(FastBufferWriter.ForEnums));
		}
		rgdCookingTable.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out CookingPotType value, default(FastBufferWriter.ForEnums));
		switch (value)
		{
		case CookingPotType.Normal:
			rgdCookingTable = new RGD_CookingPot();
			break;
		case CookingPotType.Pot:
			rgdCookingTable = new RGD_CookingTable_Pot();
			break;
		case CookingPotType.Juicer:
			rgdCookingTable = new RGD_CookingTable_Juicer();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		rgdCookingTable.DeserializeFast(reader);
	}

	public RGD_Block_CookingPot()
	{
	}

	public RGD_Block_CookingPot(Block block, RGD_CookingPot rgdCookingTable)
		: base(RGDType.Block_CookingPot, block)
	{
		this.rgdCookingTable = rgdCookingTable;
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		CookingTable component = block.GetComponent<CookingTable>();
		if (component != null)
		{
			component.RestoreCookingPot(rgdCookingTable);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CookingTable", rgdCookingTable);
	}

	public RGD_Block_CookingPot(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (name == "CookingPot" || name == "CookingTable")
			{
				rgdCookingTable = (RGD_CookingPot)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdCookingTable = null;
	}
}
