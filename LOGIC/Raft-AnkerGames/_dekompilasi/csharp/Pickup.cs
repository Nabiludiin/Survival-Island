using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour, SceneEventInterface
{
	public static bool isHoveringOverPickup;

	public LayerMask pickupMask;

	private PlayerInventory playerInventory;

	private PlayerAnimator playerAnimator;

	private DisplayTextManager displayTextManager;

	private bool showingText;

	private Network_Player playerNetwork;

	private PickupItem pickupItem;

	private PickupChanneling pickupChanneling;

	private SoundManager soundManager;

	private CanvasHelper canvas;

	private List<IRaycastable> latestRaycastables;

	[SerializeField]
	private InputActionReference inputInteract;

	private void Start()
	{
		playerNetwork = GetComponent<Network_Player>();
		playerAnimator = playerNetwork.Animator;
		playerInventory = playerNetwork.Inventory;
		displayTextManager = ComponentManager<DisplayTextManager>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		soundManager = ComponentManager<SoundManager>.Value;
	}

	private void OnDestroy()
	{
		playerNetwork = null;
		playerAnimator = null;
		playerInventory = null;
		displayTextManager = null;
		canvas = null;
		soundManager = null;
	}

	private void Update()
	{
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		if (CanvasHelper.ActiveMenu != MenuType.None)
		{
			displayTextManager.HideDisplayTexts();
		}
		else
		{
			if (PlayerItemManager.throwableAnchorBusy)
			{
				return;
			}
			RaycastInteractable raycastInteractable = RaycastForRayInteractables();
			pickupItem = null;
			pickupChanneling = null;
			bool flag = false;
			if (raycastInteractable != null)
			{
				isHoveringOverPickup = true;
				if (raycastInteractable.CompareTag("Pickup_Shovel"))
				{
					pickupItem = raycastInteractable.GetComponentInChildren<PickupItem>();
					if (pickupItem != null)
					{
						if (pickupItem.pickupItemType != PickupItemType.QuestItem)
						{
							canvas.SetAimSprite(AimSprite.Shovelable);
						}
					}
					else
					{
						canvas.SetAimSprite(AimSprite.Shovelable);
					}
				}
				else if (raycastInteractable.CompareTag("PickupHook"))
				{
					canvas.SetAimSprite(AimSprite.Mineable);
				}
				else if (raycastInteractable.CompareTag("Resource"))
				{
					ResourceRegenerative componentInChildren = raycastInteractable.transform.GetComponentInChildren<ResourceRegenerative>();
					if (componentInChildren != null)
					{
						canvas.SetAimSprite(componentInChildren.resource.aimAtSprite);
					}
				}
				else if (raycastInteractable.CompareTag("MacheteInteract"))
				{
					canvas.SetAimSprite(AimSprite.Macheteable);
				}
				else if (raycastInteractable.CompareTag("Tree"))
				{
					canvas.SetAimSprite(AimSprite.Choppable);
				}
				else
				{
					canvas.SetAimSprite(AimSprite.Default);
				}
				pickupItem = raycastInteractable.transform.GetComponent<PickupItem>();
				pickupChanneling = raycastInteractable.transform.GetComponent<PickupChanneling>();
				flag = pickupItem != null && pickupItem.canBePickedUp && !pickupItem.CompareTag("NotPickupable");
				if (pickupChanneling != null && pickupChanneling.HasYield)
				{
					DisplayPickupName(isChanneledItem: true);
				}
				else if (flag && pickupItem.pickupItemType != PickupItemType.ItemNet)
				{
					DisplayPickupName();
				}
			}
			else
			{
				if (isHoveringOverPickup)
				{
					canvas.SetAimSprite(AimSprite.Default);
					isHoveringOverPickup = false;
				}
				if (showingText)
				{
					showingText = false;
					displayTextManager.HideDisplayTexts();
				}
			}
			if (flag && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputInteract, "Interact") && pickupItem != null)
			{
				PickupItemByType(pickupItem);
			}
		}
	}

	public void DisplayPickupName(bool isChanneledItem = false)
	{
		LocalizationParameters.itemX = pickupItem.PickupName;
		if (!isChanneledItem)
		{
			displayTextManager.ShowText(Helper.GetTerm("Game/PickUpX", applyParameters: true), inputInteract.action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 1);
		}
		else
		{
			displayTextManager.ShowText(Helper.GetTerm("Game/HoldToPickupX", applyParameters: true), inputInteract.action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 1);
		}
		showingText = true;
	}

	private RaycastInteractable RaycastForRayInteractables()
	{
		RaycastInteractable raycastInteractable = Helper.FindInteractable(Player.UseDistance * 1.1f);
		List<IRaycastable> list = null;
		if (raycastInteractable != null)
		{
			list = raycastInteractable.RaycastableObjects;
			if (latestRaycastables.ContainsItems())
			{
				foreach (IRaycastable latestRaycastable in latestRaycastables)
				{
					if (list == null || !list.Contains(latestRaycastable))
					{
						latestRaycastable.OnRayExit();
					}
				}
			}
			if (list.ContainsItems())
			{
				foreach (IRaycastable item in list)
				{
					if (item != null)
					{
						if (!latestRaycastables.ContainsItems() || !latestRaycastables.Contains(item))
						{
							item.OnRayEnter();
						}
						item.OnIsRayed();
					}
				}
			}
		}
		else if (latestRaycastables.ContainsItems())
		{
			foreach (IRaycastable latestRaycastable2 in latestRaycastables)
			{
				latestRaycastable2?.OnRayExit();
			}
		}
		if (!list.ContainsItems())
		{
			latestRaycastables = null;
		}
		else
		{
			latestRaycastables = list;
		}
		return raycastInteractable;
	}

	public void PickupItemByType(PickupItem pickupItem, bool forcePickup = false)
	{
		switch (pickupItem.pickupItemType)
		{
		case PickupItemType.Default:
		case PickupItemType.QuestItem:
			PickupItem(pickupItem, forcePickup);
			break;
		case PickupItemType.ItemNet:
			PickupItemNet(pickupItem as ItemNet);
			break;
		case PickupItemType.DomesticAnimal:
			PickupDomestic(pickupItem, forcePickup);
			break;
		case PickupItemType.NoteBookNote:
			PickupNoteBookNote(pickupItem as NoteBookNotePickup);
			break;
		default:
			PickupItem(pickupItem, forcePickup);
			break;
		}
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			isHoveringOverPickup = false;
		}
	}

	public bool GetIsHoveringOverPickup()
	{
		return isHoveringOverPickup;
	}

	public void AddItemToInventory(PickupItem item)
	{
		if (item == null)
		{
			return;
		}
		if (item.itemInstance != null && item.itemInstance.Valid)
		{
			playerInventory.AddItem(item.itemInstance);
			return;
		}
		if (item.yieldHandler != null)
		{
			if (item.yieldHandler.Yield.Count != 0)
			{
				foreach (Cost item2 in item.yieldHandler.Yield)
				{
					playerInventory.AddItem(item2.item.UniqueName, item2.amount);
				}
			}
		}
		else if (item != null && !(item is QuestItemPickup))
		{
			Debug.Log("Yield handler is null" + item.gameObject.name, gameObject);
		}
		if (item.dropper != null)
		{
			Item_Base[] randomItems = item.dropper.GetRandomItems();
			for (int i = 0; i < randomItems.Length; i++)
			{
				if (randomItems[i] != null)
				{
					playerInventory.AddItem(randomItems[i].UniqueName, 1);
				}
			}
		}
		if (item.specificPickups != null)
		{
			Pickup_Specific[] specificPickups = item.specificPickups;
			for (int j = 0; j < specificPickups.Length; j++)
			{
				specificPickups[j].PickupSpecific(playerInventory);
			}
		}
	}

	public void PickupItem(PickupItem pickup, bool forcePickup = false, bool triggerHandAnimation = true)
	{
		if (!forcePickup && !pickup.canBePickedUp)
		{
			return;
		}
		if (triggerHandAnimation)
		{
			playerAnimator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
		}
		if (pickup.networkID != null)
		{
			if (pickup.networkID.stopTrackUseRPC)
			{
				PickupObjectManager.RemovePickupItemNetwork(pickup.networkID, playerNetwork.steamID);
			}
			else
			{
				PickupObjectManager.RemovePickupItem(pickup.networkID, playerNetwork.steamID);
			}
		}
		else
		{
			AddItemToInventory(pickup);
			PoolManager.ReturnObject(pickup.gameObject);
		}
		displayTextManager.HideDisplayTexts();
	}

	private void PickupDomestic(PickupItem pickup, bool forcePickup = false)
	{
		if (!forcePickup && !pickup.canBePickedUp)
		{
			return;
		}
		playerAnimator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
		if (pickup.networkID != null)
		{
			pickup.itemInstance = new ItemInstance(ItemManager.GetItemByIndex(257), 1, 1);
			AI_StateMachine_Domestic aI_StateMachine_Domestic = pickup.GetComponent<AI_StateMachine_Domestic>();
			if (aI_StateMachine_Domestic == null)
			{
				aI_StateMachine_Domestic = pickup.GetComponentInParent<AI_StateMachine_Domestic>();
			}
			pickup.itemInstance.exclusiveString = aI_StateMachine_Domestic.transform.GetComponentInChildren<TextWriterObject>().GetText();
			if (pickup.networkID.stopTrackUseRPC)
			{
				PickupObjectManager.RemovePickupItemNetwork(pickup.networkID, playerNetwork.steamID);
			}
			else
			{
				PickupObjectManager.RemovePickupItem(pickup.networkID, playerNetwork.steamID);
			}
		}
		else
		{
			AddItemToInventory(pickup);
			PoolManager.ReturnObject(pickup.gameObject);
		}
		displayTextManager.HideDisplayTexts();
	}

	private void PickupItemNet(ItemNet itemNet)
	{
		if (itemNet.itemCollector.collectedItems.Count > 0)
		{
			playerAnimator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
			itemNet.itemCollector.AddCollectedItemsToPlayer(playerNetwork);
		}
	}

	private void PickupNoteBookNote(NoteBookNotePickup note, bool triggerHandAnimation = true)
	{
		if (triggerHandAnimation)
		{
			playerAnimator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
		}
		soundManager.PlayUI_MoveItem();
		if (note.networkID != null)
		{
			if (note.networkID.stopTrackUseRPC)
			{
				PickupObjectManager.RemovePickupItemNetwork(note.networkID, playerNetwork.steamID);
			}
			else
			{
				PickupObjectManager.RemovePickupItem(note.networkID, playerNetwork.steamID);
			}
		}
		note.noteBook.UnlockSpecificNoteNetworked(note.noteIndex, showNotification: true);
	}
}
