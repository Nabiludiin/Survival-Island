using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_WindTurbine : SerializableBehaviour
{
	public RGD_Battery battery1;

	public RGD_Battery battery2;

	public RGD_Battery battery3;

	public RGD_Battery battery4;

	public uint objectIndex;

	protected bool Equals(RGD_WindTurbine other)
	{
		if (object.Equals(battery1, other.battery1) && object.Equals(battery2, other.battery2) && object.Equals(battery3, other.battery3) && object.Equals(battery4, other.battery4))
		{
			return objectIndex == other.objectIndex;
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
		return Equals((RGD_WindTurbine)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(battery1, battery2, battery3, battery4, objectIndex);
	}

	public static bool operator ==(RGD_WindTurbine left, RGD_WindTurbine right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_WindTurbine left, RGD_WindTurbine right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<bool>(battery1 != null, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<bool>(battery2 != null, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<bool>(battery3 != null, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<bool>(battery4 != null, default(FastBufferWriter.ForPrimitives));
		if (battery1 != null)
		{
			battery1.SerializeFast(writer);
		}
		if (battery2 != null)
		{
			battery2.SerializeFast(writer);
		}
		if (battery3 != null)
		{
			battery3.SerializeFast(writer);
		}
		if (battery4 != null)
		{
			battery4.SerializeFast(writer);
		}
		writer.WriteValueSafe(in objectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		bool value = false;
		bool value2 = false;
		bool value3 = false;
		bool value4 = false;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out value2, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out value3, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out value4, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			battery1 = new RGD_Battery();
			battery1.DeserializeFast(reader);
		}
		if (value2)
		{
			battery2 = new RGD_Battery();
			battery2.DeserializeFast(reader);
		}
		if (value3)
		{
			battery3 = new RGD_Battery();
			battery3.DeserializeFast(reader);
		}
		if (value4)
		{
			battery4 = new RGD_Battery();
			battery4.DeserializeFast(reader);
		}
		reader.ReadValue(out objectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_WindTurbine()
	{
	}

	public RGD_WindTurbine(WindTurbine windTurbine)
	{
		objectIndex = windTurbine.ObjectIndex;
		foreach (Battery battery in windTurbine.batteries)
		{
			switch (battery.BatteryIndex)
			{
			case 0:
				battery1 = (windTurbine.batteries[0].BatterySlotIsEmpty ? null : new RGD_Battery(windTurbine.batteries[0]));
				break;
			case 1:
				battery2 = (windTurbine.batteries[1].BatterySlotIsEmpty ? null : new RGD_Battery(windTurbine.batteries[1]));
				break;
			case 2:
				battery3 = (windTurbine.batteries[2].BatterySlotIsEmpty ? null : new RGD_Battery(windTurbine.batteries[2]));
				break;
			case 3:
				battery4 = (windTurbine.batteries[3].BatterySlotIsEmpty ? null : new RGD_Battery(windTurbine.batteries[3]));
				break;
			}
		}
	}

	public void Restore(WindTurbine windTurbine)
	{
		if (!(windTurbine == null))
		{
			windTurbine.ObjectIndex = objectIndex;
			if (battery1 != null)
			{
				battery1.RestoreBattery(windTurbine.batteries[0]);
			}
			if (battery2 != null)
			{
				battery2.RestoreBattery(windTurbine.batteries[1]);
			}
			if (battery3 != null)
			{
				battery3.RestoreBattery(windTurbine.batteries[2]);
			}
			if (battery4 != null)
			{
				battery4.RestoreBattery(windTurbine.batteries[3]);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Battery1", battery1);
		info.AddValue("Battery2", battery2);
		info.AddValue("Battery3", battery3);
		info.AddValue("Battery4", battery4);
		info.AddValue("ObjectIndex", objectIndex);
	}

	public RGD_WindTurbine(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Battery1":
				battery1 = (RGD_Battery)item.Value;
				break;
			case "Battery2":
				battery2 = (RGD_Battery)item.Value;
				break;
			case "Battery3":
				battery3 = (RGD_Battery)item.Value;
				break;
			case "Battery4":
				battery4 = (RGD_Battery)item.Value;
				break;
			case "ObjectIndex":
				objectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		battery1 = (battery2 = (battery3 = (battery4 = null)));
		objectIndex = 0u;
	}
}
