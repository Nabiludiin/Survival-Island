using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_SoundManager_PlayOneShot : Message_NetworkBehaviour_ID
{
	[SerializeField]
	protected string s;

	public int x;

	public int y;

	public int z;

	public string EventRef => s;

	public Vector3 Position
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

	protected bool Equals(Message_SoundManager_PlayOneShot other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && s == other.s && x == other.x && y == other.y)
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
		return Equals((Message_SoundManager_PlayOneShot)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), s, x, y, z);
	}

	public static bool operator ==(Message_SoundManager_PlayOneShot left, Message_SoundManager_PlayOneShot right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_SoundManager_PlayOneShot left, Message_SoundManager_PlayOneShot right)
	{
		return !object.Equals(left, right);
	}

	public Message_SoundManager_PlayOneShot()
	{
	}

	public Message_SoundManager_PlayOneShot(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, string eventRef, Vector3 position)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		s = eventRef;
		Position = position;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(s);
		if (!writer.TryBeginWrite(12))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out s, false);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
	}
}
