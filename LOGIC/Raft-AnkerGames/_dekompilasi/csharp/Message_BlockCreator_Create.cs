using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_BlockCreator_Create : Message_NetworkBehaviour
{
	public RGD_Block[] rgdBlocks;

	protected bool Equals(Message_BlockCreator_Create other)
	{
		if (rgdBlocks == null && other.rgdBlocks != null)
		{
			return false;
		}
		if (rgdBlocks != null && other.rgdBlocks == null)
		{
			return false;
		}
		for (int i = 0; i < rgdBlocks.Length; i++)
		{
			if (!rgdBlocks[i].Equals(other.rgdBlocks[i]))
			{
				Debug.LogError("Error in block match!");
			}
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdBlocks != null || other.rgdBlocks != null)
			{
				return Enumerable.SequenceEqual(rgdBlocks, other.rgdBlocks);
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
		return Equals((Message_BlockCreator_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdBlocks);
	}

	public static bool operator ==(Message_BlockCreator_Create left, Message_BlockCreator_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BlockCreator_Create left, Message_BlockCreator_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_BlockCreator_Create(Messages type, BlockCreator creator, RGD_Block[] rgdBlocks)
		: base(type, creator)
	{
		this.rgdBlocks = rgdBlocks;
	}

	public Message_BlockCreator_Create()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdBlocks != null) ? rgdBlocks.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdBlocks != null)
		{
			RGD_Block[] array = rgdBlocks;
			foreach (RGD_Block rGD_Block in array)
			{
				writer.WriteValueSafe<RGDType>(rGD_Block.Type, default(FastBufferWriter.ForEnums));
				rGD_Block.SerializeFast(writer);
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
			rgdBlocks = new RGD_Block[value];
			for (int i = 0; i < value; i++)
			{
				reader.ReadValue(out RGDType value2, default(FastBufferWriter.ForEnums));
				rgdBlocks[i] = FastMessageDeserializer.CreateBlock(value2);
				rgdBlocks[i].DeserializeFast(reader);
			}
		}
	}
}
