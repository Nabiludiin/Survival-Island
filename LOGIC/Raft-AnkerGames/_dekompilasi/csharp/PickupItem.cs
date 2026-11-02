using UnityEngine;

public class PickupItem : MonoBehaviour
{
	public PickupItemType pickupItemType;

	public string pickupTerm;

	[SerializeField]
	protected string lastLocalizedTerm;

	[SerializeField]
	protected string lastLocalizedPickupName;

	public YieldHandler yieldHandler;

	[HideInInspector]
	public ItemInstance itemInstance;

	public PickupItem_Networked networkID;

	public bool canBePickedUp = true;

	public bool requiresHands;

	public RandomDropper dropper;

	public Pickup_Specific[] specificPickups;

	public bool isDropped;

	public string PickupName => GetPickupName();

	public virtual void Start()
	{
	}

	private void OnInspectorExtension()
	{
		GUILayout.Label("Pickup name: " + GetPickupName());
	}

	public void EditorAssignSpecificPickups()
	{
		specificPickups = GetComponentsInChildren<Pickup_Specific>();
	}

	protected virtual string GetPickupName()
	{
		if (itemInstance != null && itemInstance.Valid)
		{
			return ItemManager.GetItemByIndex(itemInstance.UniqueIndex).settings_Inventory.DisplayName;
		}
		if (!string.IsNullOrEmpty(pickupTerm))
		{
			string term = Helper.GetTerm(pickupTerm);
			if (term.IsNullOrEmpty())
			{
				return string.Empty;
			}
			if (lastLocalizedTerm.IsNullOrEmpty() || !lastLocalizedTerm.Equals(term))
			{
				if (term.Contains("@"))
				{
					lastLocalizedTerm = term;
					string[] array = term.Split('@');
					if (array.ContainsItems())
					{
						lastLocalizedPickupName = array[0];
						return lastLocalizedPickupName;
					}
					return string.Empty;
				}
				lastLocalizedTerm = term;
				lastLocalizedPickupName = term;
				return lastLocalizedPickupName;
			}
			return lastLocalizedPickupName;
		}
		if (yieldHandler != null && yieldHandler.Yield.Count > 0 && yieldHandler.Yield[0] != null)
		{
			return yieldHandler.Yield[0].item.settings_Inventory.DisplayName;
		}
		return Helper.GetTerm("Item/Item");
	}
}
