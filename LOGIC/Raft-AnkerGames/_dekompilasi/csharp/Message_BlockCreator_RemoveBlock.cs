using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_BlockCreator_RemoveBlock : Message_NetworkBehaviour_SteamID
{
	public uint blockObjectIndex;

	[SerializeField]
	private bool u;

	public bool UpdateBounds => u;

	protected bool Equals(Message_BlockCreator_RemoveBlock other)
	{
		if (Equals((Message_NetworkBehaviour_SteamID)other) && blockObjectIndex == other.blockObjectIndex)
		{
			return u == other.u;
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
		return Equals((Message_BlockCreator_RemoveBlock)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), blockObjectIndex, u);
	}

	public static bool operator ==(Message_BlockCreator_RemoveBlock left, Message_BlockCreator_RemoveBlock right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BlockCreator_RemoveBlock left, Message_BlockCreator_RemoveBlock right)
	{
		return !object.Equals(left, right);
	}

	public Message_BlockCreator_RemoveBlock(Messages type, MonoBehaviour_Network behaviour, Network_UserId playerRemovingBlock, Block block, bool updateBounds)
		: base(type, behaviour, playerRemovingBlock)
	{
		blockObjectIndex = block.ObjectIndex;
		u = updateBounds;
	}

	public Message_BlockCreator_RemoveBlock()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in blockObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in u, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out blockObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out u, default(FastBufferWriter.ForPrimitives));
	}
}
