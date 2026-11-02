using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Keypad : SerializableBehaviour
{
	public string c;

	public uint o;

	public string Code
	{
		get
		{
			return c;
		}
		set
		{
			c = value;
		}
	}

	public uint KeypadObjectIndex
	{
		get
		{
			return o;
		}
		set
		{
			o = value;
		}
	}

	protected bool Equals(RGD_Keypad other)
	{
		if (c == other.c)
		{
			return o == other.o;
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
		return Equals((RGD_Keypad)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(c, o);
	}

	public static bool operator ==(RGD_Keypad left, RGD_Keypad right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Keypad left, RGD_Keypad right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(c);
		writer.WriteValueSafe(in o, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out c, false);
		reader.ReadValue(out o, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Keypad()
	{
	}

	public RGD_Keypad(Keypad keypad)
	{
		Code = keypad.CurrentCode;
		KeypadObjectIndex = keypad.ObjectIndex;
	}

	public void RestoreKeypad(Keypad keypad)
	{
		if (!(keypad == null) && !string.IsNullOrEmpty(Code))
		{
			keypad.SetCode(Code);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Code", Code);
		info.AddValue("ObjectIndex", KeypadObjectIndex);
	}

	public RGD_Keypad(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "Code"))
			{
				if (name == "ObjectIndex")
				{
					KeypadObjectIndex = Helper.ObjToValue<uint>(item.Value);
				}
			}
			else
			{
				Code = Helper.ObjToValue<string>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		KeypadObjectIndex = 0u;
		Code = string.Empty;
	}
}
