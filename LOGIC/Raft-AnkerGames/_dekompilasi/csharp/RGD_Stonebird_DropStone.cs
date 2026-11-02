using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Stonebird_DropStone : RGD_AI_State
{
	public bool hasStone;

	public float dropStoneTimer;

	protected bool Equals(RGD_Stonebird_DropStone other)
	{
		if (Equals((RGD_AI_State)other) && hasStone == other.hasStone)
		{
			return dropStoneTimer.Equals(other.dropStoneTimer);
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
		return Equals((RGD_Stonebird_DropStone)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), hasStone, dropStoneTimer);
	}

	public static bool operator ==(RGD_Stonebird_DropStone left, RGD_Stonebird_DropStone right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Stonebird_DropStone left, RGD_Stonebird_DropStone right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in hasStone, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in dropStoneTimer, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out hasStone, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out dropStoneTimer, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Stonebird_DropStone()
	{
	}

	public RGD_Stonebird_DropStone(AI_State_StoneBird_DropStone state)
		: base(state)
	{
		hasStone = state.HasStone;
		dropStoneTimer = state.DropStoneTimer;
	}

	public override void Restore(AI_State state)
	{
		base.Restore(state);
		AI_State_StoneBird_DropStone aI_State_StoneBird_DropStone = state as AI_State_StoneBird_DropStone;
		if (aI_State_StoneBird_DropStone != null)
		{
			aI_State_StoneBird_DropStone.SetStoneModelState(hasStone);
			aI_State_StoneBird_DropStone.DropStoneTimer = dropStoneTimer;
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("HasStone", hasStone);
		info.AddValue("DropStoneTimer", dropStoneTimer);
	}

	public RGD_Stonebird_DropStone(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "HasStone"))
			{
				if (name == "DropStoneTimer")
				{
					dropStoneTimer = Helper.ObjToValue<float>(item.Value);
				}
			}
			else
			{
				hasStone = Helper.ObjToValue<bool>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		hasStone = false;
		dropStoneTimer = 0f;
	}
}
