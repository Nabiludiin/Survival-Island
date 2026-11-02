using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Animal_AnimTriggers : Message_NetworkBehaviour_ID
{
	public string[] anim_triggers;

	protected bool Equals(Message_Animal_AnimTriggers other)
	{
		if (anim_triggers == null && other.anim_triggers != null)
		{
			return false;
		}
		if (anim_triggers != null && other.anim_triggers == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			if (anim_triggers != null || other.anim_triggers != null)
			{
				return Enumerable.SequenceEqual(anim_triggers, other.anim_triggers);
			}
			return true;
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
		return Equals((Message_Animal_AnimTriggers)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), anim_triggers);
	}

	public static bool operator ==(Message_Animal_AnimTriggers left, Message_Animal_AnimTriggers right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Animal_AnimTriggers left, Message_Animal_AnimTriggers right)
	{
		return !object.Equals(left, right);
	}

	public Message_Animal_AnimTriggers()
	{
	}

	public Message_Animal_AnimTriggers(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, string[] p_anim_triggers)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		anim_triggers = p_anim_triggers;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(anim_triggers.Length, default(FastBufferWriter.ForPrimitives));
		string[] array = anim_triggers;
		foreach (string s in array)
		{
			writer.WriteValueSafe(s);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		anim_triggers = new string[value];
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out anim_triggers[i], false);
		}
	}
}
