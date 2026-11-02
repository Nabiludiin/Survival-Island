using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Snowmobile : SerializableBehaviour
{
	public float PositionX;

	public float PositionY;

	public float PositionZ;

	public float RotationX;

	public float RotationY;

	public float RotationZ;

	public int mobileObjectIndex;

	public int mobileBehaviourIndex;

	public RGD_SnowmobileSeat[] rgdSeats;

	public Vector3 Position
	{
		get
		{
			return new Vector3(PositionX, PositionY, PositionZ);
		}
		set
		{
			PositionX = value.x;
			PositionY = value.y;
			PositionZ = value.z;
		}
	}

	public Vector3 Rotation
	{
		get
		{
			return new Vector3(RotationX, RotationY, RotationZ);
		}
		set
		{
			RotationX = value.x;
			RotationY = value.y;
			RotationZ = value.z;
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(32))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in PositionX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in PositionY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in PositionZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in RotationX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in RotationY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in RotationZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in mobileObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in mobileBehaviourIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>((rgdSeats != null) ? rgdSeats.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdSeats != null)
		{
			RGD_SnowmobileSeat[] array = rgdSeats;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SerializeFast(writer);
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out PositionX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out PositionY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out PositionZ, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out RotationX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out RotationY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out RotationZ, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out mobileObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out mobileBehaviourIndex, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgdSeats = new RGD_SnowmobileSeat[value];
			for (int i = 0; i < value; i++)
			{
				rgdSeats[i] = new RGD_SnowmobileSeat();
				rgdSeats[i].DeserializeFast(reader);
			}
		}
	}

	public RGD_Snowmobile()
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("PositionX", PositionX);
		info.AddValue("PositionY", PositionY);
		info.AddValue("PositionZ", PositionZ);
		info.AddValue("RotationX", RotationX);
		info.AddValue("RotationY", RotationY);
		info.AddValue("RotationZ", RotationZ);
		info.AddValue("MobileObjectIndex", mobileObjectIndex);
		info.AddValue("MobileBehaviourIndex", mobileBehaviourIndex);
		info.AddValue("Seats", rgdSeats);
	}

	public RGD_Snowmobile(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "PositionX":
				PositionX = Helper.ObjToValue<float>(item.Value);
				break;
			case "PositionY":
				PositionY = Helper.ObjToValue<float>(item.Value);
				break;
			case "PositionZ":
				PositionZ = Helper.ObjToValue<float>(item.Value);
				break;
			case "RotationX":
				RotationX = Helper.ObjToValue<float>(item.Value);
				break;
			case "RotationY":
				RotationY = Helper.ObjToValue<float>(item.Value);
				break;
			case "RotationZ":
				RotationZ = Helper.ObjToValue<float>(item.Value);
				break;
			case "MobileObjectIndex":
				mobileObjectIndex = Helper.ObjToValue<int>(item.Value);
				break;
			case "MobileBehaviourIndex":
				mobileBehaviourIndex = Helper.ObjToValue<int>(item.Value);
				break;
			case "Seats":
				rgdSeats = (RGD_SnowmobileSeat[])item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		Position = Vector3.zero;
		Rotation = Vector3.zero;
		mobileBehaviourIndex = -1;
		mobileObjectIndex = -1;
		rgdSeats = null;
	}
}
