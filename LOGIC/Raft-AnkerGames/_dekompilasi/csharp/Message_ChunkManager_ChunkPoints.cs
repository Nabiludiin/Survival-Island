using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_ChunkManager_ChunkPoints : Message_NetworkBehaviour_ID
{
	public RGD_ChunkPoint[] rgdChunkPoints;

	protected bool Equals(Message_ChunkManager_ChunkPoints other)
	{
		if (rgdChunkPoints == null && other.rgdChunkPoints != null)
		{
			return false;
		}
		if (rgdChunkPoints != null && other.rgdChunkPoints == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			if (rgdChunkPoints != null || other.rgdChunkPoints != null)
			{
				return Enumerable.SequenceEqual(rgdChunkPoints, other.rgdChunkPoints);
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
		return Equals((Message_ChunkManager_ChunkPoints)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdChunkPoints);
	}

	public static bool operator ==(Message_ChunkManager_ChunkPoints left, Message_ChunkManager_ChunkPoints right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ChunkManager_ChunkPoints left, Message_ChunkManager_ChunkPoints right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdChunkPoints != null) ? rgdChunkPoints.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdChunkPoints != null)
		{
			RGD_ChunkPoint[] array = rgdChunkPoints;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SerializeFast(writer);
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
			rgdChunkPoints = new RGD_ChunkPoint[value];
			for (int i = 0; i < value; i++)
			{
				rgdChunkPoints[i] = new RGD_ChunkPoint();
				rgdChunkPoints[i].DeserializeFast(reader);
			}
		}
	}

	public Message_ChunkManager_ChunkPoints()
	{
	}

	public Message_ChunkManager_ChunkPoints(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, List<ChunkPoint> chunkPoints)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		List<RGD_ChunkPoint> list = new List<RGD_ChunkPoint>();
		if (chunkPoints.ContainsItems())
		{
			for (int i = 0; i < chunkPoints.Count; i++)
			{
				if (chunkPoints[i] != null)
				{
					list.Add(new RGD_ChunkPoint(chunkPoints[i]));
				}
			}
		}
		if (list.ContainsItems())
		{
			rgdChunkPoints = list.ToArray();
		}
		else
		{
			rgdChunkPoints = null;
		}
	}
}
