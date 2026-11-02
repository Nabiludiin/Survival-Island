using UnityEngine;

namespace UltimateWater.Internal;

public class ApplicationSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _Instance;

	private static bool _Quiting;

	public static T Instance
	{
		get
		{
			if (_Instance != null)
			{
				return _Instance;
			}
			_Instance = (T)Object.FindObjectOfType(typeof(T));
			if (_Instance != null)
			{
				return _Instance;
			}
			if (_Quiting)
			{
				return null;
			}
			GameObject gameObject = new GameObject("[" + typeof(T).Name + "] - instance")
			{
				hideFlags = HideFlags.HideInHierarchy
			};
			Object.DontDestroyOnLoad(gameObject);
			_Instance = gameObject.AddComponent<T>();
			return _Instance;
		}
	}

	protected virtual void OnApplicationQuit()
	{
		_Quiting = true;
	}

	protected virtual void OnDestroy()
	{
		_Quiting = true;
	}
}
