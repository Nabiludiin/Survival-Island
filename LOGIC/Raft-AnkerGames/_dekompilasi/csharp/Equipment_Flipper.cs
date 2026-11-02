using UnityEngine;

public class Equipment_Flipper : Equipment_Model
{
	[SerializeField]
	private float swimSpeedMultiplier = 1.4f;

	[SerializeField]
	private float characterOffset = 0.01f;

	private float timer;

	public override void UpdateEquipment()
	{
		base.UpdateEquipment();
		if (!(playerNetwork != null) || !(playerNetwork.PersonController != null) || playerNetwork.PersonController.controllerType != ControllerType.Water || !playerNetwork.PersonController.moving)
		{
			return;
		}
		timer += Time.deltaTime;
		if (timer >= 1f)
		{
			timer = 0f;
			if (equippedSlot != null)
			{
				equippedSlot.IncrementUses(-1);
			}
		}
	}

	public override void Equip(Slot_Equip equippedSlot)
	{
		base.Equip(equippedSlot);
		playerNetwork.PersonController.swimSpeed *= swimSpeedMultiplier;
		playerNetwork.Animator.transform.localPosition += Vector3.up * characterOffset;
	}

	public override void UnEquip()
	{
		base.UnEquip();
		playerNetwork.PersonController.swimSpeed /= swimSpeedMultiplier;
		playerNetwork.Animator.transform.localPosition -= Vector3.up * characterOffset;
	}
}
