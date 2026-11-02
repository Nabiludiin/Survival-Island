using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Battery : SerializableBehaviour
{
	public int batteryUses;

	public int installedUniqueIndex;

	protected bool Equals(RGD_Battery other)
	{
		if (batteryUses == other.batteryUses)
		{
			return installedUniqueIndex == other.installedUniqueIndex;
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
		return Equals((RGD_Battery)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(batteryUses, installedUniqueIndex);
	}

	public static bool operator ==(RGD_Battery left, RGD_Battery right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Battery left, RGD_Battery right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in batteryUses, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in installedUniqueIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out batteryUses, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out installedUniqueIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Battery()
	{
	}

	public RGD_Battery(int batteryUses)
	{
		installedUniqueIndex = 176;
		this.batteryUses = batteryUses;
	}

	public RGD_Battery(Battery battery)
	{
		if (!(battery == null) && !battery.BatterySlotIsEmpty)
		{
			batteryUses = battery.BatteryUses;
			installedUniqueIndex = battery.GetBatteryInstance().UniqueIndex;
		}
	}

	public void RestoreBattery(Battery battery)
	{
		if (battery != null)
		{
			battery.Insert(null, batteryUses, installedUniqueIndex);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("BatteryUses", batteryUses);
		info.AddValue("BatteryIndex", installedUniqueIndex);
	}

	public RGD_Battery(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "BatteryUses"))
			{
				if (name == "BatteryIndex")
				{
					installedUniqueIndex = Helper.ObjToValue<int>(item.Value);
				}
			}
			else
			{
				batteryUses = Helper.ObjToValue<int>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		batteryUses = 0;
		installedUniqueIndex = -1;
	}
}
