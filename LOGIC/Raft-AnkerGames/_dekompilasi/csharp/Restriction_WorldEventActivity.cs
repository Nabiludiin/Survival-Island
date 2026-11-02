using UnityEngine;

[CreateAssetMenu(fileName = "Restriction_WorldEventActivity", menuName = "Raft/Restriction/WorldEventIncompatability")]
public class Restriction_WorldEventActivity : Restriction
{
	public SO_WorldEvent.WorldEventType[] incompabilities;

	public override bool IsRestricted(WorldEvent we)
	{
		bool result = false;
		SO_WorldEvent.WorldEventType[] array = incompabilities;
		for (int i = 0; i < array.Length; i++)
		{
			if (WorldEventManager.GetWorldEventFromType(array[i]).isActive)
			{
				result = true;
				break;
			}
		}
		return result;
	}
}
