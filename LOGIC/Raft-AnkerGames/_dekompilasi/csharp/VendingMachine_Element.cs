using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine_Element : MonoBehaviour
{
	public InteractableButton_Networked networked_button;

	public QuestEventBase.InteractRequirementItem interactRequirement;

	public QuestEventBase.InteractRequirementItem itemExtract;

	[SerializeField]
	private TextMeshProUGUI requirementLabel;

	[SerializeField]
	private Image requirementImage;

	[SerializeField]
	private Image outputImage;

	[SerializeField]
	private TextMeshProUGUI outputCountLabel;

	private void Start()
	{
		if (interactRequirement != null)
		{
			SetUI(interactRequirement, itemExtract);
		}
		else
		{
			Debug.Log("Vending machine element does not have a requirement " + transform.name, gameObject);
		}
	}

	public void SetUI(QuestEventBase.InteractRequirementItem requirement, QuestEventBase.InteractRequirementItem output)
	{
		if (requirement != null)
		{
			requirementLabel.text = "X" + requirement.amount;
			requirementImage.sprite = requirement.questItem.itemImage;
		}
		if (output != null)
		{
			outputImage.sprite = (output.IsQuestItem ? output.questItem.itemImage : output.inventoryItem.settings_Inventory.Sprite);
			outputCountLabel.text = "X" + output.amount;
		}
	}

	public bool HasInteractRequirement(Network_Player player)
	{
		if (interactRequirement != null)
		{
			if (interactRequirement.questItem != null && !QuestItemManager.HasQuestItem(interactRequirement.questItem.questItemType, interactRequirement.amount))
			{
				return false;
			}
			if (interactRequirement.inventoryItem != null && player.IsLocalPlayer && player.Inventory.GetItemCount(interactRequirement.inventoryItem) < interactRequirement.amount)
			{
				return false;
			}
			return true;
		}
		return true;
	}

	public void ConsumeRequiredItems(Network_Player player, QuestItemManager questItemManager)
	{
		if (Raft_Network.IsHost && interactRequirement != null)
		{
			if (interactRequirement.questItem != null)
			{
				QuestItemManager.AddQuestItem(interactRequirement.questItem.questItemType, -interactRequirement.amount, sendNotification: false);
				questItemManager.SendModifiedQuestItemsMessage();
			}
			if (interactRequirement.inventoryItem != null && player.IsLocalPlayer)
			{
				player.Inventory.RemoveItem(interactRequirement.inventoryItem.UniqueName, interactRequirement.amount);
			}
		}
	}

	private void RefreshUI()
	{
		SetUI(interactRequirement, itemExtract);
	}
}
