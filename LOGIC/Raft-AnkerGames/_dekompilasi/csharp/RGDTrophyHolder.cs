using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGDTrophyHolder : RGD_Block
{
	public int itemIndex;

	protected bool Equals(RGDTrophyHolder other)
	{
		if (Equals((RGD_Block)other))
		{
			return itemIndex == other.itemIndex;
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
		return Equals((RGDTrophyHolder)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), itemIndex);
	}

	public static bool operator ==(RGDTrophyHolder left, RGDTrophyHolder right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGDTrophyHolder left, RGDTrophyHolder right)
	{
		return !object.Equals(left, right);
	}

	public RGDTrophyHolder()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in itemIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out itemIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGDTrophyHolder(RGDType p_type, TrophyHolder p_trophyHolder)
		: base(p_type, p_trophyHolder.GetComponentInParent<Block>())
	{
		Item_Base currentItem = p_trophyHolder.GetCurrentItem();
		if (currentItem != null)
		{
			itemIndex = currentItem.UniqueIndex;
		}
		else
		{
			itemIndex = -1;
		}
	}

	public void RestoreTrophyHolder(TrophyHolder p_holder)
	{
		if (p_holder != null)
		{
			Item_Base itemByIndex = ItemManager.GetItemByIndex(itemIndex);
			if (itemByIndex != null)
			{
				p_holder.PlaceTrophy(null, itemByIndex);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Item", itemIndex);
	}

	public RGDTrophyHolder(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Item")
			{
				itemIndex = (int)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		itemIndex = -1;
	}
}
