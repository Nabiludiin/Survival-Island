using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_AI_Revive : RGD_AI_State
{
	public bool isPickupAvailable;

	public float reviveProgress;

	protected bool Equals(RGD_AI_Revive other)
	{
		if (Equals((RGD_AI_State)other) && isPickupAvailable == other.isPickupAvailable)
		{
			return reviveProgress.Equals(other.reviveProgress);
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
		return Equals((RGD_AI_Revive)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), isPickupAvailable, reviveProgress);
	}

	public static bool operator ==(RGD_AI_Revive left, RGD_AI_Revive right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_AI_Revive left, RGD_AI_Revive right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in isPickupAvailable, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in reviveProgress, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out isPickupAvailable, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out reviveProgress, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_AI_Revive()
	{
	}

	public RGD_AI_Revive(AI_State_Revive state)
		: base(state)
	{
		isPickupAvailable = state.pickupColliderToReset != null && state.pickupColliderToReset.enabled && state.pickupColliderToReset.gameObject.activeInHierarchy;
		TimerEventer reviveTimer = state.reviveTimer;
		if (reviveTimer != null)
		{
			reviveProgress = reviveTimer.GetProgress();
		}
	}

	public override void Restore(AI_State state)
	{
		base.Restore(state);
		AI_State_Revive aI_State_Revive = state as AI_State_Revive;
		if (aI_State_Revive != null)
		{
			aI_State_Revive.pickupColliderToReset.enabled = isPickupAvailable;
			aI_State_Revive.pickupColliderToReset.gameObject.SetActiveSafe(isPickupAvailable);
		}
		aI_State_Revive.reviveTimer.SetProgress(reviveProgress);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("IsPickupAvailable", isPickupAvailable);
		info.AddValue("ReviveProgress", reviveProgress);
	}

	public RGD_AI_Revive(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "IsPickupAvailable"))
			{
				if (name == "ReviveProgress")
				{
					reviveProgress = Helper.ObjToValue<float>(item.Value);
				}
			}
			else
			{
				isPickupAvailable = Helper.ObjToValue<bool>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		isPickupAvailable = false;
		reviveProgress = 0f;
	}
}
