using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_ChallengePipe : RGD_LandmarkItem
{
	[SerializeField]
	private RGD_RotationComponent rgd_rotationComponent;

	protected bool Equals(RGD_LandmarkItem_ChallengePipe other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return object.Equals(rgd_rotationComponent, other.rgd_rotationComponent);
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
		return Equals((RGD_LandmarkItem_ChallengePipe)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgd_rotationComponent);
	}

	public static bool operator ==(RGD_LandmarkItem_ChallengePipe left, RGD_LandmarkItem_ChallengePipe right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_ChallengePipe left, RGD_LandmarkItem_ChallengePipe right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<bool>(rgd_rotationComponent != null, default(FastBufferWriter.ForPrimitives));
		if (rgd_rotationComponent != null)
		{
			rgd_rotationComponent.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out bool value, default(FastBufferWriter.ForPrimitives));
		if (value)
		{
			rgd_rotationComponent = new RGD_RotationComponent();
			rgd_rotationComponent.DeserializeFast(reader);
		}
	}

	public RGD_LandmarkItem_ChallengePipe()
	{
	}

	public RGD_LandmarkItem_ChallengePipe(RGDType type, RGDSortingOrder sortingOrder, LandmarkItem_ChallengePipe challengePipe)
		: base(challengePipe)
	{
		if (challengePipe != null && challengePipe.challengePipe.rotationComponent != null)
		{
			rgd_rotationComponent = challengePipe.challengePipe.rotationComponent.GetSaveData();
		}
	}

	public RGD_LandmarkItem_ChallengePipe(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RotationComponent")
			{
				rgd_rotationComponent = (RGD_RotationComponent)item.Value;
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RotationComponent", rgd_rotationComponent);
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		RestoreChallengePipe(landmarkItem);
	}

	private void RestoreChallengePipe(LandmarkItem landmarkItem)
	{
		if (landmarkItem != null)
		{
			LandmarkItem_ChallengePipe landmarkItem_ChallengePipe = landmarkItem as LandmarkItem_ChallengePipe;
			if (landmarkItem_ChallengePipe != null)
			{
				rgd_rotationComponent.RestoreRotational(landmarkItem_ChallengePipe.challengePipe.rotationComponent);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgd_rotationComponent = null;
	}
}
