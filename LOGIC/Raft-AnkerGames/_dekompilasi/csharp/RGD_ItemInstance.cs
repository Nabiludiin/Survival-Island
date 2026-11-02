using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_ItemInstance : SerializableBehaviour
{
	public int itemIndex;

	public int amount;

	public int uses;

	public string exclusiveString;

	protected bool Equals(RGD_ItemInstance other)
	{
		if (itemIndex == other.itemIndex && amount == other.amount && uses == other.uses)
		{
			return exclusiveString == other.exclusiveString;
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
		return Equals((RGD_ItemInstance)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(itemIndex, amount, uses, exclusiveString);
	}

	public static bool operator ==(RGD_ItemInstance left, RGD_ItemInstance right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_ItemInstance left, RGD_ItemInstance right)
	{
		return !object.Equals(left, right);
	}

	public RGD_ItemInstance()
	{
	}

	public RGD_ItemInstance(ItemInstance instance)
	{
		itemIndex = instance.UniqueIndex;
		amount = instance.Amount;
		uses = instance.Uses;
		exclusiveString = instance.exclusiveString;
	}

	public ItemInstance GetRealInstance()
	{
		if (itemIndex != -1)
		{
			return new ItemInstance(ItemManager.GetItemByIndex(itemIndex), amount, uses, exclusiveString);
		}
		return null;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("ItemIndex", itemIndex);
		info.AddValue("Amount", amount);
		info.AddValue("Uses", uses);
		info.AddValue("ExclusiveString", exclusiveString);
	}

	public RGD_ItemInstance(SerializationInfo info, StreamingContext sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "ItemIndex":
				itemIndex = (int)item.Value;
				break;
			case "Amount":
				amount = (int)item.Value;
				break;
			case "Uses":
				uses = (int)item.Value;
				break;
			case "ExclusiveString":
				exclusiveString = (string)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		itemIndex = -1;
		amount = 0;
		uses = 1;
		exclusiveString = "NONE";
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(12))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in itemIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in amount, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in uses, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(exclusiveString);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out itemIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out amount, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out uses, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out exclusiveString, false);
	}
}
