using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_QuestEvent : RGD_LandmarkItem
{
	public int stateIndex;

	protected bool Equals(RGD_LandmarkItem_QuestEvent other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return stateIndex == other.stateIndex;
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
		return Equals((RGD_LandmarkItem_QuestEvent)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), stateIndex);
	}

	public static bool operator ==(RGD_LandmarkItem_QuestEvent left, RGD_LandmarkItem_QuestEvent right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_QuestEvent left, RGD_LandmarkItem_QuestEvent right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in stateIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out stateIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_QuestEvent()
	{
	}

	public RGD_LandmarkItem_QuestEvent(LandmarkItem landmarkItem)
		: base(landmarkItem)
	{
		QuestEventBase component = landmarkItem.connectedBehaviourID.GetComponent<QuestEventBase>();
		stateIndex = component.CurrentObjectStateIndex;
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		QuestEventBase component = landmarkItem.connectedBehaviourID.GetComponent<QuestEventBase>();
		if (component != null)
		{
			component.RestoreObjectStateIndex(stateIndex);
		}
		else
		{
			Debug.LogError("QuestEvent is null, can't restore properly " + landmarkItem.name, landmarkItem);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("StateIndex", stateIndex);
	}

	public RGD_LandmarkItem_QuestEvent(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "StateIndex")
			{
				stateIndex = Helper.ObjToValue<int>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		stateIndex = -1;
	}
}
