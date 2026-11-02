using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_WaterWell : RGD_LandmarkItem
{
	[SerializeField]
	private float fillAmount;

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in fillAmount, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out fillAmount, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkItem_WaterWell()
	{
	}

	public RGD_LandmarkItem_WaterWell(LandmarkItem landmarkItem, Landmark_WaterWell waterWell)
		: base(landmarkItem)
	{
		fillAmount = waterWell.fillCounter.GetCurrentAmount();
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		LandmarkItemWaterWell landmarkItemWaterWell = landmarkItem as LandmarkItemWaterWell;
		if (landmarkItemWaterWell != null)
		{
			landmarkItemWaterWell.waterWell.fillCounter.IncrementLocal(fillAmount);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("FillAmount", fillAmount);
	}

	public RGD_LandmarkItem_WaterWell(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "FillAmount")
			{
				fillAmount = (float)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		fillAmount = 0f;
	}
}
