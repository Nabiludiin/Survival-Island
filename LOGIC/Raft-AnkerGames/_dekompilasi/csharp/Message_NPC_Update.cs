using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_NPC_Update : Message_Animal_Update
{
	[SerializeField]
	private bool le;

	[SerializeField]
	private bool si;

	[SerializeField]
	private bool ta;

	[SerializeField]
	private bool ea;

	public bool Leaning
	{
		get
		{
			return le;
		}
		set
		{
			le = value;
		}
	}

	public bool Sitting
	{
		get
		{
			return si;
		}
		set
		{
			si = value;
		}
	}

	public bool Talking
	{
		get
		{
			return ta;
		}
		set
		{
			ta = value;
		}
	}

	public bool Eating
	{
		get
		{
			return ea;
		}
		set
		{
			ea = value;
		}
	}

	protected bool Equals(Message_NPC_Update other)
	{
		if (Equals((Message_Animal_Update)other) && le == other.le && si == other.si && ta == other.ta)
		{
			return ea == other.ea;
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
		return Equals((Message_NPC_Update)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), le, si, ta, ea);
	}

	public static bool operator ==(Message_NPC_Update left, Message_NPC_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NPC_Update left, Message_NPC_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_NPC_Update()
	{
	}

	public Message_NPC_Update(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, AI_StateMachine_Animal stateMachineAnimal)
		: base(type, behaviour, networkID_ObjectIndex, stateMachineAnimal)
	{
		Leaning = stateMachineAnimal.animController.GetBool("Leaning");
		Sitting = stateMachineAnimal.animController.GetBool("Sitting");
		Talking = stateMachineAnimal.animController.GetBool("Talking");
		Eating = stateMachineAnimal.animController.GetBool("Eating");
	}

	public override void OnDeserialize(AI_StateMachine_Animal stateMachineAnimal)
	{
		base.OnDeserialize(stateMachineAnimal);
		stateMachineAnimal.animController.SetBool("Leaning", Leaning);
		stateMachineAnimal.animController.SetBool("Sitting", Sitting);
		stateMachineAnimal.animController.SetBool("Talking", Talking);
		stateMachineAnimal.animController.SetBool("Eating", Eating);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(4))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in le, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in si, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in ta, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in ea, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out le, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out si, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out ta, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out ea, default(FastBufferWriter.ForPrimitives));
	}
}
