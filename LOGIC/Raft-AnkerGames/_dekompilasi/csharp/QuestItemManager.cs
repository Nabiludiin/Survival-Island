using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;

public class QuestItemManager : MonoBehaviour_ID_Network, SceneEventInterface
{
	public static Action<QuestItem, int, bool> OnQuestItemChange;

	public static Action OnQuestItemsModified;

	private static Raft_Network network;

	public static QuestItem[] QuestItems;

	private static SO_QuestItem[] SOQuestItems;

	private static NotificationManager notificationManager;

	private static SoundManager soundManager;

	public void Awake()
	{
		ComponentManager<QuestItemManager>.Value = this;
		OnQuestItemChange = (Action<QuestItem, int, bool>)Delegate.Combine(OnQuestItemChange, new Action<QuestItem, int, bool>(OnQuestItemChangeAction));
	}

	public void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		notificationManager = ComponentManager<NotificationManager>.Value;
		soundManager = ComponentManager<SoundManager>.Value;
		if (Raft_Network.IsHost && GameManager.IsInNewGame)
		{
			ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			NetworkIDManager.AddNetworkID(this);
		}
		InitQuestItems();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this);
		ResetStatics();
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			ResetStatics();
		}
	}

	private void OnQuestItemChangeAction(QuestItem questItem, int amountChanged, bool sendNotification)
	{
		if (sendNotification && notificationManager != null && amountChanged > 0)
		{
			(notificationManager.ShowNotification("QuestItem") as Notification_QuestItem).infoQue.Enqueue(new Notification_QuestItem_Info(questItem.SOQuestItem.DisplayName, amountChanged, questItem.SOQuestItem.itemImage));
		}
	}

	public void ResetStatics()
	{
		network = null;
		OnQuestItemChange = null;
	}

	public void AddQuestItemsNetworked(bool sendNotification, params QuestItem[] questItems)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (Raft_Network.IsHost)
		{
			AddQuestItems(questItems, sendNotification);
			Message_Quest_ModifyItemCount message = new Message_Quest_ModifyItemCount(Messages.QuestItemModifyCount, network.NetworkIDManager, ObjectIndex, QuestItems);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			Message_Quest_ModifyItemCount message2 = new Message_Quest_ModifyItemCount(Messages.QuestItemModifyCount, network.NetworkIDManager, ObjectIndex, questItems);
			network.SendP2P(network.HostID, message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void SendModifiedQuestItemsMessage()
	{
		if (Raft_Network.IsHost)
		{
			Message_Quest_ModifyItemCount message = new Message_Quest_ModifyItemCount(Messages.QuestItemModifyCount, network.NetworkIDManager, ObjectIndex, QuestItems);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			if (OnQuestItemsModified != null)
			{
				OnQuestItemsModified();
			}
		}
	}

	public static void InitQuestItems()
	{
		SOQuestItems = Resources.LoadAll<SO_QuestItem>("SO_QuestItems");
		List<QuestItem> list = new List<QuestItem>();
		for (int i = 0; i < SOQuestItems.Length; i++)
		{
			list.Add(new QuestItem(SOQuestItems[i], 0));
		}
		QuestItems = list.OrderBy((QuestItem q) => q.SOQuestItem.destinationType).ToArray();
	}

	public static T GetEnumValue<T>(string str) where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			throw new Exception("T must be an Enumeration type.");
		}
		T result = ((T[])Enum.GetValues(typeof(T)))[0];
		if (!string.IsNullOrEmpty(str))
		{
			T[] array = (T[])Enum.GetValues(typeof(T));
			for (int i = 0; i < array.Length; i++)
			{
				T val = array[i];
				if (val.ToString().ToUpper().Equals(str.ToUpper()))
				{
					result = val;
					break;
				}
			}
		}
		return result;
	}

	public static void LogAllQuestItems()
	{
		QuestItem[] questItems = QuestItems;
		foreach (QuestItem questItem in questItems)
		{
			Debug.Log(questItem.SOQuestItem.questItemType.ToString() + "has amount" + questItem.itemCount);
		}
	}

	public static QuestItem GetQuestItemFromType(QuestItemType p_type)
	{
		for (int i = 0; i < QuestItems.Length; i++)
		{
			if (QuestItems[i].SOQuestItem.questItemType == p_type)
			{
				return QuestItems[i];
			}
		}
		Debug.Log("Unable to find landmarkQuestItem from type" + p_type);
		return null;
	}

	public static SO_QuestItem GetSOQuestItemFromType(QuestItemType questItemType)
	{
		for (int i = 0; i < SOQuestItems.Length; i++)
		{
			if (SOQuestItems[i].questItemType == questItemType)
			{
				return SOQuestItems[i];
			}
		}
		return null;
	}

	public static bool AddQuestItem(QuestItemType p_type, int p_amount, bool sendNotification)
	{
		QuestItem questItemFromType = GetQuestItemFromType(p_type);
		if (questItemFromType != null)
		{
			questItemFromType.itemCount += p_amount;
			if (OnQuestItemChange != null)
			{
				OnQuestItemChange(questItemFromType, p_amount, sendNotification);
			}
		}
		else
		{
			Debug.LogError("Can't find QuestItem in QuestItemArray with type " + p_type);
		}
		if ((p_amount > 0 && soundManager != null) & sendNotification)
		{
			soundManager.PlayUI_MoveItem();
		}
		return questItemFromType != null;
	}

	public static bool AddQuestItems(QuestItem[] questItems, bool sendNotification)
	{
		bool result = true;
		if (questItems.ContainsItems())
		{
			foreach (QuestItem questItem in questItems)
			{
				if (questItem != null && questItem.SOQuestItem != null && !AddQuestItem(questItem.SOQuestItem.questItemType, questItem.itemCount, sendNotification))
				{
					result = false;
				}
			}
			return result;
		}
		return false;
	}

	public static bool SetQuestItemCount(QuestItemType p_type, int p_amount, bool sendNotification)
	{
		QuestItem questItemFromType = GetQuestItemFromType(p_type);
		if (questItemFromType != null)
		{
			if (questItemFromType.itemCount == p_amount)
			{
				return true;
			}
			int arg = p_amount - questItemFromType.itemCount;
			questItemFromType.itemCount = p_amount;
			if (OnQuestItemChange != null)
			{
				OnQuestItemChange(questItemFromType, arg, sendNotification);
			}
		}
		return questItemFromType != null;
	}

	public static bool HasQuestItem(QuestItemType p_type, int p_amount = 1)
	{
		QuestItem questItemFromType = GetQuestItemFromType(p_type);
		if (questItemFromType != null && questItemFromType.itemCount >= p_amount)
		{
			return true;
		}
		return false;
	}

	public static void ClearAllQuestItems()
	{
		QuestItem[] questItems = QuestItems;
		foreach (QuestItem questItem in questItems)
		{
			if (questItem != null)
			{
				SetQuestItemCount(questItem.SOQuestItem.questItemType, 0, sendNotification: false);
			}
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_NetworkBehaviour(RGDType.NetworkBehaviour_QuestItemManager, this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.QuestCreate:
		{
			Message_Quest_Create message_Quest_Create = msg as Message_Quest_Create;
			ObjectIndex = message_Quest_Create.NetworkID_ObjectIndex;
			NetworkIDManager.AddNetworkID(this);
			message_Quest_Create.RestoreQuestItems();
			return false;
		}
		case Messages.QuestItemModifyCount:
		{
			Message_Quest_ModifyItemCount message_Quest_ModifyItemCount = msg as Message_Quest_ModifyItemCount;
			if (Raft_Network.IsHost)
			{
				if (message_Quest_ModifyItemCount.rgdQuestItems.ContainsItems())
				{
					for (int i = 0; i < message_Quest_ModifyItemCount.rgdQuestItems.Length; i++)
					{
						RGD_QuestItem rGD_QuestItem = message_Quest_ModifyItemCount.rgdQuestItems[i];
						if (AddQuestItem(rGD_QuestItem.itemType, rGD_QuestItem.itemCount, sendNotification: true))
						{
							rGD_QuestItem.itemCount = GetQuestItemFromType(rGD_QuestItem.itemType).itemCount;
						}
					}
					return true;
				}
				return false;
			}
			for (int j = 0; j < message_Quest_ModifyItemCount.rgdQuestItems.Length; j++)
			{
				RGD_QuestItem rGD_QuestItem2 = message_Quest_ModifyItemCount.rgdQuestItems[j];
				SetQuestItemCount(rGD_QuestItem2.itemType, rGD_QuestItem2.itemCount, sendNotification: true);
			}
			if (OnQuestItemsModified != null)
			{
				OnQuestItemsModified();
			}
			return false;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
