using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_PlantationSlot : SerializableBehaviour
{
	public int uniquePlantIndex;

	public int plantationSlotIndex;

	public uint plantObjectIndex;

	public float growTimer;

	public bool hasWater;

	public List<RGD_Yield> yields = new List<RGD_Yield>();

	protected bool Equals(RGD_PlantationSlot other)
	{
		if (yields == null && other.yields != null)
		{
			return false;
		}
		if (yields != null && other.yields == null)
		{
			return false;
		}
		if (uniquePlantIndex == other.uniquePlantIndex && plantationSlotIndex == other.plantationSlotIndex && plantObjectIndex == other.plantObjectIndex && growTimer.Equals(other.growTimer) && hasWater == other.hasWater)
		{
			if (yields != null || other.yields != null)
			{
				return yields.SequenceEqual(other.yields);
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
		return Equals((RGD_PlantationSlot)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(uniquePlantIndex, plantationSlotIndex, plantObjectIndex, growTimer, hasWater, yields);
	}

	public static bool operator ==(RGD_PlantationSlot left, RGD_PlantationSlot right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_PlantationSlot left, RGD_PlantationSlot right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(17))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in uniquePlantIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in plantationSlotIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in plantObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in growTimer, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in hasWater, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>(yields.Count, default(FastBufferWriter.ForPrimitives));
		foreach (RGD_Yield yield in yields)
		{
			yield.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out uniquePlantIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out plantationSlotIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out plantObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out growTimer, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hasWater, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			yields = new List<RGD_Yield>(value);
			for (int i = 0; i < value; i++)
			{
				RGD_Yield rGD_Yield = new RGD_Yield();
				rGD_Yield.DeserializeFast(reader);
				yields.Add(rGD_Yield);
			}
		}
	}

	public RGD_PlantationSlot()
	{
	}

	public RGD_PlantationSlot(PlantationSlot plantationSlot, int plantationSlotIndex)
	{
		this.plantationSlotIndex = plantationSlotIndex;
		if (plantationSlot.plant != null)
		{
			Plant plant = plantationSlot.plant;
			hasWater = plantationSlot.hasWater;
			uniquePlantIndex = plant.item.UniqueIndex;
			plantObjectIndex = plant.pickupComponent.networkID.ObjectIndex;
			growTimer = plant.GetGrowTimer();
			{
				foreach (Cost item2 in plant.pickupComponent.yieldHandler.Yield)
				{
					RGD_Yield item = new RGD_Yield(item2);
					yields.Add(item);
				}
				return;
			}
		}
		hasWater = false;
		uniquePlantIndex = -1;
	}

	public List<Cost> GetYield()
	{
		if (yields == null)
		{
			return null;
		}
		List<Cost> list = new List<Cost>();
		foreach (RGD_Yield yield in yields)
		{
			if (!(yield == null))
			{
				Cost cost = new Cost();
				cost.amount = yield.amount;
				cost.item = ItemManager.GetItemByIndex(yield.uniqueItemIndex);
				list.Add(cost);
			}
		}
		return list;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("PlantIndex", uniquePlantIndex);
		info.AddValue("PlantSlot", plantationSlotIndex);
		info.AddValue("GrowTimer", growTimer);
		info.AddValue("Yield", yields);
		info.AddValue("PlantObjectIndex", plantObjectIndex);
		info.AddValue("HasWater", hasWater);
	}

	public RGD_PlantationSlot(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "PlantIndex":
				uniquePlantIndex = (int)item.Value;
				break;
			case "PlantSlot":
				plantationSlotIndex = (int)item.Value;
				break;
			case "GrowTimer":
				growTimer = (float)item.Value;
				break;
			case "Yield":
				yields = (List<RGD_Yield>)item.Value;
				break;
			case "PlantObjectIndex":
				plantObjectIndex = (uint)item.Value;
				break;
			case "HasWater":
				hasWater = (bool)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		uniquePlantIndex = -1;
		plantationSlotIndex = 0;
		growTimer = -1f;
		plantObjectIndex = 0u;
		yields = null;
		hasWater = false;
	}
}
