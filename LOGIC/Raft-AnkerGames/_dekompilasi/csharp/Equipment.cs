using UnityEngine;

public class Equipment : MonoBehaviour
{
	public Item_Base equipableItem;

	[Tooltip("Send equip/unequip messages")]
	public bool sendNetworkInfo;

	[HideInInspector]
	public bool isInitialized;

	protected Slot_Equip equippedSlot;

	private bool equipped;

	public bool Equipped => equipped;

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnDestroy()
	{
		equippedSlot = null;
	}

	public virtual void Initialize(Network_Player playerNetwork)
	{
		isInitialized = true;
	}

	public virtual void UpdateEquipment()
	{
	}

	public virtual void Equip(Slot_Equip equippedSlot)
	{
		this.equippedSlot = equippedSlot;
		equipped = true;
	}

	public virtual void UnEquip()
	{
		equippedSlot = null;
		equipped = false;
	}
}
