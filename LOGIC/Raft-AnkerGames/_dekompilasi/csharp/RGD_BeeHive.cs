using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_BeeHive : SerializableBehaviour
{
	public int c;

	public uint b;

	public int CurrentHoneyLevelIndex
	{
		get
		{
			return c;
		}
		set
		{
			c = value;
		}
	}

	public uint BeeHiveObjectIndex
	{
		get
		{
			return b;
		}
		set
		{
			b = value;
		}
	}

	protected bool Equals(RGD_BeeHive other)
	{
		if (c == other.c)
		{
			return b == other.b;
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
		return Equals((RGD_BeeHive)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(c, b);
	}

	public static bool operator ==(RGD_BeeHive left, RGD_BeeHive right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_BeeHive left, RGD_BeeHive right)
	{
		return !object.Equals(left, right);
	}

	public RGD_BeeHive()
	{
	}

	public RGD_BeeHive(BeeHive beeHive)
	{
		CurrentHoneyLevelIndex = beeHive.currentHoneyLevelIndex;
		BeeHiveObjectIndex = beeHive.ObjectIndex;
	}

	public void RestoreBeeHive(BeeHive beeHive)
	{
		if (!(beeHive == null))
		{
			beeHive.currentHoneyLevelIndex = CurrentHoneyLevelIndex;
			beeHive.UpdateHoneyLevel(CurrentHoneyLevelIndex);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CurrentProduction", CurrentHoneyLevelIndex);
		info.AddValue("ObjectIndex", BeeHiveObjectIndex);
	}

	public RGD_BeeHive(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "CurrentProduction"))
			{
				if (name == "ObjectIndex")
				{
					BeeHiveObjectIndex = (uint)item.Value;
				}
			}
			else
			{
				CurrentHoneyLevelIndex = (int)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		CurrentHoneyLevelIndex = 0;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in c, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in b, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out c, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out b, default(FastBufferWriter.ForPrimitives));
	}
}
