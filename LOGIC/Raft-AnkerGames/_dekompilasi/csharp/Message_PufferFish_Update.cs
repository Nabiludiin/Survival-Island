using System;
using Unity.Netcode;

[Serializable]
public class Message_PufferFish_Update : Message_Animal_Update
{
	public bool d;

	public bool e;

	public bool Anim_Dead
	{
		get
		{
			return d;
		}
		set
		{
			d = value;
		}
	}

	public bool Anim_Explode
	{
		get
		{
			return e;
		}
		set
		{
			e = value;
		}
	}

	protected bool Equals(Message_PufferFish_Update other)
	{
		if (Equals((Message_Animal_Update)other) && d == other.d)
		{
			return e == other.e;
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
		return Equals((Message_PufferFish_Update)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), d, e);
	}

	public static bool operator ==(Message_PufferFish_Update left, Message_PufferFish_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PufferFish_Update left, Message_PufferFish_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_PufferFish_Update()
	{
	}

	public Message_PufferFish_Update(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, AI_StateMachine_PufferFish pufferFish)
		: base(type, behaviour, networkID_ObjectIndex, pufferFish)
	{
		Anim_Dead = pufferFish.animController.GetBool("Dead");
		Anim_Explode = pufferFish.animController.GetBool("Explode");
	}

	public override void OnDeserialize(AI_StateMachine_Animal stateMachineAnimal)
	{
		base.OnDeserialize(stateMachineAnimal);
		stateMachineAnimal.animController.SetBool("Dead", Anim_Dead);
		stateMachineAnimal.animController.SetBool("Explode", Anim_Explode);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in d, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in e, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out d, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out e, default(FastBufferWriter.ForPrimitives));
	}
}
