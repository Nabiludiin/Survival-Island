using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseItemController : MonoBehaviour
{
	public Action<Item_Base> OnSelectItem;

	public Action<Item_Base> OnDeselectItem;

	public List<ItemConnection> allConnections = new List<ItemConnection>();

	[SerializeField]
	private Network_Player playerNetwork;

	[SerializeField]
	private PlayerAnimator playerAnimator;

	private Item_Base usableItem;

	private Dictionary<string, ItemConnection> connectionDictionary = new Dictionary<string, ItemConnection>();

	private ItemConnection activeObject;

	private bool canUse = true;

	[SerializeField]
	private StudioEventEmitter SoundEmitter_Equip;

	private InputAction fireAction;

	private void Awake()
	{
		foreach (ItemConnection allConnection in allConnections)
		{
			connectionDictionary.Add(allConnection.inventoryItem.UniqueName, allConnection);
			if (allConnection != null && allConnection.obj != null)
			{
				allConnection.SetActive(active: false);
			}
		}
	}

	private void Start()
	{
		fireAction = PlayerInput.GetPlayerByIndex(0).actions["Fire"];
	}

	private void Update()
	{
		if (!playerNetwork.IsLocalPlayer || !usableItem || !canUse || CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy)
		{
			return;
		}
		string useButtonName = usableItem.settings_usable.GetUseButtonName();
		if (string.IsNullOrEmpty(useButtonName))
		{
			return;
		}
		if (usableItem.settings_usable.AllowHoldButton())
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(fireAction, useButtonName))
			{
				Use();
			}
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(fireAction, useButtonName))
		{
			Use();
		}
	}

	public void StartUsing(Item_Base item)
	{
		string itemNameFromUsable = GetItemNameFromUsable(item);
		if (itemNameFromUsable != string.Empty)
		{
			usableItem = item;
			if (item.settings_usable.AnimationOnSelect != PlayerAnimation.None)
			{
				playerAnimator.SetAnimation(item.settings_usable.AnimationOnSelect, item.settings_usable.ForceAnimationIndex, item.settings_usable.SetTriggering);
			}
			if (playerNetwork.IsLocalPlayer && item.settings_recipe.CraftingCategory == CraftingCategory.Tools)
			{
				SoundEmitter_Equip.Play();
			}
			activeObject = connectionDictionary[itemNameFromUsable];
			activeObject.SetActive(active: true);
			activeObject.obj.SendMessage("OnSelect", SendMessageOptions.DontRequireReceiver);
			if (OnSelectItem != null)
			{
				OnSelectItem(item);
			}
		}
	}

	public void Deselect()
	{
		if (!(usableItem != null))
		{
			return;
		}
		string itemNameFromUsable = GetItemNameFromUsable(usableItem);
		if (!connectionDictionary.ContainsKey(itemNameFromUsable))
		{
			return;
		}
		if (activeObject != null)
		{
			activeObject.obj.SendMessage("OnDeSelect", SendMessageOptions.DontRequireReceiver);
			activeObject = null;
			if (OnDeselectItem != null)
			{
				OnDeselectItem(usableItem);
			}
		}
		if (playerNetwork.IsLocalPlayer && !string.IsNullOrEmpty(usableItem.settings_usable.ResetTriggerOnDeselect))
		{
			playerNetwork.Animator.anim.ResetTrigger(usableItem.settings_usable.ResetTriggerOnDeselect);
			playerNetwork.Animator.StopTriggering();
		}
		connectionDictionary[itemNameFromUsable].SetActive(active: false);
		usableItem = null;
	}

	public string GetItemNameFromUsable(Item_Base item)
	{
		_ = string.Empty;
		if (connectionDictionary.ContainsKey(item.UniqueName))
		{
			return item.UniqueName;
		}
		if (item.UniqueName.StartsWith("Placeable_Recipe_"))
		{
			return "Placeable_Recipe";
		}
		if (item.settings_buildable.Placeable)
		{
			return "Placeable";
		}
		if (item.UniqueName.StartsWith("Blueprint_"))
		{
			return "Blueprint";
		}
		return item.UniqueName;
	}

	public Item_Base GetCurrentItemInHand()
	{
		return usableItem;
	}

	private void Use()
	{
		if (canUse)
		{
			canUse = false;
			StartCoroutine(ResetUse(usableItem.settings_usable.UseButtonCooldown, usableItem));
			if (playerNetwork.IsLocalPlayer && usableItem.settings_usable.LockItemDuringCooldown)
			{
				PlayerItemManager.IsBusy = true;
			}
			if (usableItem.settings_usable.AnimationOnUse != PlayerAnimation.None)
			{
				playerAnimator.SetAnimation(usableItem.settings_usable.AnimationOnUse, usableItem.settings_usable.ForceAnimationIndex, usableItem.settings_usable.SetTriggering);
			}
			if (playerNetwork.IsLocalPlayer && usableItem.settings_usable.ConsumeUseAmount > 0)
			{
				StartCoroutine(ConsumeStacks(usableItem.settings_usable.ConsumeUseAmount, usableItem.settings_usable.UseButtonCooldown));
			}
			if (activeObject != null)
			{
				activeObject.obj.SendMessage("OnUse", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private IEnumerator ResetUse(float timeDelay, Item_Base usableItem)
	{
		yield return new WaitForSeconds(timeDelay);
		canUse = true;
		if (activeObject != null)
		{
			activeObject.obj.SendMessage("OnResetUse", SendMessageOptions.DontRequireReceiver);
		}
		if (playerNetwork.IsLocalPlayer && usableItem.settings_usable.LockItemDuringCooldown)
		{
			PlayerItemManager.IsBusy = false;
		}
	}

	private IEnumerator ConsumeStacks(int stacksToRemove, float timeDelay)
	{
		Slot selectedSlot = playerNetwork.Inventory.GetSelectedHotbarSlot();
		ItemInstance itemInstance = selectedSlot.itemInstance;
		yield return new WaitForSeconds(timeDelay);
		if (selectedSlot != null && !selectedSlot.IsEmpty && selectedSlot.itemInstance == itemInstance)
		{
			selectedSlot.IncrementUses(-stacksToRemove);
		}
	}
}
