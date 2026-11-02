using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Landmark_Raft : RGD_Landmark
{
	public bool sinking;

	public float yPosition;

	protected bool Equals(RGD_Landmark_Raft other)
	{
		if (Equals((RGD_Landmark)other) && sinking == other.sinking)
		{
			return yPosition.Equals(other.yPosition);
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
		return Equals((RGD_Landmark_Raft)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), sinking, yPosition);
	}

	public static bool operator ==(RGD_Landmark_Raft left, RGD_Landmark_Raft right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Landmark_Raft left, RGD_Landmark_Raft right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in sinking, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in yPosition, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out sinking, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yPosition, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Landmark_Raft()
	{
	}

	public RGD_Landmark_Raft(RaftLandmark raftLandmark, Landmark landmark)
		: base(landmark)
	{
		sinking = raftLandmark.sinkScript.Sinking;
		yPosition = landmark.transform.position.y;
	}

	public override void RestoreLandmark(Landmark landmark, bool isHost)
	{
		base.RestoreLandmark(landmark, isHost);
		if (sinking)
		{
			RaftLandmark component = landmark.GetComponent<RaftLandmark>();
			component.StartSinking(playParticles: false);
			Vector3 position = component.transform.position;
			position.y = yPosition;
			component.transform.position = position;
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Sinking", sinking);
		info.AddValue("YPosition", yPosition);
	}

	public RGD_Landmark_Raft(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "Sinking"))
			{
				if (name == "YPosition")
				{
					yPosition = (float)item.Value;
				}
			}
			else
			{
				sinking = (bool)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		sinking = false;
		yPosition = 0f;
	}
}
