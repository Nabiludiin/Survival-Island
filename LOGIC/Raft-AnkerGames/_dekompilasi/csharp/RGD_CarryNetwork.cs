using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_CarryNetwork : SerializableBehaviour
{
	[SerializeField]
	private float x;

	[SerializeField]
	private float y;

	[SerializeField]
	private float z;

	[SerializeField]
	private float q;

	[SerializeField]
	private float w;

	[SerializeField]
	private float e;

	[SerializeField]
	private uint oi;

	[SerializeField]
	private ulong steamID;

	public Vector3 Position
	{
		get
		{
			return new Vector3(x, y, z);
		}
		set
		{
			x = value.x;
			y = value.y;
			z = value.z;
		}
	}

	public Vector3 Euler
	{
		get
		{
			return new Vector3(q, w, e);
		}
		set
		{
			q = value.x;
			w = value.y;
			e = value.z;
		}
	}

	public uint ItemObjectIndex
	{
		get
		{
			return oi;
		}
		set
		{
			oi = value;
		}
	}

	protected bool Equals(RGD_CarryNetwork other)
	{
		if (x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && q.Equals(other.q) && w.Equals(other.w) && e.Equals(other.e) && oi == other.oi)
		{
			return steamID == other.steamID;
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
		return Equals((RGD_CarryNetwork)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, z, q, w, e, oi, steamID);
	}

	public static bool operator ==(RGD_CarryNetwork left, RGD_CarryNetwork right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_CarryNetwork left, RGD_CarryNetwork right)
	{
		return !object.Equals(left, right);
	}

	public RGD_CarryNetwork()
	{
	}

	public RGD_CarryNetwork(CarryNetworked item)
	{
		Position = item.transform.position;
		Euler = item.transform.eulerAngles;
		ItemObjectIndex = item.ObjectIndex;
		if (item.carryScript.carryingPlayer != null)
		{
			Euler = Vector3.zero;
			Position = item.carryScript.carryingPlayer.FeetPosition;
			steamID = item.carryScript.carryingPlayer.steamID;
		}
	}

	public virtual void RestoreCarryNetworked(CarryNetworked item)
	{
		if (item != null)
		{
			item.transform.position = Position;
			item.transform.eulerAngles = Euler;
			item.ObjectIndex = ItemObjectIndex;
			if (steamID != 0L)
			{
				item.StartCarry(ComponentManager<Raft_Network>.Value.GetPlayerFromID(steamID));
			}
		}
		else
		{
			Debug.LogError("Can't restore RGD_CarryItem because CarryNetworked reference is null");
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("X", x);
		info.AddValue("Y", y);
		info.AddValue("Z", z);
		info.AddValue("Q", q);
		info.AddValue("W", w);
		info.AddValue("E", e);
		info.AddValue("OI", ItemObjectIndex);
		info.AddValue("SID", steamID);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(36))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in q, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in w, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in e, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in oi, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in steamID, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out q, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out w, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out e, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out oi, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out steamID, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_CarryNetwork(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "X":
				x = Helper.ObjToValue<float>(item.Value);
				break;
			case "Y":
				y = Helper.ObjToValue<float>(item.Value);
				break;
			case "Z":
				z = Helper.ObjToValue<float>(item.Value);
				break;
			case "Q":
				q = Helper.ObjToValue<float>(item.Value);
				break;
			case "W":
				w = Helper.ObjToValue<float>(item.Value);
				break;
			case "E":
				e = Helper.ObjToValue<float>(item.Value);
				break;
			case "OI":
				ItemObjectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "SID":
				steamID = Helper.ObjToValue<ulong>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		Position = Vector3.zero;
		Euler = Vector3.zero;
		ItemObjectIndex = 0u;
		steamID = 0uL;
	}
}
