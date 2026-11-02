using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_RotationComponent : SerializableBehaviour
{
	public float currentRotation;

	protected bool Equals(RGD_RotationComponent other)
	{
		return currentRotation.Equals(other.currentRotation);
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
		return Equals((RGD_RotationComponent)obj);
	}

	public override int GetHashCode()
	{
		return currentRotation.GetHashCode();
	}

	public static bool operator ==(RGD_RotationComponent left, RGD_RotationComponent right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_RotationComponent left, RGD_RotationComponent right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in currentRotation, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out currentRotation, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_RotationComponent()
	{
	}

	public RGD_RotationComponent(RotationComponent rotationComponent)
	{
		currentRotation = rotationComponent.CurrentRotation;
	}

	public void RestoreRotational(RotationComponent rotationComponent)
	{
		if (rotationComponent != null)
		{
			if (rotationComponent.rotationTransform != null)
			{
				rotationComponent.SetRotation(currentRotation);
			}
			else
			{
				Debug.Log("Could not restore rotationComponent because rotationTransform was null on " + rotationComponent.gameObject.name);
			}
		}
		else
		{
			Debug.LogError("Could not restore rotationComponent because it was null");
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CurrentRotation", currentRotation);
	}

	public RGD_RotationComponent(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "CurrentRotation")
			{
				currentRotation = Helper.ObjToValue<float>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		currentRotation = 0f;
	}
}
