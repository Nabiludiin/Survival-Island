using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_SnowmobileSeat : SerializableBehaviour
{
	public ulong steamID;

	public int seatIndex;

	protected bool Equals(RGD_SnowmobileSeat other)
	{
		if (steamID == other.steamID)
		{
			return seatIndex == other.seatIndex;
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
		return Equals((RGD_SnowmobileSeat)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(steamID, seatIndex);
	}

	public static bool operator ==(RGD_SnowmobileSeat left, RGD_SnowmobileSeat right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_SnowmobileSeat left, RGD_SnowmobileSeat right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in steamID, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in seatIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out steamID, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out seatIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_SnowmobileSeat()
	{
	}

	public RGD_SnowmobileSeat(Network_Player player, int seatIndex)
	{
		this.seatIndex = seatIndex;
		steamID = player.steamID;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("SteamID", steamID);
		info.AddValue("Seat", seatIndex);
	}

	public RGD_SnowmobileSeat(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "SteamID"))
			{
				if (name == "Seat")
				{
					seatIndex = Helper.ObjToValue<int>(item.Value);
				}
			}
			else
			{
				steamID = Helper.ObjToValue<ulong>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		steamID = 0uL;
		seatIndex = -1;
	}
}
