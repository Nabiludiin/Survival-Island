using UnityEngine;

public class Equipment_ArmorPiece : Equipment_Model
{
	public int armorValue;

	public bool hasDurability;

	[SerializeField]
	private ArmorHandler armorHandler;

	[SerializeField]
	private Network_Entity stats;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		stats = null;
	}

	public override void Equip(Slot_Equip equippedSlot)
	{
		base.Equip(equippedSlot);
		if (armorHandler != null)
		{
			armorHandler.AddEquipmentPiece(armorValue);
		}
		if (hasDurability && playerNetwork.IsLocalPlayer)
		{
			stats.OnDamageTakenEvent += RemoveDurability;
		}
	}

	public override void UnEquip()
	{
		base.UnEquip();
		if (armorHandler != null)
		{
			armorHandler.RemoveEquipmentPiece(armorValue);
		}
		if (hasDurability && playerNetwork.IsLocalPlayer)
		{
			stats.OnDamageTakenEvent -= RemoveDurability;
		}
	}

	private void RemoveDurability(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageInflictorEntityType, bool isLocal)
	{
		if (damageInflictorEntityType == EntityType.Enemy || damageInflictorEntityType == EntityType.Player)
		{
			equippedSlot.IncrementUses(-1);
		}
	}
}
