using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Stonebird_GrabStone : RGD_AI_State
{
	public float grabStoneTimer;

	protected bool Equals(RGD_Stonebird_GrabStone other)
	{
		if (Equals((RGD_AI_State)other))
		{
			return grabStoneTimer.Equals(other.grabStoneTimer);
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
		return Equals((RGD_Stonebird_GrabStone)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), grabStoneTimer);
	}

	public static bool operator ==(RGD_Stonebird_GrabStone left, RGD_Stonebird_GrabStone right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Stonebird_GrabStone left, RGD_Stonebird_GrabStone right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in grabStoneTimer, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out grabStoneTimer, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Stonebird_GrabStone()
	{
	}

	public RGD_Stonebird_GrabStone(AI_State_StoneBird_GrabStone state)
		: base(state)
	{
		grabStoneTimer = state.grabStoneTimer;
	}

	public override void Restore(AI_State state)
	{
		base.Restore(state);
		AI_State_StoneBird_GrabStone aI_State_StoneBird_GrabStone = state as AI_State_StoneBird_GrabStone;
		if (aI_State_StoneBird_GrabStone != null)
		{
			aI_State_StoneBird_GrabStone.grabStoneTimer = grabStoneTimer;
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("GrabStoneTimer", grabStoneTimer);
	}

	public RGD_Stonebird_GrabStone(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "GrabStoneTimer")
			{
				grabStoneTimer = Helper.ObjToValue<float>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		grabStoneTimer = 0f;
	}
}
