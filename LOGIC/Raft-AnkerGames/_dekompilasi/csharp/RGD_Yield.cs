using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Yield : SerializableBehaviour
{
	public int uniqueItemIndex;

	public int amount;

	protected bool Equals(RGD_Yield other)
	{
		if (uniqueItemIndex == other.uniqueItemIndex)
		{
			return amount == other.amount;
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
		return Equals((RGD_Yield)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(uniqueItemIndex, amount);
	}

	public static bool operator ==(RGD_Yield left, RGD_Yield right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Yield left, RGD_Yield right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Yield(Cost cost)
	{
		if (cost != null)
		{
			uniqueItemIndex = ((cost.item == null) ? (-1) : cost.item.UniqueIndex);
			amount = cost.amount;
		}
	}

	public RGD_Yield()
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("ItemIndex", uniqueItemIndex);
		info.AddValue("Amount", amount);
	}

	public RGD_Yield(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "ItemIndex"))
			{
				if (name == "Amount")
				{
					amount = (int)item.Value;
				}
			}
			else
			{
				uniqueItemIndex = (int)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		uniqueItemIndex = -1;
		amount = 0;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in uniqueItemIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in amount, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out uniqueItemIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out amount, default(FastBufferWriter.ForPrimitives));
	}
}
