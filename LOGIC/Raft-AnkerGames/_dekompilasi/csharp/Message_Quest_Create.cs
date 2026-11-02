using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Quest_Create : Message_NetworkBehaviour_ID
{
	public RGD_QuestItem[] rgd_QuestItems;

	protected bool Equals(Message_Quest_Create other)
	{
		if (rgd_QuestItems == null && other.rgd_QuestItems != null)
		{
			return false;
		}
		if (rgd_QuestItems != null && other.rgd_QuestItems == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			if (rgd_QuestItems != null || other.rgd_QuestItems != null)
			{
				return Enumerable.SequenceEqual(rgd_QuestItems, other.rgd_QuestItems);
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
		return Equals((Message_Quest_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgd_QuestItems);
	}

	public static bool operator ==(Message_Quest_Create left, Message_Quest_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Quest_Create left, Message_Quest_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(rgd_QuestItems.Length, default(FastBufferWriter.ForPrimitives));
		RGD_QuestItem[] array = rgd_QuestItems;
		foreach (RGD_QuestItem rGD_QuestItem in array)
		{
			writer.WriteValueSafe(in rGD_QuestItem.itemCount, default(FastBufferWriter.ForPrimitives));
			writer.WriteValueSafe(in rGD_QuestItem.itemType, default(FastBufferWriter.ForEnums));
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		rgd_QuestItems = new RGD_QuestItem[value];
		for (int i = 0; i < value; i++)
		{
			rgd_QuestItems[i] = new RGD_QuestItem();
			reader.ReadValue(out rgd_QuestItems[i].itemCount, default(FastBufferWriter.ForPrimitives));
			reader.ReadValue(out rgd_QuestItems[i].itemType, default(FastBufferWriter.ForEnums));
		}
	}

	public Message_Quest_Create()
	{
	}

	public Message_Quest_Create(Messages type, MonoBehaviour_Network behaviour, uint objectIndex, QuestItem[] QuestItems)
		: base(type, behaviour, objectIndex)
	{
		rgd_QuestItems = new RGD_QuestItem[QuestItems.Length];
		for (int i = 0; i < QuestItems.Length; i++)
		{
			rgd_QuestItems[i] = new RGD_QuestItem(QuestItems[i]);
		}
	}

	public void RestoreQuestItems()
	{
		if (rgd_QuestItems.ContainsItems())
		{
			for (int i = 0; i < rgd_QuestItems.Length; i++)
			{
				rgd_QuestItems[i].LandmarkQuestItem();
			}
		}
	}
}
