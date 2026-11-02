using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Elevator : SerializableBehaviour
{
	public int c;

	public int CurrentFloor
	{
		get
		{
			return c;
		}
		set
		{
			c = value;
		}
	}

	protected bool Equals(RGD_Elevator other)
	{
		return c == other.c;
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
		return Equals((RGD_Elevator)obj);
	}

	public override int GetHashCode()
	{
		return c;
	}

	public static bool operator ==(RGD_Elevator left, RGD_Elevator right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Elevator left, RGD_Elevator right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in c, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out c, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Elevator()
	{
	}

	public RGD_Elevator(Elevator elevator)
	{
		CurrentFloor = elevator.CurrentFloor;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CurrentFloor", CurrentFloor);
	}

	public RGD_Elevator(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "CurrentFloor")
			{
				CurrentFloor = Helper.ObjToValue<int>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		CurrentFloor = -1;
	}
}
