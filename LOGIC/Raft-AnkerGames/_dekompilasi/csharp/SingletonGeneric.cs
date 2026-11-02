using UnityEngine;

public class SingletonGeneric<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T singleton;

	public static T Singleton
	{
		get
		{
			if (singleton == null)
			{
				singleton = Object.FindObjectOfType<T>();
			}
			return singleton;
		}
	}
}
