using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class SerializableVector3 : ISerializable
{
	[SerializeField]
	public float x;

	[SerializeField]
	public float y;

	[SerializeField]
	public float z;

	public Vector3 Value => new Vector3(float.IsNaN(x) ? 0f : x, float.IsNaN(y) ? 0f : y, float.IsNaN(z) ? 0f : z);

	protected bool Equals(SerializableVector3 other)
	{
		if (x.Equals(other.x) && y.Equals(other.y))
		{
			return z.Equals(other.z);
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
		return Equals((SerializableVector3)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, z);
	}

	public static bool operator ==(SerializableVector3 left, SerializableVector3 right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(SerializableVector3 left, SerializableVector3 right)
	{
		return !object.Equals(left, right);
	}

	public SerializableVector3()
	{
		x = (y = (z = 0f));
	}

	public SerializableVector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public SerializableVector3(Vector3 vector)
	{
		x = vector.x;
		y = vector.y;
		z = vector.z;
	}

	public static implicit operator Vector3(SerializableVector3 rValue)
	{
		return new Vector3(rValue.x, rValue.y, rValue.z);
	}

	public static implicit operator SerializableVector3(Vector3 rValue)
	{
		return new SerializableVector3(rValue.x, rValue.y, rValue.z);
	}

	public virtual void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		info.AddValue("X", x);
		info.AddValue("Y", y);
		info.AddValue("Z", z);
	}

	public SerializableVector3(SerializationInfo info, StreamingContext context)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "X":
				x = (float)item.Value;
				break;
			case "Y":
				y = (float)item.Value;
				break;
			case "Z":
				z = (float)item.Value;
				break;
			}
		}
	}

	protected virtual void SetDefaults(StreamingContext sc)
	{
		x = 0f;
		y = 0f;
		z = 0f;
	}
}
