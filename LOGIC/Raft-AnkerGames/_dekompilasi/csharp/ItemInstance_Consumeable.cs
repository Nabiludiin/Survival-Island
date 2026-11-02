using System;
using FMODUnity;
using UnityEngine;

[Serializable]
public class ItemInstance_Consumeable
{
	[SerializeField]
	private FoodType foodType;

	[SerializeField]
	private FoodForm foodForm;

	[SerializeField]
	private float oxygenYield;

	[SerializeField]
	private float hungerYield;

	[SerializeField]
	private float bonusHungerYield;

	[SerializeField]
	private float thirstYield;

	[SerializeField]
	private float bonusThirstYield;

	[SerializeField]
	private bool isRaw = true;

	[SerializeField]
	[EventRef]
	private string eventRef_consumeSound = string.Empty;

	[SerializeField]
	private Cost itemAfterUse;

	public FoodType FoodType => foodType;

	public FoodForm FoodForm => foodForm;

	public float OxygenYield => oxygenYield;

	public float HungerYield => hungerYield;

	public float BonusHungerYield => bonusHungerYield;

	public float ThirstYield => thirstYield;

	public float BonusThirstYield => bonusThirstYield;

	public bool IsRaw => isRaw;

	public string EventRef_ConsumeSound => eventRef_consumeSound;

	public Cost ItemAfterUse => itemAfterUse;

	public ItemInstance_Consumeable(float hungerYield, float bonusHungerYield, float thirstYield, float oxygenYield, bool isRaw, Cost itemAfterUse, FoodType foodType, FoodForm foodForm)
	{
		this.hungerYield = hungerYield;
		this.oxygenYield = oxygenYield;
		this.bonusHungerYield = bonusHungerYield;
		this.thirstYield = thirstYield;
		this.isRaw = isRaw;
		this.itemAfterUse = itemAfterUse;
		this.foodType = foodType;
		this.foodForm = foodForm;
	}

	public ItemInstance_Consumeable Clone()
	{
		return (ItemInstance_Consumeable)MemberwiseClone();
	}
}
