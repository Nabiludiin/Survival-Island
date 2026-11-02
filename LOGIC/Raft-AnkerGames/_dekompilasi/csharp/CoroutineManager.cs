using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
	private static CoroutineManager singleton;

	private static bool initialized;

	public static CoroutineManager Singleton
	{
		get
		{
			if (singleton == null)
			{
				Object.FindObjectOfType<CoroutineManager>();
			}
			return singleton;
		}
	}

	private void Awake()
	{
		if (!initialized)
		{
			initialized = true;
			singleton = this;
		}
	}

	public Coroutine AddCoroutine(IEnumerator routine)
	{
		if (gameObject.activeInHierarchy)
		{
			return StartCoroutine(routine);
		}
		return null;
	}
}
