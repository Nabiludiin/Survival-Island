using Knife.DeferredDecals;
using UnityEngine;

public class DecalSystemRaft : DeferredDecalsSystem
{
	private bool initialized;

	private void Awake()
	{
		if (initialized)
		{
			Object.Destroy(gameObject);
			return;
		}
		initialized = true;
		DontDestroyOnLoadManager.AddGameObject(gameObject);
	}
}
