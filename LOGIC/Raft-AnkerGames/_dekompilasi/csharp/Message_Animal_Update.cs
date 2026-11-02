using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Animal_Update : Message_NetworkBehaviour_ID
{
	public int x;

	public int y;

	public int z;

	public int q;

	public int w;

	public int v;

	public sbyte s;

	public float _s;

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

	public Vector3 Euler
	{
		get
		{
			return new Vector3((float)q / 100f, (float)w / 100f, (float)v / 100f);
		}
		set
		{
			q = (int)(value.x * 100f);
			w = (int)(value.y * 100f);
			v = (int)(value.z * 100f);
		}
	}

	public int StateMachineStateIndex
	{
		get
		{
			return s;
		}
		set
		{
			s = (sbyte)value;
		}
	}

	public float Anim_Speed
	{
		get
		{
			return _s;
		}
		set
		{
			_s = value;
		}
	}

	protected bool Equals(Message_Animal_Update other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && x == other.x && y == other.y && z == other.z && q == other.q && w == other.w && v == other.v && s == other.s)
		{
			return _s.Equals(other._s);
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
		return Equals((Message_Animal_Update)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(x);
		hashCode.Add(y);
		hashCode.Add(z);
		hashCode.Add(q);
		hashCode.Add(w);
		hashCode.Add(v);
		hashCode.Add(s);
		hashCode.Add(_s);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_Animal_Update left, Message_Animal_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Animal_Update left, Message_Animal_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_Animal_Update()
	{
	}

	public Message_Animal_Update(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, AI_StateMachine_Animal stateMachineAnimal)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		if (stateMachineAnimal.networkBehaviour.networkEntity.IsDead || stateMachineAnimal.movement.model == null)
		{
			Euler = stateMachineAnimal.transform.eulerAngles;
		}
		else
		{
			Euler = stateMachineAnimal.movement.model.eulerAngles;
		}
		Position = stateMachineAnimal.transform.position;
		StateMachineStateIndex = ((stateMachineAnimal.currentState != null) ? ((int)stateMachineAnimal.currentState.stateIndex) : (-1));
		Anim_Speed = ((stateMachineAnimal.animController != null && stateMachineAnimal.animController.hasSpeedParameter) ? stateMachineAnimal.animController.GetFloat("Speed") : (-1f));
	}

	public virtual void OnDeserialize(AI_StateMachine_Animal stateMachineAnimal)
	{
		if (!(stateMachineAnimal == null))
		{
			if (StateMachineStateIndex != -1 && (stateMachineAnimal.currentState == null || stateMachineAnimal.currentState.stateIndex != StateMachineStateIndex))
			{
				stateMachineAnimal.ChangeState((uint)StateMachineStateIndex);
			}
			if (Anim_Speed >= 0f)
			{
				stateMachineAnimal.animController.SetFloat("Speed", Anim_Speed);
			}
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(29))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in q, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in w, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in v, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in s, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in _s, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out q, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out w, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out v, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out s, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out _s, default(FastBufferWriter.ForPrimitives));
	}
}
