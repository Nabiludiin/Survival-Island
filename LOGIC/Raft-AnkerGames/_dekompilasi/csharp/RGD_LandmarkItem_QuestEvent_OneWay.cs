using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_LandmarkItem_QuestEvent_OneWay : RGD_LandmarkItem_QuestEvent
{
	public bool hasBeenOpened;

	protected bool Equals(RGD_LandmarkItem_QuestEvent_OneWay other)
	{
		if (Equals((RGD_LandmarkItem_QuestEvent)other))
		{
			return hasBeenOpened == other.hasBeenOpened;
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
		return Equals((RGD_LandmarkItem_QuestEvent_OneWay)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), hasBeenOpened);
	}

	public static bool operator ==(RGD_LandmarkItem_QuestEvent_OneWay left, RGD_LandmarkItem_QuestEvent_OneWay right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_QuestEvent_OneWay left, RGD_LandmarkItem_QuestEvent_OneWay right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in hasBeenOpened, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out hasBeenOpened, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_QuestEvent_OneWay()
	{
	}

	public RGD_LandmarkItem_QuestEvent_OneWay(LandmarkItem_Quest_OneWay oneWayInteractable)
		: base(oneWayInteractable)
	{
		hasBeenOpened = oneWayInteractable.oneWayInteractableComponent.hasBeenOpened;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("HasBeenOpened", hasBeenOpened);
	}

	public RGD_LandmarkItem_QuestEvent_OneWay(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "HasBeenOpened")
			{
				hasBeenOpened = Helper.ObjToValue<bool>(item.Value);
			}
		}
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		RestoreOneWayComponent(landmarkItem);
	}

	private void RestoreOneWayComponent(LandmarkItem landmarkItem)
	{
		if (landmarkItem == null)
		{
			return;
		}
		LandmarkItem_Quest_OneWay landmarkItem_Quest_OneWay = landmarkItem as LandmarkItem_Quest_OneWay;
		if (landmarkItem_Quest_OneWay != null && landmarkItem_Quest_OneWay.oneWayInteractableComponent != null)
		{
			landmarkItem_Quest_OneWay.oneWayInteractableComponent.hasBeenOpened = hasBeenOpened;
			if (hasBeenOpened && landmarkItem_Quest_OneWay.oneWayInteractableComponent.CurrentObjectStateIndex != landmarkItem_Quest_OneWay.oneWayInteractableComponent.openDoorIndex)
			{
				int currentObjectStateIndex = landmarkItem_Quest_OneWay.oneWayInteractableComponent.CurrentObjectStateIndex;
				landmarkItem_Quest_OneWay.oneWayInteractableComponent.SetNewObjectStateIndex(landmarkItem_Quest_OneWay.oneWayInteractableComponent.openDoorIndex);
				landmarkItem_Quest_OneWay.oneWayInteractableComponent.SetNewObjectStateIndex(currentObjectStateIndex);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		hasBeenOpened = false;
	}
}
