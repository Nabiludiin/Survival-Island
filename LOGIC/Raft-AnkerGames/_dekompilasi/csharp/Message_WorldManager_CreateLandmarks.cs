using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_WorldManager_CreateLandmarks : Message_NetworkBehaviour
{
	public RGD_Landmark[] rgdLandmarks;

	protected bool Equals(Message_WorldManager_CreateLandmarks other)
	{
		if (rgdLandmarks == null && other.rgdLandmarks != null)
		{
			return false;
		}
		if (rgdLandmarks != null && other.rgdLandmarks == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdLandmarks != null || other.rgdLandmarks != null)
			{
				return Enumerable.SequenceEqual(rgdLandmarks, other.rgdLandmarks);
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
		return Equals((Message_WorldManager_CreateLandmarks)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdLandmarks);
	}

	public static bool operator ==(Message_WorldManager_CreateLandmarks left, Message_WorldManager_CreateLandmarks right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WorldManager_CreateLandmarks left, Message_WorldManager_CreateLandmarks right)
	{
		return !object.Equals(left, right);
	}

	public Message_WorldManager_CreateLandmarks()
	{
	}

	public Message_WorldManager_CreateLandmarks(Messages type, MonoBehaviour_Network behaviour)
		: base(type, behaviour)
	{
		rgdLandmarks = new RGD_Landmark[WorldManager.AllLandmarks.Count];
		for (int i = 0; i < rgdLandmarks.Length; i++)
		{
			rgdLandmarks[i] = new RGD_Landmark(WorldManager.AllLandmarks.ElementAt(i));
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(rgdLandmarks.Length, default(FastBufferWriter.ForPrimitives));
		RGD_Landmark[] array = rgdLandmarks;
		foreach (RGD_Landmark obj in array)
		{
			if (obj is RGD_Landmark_Raft)
			{
				writer.WriteValueSafe<bool>(true, default(FastBufferWriter.ForPrimitives));
			}
			else
			{
				writer.WriteValueSafe<bool>(false, default(FastBufferWriter.ForPrimitives));
			}
			obj.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		rgdLandmarks = new RGD_Landmark[value];
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out bool value2, default(FastBufferWriter.ForPrimitives));
			if (value2)
			{
				rgdLandmarks[i] = new RGD_Landmark_Raft();
			}
			else
			{
				rgdLandmarks[i] = new RGD_Landmark();
			}
			rgdLandmarks[i].DeserializeFast(reader);
		}
	}
}
