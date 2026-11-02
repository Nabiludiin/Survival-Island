using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoaderManager : MonoBehaviour
{
	public string testSceneName;

	public void LoadSceneAsync(int p_sceneIndex)
	{
	}

	public void LoadTestScene()
	{
		StartCoroutine(LoadSceneAsync(testSceneName));
	}

	public IEnumerator LoadSceneAsync(string p_sceneName)
	{
		AsyncOperation ppp = SceneManager.LoadSceneAsync(p_sceneName, LoadSceneMode.Additive);
		while (ppp.isDone)
		{
			yield return null;
		}
		Debug.Log("Should be finished loading now");
	}
}
