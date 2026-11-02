using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_TrophyBoardCreate : Message_NetworkBehaviour
{
	public RGDTrophyHolder[] rgdTrophyHolders;

	protected bool Equals(Message_TrophyBoardCreate other)
	{
		if (rgdTrophyHolders == null && other.rgdTrophyHolders != null)
		{
			return false;
		}
		if (rgdTrophyHolders != null && other.rgdTrophyHolders == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdTrophyHolders != null || other.rgdTrophyHolders != null)
			{
				return Enumerable.SequenceEqual(rgdTrophyHolders, other.rgdTrophyHolders);
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
		return Equals((Message_TrophyBoardCreate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdTrophyHolders);
	}

	public static bool operator ==(Message_TrophyBoardCreate left, Message_TrophyBoardCreate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_TrophyBoardCreate left, Message_TrophyBoardCreate right)
	{
		return !object.Equals(left, right);
	}

	public Message_TrophyBoardCreate()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdTrophyHolders != null) ? rgdTrophyHolders.Length : 0, default(FastBufferWriter.ForPrimitives));
		RGDTrophyHolder[] array = rgdTrophyHolders;
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
		if (value > 0)
		{
			rgdTrophyHolders = new RGDTrophyHolder[value];
			for (int i = 0; i < value; i++)
			{
				rgdTrophyHolders[i] = new RGDTrophyHolder();
				rgdTrophyHolders[i].DeserializeFast(reader);
			}
		}
	}

	public Message_TrophyBoardCreate(Messages p_type, MonoBehaviour_Network p_behavior, TrophyHolder[] p_trophyHolders)
		: base(p_type, p_behavior)
	{
		if (p_trophyHolders == null)
		{
			return;
		}
		List<RGDTrophyHolder> list = new List<RGDTrophyHolder>();
		for (int i = 0; i < p_trophyHolders.Length; i++)
		{
			if (p_trophyHolders[i].GetCurrentItem() != null)
			{
				RGDTrophyHolder item = new RGDTrophyHolder(RGDType.ID_TrophyHolder, p_trophyHolders[i]);
				list.Add(item);
			}
		}
		rgdTrophyHolders = list.ToArray();
	}

	public void RestoreTrophyHolders(TrophyHolder[] p_trophyHolders)
	{
		if (rgdTrophyHolders == null)
		{
			return;
		}
		RGDTrophyHolder[] array = rgdTrophyHolders;
		foreach (RGDTrophyHolder rGDTrophyHolder in array)
		{
			if (!(rGDTrophyHolder == null))
			{
				TrophyHolder trophyHolderFromObjectIndex = GetTrophyHolderFromObjectIndex(p_trophyHolders, rGDTrophyHolder.BlockObjectIndex);
				if (trophyHolderFromObjectIndex != null)
				{
					trophyHolderFromObjectIndex.PlaceTrophy(null, ItemManager.GetItemByIndex(rGDTrophyHolder.itemIndex));
				}
				else
				{
					Helper.LogBuild("Attempted to restore a trophyholder, but failed. The Attempted ID: " + rGDTrophyHolder.BlockObjectIndex);
				}
			}
		}
	}

	private TrophyHolder GetTrophyHolderFromObjectIndex(TrophyHolder[] p_trophyHolders, uint objectIndex)
	{
		if (p_trophyHolders == null)
		{
			return null;
		}
		for (int i = 0; i < p_trophyHolders.Length; i++)
		{
			if (p_trophyHolders[i] != null && p_trophyHolders[i].ObjectIndex == objectIndex)
			{
				return p_trophyHolders[i];
			}
		}
		return null;
	}
}
