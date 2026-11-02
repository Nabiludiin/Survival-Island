using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Block_Foundation : RGD_Block
{
	[SerializeField]
	private bool r;

	public bool IsReinforced
	{
		get
		{
			return r;
		}
		set
		{
			r = value;
		}
	}

	protected bool Equals(RGD_Block_Foundation other)
	{
		if (Equals((RGD_Block)other))
		{
			return r == other.r;
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
		return Equals((RGD_Block_Foundation)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), r);
	}

	public static bool operator ==(RGD_Block_Foundation left, RGD_Block_Foundation right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_Foundation left, RGD_Block_Foundation right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Block_Foundation(RGDType type, Block_Foundation block)
		: base(type, block)
	{
		IsReinforced = block.Reinforced;
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		block.Reinforced = IsReinforced;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("IsReinforced", IsReinforced);
	}

	public RGD_Block_Foundation(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "IsReinforced")
			{
				IsReinforced = (bool)item.Value;
			}
		}
	}

	public RGD_Block_Foundation()
	{
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		IsReinforced = false;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in r, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out r, default(FastBufferWriter.ForPrimitives));
	}
}
