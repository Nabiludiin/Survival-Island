using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Shark_State_BiteRaft : RGD_AI_State
{
	public uint targetBlockIndex;

	public bool isBitingRaft;

	protected bool Equals(RGD_Shark_State_BiteRaft other)
	{
		if (Equals((RGD_AI_State)other) && targetBlockIndex == other.targetBlockIndex)
		{
			return isBitingRaft == other.isBitingRaft;
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
		return Equals((RGD_Shark_State_BiteRaft)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), targetBlockIndex, isBitingRaft);
	}

	public static bool operator ==(RGD_Shark_State_BiteRaft left, RGD_Shark_State_BiteRaft right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Shark_State_BiteRaft left, RGD_Shark_State_BiteRaft right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in targetBlockIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in isBitingRaft, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out targetBlockIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out isBitingRaft, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Shark_State_BiteRaft()
	{
	}

	public RGD_Shark_State_BiteRaft(AI_State_Attack_Block_Shark state)
		: base(state)
	{
		if (state.currentTargetBlock != null)
		{
			targetBlockIndex = state.currentTargetBlock.ObjectIndex;
		}
		isBitingRaft = state.isBitingRaft;
	}

	public override void Restore(AI_State state)
	{
		base.Restore(state);
		AI_State_Attack_Block_Shark aI_State_Attack_Block_Shark = state as AI_State_Attack_Block_Shark;
		if (aI_State_Attack_Block_Shark != null)
		{
			aI_State_Attack_Block_Shark.isBitingRaft = isBitingRaft;
			if (aI_State_Attack_Block_Shark.isBitingRaft && targetBlockIndex >= 0)
			{
				aI_State_Attack_Block_Shark.currentTargetBlock = BlockCreator.GetBlockByObjectIndex(targetBlockIndex);
			}
			if (aI_State_Attack_Block_Shark.currentTargetBlock != null)
			{
				aI_State_Attack_Block_Shark.foundBlockToAttack = true;
				aI_State_Attack_Block_Shark.InitiateAttackOnBlock(aI_State_Attack_Block_Shark.currentTargetBlock);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("TargetBlock", targetBlockIndex);
		info.AddValue("IsBitingRaft", isBitingRaft);
	}

	public RGD_Shark_State_BiteRaft(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "TargetBlock"))
			{
				if (name == "IsBitingRaft")
				{
					isBitingRaft = Helper.ObjToValue<bool>(item.Value);
				}
			}
			else
			{
				targetBlockIndex = Helper.ObjToValue<uint>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		targetBlockIndex = 0u;
		isBitingRaft = false;
	}
}
