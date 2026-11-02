using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Scarecrow_Create : Message_NetworkBehaviour
{
	public RGD_Scarecrow[] rgd_scareCrows;

	protected bool Equals(Message_Scarecrow_Create other)
	{
		if (rgd_scareCrows == null && other.rgd_scareCrows != null)
		{
			return false;
		}
		if (rgd_scareCrows != null && other.rgd_scareCrows == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgd_scareCrows != null || other.rgd_scareCrows != null)
			{
				return Enumerable.SequenceEqual(rgd_scareCrows, other.rgd_scareCrows);
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
		return Equals((Message_Scarecrow_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgd_scareCrows);
	}

	public static bool operator ==(Message_Scarecrow_Create left, Message_Scarecrow_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Scarecrow_Create left, Message_Scarecrow_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(rgd_scareCrows.Length, default(FastBufferWriter.ForPrimitives));
		RGD_Scarecrow[] array = rgd_scareCrows;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		rgd_scareCrows = new RGD_Scarecrow[value];
		for (int i = 0; i < value; i++)
		{
			rgd_scareCrows[i] = new RGD_Scarecrow();
			rgd_scareCrows[i].DeserializeFast(reader);
		}
	}

	public Message_Scarecrow_Create()
	{
	}

	public Message_Scarecrow_Create(Messages type, MonoBehaviour_Network behaviour, Scarecrow[] scarecrows)
		: base(type, behaviour)
	{
		rgd_scareCrows = new RGD_Scarecrow[scarecrows.Length];
		for (int i = 0; i < rgd_scareCrows.Length; i++)
		{
			rgd_scareCrows[i] = new RGD_Scarecrow(RGDType.Block_Scarecrow, scarecrows[i].block, scarecrows[i]);
		}
	}

	public void RestoreScareCrows(Scarecrow[] scareCrows, NetworkIDManager manager)
	{
		RGD_Scarecrow[] array = rgd_scareCrows;
		foreach (RGD_Scarecrow rGD_Scarecrow in array)
		{
			if (!(rGD_Scarecrow == null))
			{
				Scarecrow scareCrowFromObjectIndex = GetScareCrowFromObjectIndex(scareCrows, rGD_Scarecrow.BlockObjectIndex);
				rGD_Scarecrow.RestoreScarecrow(scareCrowFromObjectIndex);
			}
		}
	}

	private Scarecrow GetScareCrowFromObjectIndex(Scarecrow[] scareCrows, uint objectIndex)
	{
		for (int i = 0; i < scareCrows.Length; i++)
		{
			if (scareCrows[i].ObjectIndex == objectIndex)
			{
				return scareCrows[i];
			}
		}
		return null;
	}
}
