using System;
using FMODUnity;
using UnityEngine;

[Serializable]
public class ItemInstance_Usable
{
	[SerializeField]
	private bool isUsable;

	[SerializeField]
	private bool allowHoldButton;

	[SerializeField]
	private string useButtonName = "LeftClick";

	[SerializeField]
	private float useButtonCooldown = 0.2f;

	[SerializeField]
	private PlayerAnimation animationOnSelect = PlayerAnimation.None;

	[SerializeField]
	private PlayerAnimation animationOnUse = PlayerAnimation.None;

	[SerializeField]
	private bool forceAnimationIndex;

	[SerializeField]
	private bool setTriggering;

	[SerializeField]
	private bool lockItemDuringCooldown;

	[SerializeField]
	private string resetTriggerOnDeselect = string.Empty;

	[SerializeField]
	private int consumeUseAmount;

	[SerializeField]
	[EventRef]
	[Tooltip("Playe when durability is 0")]
	private string eventRef_break;

	public float UseButtonCooldown => useButtonCooldown;

	public PlayerAnimation AnimationOnSelect => animationOnSelect;

	public PlayerAnimation AnimationOnUse => animationOnUse;

	public bool ForceAnimationIndex => forceAnimationIndex;

	public bool SetTriggering => setTriggering;

	public bool LockItemDuringCooldown => lockItemDuringCooldown;

	public string ResetTriggerOnDeselect => resetTriggerOnDeselect;

	public int ConsumeUseAmount => consumeUseAmount;

	public string EventRef_Break => eventRef_break;

	public ItemInstance_Usable(string useButtonName, float useButtonCooldown, int consumeUseAmount, bool isUsable, bool allowHoldButton, PlayerAnimation animationOnSelect, PlayerAnimation animationOnUse, bool forceAnimationIndex, bool setTriggering, bool lockDuringCooldown, string resetTrigger)
	{
		this.animationOnSelect = animationOnSelect;
		this.animationOnUse = animationOnUse;
		this.useButtonName = useButtonName;
		this.consumeUseAmount = consumeUseAmount;
		this.useButtonCooldown = useButtonCooldown;
		this.isUsable = isUsable;
		this.allowHoldButton = allowHoldButton;
		this.forceAnimationIndex = forceAnimationIndex;
		this.setTriggering = setTriggering;
		lockItemDuringCooldown = lockDuringCooldown;
		resetTriggerOnDeselect = resetTrigger;
	}

	public ItemInstance_Usable Clone()
	{
		return (ItemInstance_Usable)MemberwiseClone();
	}

	public string GetUseButtonName()
	{
		return useButtonName;
	}

	public bool IsUsable()
	{
		return isUsable;
	}

	public bool AllowHoldButton()
	{
		return allowHoldButton;
	}
}
