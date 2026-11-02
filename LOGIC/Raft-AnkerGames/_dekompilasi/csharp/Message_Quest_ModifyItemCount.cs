using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Quest_ModifyItemCount : Message_NetworkBehaviour_ID
{
	public RGD_QuestItem[] rgdQuestItems;

	protected bool Equals(Message_Quest_ModifyItemCount other)
	{
		if (rgdQuestItems == null && other.rgdQuestItems != null)
		{
			return false;
		}
		if (rgdQuestItems != null && other.rgdQuestItems == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			if (rgdQuestItems != null || other.rgdQuestItems != null)
			{
				return Enumerable.SequenceEqual(rgdQuestItems, other.rgdQuestItems);
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
		return Equals((Message_Quest_ModifyItemCount)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdQuestItems);
	}

	public static bool operator ==(Message_Quest_ModifyItemCount left, Message_Quest_ModifyItemCount right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Quest_ModifyItemCount left, Message_Quest_ModifyItemCount right)
	{
		return !object.Equals(left, right);
	}

	public Message_Quest_ModifyItemCount()
	{
	}

	public Message_Quest_ModifyItemCount(Messages type, MonoBehaviour_Network behaviour, uint objectIndex, params QuestItem[] questItems)
		: base(type, behaviour, objectIndex)
	{
		if (questItems.ContainsItems())
		{
			rgdQuestItems = new RGD_QuestItem[questItems.Length];
			for (int i = 0; i < questItems.Length; i++)
			{
				rgdQuestItems[i] = new RGD_QuestItem(questItems[i]);
			}
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdQuestItems != null) ? rgdQuestItems.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdQuestItems != null)
		{
			RGD_QuestItem[] array = rgdQuestItems;
			foreach (RGD_QuestItem rGD_QuestItem in array)
			{
				writer.WriteValueSafe(in rGD_QuestItem.itemCount, default(FastBufferWriter.ForPrimitives));
				writer.WriteValueSafe(in rGD_QuestItem.itemType, default(FastBufferWriter.ForEnums));
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
			rgdQuestItems = new RGD_QuestItem[value];
			for (int i = 0; i < value; i++)
			{
				rgdQuestItems[i] = new RGD_QuestItem();
				reader.ReadValue(out rgdQuestItems[i].itemCount, default(FastBufferWriter.ForPrimitives));
				reader.ReadValue(out rgdQuestItems[i].itemType, default(FastBufferWriter.ForEnums));
			}
		}
	}
}
