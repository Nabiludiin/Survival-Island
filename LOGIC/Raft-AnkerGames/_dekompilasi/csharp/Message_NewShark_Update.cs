using System;
using Unity.Netcode;

[Serializable]
public class Message_NewShark_Update : Message_Animal_Update
{
	public new bool b;

	public bool p;

	public bool Biting
	{
		get
		{
			return b;
		}
		set
		{
			b = value;
		}
	}

	public bool IsParentedToRaft
	{
		get
		{
			return p;
		}
		set
		{
			p = value;
		}
	}

	protected bool Equals(Message_NewShark_Update other)
	{
		if (Equals((Message_Animal_Update)other) && b == other.b)
		{
			return p == other.p;
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
		return Equals((Message_NewShark_Update)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), b, p);
	}

	public static bool operator ==(Message_NewShark_Update left, Message_NewShark_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NewShark_Update left, Message_NewShark_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_NewShark_Update()
	{
	}

	public Message_NewShark_Update(Messages type, MonoBehaviour_Network behaviour, uint objectIndex, AI_StateMachine_Animal stateMachine, bool isBiting)
		: base(type, behaviour, objectIndex, stateMachine)
	{
		Biting = isBiting;
		IsParentedToRaft = stateMachine.transform.ParentedToRaft();
		if (IsParentedToRaft)
		{
			Position = new SerializableVector3(stateMachine.transform.localPosition);
		}
		else
		{
			Position = new SerializableVector3(stateMachine.transform.position);
		}
		Euler = new SerializableVector3(stateMachine.transform.rotation.eulerAngles);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in b, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in p, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out b, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out p, default(FastBufferWriter.ForPrimitives));
	}
}
