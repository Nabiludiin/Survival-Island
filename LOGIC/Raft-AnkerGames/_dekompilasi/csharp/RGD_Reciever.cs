using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Reciever : RGD_Block
{
	public bool batteryOn;

	public RGD_Battery rgdBattery;

	public int[] antennaBlockIndexes;

	public uint recieverObjectIndex;

	public int[] frequencyNumbers;

	public int frequencyNumberIndex;

	protected bool Equals(RGD_Reciever other)
	{
		if (frequencyNumbers == null && other.frequencyNumbers != null)
		{
			return false;
		}
		if (frequencyNumbers != null && other.frequencyNumbers == null)
		{
			return false;
		}
		if (Equals((RGD_Block)other) && batteryOn == other.batteryOn && object.Equals(rgdBattery, other.rgdBattery) && recieverObjectIndex == other.recieverObjectIndex && frequencyNumberIndex == other.frequencyNumberIndex && Enumerable.SequenceEqual(antennaBlockIndexes, other.antennaBlockIndexes))
		{
			if (frequencyNumbers != null || other.frequencyNumbers != null)
			{
				return Enumerable.SequenceEqual(frequencyNumbers, other.frequencyNumbers);
			}
			return true;
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
		return Equals((RGD_Reciever)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), batteryOn, rgdBattery, antennaBlockIndexes, recieverObjectIndex, frequencyNumbers, frequencyNumberIndex);
	}

	public static bool operator ==(RGD_Reciever left, RGD_Reciever right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Reciever left, RGD_Reciever right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in batteryOn, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<bool>(rgdBattery != null, default(FastBufferWriter.ForPrimitives));
		if (rgdBattery != null)
		{
			rgdBattery.SerializeFast(writer);
		}
		writer.WriteValueSafe(antennaBlockIndexes, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in recieverObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>((frequencyNumbers != null) ? frequencyNumbers.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (frequencyNumbers != null)
		{
			int[] array = frequencyNumbers;
			for (int i = 0; i < array.Length; i++)
			{
				int value = array[i];
				writer.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
			}
		}
		writer.WriteValueSafe(in frequencyNumberIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out batteryOn, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out bool value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			rgdBattery = new RGD_Battery();
			rgdBattery.DeserializeFast(reader);
		}
		reader.ReadValue(out antennaBlockIndexes, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out recieverObjectIndex, default(FastBufferWriter.ForPrimitives));
		int value2 = 0;
		reader.ReadValue(out value2, default(FastBufferWriter.ForPrimitives));
		if (value2 > 0)
		{
			frequencyNumbers = new int[value2];
			for (int i = 0; i < value2; i++)
			{
				reader.ReadValue(out frequencyNumbers[i], default(FastBufferWriter.ForPrimitives));
			}
		}
		reader.ReadValue(out frequencyNumberIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Reciever()
	{
	}

	public RGD_Reciever(RGDType type, Reciever reciever, int[] frequencyNumbers, int frequencyNumberIndex)
		: base(type, reciever.GetComponent<Block>())
	{
		recieverObjectIndex = reciever.ObjectIndex;
		this.frequencyNumbers = frequencyNumbers;
		this.frequencyNumberIndex = frequencyNumberIndex;
		batteryOn = reciever.battery.On;
		if (!reciever.battery.BatterySlotIsEmpty)
		{
			rgdBattery = new RGD_Battery(reciever.battery);
		}
		antennaBlockIndexes = new int[reciever.antennas.Length];
		for (int i = 0; i < reciever.antennas.Length; i++)
		{
			if (reciever.antennas[i] != null)
			{
				antennaBlockIndexes[i] = (int)reciever.antennas[i].GetComponent<Block>().ObjectIndex;
			}
			else
			{
				antennaBlockIndexes[i] = -1;
			}
		}
	}

	public IEnumerator RestoreLate(float timeDelay, Reciever reciever)
	{
		yield return new WaitForSeconds(timeDelay);
		reciever.Restore(this);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("BatteryOn", batteryOn);
		info.AddValue("RGDBattery", rgdBattery);
		info.AddValue("AntennaIndexes", antennaBlockIndexes);
		info.AddValue("RecieverObjectIndex", recieverObjectIndex);
		info.AddValue("FrequencyNumbers", frequencyNumbers);
		info.AddValue("FrequencyNumberIndex", frequencyNumberIndex);
	}

	public RGD_Reciever(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "BatteryOn":
				batteryOn = (bool)item.Value;
				break;
			case "BatteryUses":
				rgdBattery = new RGD_Battery((int)item.Value);
				break;
			case "AntennaIndexes":
				antennaBlockIndexes = (int[])item.Value;
				break;
			case "RecieverObjectIndex":
				recieverObjectIndex = (uint)item.Value;
				break;
			case "FrequencyNumbers":
				frequencyNumbers = (int[])item.Value;
				break;
			case "FrequencyNumberIndex":
				frequencyNumberIndex = Helper.ObjToValue<int>(item.Value);
				break;
			case "RGDBattery":
				rgdBattery = (RGD_Battery)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		batteryOn = false;
		rgdBattery = null;
		antennaBlockIndexes = null;
		recieverObjectIndex = 0u;
		frequencyNumberIndex = 0;
		frequencyNumbers = null;
	}
}
