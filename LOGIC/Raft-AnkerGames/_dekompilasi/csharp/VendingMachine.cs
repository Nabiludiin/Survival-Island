using UnityEngine;

public class VendingMachine : MonoBehaviour_ID_Network
{
	public InteractableButton_Networked hatchButton;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private VendingMachine_Element[] vendingElements;

	[SerializeField]
	private TimerEventer checkPlayerDistanceTimer;

	[SerializeField]
	private GameObject canvasParent;

	[SerializeField]
	private float hideCanvasDistance = 10f;

	private VendingMachine_Element latestPurchase;

	private InteractableButton_Network_Forwarder buttonForwarder;

	private Raft_Network network;

	private QuestItemManager questItemManager;

	public int LatestPurchaseButtonIndex
	{
		get
		{
			if (!(latestPurchase == null))
			{
				return latestPurchase.networked_button.buttonIndex;
			}
			return -1;
		}
	}

	private void FindElements()
	{
		vendingElements = GetComponentsInChildren<VendingMachine_Element>(includeInactive: true);
	}

	private void Awake()
	{
		InteractableButton_Networked[] componentsInChildren = GetComponentsInChildren<InteractableButton_Networked>(includeInactive: true);
		buttonForwarder = new InteractableButton_Network_Forwarder(componentsInChildren);
	}

	protected virtual void Start()
	{
		questItemManager = ComponentManager<QuestItemManager>.Value;
		network = ComponentManager<Raft_Network>.Value;
	}

	private void Update()
	{
		if (checkPlayerDistanceTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
		{
			canvasParent.SetActiveSafe(Helper.LocalPlayerIsWithinDistance(transform.position, hideCanvasDistance));
		}
	}

	private void AddTokensToAnimator(int amount)
	{
		if (animator != null)
		{
			int integer = animator.GetInteger("Coins");
			animator.SetInteger("Coins", integer + amount);
		}
	}

	private VendingMachine_Element GetVendingMachineButton(int index)
	{
		VendingMachine_Element[] array = vendingElements;
		foreach (VendingMachine_Element vendingMachine_Element in array)
		{
			if (vendingMachine_Element.networked_button.buttonIndex == index)
			{
				return vendingMachine_Element;
			}
		}
		return null;
	}

	private bool ButtonPress(int buttonIndex, Network_UserId steamID)
	{
		if (buttonIndex < 0)
		{
			return false;
		}
		if (Raft_Network.IsHost)
		{
			VendingMachine_Element vendingMachineButton = GetVendingMachineButton(buttonIndex);
			Network_Player playerFromID = network.GetPlayerFromID(steamID);
			if (buttonIndex == hatchButton.buttonIndex && playerFromID != null && latestPurchase != null)
			{
				return true;
			}
			if (vendingMachineButton != null && playerFromID != null && latestPurchase == null && vendingMachineButton.HasInteractRequirement(playerFromID))
			{
				vendingMachineButton.ConsumeRequiredItems(playerFromID, questItemManager);
				latestPurchase = vendingMachineButton;
				return true;
			}
		}
		return false;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if (type == Messages.InteractableButtonPress || type == Messages.InteractableButtonPress_Int)
		{
			Message_InteractableButtonPress message_InteractableButtonPress = msg as Message_InteractableButtonPress;
			if (message_InteractableButtonPress.success)
			{
				if (Raft_Network.IsHost)
				{
					message_InteractableButtonPress.success = ButtonPress(message_InteractableButtonPress.buttonIndex, message_InteractableButtonPress.SteamID);
				}
				if (message_InteractableButtonPress.success)
				{
					Network_Player playerFromID = network.GetPlayerFromID(message_InteractableButtonPress.SteamID);
					if (playerFromID != null)
					{
						if (message_InteractableButtonPress.buttonIndex == hatchButton.buttonIndex)
						{
							PickupVendingHatch(playerFromID);
						}
						else
						{
							VendingMachine_Element vendingMachineButton = GetVendingMachineButton(message_InteractableButtonPress.buttonIndex);
							if (vendingMachineButton != null)
							{
								hatchButton.gameObject.SetActiveSafe(value: true);
								if (!Raft_Network.IsHost)
								{
									latestPurchase = vendingMachineButton;
								}
								AddTokensToAnimator(vendingMachineButton.interactRequirement.amount);
								animator?.SetBool("Package", latestPurchase != null);
								if (vendingMachineButton.interactRequirement != null && vendingMachineButton.interactRequirement.questItem != null && vendingMachineButton.interactRequirement.questItem.questItemType == QuestItemType.Tangaroa_Token)
								{
									AchievementHandler.AddPlayerTokenSpendTangaroa(vendingMachineButton.interactRequirement.amount);
								}
							}
						}
					}
				}
			}
			buttonForwarder.HandleMessage(message_InteractableButtonPress);
			return true;
		}
		return base.Deserialize(msg, remoteID);
	}

	private void PickupVendingHatch(Network_Player interactPlayer)
	{
		if (latestPurchase == null || interactPlayer == null)
		{
			Helper.LogBuild("Failed to loot vendingmachine hatch, latest purchase or interacting player was null");
			return;
		}
		if (Raft_Network.IsHost && latestPurchase.itemExtract.IsQuestItem)
		{
			QuestItemManager.AddQuestItem(latestPurchase.itemExtract.questItem.questItemType, latestPurchase.itemExtract.amount, sendNotification: true);
		}
		if (interactPlayer.IsLocalPlayer && latestPurchase != null && !latestPurchase.itemExtract.IsQuestItem)
		{
			interactPlayer.Inventory.AddItem(latestPurchase.itemExtract.inventoryItem.UniqueName, latestPurchase.itemExtract.amount);
		}
		latestPurchase = null;
		hatchButton.gameObject.SetActiveSafe(value: false);
		animator?.SetBool("Package", value: false);
	}

	public void RestoreVendingMachine(RGD_LandmarkItem_VendingMachine rgd_vending)
	{
		if (rgd_vending.lastPurchaseButtonIndex != -1)
		{
			VendingMachine_Element vendingMachineButton = GetVendingMachineButton(rgd_vending.lastPurchaseButtonIndex);
			if (vendingMachineButton != null)
			{
				latestPurchase = vendingMachineButton;
				hatchButton.gameObject.SetActiveSafe(value: true);
			}
		}
		animator?.SetBool("Package", latestPurchase != null);
	}
}
