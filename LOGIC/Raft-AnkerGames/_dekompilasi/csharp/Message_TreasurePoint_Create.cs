using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_TreasurePoint_Create : Message_NetworkBehaviour
{
	public RGD_TreasurePoint_Networked[] rgd_treasurePoints;

	protected bool Equals(Message_TreasurePoint_Create other)
	{
		if (rgd_treasurePoints == null && other.rgd_treasurePoints != null)
		{
			return false;
		}
		if (rgd_treasurePoints != null && other.rgd_treasurePoints == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgd_treasurePoints != null || other.rgd_treasurePoints != null)
			{
				return Enumerable.SequenceEqual(rgd_treasurePoints, other.rgd_treasurePoints);
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
		return Equals((Message_TreasurePoint_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgd_treasurePoints);
	}

	public static bool operator ==(Message_TreasurePoint_Create left, Message_TreasurePoint_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_TreasurePoint_Create left, Message_TreasurePoint_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgd_treasurePoints != null) ? rgd_treasurePoints.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgd_treasurePoints != null)
		{
			RGD_TreasurePoint_Networked[] array = rgd_treasurePoints;
			foreach (RGD_TreasurePoint_Networked rGD_TreasurePoint_Networked in array)
			{
				writer.WriteValueSafe<bool>(rGD_TreasurePoint_Networked is RGD_TreasurePoint_Landmark_Networked, default(FastBufferWriter.ForPrimitives));
				rGD_TreasurePoint_Networked.SerializeFast(writer);
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value <= 0)
		{
			return;
		}
		rgd_treasurePoints = new RGD_TreasurePoint_Networked[value];
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out bool value2, default(FastBufferWriter.ForPrimitives));
			if (value2)
			{
				rgd_treasurePoints[i] = new RGD_TreasurePoint_Landmark_Networked();
			}
			else
			{
				rgd_treasurePoints[i] = new RGD_TreasurePoint_Networked();
			}
			rgd_treasurePoints[i].DeserializeFast(reader);
		}
	}

	public Message_TreasurePoint_Create()
	{
	}

	public Message_TreasurePoint_Create(Messages type, MonoBehaviour_Network behaviour, TreasurePoint[] treasurePoints)
		: base(type, behaviour)
	{
		List<RGD_TreasurePoint_Networked> list = new List<RGD_TreasurePoint_Networked>();
		if (treasurePoints == null)
		{
			return;
		}
		for (int i = 0; i < treasurePoints.Length; i++)
		{
			if (treasurePoints[i] != null)
			{
				list.Add(treasurePoints[i].GetSave());
			}
		}
		rgd_treasurePoints = list.ToArray();
	}
}
