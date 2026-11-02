using System;
using UnityEngine;

[Serializable]
public class ItemInstance_Cookable
{
	[SerializeField]
	private float cookTime = 1f;

	[SerializeField]
	private Cost cookingResult;

	[SerializeField]
	private int cookingSlotsRequired = 1;

	public float CookingTime => cookTime;

	public Cost CookingResult => cookingResult;

	public int CookingSlotsRequired => cookingSlotsRequired;

	public ItemInstance_Cookable(int cookingSlotsRequired, float cookTime, Cost cookingResult)
	{
		this.cookingSlotsRequired = cookingSlotsRequired;
		this.cookTime = cookTime;
		this.cookingResult = cookingResult;
	}

	public ItemInstance_Cookable Clone()
	{
		return (ItemInstance_Cookable)MemberwiseClone();
	}
}
