using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Birdsnest : RGD_Block
{
	public bool[] activeEggs;

	protected bool Equals(RGD_Birdsnest other)
	{
		if (Equals((RGD_Block)other))
		{
			return object.Equals(activeEggs, other.activeEggs);
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
		return Equals((RGD_Birdsnest)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), activeEggs);
	}

	public static bool operator ==(RGD_Birdsnest left, RGD_Birdsnest right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Birdsnest left, RGD_Birdsnest right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(activeEggs, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValueSafe(out activeEggs, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Birdsnest()
	{
	}

	public RGD_Birdsnest(RGDType type, Block block, BirdsNest birdsNest)
		: base(type, block)
	{
		activeEggs = new bool[birdsNest.eggs.Length];
		for (int i = 0; i < activeEggs.Length; i++)
		{
			activeEggs[i] = birdsNest.eggs[i].activeInHierarchy;
		}
	}

	public void RestoreBirdsNest(BirdsNest birdsNest)
	{
		if (activeEggs == null)
		{
			return;
		}
		for (int i = 0; i < birdsNest.eggs.Length; i++)
		{
			if (i < activeEggs.Length)
			{
				birdsNest.eggs[i].SetActive(activeEggs[i]);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Eggs", activeEggs);
	}

	public RGD_Birdsnest(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Eggs")
			{
				activeEggs = (bool[])item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		activeEggs = null;
	}
}
