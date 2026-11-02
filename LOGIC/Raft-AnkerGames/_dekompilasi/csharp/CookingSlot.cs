using System;
using System.Collections.Generic;
using UnityEngine;

public class CookingSlot : MonoBehaviour
{
	public delegate void OnCookingSlotChanged(CookingSlot slot);

	public Action<CookingSlot> OnSlotModified;

	[SerializeField]
	private List<CookItemConnection> itemConnections = new List<CookItemConnection>();

	[SerializeField]
	private float cookTimeMultiplier = 1f;

	private Item_Base currentItem;

	private bool complete;

	private float cookTimer;

	private Collider[] colliders;

	public CookingSlot[] activeCookingSlots;

	public CookingSlot connectedSlot;

	public bool IsComplete => complete;

	public bool IsBusy
	{
		get
		{
			if (!(currentItem != null))
			{
				return FakeSlot;
			}
			return true;
		}
	}

	public bool FakeSlot => connectedSlot != null;

	public float CookTimer => cookTimer;

	public Item_Base CurrentItem => currentItem;

	public float CookingTimeLeft => CurrentItem.settings_cookable.CookingTime - CookTimer;

	public event OnCookingSlotChanged OnChangeEvent;

	private void Awake()
	{
		colliders = GetComponents<Collider>();
		SetColliderState(enabled: false);
		DisableItems();
		activeCookingSlots = null;
	}

	private void OnValidate()
	{
		foreach (CookItemConnection itemConnection in itemConnections)
		{
			if (itemConnection.cookableItem != null)
			{
				itemConnection.name = itemConnection.cookableItem.UniqueName;
			}
			else
			{
				itemConnection.name = "No cookable attached";
			}
		}
	}

	public void UpdateSlot()
	{
		if (!IsBusy || IsComplete || activeCookingSlots == null)
		{
			return;
		}
		cookTimer += Time.deltaTime;
		if (cookTimer >= currentItem.settings_cookable.CookingTime * cookTimeMultiplier)
		{
			OnComplete();
			if (OnChangeEvent != null)
			{
				OnChangeEvent(this);
			}
		}
	}

	public bool CanCookItem(Item_Base itemToCheck)
	{
		foreach (CookItemConnection itemConnection in itemConnections)
		{
			if (itemConnection.cookableItem.UniqueIndex == itemToCheck.UniqueIndex)
			{
				return true;
			}
		}
		return false;
	}

	public void StartCooking(Item_Base cookableItem, CookingSlot[] emptySlots)
	{
		activeCookingSlots = emptySlots;
		currentItem = cookableItem;
		EnableRawItem(cookableItem);
		for (int i = 1; i < activeCookingSlots.Length; i++)
		{
			CookingSlot cookingSlot = activeCookingSlots[i];
			if (cookingSlot != this)
			{
				cookingSlot.currentItem = cookableItem;
				cookingSlot.connectedSlot = this;
			}
		}
		OnSlotModified?.Invoke(this);
	}

	public void ResetSlot()
	{
		if (activeCookingSlots != null)
		{
			for (int i = 1; i < activeCookingSlots.Length; i++)
			{
				activeCookingSlots[i].ResetSlot();
			}
			activeCookingSlots = null;
		}
		connectedSlot = null;
		complete = false;
		currentItem = null;
		cookTimer = 0f;
		SetColliderState(enabled: false);
		DisableItems();
		OnSlotModified?.Invoke(this);
	}

	public void ForceSetTimer(float newTimerValue)
	{
		cookTimer = newTimerValue;
		if (currentItem != null && activeCookingSlots != null && CookTimer >= currentItem.settings_cookable.CookingTime * cookTimeMultiplier)
		{
			OnComplete();
			if (OnChangeEvent != null)
			{
				OnChangeEvent(this);
			}
		}
	}

	public void IncrementTimer(float amount)
	{
		cookTimer += amount;
	}

	public void EnableRawItem(Item_Base cookableItem)
	{
		foreach (CookItemConnection itemConnection in itemConnections)
		{
			if (itemConnection.cookableItem == cookableItem)
			{
				itemConnection.SetRawState(state: true);
				itemConnection.SetCookedState(state: false);
				break;
			}
		}
		OnSlotModified?.Invoke(this);
	}

	public void EnableCookedItem(Item_Base cookableItem)
	{
		foreach (CookItemConnection itemConnection in itemConnections)
		{
			if (itemConnection.cookableItem == cookableItem)
			{
				itemConnection.SetRawState(state: false);
				itemConnection.SetCookedState(state: true);
				break;
			}
		}
		OnSlotModified?.Invoke(this);
	}

	public void OnComplete()
	{
		complete = true;
		SetColliderState(enabled: true);
		if (activeCookingSlots != null)
		{
			EnableCookedItem(currentItem);
			for (int i = 1; i < activeCookingSlots.Length; i++)
			{
				activeCookingSlots[i].OnComplete();
			}
		}
	}

	private void DisableItems()
	{
		foreach (CookItemConnection itemConnection in itemConnections)
		{
			itemConnection.SetRawState(state: false);
			itemConnection.SetCookedState(state: false);
		}
	}

	private void SetColliderState(bool enabled)
	{
		Collider[] array = colliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = enabled;
		}
	}
}
