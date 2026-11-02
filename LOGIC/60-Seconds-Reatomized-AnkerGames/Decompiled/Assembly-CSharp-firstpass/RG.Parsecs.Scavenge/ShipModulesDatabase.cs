using UnityEngine;

namespace RG.Parsecs.Scavenge;

[CreateAssetMenu(fileName = "ShipModulesDatabase", menuName = "Scavenge/Ship Modules Database")]
public class ShipModulesDatabase : ScriptableObject
{
	public SceneField[] Left;

	public SceneField[] Right;
}
