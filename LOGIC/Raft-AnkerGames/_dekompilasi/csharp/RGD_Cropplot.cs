using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Cropplot : RGD_Block
{
	public uint cropplotObjectIndex;

	public List<RGD_PlantationSlot> plantationSlots = new List<RGD_PlantationSlot>();

	protected bool Equals(RGD_Cropplot other)
	{
		if (plantationSlots == null && other.plantationSlots != null)
		{
			return false;
		}
		if (plantationSlots != null && other.plantationSlots == null)
		{
			return false;
		}
		if (Equals((RGD_Block)other) && cropplotObjectIndex == other.cropplotObjectIndex)
		{
			if (plantationSlots != null || other.plantationSlots != null)
			{
				return plantationSlots.SequenceEqual(other.plantationSlots);
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
		return Equals((RGD_Cropplot)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), cropplotObjectIndex, plantationSlots);
	}

	public static bool operator ==(RGD_Cropplot left, RGD_Cropplot right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Cropplot left, RGD_Cropplot right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Cropplot()
	{
	}

	public RGD_Cropplot(RGDType type, Cropplot cropplot)
		: base(type, cropplot)
	{
		cropplotObjectIndex = cropplot.ObjectIndex;
		for (int i = 0; i < cropplot.plantationSlots.Count; i++)
		{
			RGD_PlantationSlot item = new RGD_PlantationSlot(cropplot.plantationSlots[i], i);
			plantationSlots.Add(item);
		}
	}

	public void RestoreCropplot(Cropplot plot, PlantManager plantManager)
	{
		if (plantationSlots == null)
		{
			return;
		}
		foreach (RGD_PlantationSlot plantationSlot in plantationSlots)
		{
			if (plantationSlot.uniquePlantIndex == -1)
			{
				continue;
			}
			int plantationSlotIndex = plantationSlot.plantationSlotIndex;
			Plant plantByIndex = plantManager.GetPlantByIndex(plantationSlot.uniquePlantIndex);
			if (!(plantByIndex != null))
			{
				continue;
			}
			Plant plant = plot.PlantSeed(plantByIndex, plantationSlot.plantObjectIndex, waterCropAswell: false, plantationSlotIndex);
			if (plant == null)
			{
				continue;
			}
			if (plantationSlot.hasWater && plantationSlotIndex < plot.plantationSlots.Count && plantationSlotIndex >= 0)
			{
				plot.plantationSlots[plantationSlotIndex].AddWater(playEffects: false);
				if (plantManager != null)
				{
					plantManager.SendOnCropplotModified(plot, PlantManager.ObjectModification.WATERED);
				}
			}
			if (plantationSlot.growTimer != -1f)
			{
				plant.SetGrowTimer(plantationSlot.growTimer);
				if (plant.ShouldBeFullyGrown())
				{
					plant.Complete();
				}
			}
			List<Cost> yield = plantationSlot.GetYield();
			if (yield != null)
			{
				plant.SetYield(yield);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CropplotObjectIndex", cropplotObjectIndex);
		info.AddValue("PlantationSlots", plantationSlots);
	}

	public RGD_Cropplot(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "CropplotObjectIndex"))
			{
				if (name == "PlantationSlots")
				{
					plantationSlots = (List<RGD_PlantationSlot>)item.Value;
				}
			}
			else
			{
				cropplotObjectIndex = (uint)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		cropplotObjectIndex = 0u;
		plantationSlots = null;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in cropplotObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>(plantationSlots.Count, default(FastBufferWriter.ForPrimitives));
		foreach (RGD_PlantationSlot plantationSlot in plantationSlots)
		{
			plantationSlot.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out cropplotObjectIndex, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			plantationSlots = new List<RGD_PlantationSlot>(value);
			for (int i = 0; i < value; i++)
			{
				RGD_PlantationSlot rGD_PlantationSlot = new RGD_PlantationSlot();
				rGD_PlantationSlot.DeserializeFast(reader);
				plantationSlots.Add(rGD_PlantationSlot);
			}
		}
	}
}
