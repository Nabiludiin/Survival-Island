using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_Keypad : RGD_LandmarkItem
{
	public RGD_Keypad rgdKeypad;

	protected bool Equals(RGD_LandmarkItem_Keypad other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return object.Equals(rgdKeypad, other.rgdKeypad);
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
		return Equals((RGD_LandmarkItem_Keypad)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdKeypad);
	}

	public static bool operator ==(RGD_LandmarkItem_Keypad left, RGD_LandmarkItem_Keypad right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_Keypad left, RGD_LandmarkItem_Keypad right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdKeypad.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdKeypad = new RGD_Keypad();
		rgdKeypad.DeserializeFast(reader);
	}

	public RGD_LandmarkItem_Keypad()
	{
	}

	public RGD_LandmarkItem_Keypad(LandmarkItem landmarkItem)
		: base(landmarkItem)
	{
		Keypad component = landmarkItem.GetComponent<Keypad>();
		if (component != null)
		{
			rgdKeypad = new RGD_Keypad(component);
		}
		else
		{
			Debug.LogError("Can't save keypad, could not find Keypad script next to landmarkitem " + landmarkItem.name);
		}
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		Keypad component = landmarkItem.GetComponent<Keypad>();
		if (component != null && rgdKeypad != null)
		{
			rgdKeypad.RestoreKeypad(component);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDKeypad", rgdKeypad);
	}

	public RGD_LandmarkItem_Keypad(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDKeypad")
			{
				rgdKeypad = (RGD_Keypad)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdKeypad = null;
	}
}
