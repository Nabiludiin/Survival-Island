using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Boar : RGD_AI_NetworkBehaviour
{
	public ulong targetSteamID;

	protected bool Equals(RGD_Boar other)
	{
		if (Equals((RGD_AI_NetworkBehaviour)other))
		{
			return targetSteamID == other.targetSteamID;
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
		return Equals((RGD_Boar)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), targetSteamID);
	}

	public static bool operator ==(RGD_Boar left, RGD_Boar right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Boar left, RGD_Boar right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in targetSteamID, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out targetSteamID, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Boar()
	{
	}

	public RGD_Boar(AI_NetworkBehaviour_Boar boarBehaviour)
		: base(boarBehaviour)
	{
		targetSteamID = ((boarBehaviour.boarSM.enemyTarget == null) ? ((Network_UserId)0uL) : boarBehaviour.boarSM.enemyTarget.GetComponent<Network_Player>().steamID);
		pickupYieldCount = ((!(boarBehaviour.pickupItemChanneling == null)) ? boarBehaviour.pickupItemChanneling.ItemsRemaining : 0);
	}

	public void RestoreBoar(AI_NetworkBehaviour_Boar boarBehaviour)
	{
		Network_UserId id = new Network_UserId(targetSteamID);
		if (id.IsValid())
		{
			Raft_Network value = ComponentManager<Raft_Network>.Value;
			if (value != null)
			{
				boarBehaviour.boarSM.enemyTarget = value.GetPlayerFromID(id);
			}
		}
		if (boarBehaviour.pickupItemChanneling != null)
		{
			boarBehaviour.pickupItemChanneling.RestoreYield(pickupYieldCount);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("TargetSteamID", targetSteamID);
	}

	public RGD_Boar(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "TargetSteamID"))
			{
				if (name == "PickupYieldCount")
				{
					pickupYieldCount = Helper.ObjToValue<int>(item.Value);
				}
			}
			else
			{
				targetSteamID = Helper.ObjToValue<ulong>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		targetSteamID = 0uL;
		pickupYieldCount = 0;
	}
}
