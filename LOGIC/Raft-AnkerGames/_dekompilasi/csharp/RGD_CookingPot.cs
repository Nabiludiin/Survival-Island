using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_CookingPot : SerializableBehaviour
{
	public RGD_CookingPot_Slot[] rgdSlots;

	public uint cookingPotObjectIndex;

	public float cookTimer;

	public uint portions;

	public int recipeIndex;

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdSlots != null) ? rgdSlots.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdSlots != null)
		{
			RGD_CookingPot_Slot[] array = rgdSlots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SerializeFast(writer);
			}
		}
		writer.WriteValueSafe(in cookingPotObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in cookTimer, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in portions, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in recipeIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgdSlots = new RGD_CookingPot_Slot[value];
			for (int i = 0; i < value; i++)
			{
				rgdSlots[i] = new RGD_CookingPot_Slot();
				rgdSlots[i].DeserializeFast(reader);
			}
		}
		reader.ReadValue(out cookingPotObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out cookTimer, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out portions, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out recipeIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_CookingPot()
	{
	}

	public RGD_CookingPot(CookingTable cookingPot)
	{
		cookingPotObjectIndex = cookingPot.ObjectIndex;
		cookTimer = cookingPot.CookTimer;
		portions = cookingPot.Portions;
		recipeIndex = ((cookingPot.CurrentRecipe == null) ? (-1) : ((int)cookingPot.CurrentRecipe.RecipeIndex));
		if (cookingPot.Slots == null || cookingPot.Slots.Length == 0)
		{
			rgdSlots = null;
			return;
		}
		List<RGD_CookingPot_Slot> list = new List<RGD_CookingPot_Slot>();
		for (int i = 0; i < cookingPot.Slots.Length; i++)
		{
			if (cookingPot.Slots[i].CurrentItem != null)
			{
				list.Add(new RGD_CookingPot_Slot(cookingPot.Slots[i]));
			}
		}
		rgdSlots = list.ToArray();
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CookingPotObjectIndex", cookingPotObjectIndex);
		info.AddValue("CookTimer", cookTimer);
		info.AddValue("Portions", portions);
		info.AddValue("RecipeIndex", recipeIndex);
		info.AddValue("Slots", rgdSlots);
	}

	public RGD_CookingPot(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "CookingPotObjectIndex":
				cookingPotObjectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "CookTimer":
				cookTimer = Helper.ObjToValue<float>(item.Value);
				break;
			case "Portions":
				portions = Helper.ObjToValue<uint>(item.Value);
				break;
			case "RecipeIndex":
				recipeIndex = Helper.ObjToValue<int>(item.Value);
				break;
			case "Slots":
				rgdSlots = (RGD_CookingPot_Slot[])item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		cookingPotObjectIndex = 0u;
		cookTimer = 0f;
		portions = 0u;
		recipeIndex = -1;
		rgdSlots = null;
	}
}
