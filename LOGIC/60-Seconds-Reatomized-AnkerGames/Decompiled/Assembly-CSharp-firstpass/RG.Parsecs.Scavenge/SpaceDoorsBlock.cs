using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SpaceDoorsBlock : MonoBehaviour
{
	public SpaceDoors doors;

	private void OnTriggerEnter(Collider other)
	{
		doors.blockersInDoors.Add(other.gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		doors.blockersInDoors.Remove(other.gameObject);
	}
}
