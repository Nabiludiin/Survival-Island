using UnityEngine;

[CreateAssetMenu(fileName = "FuelValue", menuName = "Raft/Fuel/FuelValue")]
public class SO_FuelValue : ScriptableObject
{
	public Item_Base itemType;

	public int fuelValueOfType;

	public FuelValue_FiltrationType fuelFiltrationType;
}
