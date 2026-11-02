using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD : SerializableBehaviour
{
	public byte t;

	public byte s;

	public RGDType Type
	{
		get
		{
			return (RGDType)t;
		}
		set
		{
			t = (byte)value;
		}
	}

	public RGDSortingOrder SortingOrder
	{
		get
		{
			return (RGDSortingOrder)s;
		}
		set
		{
			s = (byte)value;
		}
	}

	protected bool Equals(RGD other)
	{
		if (t == other.t)
		{
			return s == other.s;
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
		return Equals((RGD)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(t, s);
	}

	public static bool operator ==(RGD left, RGD right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD left, RGD right)
	{
		return !object.Equals(left, right);
	}

	public RGD(RGDType type, RGDSortingOrder sortingOrder)
	{
		Type = type;
		SortingOrder = sortingOrder;
	}

	public RGD()
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		info.AddValue("RGDTypeByte", t);
		info.AddValue("SortingOrderByte", s);
	}

	public RGD(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "RGDType":
				t = (byte)(RGDType)item.Value;
				break;
			case "SortingOrder":
				s = (byte)(RGDSortingOrder)item.Value;
				break;
			case "RGDTypeByte":
				t = Helper.ObjToValue<byte>(item.Value);
				break;
			case "SortingOrderByte":
				s = Helper.ObjToValue<byte>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		Type = RGDType.None;
		SortingOrder = RGDSortingOrder.Base;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(2))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in t, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in s, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out t, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out s, default(FastBufferWriter.ForPrimitives));
	}
}
