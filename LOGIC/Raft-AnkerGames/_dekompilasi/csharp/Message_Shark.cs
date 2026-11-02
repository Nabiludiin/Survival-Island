using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Shark : Message_NetworkBehaviour
{
	[SerializeField]
	private float posX;

	[SerializeField]
	private float posY;

	[SerializeField]
	private float posZ;

	[SerializeField]
	private float rotX;

	[SerializeField]
	private float rotY;

	[SerializeField]
	private float rotZ;

	public bool bitingBait;

	public bool anim_BitingRaft;

	public string[] anim_triggers;

	public Vector3 Position
	{
		get
		{
			return new Vector3(posX, posY, posZ);
		}
		private set
		{
			posX = value.x;
			posY = value.y;
			posZ = value.z;
		}
	}

	public Vector3 ForwardDirection
	{
		get
		{
			return new Vector3(rotX, rotY, rotZ);
		}
		private set
		{
			rotX = value.x;
			rotY = value.y;
			rotZ = value.z;
		}
	}

	protected bool Equals(Message_Shark other)
	{
		if (Equals((Message_NetworkBehaviour)other) && posX.Equals(other.posX) && posY.Equals(other.posY) && posZ.Equals(other.posZ) && rotX.Equals(other.rotX) && rotY.Equals(other.rotY) && rotZ.Equals(other.rotZ) && bitingBait == other.bitingBait && anim_BitingRaft == other.anim_BitingRaft)
		{
			return object.Equals(anim_triggers, other.anim_triggers);
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
		return Equals((Message_Shark)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(posX);
		hashCode.Add(posY);
		hashCode.Add(posZ);
		hashCode.Add(rotX);
		hashCode.Add(rotY);
		hashCode.Add(rotZ);
		hashCode.Add(bitingBait);
		hashCode.Add(anim_BitingRaft);
		hashCode.Add(anim_triggers);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_Shark left, Message_Shark right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Shark left, Message_Shark right)
	{
		return !object.Equals(left, right);
	}

	public Message_Shark(Messages type, MonoBehaviour_Network behaviour, Shark shark)
		: base(type, behaviour)
	{
		bitingBait = shark.targetBlock != null && shark.targetBlock is SharkBait;
		if (!shark.bitingRaft || bitingBait)
		{
			Position = shark.transform.position;
		}
		else
		{
			Position = shark.transform.localPosition;
		}
		ForwardDirection = shark.transform.forward;
		anim_BitingRaft = shark.animator.GetBool("BitingRaft");
		anim_triggers = shark.networkAnimTriggers.ToArray();
		shark.networkAnimTriggers.Clear();
	}

	public Message_Shark()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(26))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in posX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in rotX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in rotY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in rotZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in bitingBait, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in anim_BitingRaft, default(FastBufferWriter.ForPrimitives));
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
		reader.ReadValue(out posX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out posY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out posZ, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out rotX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out rotY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out rotZ, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out bitingBait, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out anim_BitingRaft, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		anim_triggers = new string[value];
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out anim_triggers[i], false);
		}
	}
}
