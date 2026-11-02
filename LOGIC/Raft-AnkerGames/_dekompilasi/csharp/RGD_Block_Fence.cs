using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Block_Fence : RGD_Block
{
	[SerializeField]
	private bool c;

	[SerializeField]
	private bool a;

	public bool RightKnobState
	{
		get
		{
			return c;
		}
		private set
		{
			c = value;
		}
	}

	public bool LeftKnobState
	{
		get
		{
			return a;
		}
		private set
		{
			a = value;
		}
	}

	protected bool Equals(RGD_Block_Fence other)
	{
		if (Equals((RGD_Block)other) && c == other.c)
		{
			return a == other.a;
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
		return Equals((RGD_Block_Fence)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), c, a);
	}

	public static bool operator ==(RGD_Block_Fence left, RGD_Block_Fence right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_Fence left, RGD_Block_Fence right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in c, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in a, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out c, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out a, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Block_Fence()
	{
	}

	public RGD_Block_Fence(RGDType type, Block_Fence block)
		: base(type, block)
	{
		if (block != null)
		{
			RightKnobState = block.knobRight.gameObject.activeSelf;
			LeftKnobState = block.knobLeft.gameObject.activeSelf;
		}
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		Block_Fence block_Fence = block as Block_Fence;
		if (block_Fence != null)
		{
			if (block_Fence.knobRight != null)
			{
				block_Fence.knobRight.gameObject.SetActiveSafe(RightKnobState);
			}
			if (block_Fence.knobLeft != null)
			{
				block_Fence.knobLeft.gameObject.SetActiveSafe(LeftKnobState);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RS", RightKnobState);
		info.AddValue("LS", LeftKnobState);
	}

	public RGD_Block_Fence(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "RS"))
			{
				if (name == "LS")
				{
					LeftKnobState = Helper.ObjToValue<bool>(item.Value);
				}
			}
			else
			{
				RightKnobState = Helper.ObjToValue<bool>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		RightKnobState = false;
		LeftKnobState = false;
	}
}
