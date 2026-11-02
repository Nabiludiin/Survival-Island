using UnityEngine;

namespace UltimateWater.Internal;

public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
			_Instance = new GameObject("[" + typeof(T).Name + "] - instance")
			{
				hideFlags = HideFlags.HideInHierarchy
			}.AddComponent<T>();
			return _Instance;
		}
	}

	private void OnApplicationQuit()
	{
		_Quiting = true;
	}

	private void OnDestroy()
	{
		_Quiting = true;
	}
}
