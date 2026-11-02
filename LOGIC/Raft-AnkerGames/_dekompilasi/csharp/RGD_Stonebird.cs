using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Stonebird : RGD_AI_NetworkBehaviour
{
	public ulong targetSteamID;

	protected bool Equals(RGD_Stonebird other)
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
		return Equals((RGD_Stonebird)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), targetSteamID);
	}

	public static bool operator ==(RGD_Stonebird left, RGD_Stonebird right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Stonebird left, RGD_Stonebird right)
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

	public RGD_Stonebird()
	{
	}

	public RGD_Stonebird(AI_NetworkBehaviour_StoneBird stonebirdBehaviour)
		: base(stonebirdBehaviour)
	{
		targetSteamID = ((stonebirdBehaviour.stateMachineStoneBird.target == null) ? ((Network_UserId)0uL) : stonebirdBehaviour.stateMachineStoneBird.target.steamID);
	}

	public void RestoreBird(AI_NetworkBehaviour_StoneBird stonebirdBehaviour)
	{
		Network_UserId id = new Network_UserId(targetSteamID);
		if (id.IsValid())
		{
			Raft_Network value = ComponentManager<Raft_Network>.Value;
			if (value != null)
			{
				stonebirdBehaviour.stateMachineStoneBird.target = value.GetPlayerFromID(id);
			}
		}
		if (stonebirdBehaviour.stateMachineStoneBird.dropStoneState.HasStone)
		{
			stonebirdBehaviour.stateMachineStoneBird.animController.SetBool("HasStone", value: true);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("TargetSteamID", targetSteamID);
	}

	public RGD_Stonebird(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "TargetSteamID")
			{
				targetSteamID = Helper.ObjToValue<ulong>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		targetSteamID = 0uL;
	}
}
