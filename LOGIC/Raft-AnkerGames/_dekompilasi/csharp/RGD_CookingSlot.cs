using System;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_CookingSlot : SerializableBehaviour
{
	public bool isComplete;

	public int cookableUniqueItemIndex;

	public float cookTimer;

	public int[] busySlotIndexes;

	protected bool Equals(RGD_CookingSlot other)
	{
		if (busySlotIndexes == null && other.busySlotIndexes != null)
		{
			return false;
		}
		if (busySlotIndexes != null && other.busySlotIndexes == null)
		{
			return false;
		}
		if (isComplete == other.isComplete && cookableUniqueItemIndex == other.cookableUniqueItemIndex && cookTimer.Equals(other.cookTimer))
		{
			if (busySlotIndexes != null || other.busySlotIndexes != null)
			{
				return Enumerable.SequenceEqual(busySlotIndexes, other.busySlotIndexes);
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
		return Equals((RGD_CookingSlot)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(isComplete, cookableUniqueItemIndex, cookTimer, busySlotIndexes);
	}

	public static bool operator ==(RGD_CookingSlot left, RGD_CookingSlot right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_CookingSlot left, RGD_CookingSlot right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in isComplete, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in cookableUniqueItemIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in cookTimer, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>((busySlotIndexes != null) ? busySlotIndexes.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (busySlotIndexes != null)
		{
			int[] array = busySlotIndexes;
			for (int i = 0; i < array.Length; i++)
			{
				int value = array[i];
				writer.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out isComplete, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out cookableUniqueItemIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out cookTimer, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			busySlotIndexes = new int[value];
			for (int i = 0; i < value; i++)
			{
				reader.ReadValue(out busySlotIndexes[i], default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public RGD_CookingSlot()
	{
	}

	public RGD_CookingSlot(CookingSlot cookingSlot, Block_CookingStand stand)
	{
		isComplete = cookingSlot.IsComplete;
		cookTimer = cookingSlot.CookTimer;
		cookableUniqueItemIndex = ((cookingSlot.CurrentItem == null) ? (-1) : cookingSlot.CurrentItem.UniqueIndex);
		if (cookingSlot.activeCookingSlots != null && cookingSlot.activeCookingSlots.Length != 0)
		{
			busySlotIndexes = new int[cookingSlot.activeCookingSlots.Length];
			for (int i = 0; i < cookingSlot.activeCookingSlots.Length; i++)
			{
				busySlotIndexes[i] = stand.GetIndexOfCookingSlot(cookingSlot.activeCookingSlots[i]);
			}
		}
		else
		{
			busySlotIndexes = null;
		}
	}

	public void RestoreSlot(CookingSlot cookingSlot, Block_CookingStand stand)
	{
		if (cookableUniqueItemIndex == -1 || busySlotIndexes == null)
		{
			return;
		}
		Item_Base itemByIndex = ItemManager.GetItemByIndex(cookableUniqueItemIndex);
		if (itemByIndex == null)
		{
			return;
		}
		CookingSlot[] array = new CookingSlot[busySlotIndexes.Length];
		for (int i = 0; i < busySlotIndexes.Length; i++)
		{
			if (busySlotIndexes[i] >= 0 && busySlotIndexes[i] < stand.cookingSlots.Length)
			{
				array[i] = stand.cookingSlots[busySlotIndexes[i]];
			}
		}
		float newTimerValue = (isComplete ? float.MaxValue : cookTimer);
		cookingSlot.StartCooking(itemByIndex, array);
		cookingSlot.ForceSetTimer(newTimerValue);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Complete", isComplete);
		info.AddValue("CookableIndex", cookableUniqueItemIndex);
		info.AddValue("CookTimer", cookTimer);
		info.AddValue("BusySlotIndexes", busySlotIndexes);
	}

	public RGD_CookingSlot(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Complete":
				isComplete = (bool)item.Value;
				break;
			case "CookableIndex":
				cookableUniqueItemIndex = (int)item.Value;
				break;
			case "CookTimer":
				cookTimer = (float)item.Value;
				break;
			case "BusySlotIndexes":
				busySlotIndexes = (int[])item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		isComplete = false;
		cookableUniqueItemIndex = -1;
		cookTimer = -1f;
		busySlotIndexes = null;
	}
}
