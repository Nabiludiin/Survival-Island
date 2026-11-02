using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class GlobalEventHubResolver : MonoBehaviour
{
	private Dictionary<Type, MonoBehaviour> _dictionary = new Dictionary<Type, MonoBehaviour>();

	public static GlobalEventHubResolver I { get; protected set; }

	protected void Awake()
	{
		if (I != null)
		{
			Debug.LogError("More than one instance of GlobalActorEventsHub in the scene");
		}
		else
		{
			I = this;
		}
	}

	public void RegisterEventHub<T>(T hub) where T : MonoBehaviour
	{
		Type typeFromHandle = typeof(T);
		if (_dictionary.ContainsKey(typeFromHandle))
		{
			_dictionary[typeFromHandle] = hub;
		}
		else
		{
			_dictionary.Add(typeFromHandle, hub);
		}
	}

	public T GetEventHub<T>() where T : MonoBehaviour
	{
		MonoBehaviour value = null;
		Type typeFromHandle = typeof(T);
		if (!_dictionary.TryGetValue(typeFromHandle, out value))
		{
			Debug.LogErrorFormat("GlobalEventHubResolver.GetEventHub there is no GlobalHub for {0}", typeFromHandle.Name);
		}
		return value as T;
	}

	private void OnDestroy()
	{
		if (I == this)
		{
			I = null;
		}
	}
}
