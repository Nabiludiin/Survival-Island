using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_ClawChallengeLiftableObject : SerializableBehaviour
{
	public int uniqueIndex;

	public int x;

	public int y;

	public int z;

	public Vector3 LocalPosition
	{
		get
		{
			return new Vector3((float)x / 100f, (float)y / 100f, (float)z / 100f);
		}
		set
		{
			x = (int)(value.x * 100f);
			y = (int)(value.y * 100f);
			z = (int)(value.z * 100f);
		}
	}

	protected bool Equals(RGD_ClawChallengeLiftableObject other)
	{
		if (uniqueIndex == other.uniqueIndex && x == other.x && y == other.y)
		{
			return z == other.z;
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
		return Equals((RGD_ClawChallengeLiftableObject)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(uniqueIndex, x, y, z);
	}

	public static bool operator ==(RGD_ClawChallengeLiftableObject left, RGD_ClawChallengeLiftableObject right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_ClawChallengeLiftableObject left, RGD_ClawChallengeLiftableObject right)
	{
		return !object.Equals(left, right);
	}

	public RGD_ClawChallengeLiftableObject()
	{
	}

	public RGD_ClawChallengeLiftableObject(int uniqueIndex, Vector3 localPosition)
	{
		this.uniqueIndex = uniqueIndex;
		LocalPosition = localPosition;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("X", x);
		info.AddValue("Y", y);
		info.AddValue("Z", z);
		info.AddValue("Index", uniqueIndex);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in uniqueIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in z, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out uniqueIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_ClawChallengeLiftableObject(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "X":
				x = Helper.ObjToValue<int>(item.Value);
				break;
			case "Y":
				y = Helper.ObjToValue<int>(item.Value);
				break;
			case "Z":
				z = Helper.ObjToValue<int>(item.Value);
				break;
			case "Index":
				uniqueIndex = Helper.ObjToValue<int>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		uniqueIndex = -1;
		x = (y = (z = 0));
	}
}
