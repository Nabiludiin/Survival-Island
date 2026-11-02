using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadManager
{
	private static List<GameObject> dontDestroyGameObjs = new List<GameObject>();

	public static void AddGameObject(GameObject obj)
	{
		if (!dontDestroyGameObjs.Contains(obj))
		{
			Object.DontDestroyOnLoad(obj);
			dontDestroyGameObjs.Add(obj);
		}
	}

	public static List<GameObject> GetGameObjects()
	{
		return dontDestroyGameObjs;
	}
}
