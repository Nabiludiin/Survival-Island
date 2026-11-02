using System;
using Unity.Netcode;

[Serializable]
public class Message_MeshPath : Message_NetworkBehaviour
{
	public RGD_MeshPath rgdMeshPath;

	protected bool Equals(Message_MeshPath other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return object.Equals(rgdMeshPath, other.rgdMeshPath);
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
		return Equals((Message_MeshPath)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdMeshPath);
	}

	public static bool operator ==(Message_MeshPath left, Message_MeshPath right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_MeshPath left, Message_MeshPath right)
	{
		return !object.Equals(left, right);
	}

	public Message_MeshPath()
	{
	}

	public Message_MeshPath(Messages type, MonoBehaviour_Network behaviour, MeshPathBase baseA, MeshPathBase baseB, float slack)
		: base(type, behaviour)
	{
		rgdMeshPath = new RGD_MeshPath(baseA, baseB, slack);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdMeshPath.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdMeshPath = new RGD_MeshPath();
		rgdMeshPath.DeserializeFast(reader);
	}
}
