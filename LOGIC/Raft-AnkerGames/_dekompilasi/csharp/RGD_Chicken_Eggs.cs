using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Chicken_Eggs : RGD
{
	public RGD_Egg[] rgdEggs;

	protected bool Equals(RGD_Chicken_Eggs other)
	{
		if (rgdEggs == null && other.rgdEggs != null)
		{
			return false;
		}
		if (rgdEggs != null && other.rgdEggs == null)
		{
			return false;
		}
		if (rgdEggs != null || other.rgdEggs != null)
		{
			return Enumerable.SequenceEqual(rgdEggs, other.rgdEggs);
		}
		return true;
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
		return Equals((RGD_Chicken_Eggs)obj);
	}

	public override int GetHashCode()
	{
		if (rgdEggs == null)
		{
			return 0;
		}
		return rgdEggs.GetHashCode();
	}

	public static bool operator ==(RGD_Chicken_Eggs left, RGD_Chicken_Eggs right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Chicken_Eggs left, RGD_Chicken_Eggs right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdEggs != null) ? rgdEggs.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdEggs != null)
		{
			RGD_Egg[] array = rgdEggs;
			foreach (RGD_Egg rGD_Egg in array)
			{
				writer.WriteValueSafe(in rGD_Egg.localRaftPosition.x, default(FastBufferWriter.ForPrimitives));
				writer.WriteValueSafe(in rGD_Egg.localRaftPosition.y, default(FastBufferWriter.ForPrimitives));
				writer.WriteValueSafe(in rGD_Egg.localRaftPosition.z, default(FastBufferWriter.ForPrimitives));
				writer.WriteValueSafe(in rGD_Egg.eggObjectIndex, default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgdEggs = new RGD_Egg[value];
			for (int i = 0; i < value; i++)
			{
				rgdEggs[i] = new RGD_Egg();
				rgdEggs[i].localRaftPosition = new SerializableVector3();
				reader.ReadValue(out rgdEggs[i].localRaftPosition.x, default(FastBufferWriter.ForPrimitives));
				reader.ReadValue(out rgdEggs[i].localRaftPosition.y, default(FastBufferWriter.ForPrimitives));
				reader.ReadValue(out rgdEggs[i].localRaftPosition.z, default(FastBufferWriter.ForPrimitives));
				reader.ReadValue(out rgdEggs[i].eggObjectIndex, default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public RGD_Chicken_Eggs()
	{
	}

	public RGD_Chicken_Eggs(PickupItem_Networked[] eggs)
		: base(RGDType.ID_ChickenEggs, RGDSortingOrder.Pickups)
	{
		List<RGD_Egg> list = new List<RGD_Egg>();
		for (int i = 0; i < eggs.Length; i++)
		{
			if (eggs[i] != null)
			{
				list.Add(new RGD_Egg(eggs[i].transform.localPosition, eggs[i].ObjectIndex));
			}
		}
		if (list.ContainsItems())
		{
			rgdEggs = list.ToArray();
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("EggPositions", rgdEggs);
	}

	public RGD_Chicken_Eggs(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "EggPositions")
			{
				rgdEggs = (RGD_Egg[])item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdEggs = null;
	}
}
