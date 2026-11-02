using UltimateWater;
using UnityEngine;

public class Equipment_AirBottle : Equipment_Model
{
	[SerializeField]
	private float oxygenLostMultiplier = 0.6f;

	private float timer;

	public override void UpdateEquipment()
	{
		base.UpdateEquipment();
		if (!(playerNetwork != null) || !(playerNetwork.PersonController != null) || playerNetwork.PersonController.SubmersionState != SubmersionState.Full)
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
		float oxygenLostPerSecond = playerNetwork.Stats.stat_oxygen.GetOxygenLostPerSecond();
		playerNetwork.Stats.stat_oxygen.SetOxygenLostPerSecond(oxygenLostPerSecond * oxygenLostMultiplier);
	}

	public override void UnEquip()
	{
		base.UnEquip();
		float oxygenLostPerSecond = playerNetwork.Stats.stat_oxygen.GetOxygenLostPerSecond();
		playerNetwork.Stats.stat_oxygen.SetOxygenLostPerSecond(oxygenLostPerSecond / oxygenLostMultiplier);
	}
}
