using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_WaterLoggedHatch : RGD_LandmarkItem_QuestEvent
{
	[SerializeField]
	private float normalizedFill;

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in normalizedFill, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out normalizedFill, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_WaterLoggedHatch()
	{
	}

	public RGD_LandmarkItem_WaterLoggedHatch(LandmarkItem landmarkItem, LandmarkItem_WaterLoggedHatch waterHatch)
		: base(landmarkItem)
	{
		normalizedFill = waterHatch.fillComponent.CurrentNormalizedValue;
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		LandmarkItem_WaterLoggedHatch landmarkItem_WaterLoggedHatch = landmarkItem as LandmarkItem_WaterLoggedHatch;
		if (landmarkItem_WaterLoggedHatch != null)
		{
			if (normalizedFill > 0f)
			{
				landmarkItem_WaterLoggedHatch.HatchInteract();
			}
			landmarkItem_WaterLoggedHatch.fillComponent.SetNormalizedValue(normalizedFill, instant: true);
			if (normalizedFill >= 1f)
			{
				landmarkItem_WaterLoggedHatch.HardStopEffectsViaRestore();
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("NormalizedFill", normalizedFill);
	}

	public RGD_LandmarkItem_WaterLoggedHatch(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "NormalizedFill")
			{
				normalizedFill = (float)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		normalizedFill = 0f;
	}
}
