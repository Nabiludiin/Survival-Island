using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_CookingStand : RGD_Block
{
	public uint cookingStandObjectIndex;

	public float fuelTimer;

	public int fuelCount;

	public List<RGD_CookingSlot> cookingSlots = new List<RGD_CookingSlot>();

	protected bool Equals(RGD_CookingStand other)
	{
		if (cookingSlots == null && other.cookingSlots != null)
		{
			return false;
		}
		if (cookingSlots != null && other.cookingSlots == null)
		{
			return false;
		}
		if (Equals((RGD_Block)other) && cookingStandObjectIndex == other.cookingStandObjectIndex && fuelTimer.Equals(other.fuelTimer) && fuelCount == other.fuelCount)
		{
			if (cookingSlots != null || other.cookingSlots != null)
			{
				return cookingSlots.SequenceEqual(other.cookingSlots);
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
		return Equals((RGD_CookingStand)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), cookingStandObjectIndex, fuelTimer, fuelCount, cookingSlots);
	}

	public static bool operator ==(RGD_CookingStand left, RGD_CookingStand right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_CookingStand left, RGD_CookingStand right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in cookingStandObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in fuelTimer, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in fuelCount, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>(cookingSlots.Count, default(FastBufferWriter.ForPrimitives));
		foreach (RGD_CookingSlot cookingSlot in cookingSlots)
		{
			cookingSlot.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out cookingStandObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out fuelTimer, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out fuelCount, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		for (int i = 0; i < value; i++)
		{
			RGD_CookingSlot rGD_CookingSlot = new RGD_CookingSlot();
			rGD_CookingSlot.DeserializeFast(reader);
			cookingSlots.Add(rGD_CookingSlot);
		}
	}

	public RGD_CookingStand()
	{
	}

	public RGD_CookingStand(RGDType type, Block_CookingStand stand)
		: base(type, stand)
	{
		cookingStandObjectIndex = stand.ObjectIndex;
		if (stand.fuel != null)
		{
			fuelTimer = stand.fuel.GetTimer();
			fuelCount = stand.fuel.GetFuelCount();
		}
		for (int i = 0; i < stand.cookingSlots.Length; i++)
		{
			RGD_CookingSlot item = new RGD_CookingSlot(stand.cookingSlots[i], stand);
			cookingSlots.Add(item);
		}
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		Block_CookingStand block_CookingStand = block as Block_CookingStand;
		if (block_CookingStand != null)
		{
			RestoreStand(block_CookingStand);
		}
		else
		{
			Debug.LogError("Trying to restore a cookingstand, block casting failed");
		}
	}

	public virtual void RestoreStand(Block_CookingStand stand)
	{
		if (stand == null)
		{
			Debug.Log("NULL, could not restore cookingstand because stand was null");
			return;
		}
		if (stand.fuel != null)
		{
			if (fuelCount != -1)
			{
				stand.fuel.AddFuel(fuelCount);
			}
			if (fuelTimer != -1f)
			{
				stand.fuel.ForceSetTimer(fuelTimer);
			}
		}
		if (cookingSlots != null)
		{
			for (int i = 0; i < cookingSlots.Count; i++)
			{
				RGD_CookingSlot rGD_CookingSlot = cookingSlots[i];
				CookingSlot cookingSlot = stand.cookingSlots[i];
				rGD_CookingSlot.RestoreSlot(cookingSlot, stand);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CookingStandObjectIndex", cookingStandObjectIndex);
		info.AddValue("FuelTimer", fuelTimer);
		info.AddValue("FuelCount", fuelCount);
		info.AddValue("CookingSlots", cookingSlots);
	}

	public RGD_CookingStand(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "CookingStandObjectIndex":
				cookingStandObjectIndex = (uint)item.Value;
				break;
			case "FuelTimer":
				fuelTimer = (float)item.Value;
				break;
			case "FuelCount":
				fuelCount = (int)item.Value;
				break;
			case "CookingSlots":
				cookingSlots = (List<RGD_CookingSlot>)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		cookingStandObjectIndex = 0u;
		fuelTimer = -1f;
		fuelCount = -1;
		cookingSlots = null;
	}
}
