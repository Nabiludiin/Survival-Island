using System;
using System.Collections.Generic;
using UnityEngine;

public class MissingQuestItemFixer : MonoBehaviour
{
	private void Start()
	{
		if (Raft_Network.IsHost)
		{
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
		}
	}

	private void OnWorldRecievedLate()
	{
		if (Raft_Network.IsHost)
		{
			Invoke("FixLandmarks", 5f);
		}
	}

	private void FixLandmarks()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		List<Landmark> allLandmarks = WorldManager.AllLandmarks;
		if (allLandmarks == null)
		{
			return;
		}
		foreach (Landmark item in allLandmarks)
		{
			if (item == null)
			{
				continue;
			}
			QuestEventBase[] componentsInChildren = item.GetComponentsInChildren<QuestEventBase>(includeInactive: true);
			QuestItemPickup[] componentsInChildren2 = item.GetComponentsInChildren<QuestItemPickup>(includeInactive: true);
			if (componentsInChildren != null)
			{
				QuestEventBase[] array = componentsInChildren;
				foreach (QuestEventBase questEventBase in array)
				{
					if (questEventBase == null)
					{
						continue;
					}
					if (questEventBase.CurrentObjectStateIndex > 0)
					{
						QuestInteractableComponent_Inventory component = questEventBase.GetComponent<QuestInteractableComponent_Inventory>();
						if (!(component != null))
						{
							continue;
						}
						foreach (QuestInteractable_ComponentData_Inventory dataComponent in component.dataComponents)
						{
							if (dataComponent == null || dataComponent.questItemYield == null)
							{
								continue;
							}
							QuestItem[] questItemYield = dataComponent.questItemYield;
							foreach (QuestItem questItem in questItemYield)
							{
								if (questItem == null || questItem.SOQuestItem == null)
								{
									continue;
								}
								QuestItem questItemFromType = QuestItemManager.GetQuestItemFromType(questItem.SOQuestItem.questItemType);
								if (questItemFromType == null || questItemFromType.itemCount != 0)
								{
									continue;
								}
								QuestEventBase[] array2 = componentsInChildren;
								foreach (QuestEventBase questEventBase2 in array2)
								{
									if (questEventBase2 == null || questEventBase2.CurrentObjectStateIndex != 0 || questEventBase2.interactRequirement == null || questEventBase2.interactRequirement.Count == 0)
									{
										continue;
									}
									bool flag = false;
									for (int l = 0; l < questEventBase2.interactRequirement.Count; l++)
									{
										QuestEventBase.InteractRequirementItem interactRequirementItem = questEventBase2.interactRequirement[l];
										if (interactRequirementItem != null && interactRequirementItem.questItem != null && interactRequirementItem.questItem.questItemType == questItemFromType.SOQuestItem.questItemType)
										{
											QuestItemManager.AddQuestItem(questItem.SOQuestItem.questItemType, questItem.itemCount, sendNotification: true);
											flag = true;
											break;
										}
									}
									if (flag)
									{
										break;
									}
								}
							}
						}
					}
					else
					{
						if (questEventBase.CurrentObjectStateIndex != 0 || questEventBase.interactRequirement == null || questEventBase.interactRequirement.Count == 0)
						{
							continue;
						}
						for (int m = 0; m < questEventBase.interactRequirement.Count; m++)
						{
							QuestEventBase.InteractRequirementItem interactRequirementItem2 = questEventBase.interactRequirement[m];
							if (interactRequirementItem2 == null || !(interactRequirementItem2.questItem != null) || interactRequirementItem2.questItem.questItemType == QuestItemType.Tangaroa_Tape)
							{
								continue;
							}
							QuestItem questItemFromType2 = QuestItemManager.GetQuestItemFromType(interactRequirementItem2.questItem.questItemType);
							if (questItemFromType2 == null || questItemFromType2.itemCount >= interactRequirementItem2.amount)
							{
								continue;
							}
							int num = 0;
							if (componentsInChildren2 != null)
							{
								QuestItemPickup[] array3 = componentsInChildren2;
								foreach (QuestItemPickup questItemPickup in array3)
								{
									if (questItemPickup != null && questItemPickup.ContainsQuestItemInYield(interactRequirementItem2.questItem.questItemType) && questItemPickup.gameObject != null && !questItemPickup.gameObject.activeSelf)
									{
										num++;
									}
								}
							}
							int num2 = Mathf.Clamp(num - questItemFromType2.itemCount, 0, int.MaxValue);
							if (num2 > 0)
							{
								QuestItemManager.AddQuestItem(questItemFromType2.SOQuestItem.questItemType, num2, sendNotification: true);
							}
						}
					}
				}
			}
			componentsInChildren = null;
			componentsInChildren2 = null;
		}
	}
}
