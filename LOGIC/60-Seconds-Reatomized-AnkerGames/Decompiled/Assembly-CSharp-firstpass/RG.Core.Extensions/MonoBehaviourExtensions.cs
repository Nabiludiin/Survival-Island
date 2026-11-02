using System;
using System.Collections;
using UnityEngine;

namespace RG.Core.Extensions;

public static class MonoBehaviourExtensions
{
	public static T GetRequiredComponent<T>(this MonoBehaviour obj) where T : MonoBehaviour
	{
		T component = obj.GetComponent<T>();
		if (component == null)
		{
			Debug.LogErrorFormat(obj.gameObject, "Missing component: {0}", typeof(T).Name);
		}
		return component;
	}

	public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
	{
		return monoBehaviour.StartCoroutine(InvokeImpl(action, time));
	}

	private static IEnumerator InvokeImpl(Action action, float time)
	{
		yield return new WaitForSeconds(time);
		action();
	}
}
