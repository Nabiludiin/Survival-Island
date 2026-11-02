using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_Quest : SerializableBehaviour
{
	public QuestType questType = QuestType.None;

	public int progressAmount;

	public bool isQuestComplete;

	public bool isQuestStarted;

	public RGD_Quest(Quest quest)
	{
		progressAmount = quest.questProgress;
		isQuestComplete = quest.isQuestComplete;
		isQuestStarted = quest.isQuestStarted;
		questType = quest.questType;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("QuestProgress", progressAmount);
		info.AddValue("IsQuestComplete", isQuestComplete);
		info.AddValue("IsQuestStarted", isQuestStarted);
		info.AddValue("QuestType", questType);
	}

	public RGD_Quest(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "QuestProgress":
				progressAmount = Helper.ObjToValue<int>(item.Value);
				break;
			case "IsQuestComplete":
				isQuestComplete = Helper.ObjToValue<bool>(item.Value);
				break;
			case "IsQuestStarted":
				isQuestStarted = Helper.ObjToValue<bool>(item.Value);
				break;
			case "QuestType":
				questType = Helper.ObjToValue<QuestType>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		progressAmount = 0;
		isQuestComplete = false;
		isQuestStarted = false;
		questType = QuestType.None;
	}
}
