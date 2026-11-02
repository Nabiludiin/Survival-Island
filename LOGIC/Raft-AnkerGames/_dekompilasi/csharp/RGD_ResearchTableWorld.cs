using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_ResearchTableWorld : SerializableBehaviour
{
	public List<int> learnedItems = new List<int>();

	public List<int> researchedItems = new List<int>();

	protected bool Equals(RGD_ResearchTableWorld other)
	{
		if (learnedItems == null && other.learnedItems != null)
		{
			return false;
		}
		if (learnedItems != null && other.learnedItems == null)
		{
			return false;
		}
		if (researchedItems == null && other.researchedItems != null)
		{
			return false;
		}
		if (researchedItems != null && other.researchedItems == null)
		{
			return false;
		}
		if ((learnedItems == null && other.learnedItems == null) || learnedItems.SequenceEqual(other.learnedItems))
		{
			if (researchedItems != null || other.researchedItems != null)
			{
				return researchedItems.SequenceEqual(other.researchedItems);
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
		return Equals((RGD_ResearchTableWorld)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(learnedItems, researchedItems);
	}

	public static bool operator ==(RGD_ResearchTableWorld left, RGD_ResearchTableWorld right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_ResearchTableWorld left, RGD_ResearchTableWorld right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(learnedItems.Count, default(FastBufferWriter.ForPrimitives));
		foreach (int learnedItem in learnedItems)
		{
			writer.WriteValueSafe<int>(learnedItem, default(FastBufferWriter.ForPrimitives));
		}
		writer.WriteValueSafe<int>(researchedItems.Count, default(FastBufferWriter.ForPrimitives));
		foreach (int researchedItem in researchedItems)
		{
			writer.WriteValueSafe<int>(researchedItem, default(FastBufferWriter.ForPrimitives));
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		learnedItems = new List<int>(value);
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out int value2, default(FastBufferWriter.ForPrimitives));
			learnedItems.Add(value2);
		}
		int value3 = 0;
		reader.ReadValue(out value3, default(FastBufferWriter.ForPrimitives));
		researchedItems = new List<int>(value3);
		for (int j = 0; j < value3; j++)
		{
			reader.ReadValue(out int value4, default(FastBufferWriter.ForPrimitives));
			researchedItems.Add(value4);
		}
	}

	public RGD_ResearchTableWorld()
	{
	}

	public RGD_ResearchTableWorld(Inventory_ResearchTable inventoryRef)
	{
		Inventory_ResearchTable.hasBeenSaved = true;
		List<ResearchMenuItem> menuItems = inventoryRef.GetMenuItems();
		for (int i = 0; i < menuItems.Count; i++)
		{
			if (menuItems[i].Learned)
			{
				learnedItems.Add(menuItems[i].GetItem().UniqueIndex);
			}
		}
		List<Item_Base> list = inventoryRef.GetResearchedItems();
		for (int j = 0; j < list.Count; j++)
		{
			researchedItems.Add(list[j].UniqueIndex);
		}
	}

	public void RestoreResearchTableWorld(Inventory_ResearchTable inventoryRef)
	{
		if (researchedItems != null)
		{
			for (int i = 0; i < researchedItems.Count; i++)
			{
				Item_Base itemByIndex = ItemManager.GetItemByIndex(researchedItems[i]);
				if (itemByIndex != null)
				{
					inventoryRef.Research(itemByIndex);
				}
			}
		}
		if (learnedItems != null)
		{
			List<ResearchMenuItem> menuItems = inventoryRef.GetMenuItems();
			for (int j = 0; j < menuItems.Count; j++)
			{
				if (learnedItems.Contains(menuItems[j].GetItem().UniqueIndex))
				{
					menuItems[j].LearnInstantly();
				}
			}
		}
		inventoryRef.SortMenuItems();
		inventoryRef.TestResearcherAchievement(autoLearnRecipe: false);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("LearnedItems", learnedItems);
		info.AddValue("ResearchedItems", researchedItems);
	}

	public RGD_ResearchTableWorld(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "LearnedItems"))
			{
				if (name == "ResearchedItems")
				{
					researchedItems = (List<int>)item.Value;
				}
			}
			else
			{
				learnedItems = (List<int>)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		learnedItems = null;
		researchedItems = null;
	}
}
