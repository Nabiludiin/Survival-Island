using UnityEngine;

public class ObjectSpawnerNoSpawnZone : MonoBehaviour
{
	public float radius = 10f;

	private void OnEnable()
	{
		if (ObjectSpawner_RaftDirection.NoSpawnZones != null)
		{
			ObjectSpawner_RaftDirection.NoSpawnZones.AddUniqueOnly(this);
		}
	}

	private void OnDisable()
	{
		if (ObjectSpawner_RaftDirection.NoSpawnZones != null)
		{
			ObjectSpawner_RaftDirection.NoSpawnZones.Remove(this);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
