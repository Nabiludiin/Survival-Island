using System;
using UnityEngine;

[Serializable]
public class ItemInstance
{
	private int amount;

	[SerializeField]
	private int uses = 1;

	public Item_Base baseItem;

	private string uniqueName;

	private int uniqueIndex;

	public ItemInstance_Cookable settings_cookable;

	public ItemInstance_Consumeable settings_consumeable;

	public ItemInstance_Usable settings_usable;

	public ItemInstance_Inventory settings_Inventory;

	public ItemInstance_Buildable settings_buildable;

	public ItemInstance_Equipment settings_equipment;

	public ItemInstance_Recipe settings_recipe;

	public string exclusiveString;

	public int Amount
	{
		get
		{
			return amount;
		}
		set
		{
			amount = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public float NormalizedUses => (float)Uses / (float)BaseItemMaxUses;

	public int UsesInStack => Mathf.Clamp(Amount - 1, 0, int.MaxValue) * BaseItemMaxUses + Uses;

	public int Uses
	{
		get
		{
			return uses;
		}
		set
		{
			uses = Mathf.Clamp(value, 0, BaseItemMaxUses);
		}
	}

	public int BaseItemMaxUses
	{
		get
		{
			if (!(baseItem == null))
			{
				return baseItem.MaxUses;
			}
			return int.MaxValue;
		}
	}

	public bool HasMaxUses => uses == BaseItemMaxUses;

	public bool Valid
	{
		get
		{
			if (baseItem != null)
			{
				return !string.IsNullOrEmpty(uniqueName);
			}
			return false;
		}
	}

	public string UniqueName
	{
		get
		{
			return uniqueName;
		}
		private set
		{
			uniqueName = value;
		}
	}

	public int UniqueIndex
	{
		get
		{
			return uniqueIndex;
		}
		private set
		{
			uniqueIndex = value;
		}
	}

	public ItemInstance()
	{
	}

	public ItemInstance(Item_Base itemBase, int amount, int uses, string exclusiveString = "")
	{
		baseItem = itemBase;
		this.amount = amount;
		this.uses = uses;
		this.exclusiveString = exclusiveString;
		uniqueIndex = itemBase.UniqueIndex;
		uniqueName = itemBase.UniqueName;
		settings_cookable = itemBase.settings_cookable.Clone();
		settings_consumeable = itemBase.settings_consumeable.Clone();
		settings_usable = itemBase.settings_usable.Clone();
		settings_Inventory = itemBase.settings_Inventory.Clone();
		settings_buildable = itemBase.settings_buildable.Clone();
		settings_equipment = itemBase.settings_equipment.Clone();
		settings_recipe = itemBase.settings_recipe.Clone();
	}

	public ItemInstance Clone()
	{
		return new ItemInstance
		{
			baseItem = baseItem,
			Amount = Amount,
			Uses = Uses,
			uniqueIndex = uniqueIndex,
			uniqueName = uniqueName,
			exclusiveString = exclusiveString,
			settings_cookable = settings_cookable.Clone(),
			settings_consumeable = settings_consumeable.Clone(),
			settings_usable = settings_usable.Clone(),
			settings_Inventory = settings_Inventory.Clone(),
			settings_buildable = settings_buildable.Clone(),
			settings_equipment = settings_equipment.Clone(),
			settings_recipe = settings_recipe.Clone()
		};
	}

	public void SetUsesToMax()
	{
		Uses = BaseItemMaxUses;
	}
}
