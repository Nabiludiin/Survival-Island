using System;
using Unity.Netcode;

[Serializable]
public class Message_StoneBird_Update : Message_Animal_Update
{
	public bool h;

	public new bool s;

	public bool f;

	public bool g;

	public int e;

	public bool HasStone
	{
		get
		{
			return h;
		}
		set
		{
			h = value;
		}
	}

	public bool Anim_HasStone
	{
		get
		{
			return s;
		}
		set
		{
			s = value;
		}
	}

	public bool Anim_Flying
	{
		get
		{
			return f;
		}
		set
		{
			f = value;
		}
	}

	public bool Anim_GrabbingStone
	{
		get
		{
			return g;
		}
		set
		{
			g = value;
		}
	}

	public float Anim_FlySpeed
	{
		get
		{
			return (float)e / 100f;
		}
		set
		{
			e = (int)(value * 100f);
		}
	}

	protected bool Equals(Message_StoneBird_Update other)
	{
		if (Equals((Message_Animal_Update)other) && h == other.h && s == other.s && f == other.f && g == other.g)
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
		return Equals((Message_StoneBird_Update)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), h, s, f, g, e);
	}

	public static bool operator ==(Message_StoneBird_Update left, Message_StoneBird_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_StoneBird_Update left, Message_StoneBird_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_StoneBird_Update()
	{
	}

	public Message_StoneBird_Update(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, AI_StateMachine_StoneBird stoneBird)
		: base(type, behaviour, networkID_ObjectIndex, stoneBird)
	{
		HasStone = stoneBird.dropStoneState.HasStone;
		Anim_HasStone = stoneBird.animController.GetBool("HasStone");
		Anim_Flying = stoneBird.animController.GetBool("Flying");
		Anim_GrabbingStone = stoneBird.animController.GetBool("GrabbingStone");
		Anim_FlySpeed = stoneBird.animController.GetFloat("FlySpeed");
	}

	public override void OnDeserialize(AI_StateMachine_Animal stateMachineAnimal)
	{
		base.OnDeserialize(stateMachineAnimal);
		AI_StateMachine_StoneBird obj = stateMachineAnimal as AI_StateMachine_StoneBird;
		obj.dropStoneState.SetStoneModelState(HasStone);
		obj.animController.SetBool("HasStone", Anim_HasStone);
		obj.animController.SetBool("Flying", Anim_Flying);
		obj.animController.SetBool("GrabbingStone", Anim_GrabbingStone);
		obj.animController.SetFloat("FlySpeed", Anim_FlySpeed);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(8))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in h, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in s, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in f, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in g, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in e, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out h, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out s, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out f, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out g, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out e, default(FastBufferWriter.ForPrimitives));
	}
}
