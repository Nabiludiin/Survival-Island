using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Block_Interactable : RGD_Block
{
	[SerializeField]
	private int ii;

	protected bool Equals(RGD_Block_Interactable other)
	{
		if (Equals((RGD_Block)other))
		{
			return ii == other.ii;
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
		return Equals((RGD_Block_Interactable)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), ii);
	}

	public static bool operator ==(RGD_Block_Interactable left, RGD_Block_Interactable right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_Interactable left, RGD_Block_Interactable right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in ii, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out ii, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Block_Interactable()
	{
	}

	public RGD_Block_Interactable(RGDType type, Block block, int index)
		: base(type, block)
	{
		ii = index;
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		if (!(block == null))
		{
			Placeable_Interactable componentInChildren = block.GetComponentInChildren<Placeable_Interactable>();
			if (componentInChildren != null)
			{
				componentInChildren.RestoreIndex(ii);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("II", ii);
	}

	public RGD_Block_Interactable(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "II")
			{
				ii = Helper.ObjToValue<int>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		ii = 0;
	}
}
