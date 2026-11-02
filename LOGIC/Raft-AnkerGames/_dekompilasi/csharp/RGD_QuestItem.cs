using System;

[Serializable]
public class RGD_QuestItem
{
	public QuestItemType itemType;

	public int itemCount;

	protected bool Equals(RGD_QuestItem other)
	{
		if (itemType == other.itemType)
		{
			return itemCount == other.itemCount;
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
		return Equals((RGD_QuestItem)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine((int)itemType, itemCount);
	}

	public static bool operator ==(RGD_QuestItem left, RGD_QuestItem right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_QuestItem left, RGD_QuestItem right)
	{
		return !object.Equals(left, right);
	}

	public RGD_QuestItem()
	{
	}

	public RGD_QuestItem(QuestItem landmarkQuestItem)
	{
		itemType = landmarkQuestItem.SOQuestItem.questItemType;
		itemCount = landmarkQuestItem.itemCount;
	}

	public void LandmarkQuestItem()
	{
		QuestItemManager.SetQuestItemCount(itemType, itemCount, sendNotification: false);
	}
}
