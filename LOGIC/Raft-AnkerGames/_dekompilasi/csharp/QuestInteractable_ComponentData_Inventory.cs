using System;
using UnityEngine;

[Serializable]
public class QuestInteractable_ComponentData_Inventory : QuestInteractable_ComponentData
{
	public QuestItem[] questItemYield;

	public Cost[] itemYield;

	public bool giveToAllPlayers;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		if (Raft_Network.IsHost && questItemYield.ContainsItems())
		{
			QuestItemManager value = ComponentManager<QuestItemManager>.Value;
			if (value != null)
			{
				value.AddQuestItemsNetworked(sendNotification: true, questItemYield);
			}
			else
			{
				string text = string.Empty;
				for (int i = 0; i < questItemYield.Length; i++)
				{
					if (questItemYield[i] != null)
					{
						text = text + questItemYield[i].ToString() + ", ";
					}
				}
				Debug.LogError("Could not find questitemmanager in QuestInteractable_ComponentData_Inventory.cs when trying to give items: " + text);
			}
		}
		if (itemYield.ContainsItems() && player != null && (player.IsLocalPlayer || giveToAllPlayers))
		{
			Cost[] array = itemYield;
			foreach (Cost cost in array)
			{
				player.Inventory.AddItem(cost.item.UniqueName, cost.amount);
			}
		}
	}
}
