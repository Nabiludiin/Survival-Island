using System;
using System.Runtime.Serialization;
using Steamworks;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Settings_Character : SerializableBehaviour
{
	[SerializeField]
	private string name;

	[SerializeField]
	private int modelIndex;

	[SerializeField]
	private int oi;

	[SerializeField]
	private int p;

	[SerializeField]
	private byte u;

	public const byte VersionOneTestByte = byte.MaxValue;

	public string Name
	{
		get
		{
			if (string.IsNullOrEmpty(name))
			{
				name = SteamFriends.GetPersonaName();
			}
			return name;
		}
		set
		{
			name = value;
		}
	}

	public int ModelIndex
	{
		get
		{
			return modelIndex;
		}
		set
		{
			modelIndex = value;
		}
	}

	public int OutfitIndex
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

	public int Platform
	{
		get
		{
			return p;
		}
		set
		{
			p = value;
		}
	}

	public byte Unlocks
	{
		get
		{
			return u;
		}
		set
		{
			u = value;
		}
	}

	protected bool Equals(RGD_Settings_Character other)
	{
		if (name == other.name && modelIndex == other.modelIndex && oi == other.oi && p == other.p)
		{
			return u == other.u;
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
		return Equals((RGD_Settings_Character)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(name, modelIndex, oi, p, u);
	}

	public static bool operator ==(RGD_Settings_Character left, RGD_Settings_Character right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Settings_Character left, RGD_Settings_Character right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Settings_Character()
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Name", name);
		info.AddValue("Model", modelIndex);
		info.AddValue("OutfitIndex", oi);
		info.AddValue("Platform", p);
		info.AddValue("Unlocks", u);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(name);
		if (!writer.TryBeginWrite(13))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in modelIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in oi, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in p, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue<byte>(byte.MaxValue, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out name, false);
		reader.ReadValue(out modelIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out oi, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out p, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out u, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Settings_Character(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Name":
				name = (string)item.Value;
				break;
			case "Model":
				modelIndex = Helper.ObjToValue<int>(item.Value);
				break;
			case "OutfitIndex":
				oi = Helper.ObjToValue<int>(item.Value);
				break;
			case "Platform":
				p = Helper.ObjToValue<int>(item.Value);
				break;
			case "Unlocks":
				u = Helper.ObjToValue<byte>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		name = string.Empty;
		modelIndex = 0;
		oi = 0;
		p = 0;
		u = 0;
	}
}
