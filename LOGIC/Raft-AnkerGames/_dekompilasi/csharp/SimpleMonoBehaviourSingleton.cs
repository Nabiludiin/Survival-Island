using UnityEngine;

public class SimpleMonoBehaviourSingleton<T> : MonoBehaviour where T : SimpleMonoBehaviourSingleton<T>
{
	private static T s_instance;

	public static T Instance => s_instance;

	protected virtual void Awake()
	{
		if ((bool)s_instance)
		{
			Object.Destroy(gameObject);
			return;
		}
		s_instance = (T)this;
		Object.DontDestroyOnLoad(gameObject);
	}

	protected virtual void OnDestroy()
	{
		if (s_instance == (T)this)
		{
			s_instance = null;
		}
	}
}
