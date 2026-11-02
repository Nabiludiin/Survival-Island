using UnityEngine;

public class LandmarkEntitySpawner_Trigger : LandmarkEntitySpawner
{
	private void OnTriggerEnter(Collider other)
	{
		if (Raft_Network.IsHost && !(spawnedEntityBehaviour != null) && other.CompareTag("Player") && other.GetComponent<Network_Player>() != null)
		{
			CreateEntity();
		}
	}
}
