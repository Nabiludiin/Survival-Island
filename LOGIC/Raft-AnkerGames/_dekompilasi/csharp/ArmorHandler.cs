using UnityEngine;

public class ArmorHandler : MonoBehaviour
{
	private int totallArmorValue;

	private int minimumArmor;

	private int maximumArmor = 100;

	public float DamageReduction => Helper.MapValue(minimumArmor, maximumArmor, ArmorValue, 0f, 1f);

	public int ArmorValue => totallArmorValue;

	public void AddEquipmentPiece(int armorValue)
	{
		totallArmorValue += armorValue;
	}

	public void RemoveEquipmentPiece(int armorValue)
	{
		totallArmorValue -= armorValue;
	}
}
