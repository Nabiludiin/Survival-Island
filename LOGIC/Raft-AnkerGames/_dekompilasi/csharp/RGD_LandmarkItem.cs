using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem : SerializableBehaviour
{
	public float LocalPositionX;

	public float LocalPositionY;

	public float LocalPositionZ;

	public uint connectedObjectIndex;

	public bool active;

	public Vector3 LocalPosition
	{
		get
		{
			return new Vector3(LocalPositionX, LocalPositionY, LocalPositionZ);
		}
		set
		{
			LocalPositionX = value.x;
			LocalPositionY = value.y;
			LocalPositionZ = value.z;
		}
	}

	protected bool Equals(RGD_LandmarkItem other)
	{
		if (LocalPositionX.Equals(other.LocalPositionX) && LocalPositionY.Equals(other.LocalPositionY) && LocalPositionZ.Equals(other.LocalPositionZ) && connectedObjectIndex == other.connectedObjectIndex)
		{
			return active == other.active;
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
		return Equals((RGD_LandmarkItem)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(LocalPositionX, LocalPositionY, LocalPositionZ, connectedObjectIndex, active);
	}

	public static bool operator ==(RGD_LandmarkItem left, RGD_LandmarkItem right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem left, RGD_LandmarkItem right)
	{
		return !object.Equals(left, right);
	}

	public RGD_LandmarkItem()
	{
	}

	public RGD_LandmarkItem(LandmarkItem landmarkItem)
	{
		if (landmarkItem == null)
		{
			Debug.LogWarning("Pickup was null when trying to save landmark item");
			return;
		}
		if (landmarkItem.connectedBehaviourID == null)
		{
			Debug.LogWarning("Pickup networkID was null when trying to save landmarkitem " + landmarkItem.name);
		}
		else
		{
			connectedObjectIndex = landmarkItem.connectedBehaviourID.ObjectIndex;
		}
		active = landmarkItem.gameObject.activeSelf;
		LocalPosition = landmarkItem.localLandmarkPosition;
	}

	public virtual void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		if (Raft_Network.IsHost && connectedObjectIndex == 0)
		{
			if (landmarkItem.connectedBehaviourID != null)
			{
				landmarkItem.AssignIndexAndTrack(SaveAndLoad.GetUniqueObjectIndex());
			}
		}
		else
		{
			landmarkItem.AssignIndexAndTrack(connectedObjectIndex);
		}
		landmarkItem.gameObject.SetActive(active);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("ObjectIndex", connectedObjectIndex);
		info.AddValue("LocalPositionX", LocalPositionX);
		info.AddValue("LocalPositionY", LocalPositionY);
		info.AddValue("LocalPositionZ", LocalPositionZ);
		info.AddValue("Active", active);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(17))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in connectedObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in LocalPositionX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in LocalPositionY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in LocalPositionZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in active, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out connectedObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out LocalPositionX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out LocalPositionY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out LocalPositionZ, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out active, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "LocalPosition":
				LocalPosition = (SerializableVector3)item.Value;
				break;
			case "LocalPositionX":
				LocalPositionX = Helper.ObjToValue<float>(item.Value);
				break;
			case "LocalPositionY":
				LocalPositionY = Helper.ObjToValue<float>(item.Value);
				break;
			case "LocalPositionZ":
				LocalPositionZ = Helper.ObjToValue<float>(item.Value);
				break;
			case "Index":
			case "ObjectIndex":
				connectedObjectIndex = (uint)item.Value;
				break;
			case "Active":
				active = (bool)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		LocalPosition = Vector3.zero;
		active = true;
		connectedObjectIndex = 0u;
	}
}
