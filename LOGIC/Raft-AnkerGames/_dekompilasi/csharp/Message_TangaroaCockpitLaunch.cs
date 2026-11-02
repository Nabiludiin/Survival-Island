using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_TangaroaCockpitLaunch : Message_NetworkBehaviour_ID
{
	[SerializeField]
	private int x;

	[SerializeField]
	private int z;

	public float TargetLocalX
	{
		get
		{
			return (float)x / 100f;
		}
		set
		{
			x = (int)(value * 100f);
		}
	}

	public float TargetLocalZ
	{
		get
		{
			return (float)z / 100f;
		}
		set
		{
			z = (int)(value * 100f);
		}
	}

	protected bool Equals(Message_TangaroaCockpitLaunch other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && x == other.x)
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
		return Equals((Message_TangaroaCockpitLaunch)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), x, z);
	}

	public static bool operator ==(Message_TangaroaCockpitLaunch left, Message_TangaroaCockpitLaunch right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_TangaroaCockpitLaunch left, Message_TangaroaCockpitLaunch right)
	{
		return !object.Equals(left, right);
	}

	public Message_TangaroaCockpitLaunch()
	{
	}

	public Message_TangaroaCockpitLaunch(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, float targetLocalPositionX, float targetLocalPositionZ)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		TargetLocalX = targetLocalPositionX;
		TargetLocalZ = targetLocalPositionZ;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in z, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
	}
}
