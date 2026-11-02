using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_MeshPath : SerializableBehaviour
{
	public uint baseAObjectIndex;

	public uint baseBObjectIndex;

	public float slack;

	protected bool Equals(RGD_MeshPath other)
	{
		if (baseAObjectIndex == other.baseAObjectIndex && baseBObjectIndex == other.baseBObjectIndex)
		{
			return slack.Equals(other.slack);
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
		return Equals((RGD_MeshPath)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(baseAObjectIndex, baseBObjectIndex, slack);
	}

	public static bool operator ==(RGD_MeshPath left, RGD_MeshPath right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_MeshPath left, RGD_MeshPath right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(12))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in baseAObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in baseBObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in slack, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out baseAObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out baseBObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out slack, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_MeshPath()
	{
	}

	public RGD_MeshPath(MeshPathConnection ziplineConnection)
	{
		baseAObjectIndex = ziplineConnection.baseA.ObjectIndex;
		baseBObjectIndex = ziplineConnection.baseB.ObjectIndex;
		slack = ziplineConnection.path.currentSlack;
	}

	public RGD_MeshPath(MeshPathBase baseA, MeshPathBase baseB, float slack)
	{
		baseAObjectIndex = baseA.ObjectIndex;
		baseBObjectIndex = baseB.ObjectIndex;
		this.slack = slack;
	}

	public bool Restore(bool replicating)
	{
		if (baseAObjectIndex != 0 && baseBObjectIndex != 0)
		{
			MeshPathBase networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex<MeshPathBase>(baseAObjectIndex);
			MeshPathBase networkIDFromObjectIndex2 = NetworkIDManager.GetNetworkIDFromObjectIndex<MeshPathBase>(baseBObjectIndex);
			if (networkIDFromObjectIndex != null && networkIDFromObjectIndex2 != null)
			{
				return MeshPath.CreateMeshPath(networkIDFromObjectIndex, networkIDFromObjectIndex2, slack, replicating);
			}
		}
		return false;
	}

	public MeshPathConnection GetMeshPathConnection()
	{
		MeshPathBase networkIDFromObjectIndex = NetworkIDManager.GetNetworkIDFromObjectIndex<MeshPathBase>(baseAObjectIndex);
		MeshPathBase networkIDFromObjectIndex2 = NetworkIDManager.GetNetworkIDFromObjectIndex<MeshPathBase>(baseBObjectIndex);
		if (networkIDFromObjectIndex == null || networkIDFromObjectIndex2 == null)
		{
			return null;
		}
		return MeshPath.GetConnection(networkIDFromObjectIndex, networkIDFromObjectIndex2);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("BaseAObjectIndex", baseAObjectIndex);
		info.AddValue("BaseBObjectIndex", baseBObjectIndex);
		info.AddValue("Slack", slack);
	}

	public RGD_MeshPath(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "BaseAObjectIndex":
				baseAObjectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "BaseBObjectIndex":
				baseBObjectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "Slack":
				slack = Helper.ObjToValue<float>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		baseAObjectIndex = (baseBObjectIndex = 0u);
		slack = -0.5f;
	}
}
