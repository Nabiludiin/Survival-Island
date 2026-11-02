using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Domestic : RGD_AI_NetworkBehaviour
{
	public float hungerTimer;

	public bool isTamed;

	public DomesticStateType domesticState;

	public int rareMaterialIndex;

	protected bool Equals(RGD_Domestic other)
	{
		if (Equals((RGD_AI_NetworkBehaviour)other) && hungerTimer.Equals(other.hungerTimer) && isTamed == other.isTamed && domesticState == other.domesticState)
		{
			return rareMaterialIndex == other.rareMaterialIndex;
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
		return Equals((RGD_Domestic)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), hungerTimer, isTamed, (int)domesticState, rareMaterialIndex);
	}

	public static bool operator ==(RGD_Domestic left, RGD_Domestic right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Domestic left, RGD_Domestic right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in hungerTimer, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in isTamed, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in domesticState, default(FastBufferWriter.ForEnums));
		writer.WriteValueSafe(in rareMaterialIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out hungerTimer, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out isTamed, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out domesticState, default(FastBufferWriter.ForEnums));
		reader.ReadValue(out rareMaterialIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Domestic()
	{
	}

	public RGD_Domestic(AI_NetworkBehaviour_Domestic domesticBehaviour)
		: base(domesticBehaviour)
	{
		hungerTimer = domesticBehaviour.HungerTimer;
		isTamed = domesticBehaviour.IsTamed;
		domesticState = domesticBehaviour.DomesticState;
		if (domesticBehaviour.rareMaterialScript != null)
		{
			rareMaterialIndex = (int)domesticBehaviour.rareMaterialScript.randomizer.lastRandomizedIndex;
		}
		else
		{
			rareMaterialIndex = -1;
		}
	}

	public void RestoreDomesticBehaviour(AI_NetworkBehaviour_Domestic domesticBehaviour)
	{
		if (hungerTimer != -1f)
		{
			domesticBehaviour.HungerTimer = hungerTimer;
		}
		domesticBehaviour.IsTamed = isTamed;
		if (domesticBehaviour.IsTamed)
		{
			domesticBehaviour.carryScript.EnableCollider();
		}
		if (rareMaterialIndex != -1 && domesticBehaviour.rareMaterialScript != null)
		{
			domesticBehaviour.rareMaterialScript.AssignSpecificMaterial((uint)rareMaterialIndex);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("HungerTimer", hungerTimer);
		info.AddValue("IsTamed", isTamed);
		info.AddValue("DomesticState", domesticState);
		info.AddValue("RareMaterialIndex", rareMaterialIndex);
	}

	public RGD_Domestic(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "HungerTimer":
				hungerTimer = Helper.ObjToValue<float>(item.Value);
				break;
			case "IsTamed":
				isTamed = Helper.ObjToValue<bool>(item.Value);
				break;
			case "DomesticState":
				domesticState = Helper.ObjToValue<DomesticStateType>(item.Value);
				break;
			case "RareMaterialIndex":
				rareMaterialIndex = Helper.ObjToValue<int>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		hungerTimer = -1f;
		isTamed = false;
		domesticState = DomesticStateType.Landmark;
		rareMaterialIndex = -1;
	}
}
