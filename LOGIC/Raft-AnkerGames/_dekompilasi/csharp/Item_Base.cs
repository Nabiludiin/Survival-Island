using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Base", menuName = "Raft/Item/Item_Base", order = 1)]
public class Item_Base : ScriptableObject, IComparable<Item_Base>
{
	[SerializeField]
	private string uniqueName;

	[SerializeField]
	private int uniqueIndex;

	private bool hasBeenInitialized;

	[SerializeField]
	[Min(0f)]
	private int maxUses = 1;

	[SerializeField]
	private Gradient barGradient;

	public ItemInstance_Cookable settings_cookable;

	public ItemInstance_Consumeable settings_consumeable;

	public ItemInstance_Usable settings_usable;

	public ItemInstance_Inventory settings_Inventory;

	public ItemInstance_Buildable settings_buildable;

	public ItemInstance_Equipment settings_equipment;

	public ItemInstance_Recipe settings_recipe;

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

	public int MaxUses => maxUses;

	public Gradient BarGradient => barGradient;

	public void Initialize(int uniqueIndex, string uniqueName, int maxUses)
	{
		if (!hasBeenInitialized)
		{
			hasBeenInitialized = true;
			this.uniqueIndex = uniqueIndex;
			this.uniqueName = uniqueName;
			this.maxUses = maxUses;
		}
	}

	private void OnValidate()
	{
	}

	public int CompareTo(Item_Base other)
	{
		return uniqueIndex.CompareTo(other.uniqueIndex);
	}
}
