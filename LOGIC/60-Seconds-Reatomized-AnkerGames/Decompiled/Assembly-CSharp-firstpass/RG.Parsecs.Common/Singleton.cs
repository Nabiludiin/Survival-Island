using UnityEngine;

namespace RG.Parsecs.Common;

public class Singleton<T> : MonoBehaviourExtended where T : MonoBehaviourExtended
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null && Application.isPlaying)
			{
				Debug.LogWarning("There is no " + typeof(T)?.ToString() + " instantiated on scene. Instantiate object first.");
			}
			return _instance;
		}
	}

	protected void Awake()
	{
		if (_instance == null)
		{
			_instance = Object.FindObjectOfType<T>();
			Object.DontDestroyOnLoad(gameObject);
		}
		else if (_instance != this)
		{
			Object.Destroy(gameObject);
		}
	}

	protected virtual void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = null;
		}
	}
}
