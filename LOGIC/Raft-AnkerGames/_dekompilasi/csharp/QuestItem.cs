using System;

[Serializable]
public class QuestItem
{
	public SO_QuestItem SOQuestItem;

	public int itemCount;

	public void RestoreQuestItem(RGD_QuestItem p_rgdQuestItem)
	{
		SOQuestItem = QuestItemManager.GetSOQuestItemFromType(p_rgdQuestItem.itemType);
		itemCount = p_rgdQuestItem.itemCount;
	}

	public QuestItem(SO_QuestItem SOQuestItem, int itemCount)
	{
		this.SOQuestItem = SOQuestItem;
		this.itemCount = itemCount;
	}
}
