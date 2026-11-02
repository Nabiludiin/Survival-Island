using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_AI_State : SerializableBehaviour
{
	public uint stateIndex;

	protected bool Equals(RGD_AI_State other)
	{
		return stateIndex == other.stateIndex;
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
		return Equals((RGD_AI_State)obj);
	}

	public override int GetHashCode()
	{
		return (int)stateIndex;
	}

	public static bool operator ==(RGD_AI_State left, RGD_AI_State right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_AI_State left, RGD_AI_State right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in stateIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out stateIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_AI_State()
	{
	}

	public RGD_AI_State(AI_State state)
	{
		stateIndex = state.stateIndex;
	}

	public virtual void Restore(AI_State state)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("StateIndex", stateIndex);
	}

	public RGD_AI_State(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "StateIndex")
			{
				stateIndex = Helper.ObjToValue<uint>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		stateIndex = 0u;
	}
}
