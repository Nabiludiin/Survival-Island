using System;
using Unity.Netcode;

[Serializable]
public class Message_Zipline_Attach : Message_NetworkBehaviour
{
	public RGD_MeshPath rgdMeshPath;

	public int modelIndex = 1;

	protected bool Equals(Message_Zipline_Attach other)
	{
		if (Equals((Message_NetworkBehaviour)other) && object.Equals(rgdMeshPath, other.rgdMeshPath))
		{
			return modelIndex == other.modelIndex;
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
		return Equals((Message_Zipline_Attach)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdMeshPath, modelIndex);
	}

	public static bool operator ==(Message_Zipline_Attach left, Message_Zipline_Attach right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Zipline_Attach left, Message_Zipline_Attach right)
	{
		return !object.Equals(left, right);
	}

	public Message_Zipline_Attach()
	{
	}

	public Message_Zipline_Attach(Messages type, MonoBehaviour_Network behaviour, MeshPathConnection pathConnection, int modelIndex = 1)
		: base(type, behaviour)
	{
		if (pathConnection != null)
		{
			rgdMeshPath = new RGD_MeshPath(pathConnection);
		}
		this.modelIndex = modelIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdMeshPath.SerializeFast(writer);
		writer.WriteValueSafe(in modelIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdMeshPath = new RGD_MeshPath();
		rgdMeshPath.DeserializeFast(reader);
		reader.ReadValue(out modelIndex, default(FastBufferWriter.ForPrimitives));
	}
}
