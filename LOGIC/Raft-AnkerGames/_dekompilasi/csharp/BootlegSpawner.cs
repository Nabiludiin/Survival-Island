using UnityEngine;

public class BootlegSpawner : MonoBehaviour
{
	public GameObject prefabToSpawn;

	public GameObject latestSpawned;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			latestSpawned = Object.Instantiate(prefabToSpawn, new Vector3(0f, 40f, 100f), Quaternion.identity, null);
		}
		if (Input.GetKeyDown(KeyCode.Alpha9) && latestSpawned != null)
		{
			if (latestSpawned.gameObject.activeInHierarchy)
			{
				latestSpawned.SetActiveSafe(value: false);
			}
			else
			{
				latestSpawned.SetActiveSafe(value: true);
			}
		}
	}
}
