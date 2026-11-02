using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Sign_Create : Message_NetworkBehaviour
{
	public RGD_TextWriterObject[] rgdsigns;

	protected bool Equals(Message_Sign_Create other)
	{
		if (rgdsigns == null && other.rgdsigns != null)
		{
			return false;
		}
		if (rgdsigns != null && other.rgdsigns == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdsigns != null || other.rgdsigns != null)
			{
				return Enumerable.SequenceEqual(rgdsigns, other.rgdsigns);
			}
			return true;
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
		return Equals((Message_Sign_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdsigns);
	}

	public static bool operator ==(Message_Sign_Create left, Message_Sign_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Sign_Create left, Message_Sign_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdsigns != null) ? rgdsigns.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdsigns != null)
		{
			RGD_TextWriterObject[] array = rgdsigns;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SerializeFast(writer);
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgdsigns = new RGD_TextWriterObject[value];
			for (int i = 0; i < value; i++)
			{
				rgdsigns[i] = new RGD_TextWriterObject();
				rgdsigns[i].DeserializeFast(reader);
			}
		}
	}

	public Message_Sign_Create()
	{
	}

	public Message_Sign_Create(Messages p_type, MonoBehaviour_Network p_behaviour, TextWriterObject[] p_signs)
		: base(p_type, p_behaviour)
	{
		List<RGD_TextWriterObject> list = new List<RGD_TextWriterObject>();
		if (p_signs == null)
		{
			return;
		}
		for (int i = 0; i < p_signs.Length; i++)
		{
			if (p_signs[i].GetText() != string.Empty)
			{
				list.Add(new RGD_TextWriterObject(RGDType.Block, p_signs[i]));
			}
		}
		rgdsigns = ((list.Count > 0) ? list.ToArray() : null);
	}

	public void RestoreSigns(TextWriterObject[] p_signs)
	{
		if (p_signs == null || p_signs.Length == 0 || rgdsigns == null)
		{
			return;
		}
		RGD_TextWriterObject[] array = rgdsigns;
		foreach (RGD_TextWriterObject rGD_TextWriterObject in array)
		{
			if (!(rGD_TextWriterObject == null))
			{
				GetSignFromObjectIndex(p_signs, rGD_TextWriterObject.connectedObjectIndex).SetTextFiltered(rGD_TextWriterObject.text);
			}
		}
	}

	private TextWriterObject GetSignFromObjectIndex(TextWriterObject[] p_signs, uint objectIndex)
	{
		for (int i = 0; i < p_signs.Length; i++)
		{
			if (p_signs[i].ObjectIndex == objectIndex)
			{
				return p_signs[i];
			}
		}
		return null;
	}
}
