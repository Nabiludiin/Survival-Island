using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RG.Parsecs.Scavenge;

public class ModuleLoader
{
	public string SceneName;

	private Scene _scene;

	private AsyncOperation _loadingOperation;

	private List<GameObject> _rootGameObjects = new List<GameObject>(4);

	public Module Module { get; protected set; }

	public bool IsLoaded { get; protected set; }

	public Scene Scene => _scene;

	public ModuleLoader(string name)
	{
		SceneName = name;
	}

	public IEnumerator Load(Vector3 position, Action<ModuleLoader, Module> callback)
	{
		_loadingOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
		while (!_loadingOperation.isDone)
		{
			yield return null;
		}
		_scene = SceneManager.GetSceneByName(SceneName);
		_scene.GetRootGameObjects(_rootGameObjects);
		for (int i = 0; i < _rootGameObjects.Count; i++)
		{
			GameObject gameObject = _rootGameObjects[i];
			gameObject.transform.position = position;
			if (Module == null)
			{
				Module = gameObject.GetComponent<Module>();
			}
		}
		Module.Load();
		IsLoaded = true;
		yield return null;
		yield return null;
		callback?.Invoke(this, Module);
	}
}
