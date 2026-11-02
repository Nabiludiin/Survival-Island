using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class AsyncedLandmark
{
	public bool isSpawned;

	[HideInInspector]
	public AsyncOperation aO;

	public Scene scene;

	public string sceneName;

	public Landmark landmark;

	public AsyncedLandmark(AsyncOperation p_aO, string p_sceneName)
	{
		aO = p_aO;
		sceneName = p_sceneName;
	}

	public AsyncedLandmark(string p_sceneName)
	{
		sceneName = p_sceneName;
	}
}
