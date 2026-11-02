using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_TextWriterObject : RGD
{
	public string text;

	public uint connectedObjectIndex;

	protected bool Equals(RGD_TextWriterObject other)
	{
		if (text == other.text)
		{
			return connectedObjectIndex == other.connectedObjectIndex;
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
		return Equals((RGD_TextWriterObject)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(text, connectedObjectIndex);
	}

	public static bool operator ==(RGD_TextWriterObject left, RGD_TextWriterObject right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_TextWriterObject left, RGD_TextWriterObject right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(text);
		writer.WriteValueSafe(in connectedObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out text, false);
		reader.ReadValue(out connectedObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_TextWriterObject()
	{
	}

	public RGD_TextWriterObject(RGDType type, TextWriterObject textWriterObj)
		: base(type, RGDSortingOrder.TextWriterObjects)
	{
		text = textWriterObj.GetText();
		connectedObjectIndex = textWriterObj.ObjectIndex;
	}

	public void RestoreTextWriterObject(TextWriterObject textWriterObject)
	{
		if (textWriterObject != null)
		{
			textWriterObject.SetTextFiltered(text);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Text", text);
		info.AddValue("BlockObjectIndex", connectedObjectIndex);
	}

	public RGD_TextWriterObject(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "Text"))
			{
				if (name == "BlockObjectIndex")
				{
					connectedObjectIndex = (uint)item.Value;
				}
			}
			else
			{
				text = (string)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		text = string.Empty;
		connectedObjectIndex = 0u;
	}
}
