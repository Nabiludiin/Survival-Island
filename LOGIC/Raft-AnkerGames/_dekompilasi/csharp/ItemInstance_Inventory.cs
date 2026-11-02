using System;
using UnityEngine;

[Serializable]
public class ItemInstance_Inventory
{
	[SerializeField]
	private Sprite sprite;

	[SerializeField]
	private string localizationTerm = "Item/...";

	[SerializeField]
	private string displayName = "An item";

	[SerializeField]
	private string description = "A description of an item";

	[SerializeField]
	private int stackSize = 1;

	public string LocalizationTerm
	{
		get
		{
			return localizationTerm;
		}
		set
		{
			localizationTerm = value;
		}
	}

	public string DisplayName
	{
		get
		{
			return displayName;
		}
		set
		{
			displayName = value;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	public Sprite Sprite
	{
		get
		{
			return sprite;
		}
		set
		{
			sprite = value;
		}
	}

	public int StackSize => stackSize;

	public bool Stackable => StackSize > 1;

	public ItemInstance_Inventory(Sprite sprite, string localizationTerm, int stackSize)
	{
		this.sprite = sprite;
		this.localizationTerm = localizationTerm;
		this.stackSize = stackSize;
	}

	public ItemInstance_Inventory Clone()
	{
		return (ItemInstance_Inventory)MemberwiseClone();
	}
}
