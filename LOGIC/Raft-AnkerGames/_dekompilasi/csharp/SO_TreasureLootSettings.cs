using UnityEngine;

[CreateAssetMenu(fileName = "TreasureLootSettings", menuName = "Raft/TreasureLootSettings")]
public class SO_TreasureLootSettings : ScriptableObject
{
	public Randomizer lootTable = new Randomizer();

	public Interval_Int numberOfTreasures = new Interval_Int();
}
