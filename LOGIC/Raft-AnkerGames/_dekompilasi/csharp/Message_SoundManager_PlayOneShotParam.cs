using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_SoundManager_PlayOneShotParam : Message_SoundManager_PlayOneShot
{
	[SerializeField]
	private new string n;

	[SerializeField]
	private float f;

	public string ParamName => n;

	public float ParamValue => f;

	protected bool Equals(Message_SoundManager_PlayOneShotParam other)
	{
		if (Equals((Message_SoundManager_PlayOneShot)other) && n == other.n)
		{
			return f.Equals(other.f);
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
		return Equals((Message_SoundManager_PlayOneShotParam)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), n, f);
	}

	public static bool operator ==(Message_SoundManager_PlayOneShotParam left, Message_SoundManager_PlayOneShotParam right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_SoundManager_PlayOneShotParam left, Message_SoundManager_PlayOneShotParam right)
	{
		return !object.Equals(left, right);
	}

	public Message_SoundManager_PlayOneShotParam()
	{
	}

	public Message_SoundManager_PlayOneShotParam(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, string eventRef, Vector3 position, string paramName, float paramValue)
		: base(type, behaviour, networkID_ObjectIndex, eventRef, position)
	{
		s = eventRef;
		Position = position;
		n = paramName;
		f = paramValue;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(n);
		writer.WriteValueSafe(in f, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out n, false);
		reader.ReadValue(out f, default(FastBufferWriter.ForPrimitives));
	}
}
