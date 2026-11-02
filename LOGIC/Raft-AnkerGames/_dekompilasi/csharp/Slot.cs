using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
	public ItemInstance itemInstance;

	public SlotType slotType = SlotType.Normal;

	public bool locked;

	public bool active = true;

	public Image imageComponent;

	public Text textComponent;

	public Slider sliderComponent;

	public Image sliderFillComponent;

	public Image imageFocused;

	[HideInInspector]
	public RectTransform rectTransform;

	public Inventory inventory;

	public bool IsEmpty
	{
		get
		{
			if (itemInstance != null)
			{
				return !itemInstance.Valid;
			}
			return true;
		}
	}

	protected virtual void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void InitializeEventListeners(Inventory inventory)
	{
		this.inventory = inventory;
	}

	protected virtual void OnDestroy()
	{
		inventory = null;
	}

	protected virtual void Start()
	{
		RefreshComponents();
	}

	private void OnEnable()
	{
		if (sliderComponent != null && sliderComponent.gameObject.activeInHierarchy && IsEmpty)
		{
			sliderComponent.gameObject.SetActive(value: false);
		}
	}

	public virtual void SetItem(ItemInstance newInstance)
	{
		if (newInstance != null)
		{
			itemInstance = newInstance.Clone();
		}
		else
		{
			itemInstance = null;
		}
		RefreshComponents();
	}

	public virtual void SetItem(Item_Base newItem, int amount)
	{
		if (newItem != null)
		{
			itemInstance = new ItemInstance(newItem, amount, newItem.MaxUses);
		}
		else
		{
			itemInstance = null;
		}
		RefreshComponents();
	}

	public void AddItem(Item_Base newItem, int amount = 1)
	{
		if (!ValidInstance(itemInstance))
		{
			itemInstance = new ItemInstance(newItem, amount, newItem.MaxUses);
		}
		else if (itemInstance.UniqueIndex == newItem.UniqueIndex)
		{
			itemInstance.Amount += amount;
		}
		RefreshComponents();
	}

	public void RemoveItem(int amount)
	{
		if (!IsEmpty)
		{
			itemInstance.Amount -= amount;
			if (itemInstance.Amount <= 0)
			{
				SetItem(null);
			}
			else
			{
				RefreshComponents();
			}
		}
	}

	public void SetUses(int numberOfUses)
	{
		if (!IsEmpty)
		{
			int amountOfUsesToAdd = numberOfUses - itemInstance.Uses;
			IncrementUses(amountOfUsesToAdd);
		}
	}

	public void IncrementUses(int amountOfUsesToAdd, bool addItemAfterUseToInventory = true)
	{
		if (IsEmpty || (itemInstance.HasMaxUses && amountOfUsesToAdd > 0))
		{
			return;
		}
		itemInstance.Uses += amountOfUsesToAdd;
		if (itemInstance.Uses <= 0 && itemInstance.Amount > 0)
		{
			itemInstance.Amount--;
			if (itemInstance.Amount >= 1)
			{
				itemInstance.SetUsesToMax();
				Cost itemAfterUse = itemInstance.settings_consumeable.ItemAfterUse;
				if (addItemAfterUseToInventory && itemAfterUse.item != null)
				{
					SetItem(itemAfterUse.item, itemAfterUse.amount);
				}
			}
			else
			{
				Cost itemAfterUse2 = itemInstance.settings_consumeable.ItemAfterUse;
				if (addItemAfterUseToInventory && itemAfterUse2.item != null)
				{
					SetItem(itemAfterUse2.item, itemAfterUse2.amount);
				}
				else
				{
					SetItem(null);
				}
				if (inventory is PlayerInventory)
				{
					(inventory as PlayerInventory).hotbar.ReselectCurrentSlot();
				}
			}
		}
		RefreshComponents();
	}

	public int GetMaxStackUses()
	{
		if (IsEmpty)
		{
			return 0;
		}
		return itemInstance.baseItem.settings_Inventory.StackSize * itemInstance.baseItem.MaxUses;
	}

	public int GetCurrentTotalUses()
	{
		if (IsEmpty)
		{
			return 0;
		}
		return itemInstance.Uses + (itemInstance.Amount - 1) * itemInstance.baseItem.MaxUses;
	}

	public bool StackIsFull()
	{
		if (IsEmpty)
		{
			return false;
		}
		if (!itemInstance.settings_Inventory.Stackable)
		{
			return true;
		}
		if (itemInstance.Amount >= itemInstance.settings_Inventory.StackSize)
		{
			return true;
		}
		return false;
	}

	public void RefreshComponents()
	{
		if (imageComponent != null)
		{
			if (IsEmpty)
			{
				imageComponent.sprite = null;
			}
			else
			{
				imageComponent.sprite = itemInstance.settings_Inventory.Sprite;
			}
			imageComponent.enabled = imageComponent.sprite != null;
		}
		if (!IsEmpty && itemInstance.Amount > 1)
		{
			textComponent.text = itemInstance.Amount.ToString();
		}
		else
		{
			textComponent.text = string.Empty;
		}
		if (sliderComponent != null)
		{
			if (IsEmpty || itemInstance.baseItem.MaxUses <= 1)
			{
				sliderComponent.gameObject.SetActiveSafe(value: false);
				return;
			}
			sliderComponent.gameObject.SetActiveSafe(value: true);
			sliderComponent.value = itemInstance.NormalizedUses;
			sliderFillComponent.color = itemInstance.baseItem.BarGradient.Evaluate(sliderComponent.value);
		}
	}

	public void Reset()
	{
		SetItem(null);
		RefreshComponents();
	}

	public Item_Base GetItemBase()
	{
		if (!ValidInstance(itemInstance))
		{
			return null;
		}
		return itemInstance.baseItem;
	}

	public bool HasValidItemInstance()
	{
		return ValidInstance(itemInstance);
	}

	protected bool ValidInstance(ItemInstance instance)
	{
		return instance?.Valid ?? false;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		inventory.HoverExit(this, eventData);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		inventory.HoverEnter(this, eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!locked && PlayerInput.GetPlayerByIndex(0).currentControlScheme != "Gamepad")
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				inventory.MoveItem(this, eventData.button);
			}
			else
			{
				inventory.ShiftMoveItem(this);
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!locked && PlayerInput.GetPlayerByIndex(0).currentControlScheme != "Gamepad")
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				inventory.MoveItem(this, eventData.button);
			}
			inventory.ResetMoveSlots();
		}
	}

	public void Focus(bool value)
	{
		imageFocused.gameObject.SetActive(value);
	}
}
