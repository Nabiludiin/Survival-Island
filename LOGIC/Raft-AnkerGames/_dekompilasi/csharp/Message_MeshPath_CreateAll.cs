using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_MeshPath_CreateAll : Message_NetworkBehaviour
{
	public RGD_MeshPath[] rgdMeshPaths;

	protected bool Equals(Message_MeshPath_CreateAll other)
	{
		if (rgdMeshPaths == null && other.rgdMeshPaths != null)
		{
			return false;
		}
		if (rgdMeshPaths != null && other.rgdMeshPaths == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdMeshPaths != null || other.rgdMeshPaths != null)
			{
				return Enumerable.SequenceEqual(rgdMeshPaths, other.rgdMeshPaths);
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
		return Equals((Message_MeshPath_CreateAll)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdMeshPaths);
	}

	public static bool operator ==(Message_MeshPath_CreateAll left, Message_MeshPath_CreateAll right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_MeshPath_CreateAll left, Message_MeshPath_CreateAll right)
	{
		return !object.Equals(left, right);
	}

	public Message_MeshPath_CreateAll()
	{
	}

	public Message_MeshPath_CreateAll(Messages type, MonoBehaviour_Network behaviour)
		: base(type, behaviour)
	{
		List<RGD_MeshPath> list = new List<RGD_MeshPath>();
		for (int i = 0; i < MeshPath.PathConnections.Count; i++)
		{
			MeshPathConnection meshPathConnection = MeshPath.PathConnections[i];
			if (meshPathConnection != null)
			{
				list.Add(new RGD_MeshPath(meshPathConnection));
			}
		}
		rgdMeshPaths = list.ToArray();
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(rgdMeshPaths.Length, default(FastBufferWriter.ForPrimitives));
		RGD_MeshPath[] array = rgdMeshPaths;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		rgdMeshPaths = new RGD_MeshPath[value];
		for (int i = 0; i < value; i++)
		{
			rgdMeshPaths[i] = new RGD_MeshPath();
			rgdMeshPaths[i].DeserializeFast(reader);
		}
	}
}
