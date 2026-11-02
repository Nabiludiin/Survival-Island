using UnityEngine;

public class ParticleManager : MonoBehaviour
{
	private static PoolManager poolManager;

	private void Start()
	{
		poolManager = ComponentManager<PoolManager>.Value;
	}

	public static GameObject PlaySystem(string poolName, Vector3 position, bool activateSystem = true)
	{
		GameObject gameObject = poolManager.GetObject(poolName, returnObjectActive: false);
		if (gameObject != null)
		{
			gameObject.transform.position = position;
			if (activateSystem && !gameObject.activeSelf)
			{
				gameObject.SetActive(value: true);
			}
		}
		return gameObject;
	}

	public static GameObject PlaySystem(string poolName, Vector3 position, Quaternion rotation, bool activateSystem = true)
	{
		GameObject gameObject = PlaySystem(poolName, position, activateSystem: false);
		if (gameObject != null)
		{
			gameObject.transform.rotation = rotation;
			if (activateSystem && !gameObject.activeSelf)
			{
				gameObject.SetActive(value: true);
			}
		}
		return gameObject;
	}

	public static GameObject PlaySystem(string poolName, Vector3 position, Transform parent, bool activateSystem = true)
	{
		GameObject gameObject = PlaySystem(poolName, position, activateSystem);
		if (gameObject != null)
		{
			gameObject.transform.SetParent(parent);
		}
		return gameObject;
	}

	public static GameObject PlaySystem(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
	{
		return Object.Instantiate(prefab, position, rotation, parent);
	}
}
