using System;

[Serializable]
public class PlayerSpecificVariables
{
	public bool clearInventoryOnSurrender;

	public bool leaveSomeItems;

	public float damageTakenMultiplier = 1f;

	public bool canSurrender = true;

	public bool alwaysHealthy;

	public bool unlimitedResources = true;

	public bool giveStartingItems;

	public bool learnAllRecipiesAtStart = true;

	public bool negateOutgoingPlayerDamage;

	public float outgoingDamageMultiplierPVE = 1f;

	public bool recieveFallDamage;

	public bool allowNoclip;
}
