using UnityEngine;

public class Stat_Bonus_Consumable : Stat_Bonus
{
	[HideInInspector]
	public Stat_Consumable normalConsumable;

	[HideInInspector]
	public Stat_Consumable bonusConsumable;

	private void Awake()
	{
		normalConsumable = Normal as Stat_Consumable;
		bonusConsumable = Bonus as Stat_Consumable;
	}

	private void Update()
	{
		if (!GameModeValueManager.IsPlayerInvurnerable)
		{
			if (normalConsumable.Consuming || (bonusConsumable.Value <= 0f && !bonusConsumable.Consuming))
			{
				normalConsumable.HandleValue();
			}
			else
			{
				bonusConsumable.HandleValue();
			}
		}
	}

	public void Consume(float yield, float bonusYield, bool isRaw)
	{
		if (bonusYield != 0f)
		{
			if (yield != 0f)
			{
				normalConsumable.Consume(yield);
			}
			bonusConsumable.Consume(bonusYield);
		}
		else if (yield != 0f)
		{
			normalConsumable.ConsumeWithManipulatorCurve(yield, isRaw);
		}
	}

	public void RemoveValue(float amount)
	{
		if (Bonus.Value > 0f)
		{
			float num = Bonus.Value - amount;
			if (num < 0f)
			{
				Bonus.Value -= amount + num;
				Normal.Value -= num;
			}
			else
			{
				Bonus.Value -= amount;
			}
		}
		else
		{
			Normal.Value -= amount;
		}
	}
}
