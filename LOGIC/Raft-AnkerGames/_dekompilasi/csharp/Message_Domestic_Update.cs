using System;
using Unity.Netcode;

[Serializable]
public class Message_Domestic_Update : Message_Animal_Update
{
	public bool p;

	public bool a;

	public bool ParentedToRaft
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

	public bool Anim_Starving
	{
		get
		{
			return a;
		}
		set
		{
			a = value;
		}
	}

	protected bool Equals(Message_Domestic_Update other)
	{
		if (Equals((Message_Animal_Update)other) && p == other.p)
		{
			return a == other.a;
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
		return Equals((Message_Domestic_Update)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), p, a);
	}

	public static bool operator ==(Message_Domestic_Update left, Message_Domestic_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Domestic_Update left, Message_Domestic_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_Domestic_Update()
	{
	}

	public Message_Domestic_Update(Messages type, MonoBehaviour_Network behaviour, AI_StateMachine_Animal stateMachine, uint networkID_ObjectIndex)
		: base(type, behaviour, networkID_ObjectIndex, stateMachine)
	{
		ParentedToRaft = stateMachine.transform.ParentedToRaft();
		if (ParentedToRaft)
		{
			Position = new SerializableVector3(stateMachine.transform.localPosition);
		}
		else
		{
			Position = new SerializableVector3(stateMachine.transform.position);
		}
		Anim_Starving = stateMachine.animController.GetBool("Starving");
	}

	public override void OnDeserialize(AI_StateMachine_Animal stateMachineAnimal)
	{
		base.OnDeserialize(stateMachineAnimal);
		stateMachineAnimal.animController.SetBool("Starving", Anim_Starving);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in p, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in a, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out p, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out a, default(FastBufferWriter.ForPrimitives));
	}
}
