using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_TicTacToe : RGD_Block
{
	public bool[] meshType;

	public bool[] enabled;

	protected bool Equals(RGD_Block_TicTacToe other)
	{
		if (Equals((RGD_Block)other) && object.Equals(meshType, other.meshType))
		{
			return object.Equals(enabled, other.enabled);
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
		return Equals((RGD_Block_TicTacToe)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), meshType, enabled);
	}

	public static bool operator ==(RGD_Block_TicTacToe left, RGD_Block_TicTacToe right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block_TicTacToe left, RGD_Block_TicTacToe right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(meshType, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(enabled, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out meshType, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out enabled, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Block_TicTacToe()
	{
	}

	public RGD_Block_TicTacToe(RGDType type, Block block)
		: base(type, block)
	{
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		Block_TicTacToe block_TicTacToe = block as Block_TicTacToe;
		if (block_TicTacToe != null)
		{
			block_TicTacToe.RestoreRGD(this);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("MeshType", meshType);
		info.AddValue("Enabled", enabled);
	}

	public RGD_Block_TicTacToe(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "MeshType"))
			{
				if (name == "Enabled")
				{
					enabled = (bool[])item.Value;
				}
			}
			else
			{
				meshType = (bool[])item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		meshType = null;
		enabled = null;
	}
}
