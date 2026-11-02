using System;
using Unity.Netcode;

[Serializable]
public class Message_BedManager_Slumber : Message_NetworkBehaviour
{
	public bool raftIsAnchored;

	protected bool Equals(Message_BedManager_Slumber other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return raftIsAnchored == other.raftIsAnchored;
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
		return Equals((Message_BedManager_Slumber)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), raftIsAnchored);
	}

	public static bool operator ==(Message_BedManager_Slumber left, Message_BedManager_Slumber right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BedManager_Slumber left, Message_BedManager_Slumber right)
	{
		return !object.Equals(left, right);
	}

	public Message_BedManager_Slumber()
	{
	}

	public Message_BedManager_Slumber(Messages type, MonoBehaviour_Network behaviour, bool raftIsAnchored)
		: base(type, behaviour)
	{
		this.raftIsAnchored = raftIsAnchored;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in raftIsAnchored, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out raftIsAnchored, default(FastBufferWriter.ForPrimitives));
	}
}
