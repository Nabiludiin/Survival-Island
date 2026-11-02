using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class QuestProgressTracker
{
	private static List<Quest> quests = new List<Quest>();

	private static QuestData[] questData = null;

	public static Raft_Network network;

	public static void AwakeCall()
	{
		if (questData == null)
		{
			questData = InitializeQuestData();
		}
		InitializeQuests(questData);
	}

	public static void StartCall()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	public static void Reset()
	{
		if (quests != null)
		{
			quests.Clear();
		}
	}

	public static List<Quest> GetQuests()
	{
		return quests;
	}

	public static QuestType GetQuestTypeByName(string questName)
	{
		foreach (QuestType value in Enum.GetValues(typeof(QuestType)))
		{
			if (value.ToString().Contains(questName, StringComparison.OrdinalIgnoreCase))
			{
				return value;
			}
		}
		return QuestType.None;
	}

	public static Quest GetQuestByName(string questName)
	{
		return GetQuestByType(GetQuestTypeByName(questName));
	}

	public static Quest GetQuestByType(QuestType quest)
	{
		for (int i = 0; i < quests.Count; i++)
		{
			if (quests[i].questType == quest)
			{
				return quests[i];
			}
		}
		return null;
	}

	public static void ProgressQuestRPC(QuestType questType, int amount = 1)
	{
		Message_ProgressQuest message = new Message_ProgressQuest(Messages.QuestProgress, network.NetworkIDManager, (int)questType, amount);
		if (Raft_Network.IsHost)
		{
			ProgressQuest(questType, amount);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static void ProgressQuest(QuestType questType, int amount = 1)
	{
		Quest questByType = GetQuestByType(questType);
		if (questByType != null)
		{
			questByType.questProgress += amount;
			if (questByType.questProgress >= questByType.questData.progressCountRequired)
			{
				FinishQuestRPC(questByType.questType);
			}
		}
	}

	public static void FinishQuestRPC(QuestType questType)
	{
		Message_FinishQuest message = new Message_FinishQuest(Messages.QuestFinish, network.NetworkIDManager, (int)questType);
		if (Raft_Network.IsHost)
		{
			FinishQuest(questType);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static void FinishQuest(QuestType questType)
	{
		Quest questByType = GetQuestByType(questType);
		if (questByType != null)
		{
			bool isQuestComplete = questByType.isQuestComplete;
			questByType.isQuestComplete = true;
			questByType.isQuestStarted = true;
			questByType.questProgress = questByType.questData.progressCountRequired;
			if (!isQuestComplete && questByType.isQuestComplete)
			{
				questByType.OnQuestComplete.CallSafe();
			}
		}
		else
		{
			Debug.Log("Tried to complete quest with type " + questType.ToString() + " but now quest could be found. Maybe you have forgotten to create a QuestData scriptable object!");
		}
	}

	public static void DeCompleteQuest(QuestType questType)
	{
		Quest questByType = GetQuestByType(questType);
		if (questByType != null)
		{
			questByType.isQuestComplete = false;
			questByType.isQuestStarted = false;
			questByType.questProgress = 0;
		}
	}

	public static bool HasStartedQuest(QuestType questType)
	{
		Quest questByType = GetQuestByType(questType);
		if (questByType != null && questByType.isQuestStarted)
		{
			return true;
		}
		return false;
	}

	public static bool HasFinishedQuest(QuestType questType)
	{
		Quest questByType = GetQuestByType(questType);
		if (questByType != null && questByType.isQuestComplete)
		{
			return true;
		}
		return false;
	}

	public static int GetQuestProgress(QuestType questType)
	{
		return GetQuestByType(questType)?.questProgress ?? (-1);
	}

	public static void LogQuestData(QuestType questType)
	{
		Quest questByType = GetQuestByType(questType);
		if (questByType != null)
		{
			Debug.Log("QuestTypeName = " + questType.ToString() + " IsQuestComplete = " + questByType.isQuestComplete + " IsQuestStarted " + questByType.isQuestStarted + " QuestProgress = " + questByType.questProgress);
		}
	}

	public static Quest[] GetUnlockedQuests()
	{
		List<Quest> list = new List<Quest>();
		foreach (Quest quest in quests)
		{
			if (quest.isQuestComplete)
			{
				list.Add(quest);
			}
		}
		return list.ToArray();
	}

	public static int[] GetUnlockedQuestsIndexes()
	{
		List<int> list = new List<int>();
		foreach (Quest quest in quests)
		{
			if (quest.isQuestComplete)
			{
				list.Add((int)quest.questType);
			}
		}
		return list.ToArray();
	}

	public static void Deserialize_NetworkMessage(Message_NetworkBehaviour msg)
	{
		switch (msg.Type)
		{
		case Messages.QuestRestore:
		{
			if (Raft_Network.IsHost)
			{
				Debug.Log("Host should not recieve this message");
				break;
			}
			int[] unlockedQuestIndexes = (msg as Message_RestoreQuests).unlockedQuestIndexes;
			for (int i = 0; i < unlockedQuestIndexes.Length; i++)
			{
				FinishQuest((QuestType)unlockedQuestIndexes[i]);
			}
			break;
		}
		case Messages.QuestFinish:
		{
			Message_FinishQuest message_FinishQuest = msg as Message_FinishQuest;
			if (Raft_Network.IsHost)
			{
				FinishQuestRPC((QuestType)message_FinishQuest.questIndex);
			}
			else
			{
				FinishQuest((QuestType)message_FinishQuest.questIndex);
			}
			break;
		}
		case Messages.QuestProgress:
		{
			Message_ProgressQuest message_ProgressQuest = msg as Message_ProgressQuest;
			if (Raft_Network.IsHost)
			{
				ProgressQuestRPC((QuestType)message_ProgressQuest.questIndex);
			}
			else
			{
				ProgressQuest((QuestType)message_ProgressQuest.questIndex);
			}
			break;
		}
		}
	}

	private static QuestData[] InitializeQuestData()
	{
		return Resources.LoadAll<QuestData>("QuestData");
	}

	private static void InitializeQuest(QuestType questType)
	{
		Quest quest = new Quest(questType);
		quests.Add(quest);
		Debug.Log(quest.questType);
	}

	private static void InitializeQuest(QuestType questType, QuestData questData)
	{
		Quest quest = new Quest(questType);
		quest.questData = questData;
		quests.Add(quest);
	}

	private static void InitializeQuests(QuestData[] questData)
	{
		foreach (QuestData questData2 in questData)
		{
			InitializeQuest(questData2.quest, questData2);
		}
	}
}
