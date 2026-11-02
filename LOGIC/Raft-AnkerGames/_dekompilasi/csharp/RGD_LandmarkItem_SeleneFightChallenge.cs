using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_SeleneFightChallenge : RGD_LandmarkItem
{
	public bool complete;

	public bool firstInteractionCompleted;

	public float holdProgress;

	protected bool Equals(RGD_LandmarkItem_SeleneFightChallenge other)
	{
		if (Equals((RGD_LandmarkItem)other) && complete == other.complete && firstInteractionCompleted == other.firstInteractionCompleted)
		{
			return holdProgress.Equals(other.holdProgress);
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
		return Equals((RGD_LandmarkItem_SeleneFightChallenge)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), complete, firstInteractionCompleted, holdProgress);
	}

	public static bool operator ==(RGD_LandmarkItem_SeleneFightChallenge left, RGD_LandmarkItem_SeleneFightChallenge right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_SeleneFightChallenge left, RGD_LandmarkItem_SeleneFightChallenge right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in complete, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in firstInteractionCompleted, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in holdProgress, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out complete, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out firstInteractionCompleted, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out holdProgress, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_SeleneFightChallenge()
	{
	}

	public RGD_LandmarkItem_SeleneFightChallenge(LandmarkItem landmarkItem, Fight_Challenge_Selene fightChallenge)
		: base(landmarkItem)
	{
		complete = fightChallenge.isCompleted;
		firstInteractionCompleted = !fightChallenge.isFirstInteraction;
		holdProgress = fightChallenge.HoldProgress;
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		LandmarkItem_FightChallenge landmarkItem_FightChallenge = landmarkItem as LandmarkItem_FightChallenge;
		Fight_Challenge_Selene fight_Challenge_Selene = null;
		if (landmarkItem_FightChallenge != null)
		{
			fight_Challenge_Selene = landmarkItem_FightChallenge.fightChallenge;
		}
		else
		{
			Debug.LogError("unable to cast landmark item to landmarkitem_fightchallenge");
		}
		if (fight_Challenge_Selene != null)
		{
			fight_Challenge_Selene.RestoreRGDSave(this);
		}
		else
		{
			Debug.LogError("Can't restore RGD_LandmarkItem_FightChallenge, could not find script next to landmarkitem");
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Complete", complete);
		info.AddValue("FirstInteractionComplete", firstInteractionCompleted);
		info.AddValue("HoldProgress", holdProgress);
	}

	public RGD_LandmarkItem_SeleneFightChallenge(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Complete":
				complete = Helper.ObjToValue<bool>(item.Value);
				break;
			case "FirstInteractionComplete":
				firstInteractionCompleted = Helper.ObjToValue<bool>(item.Value);
				break;
			case "HoldProgress":
				holdProgress = Helper.ObjToValue<float>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		complete = false;
		firstInteractionCompleted = false;
		holdProgress = 0f;
	}
}
