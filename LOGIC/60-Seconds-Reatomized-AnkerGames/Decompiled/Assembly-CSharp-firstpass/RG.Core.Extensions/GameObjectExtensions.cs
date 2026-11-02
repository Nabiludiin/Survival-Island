using System.Collections.Generic;
using RG.Utils;
using UnityEngine;

namespace RG.Core.Extensions;

public static class GameObjectExtensions
{
	public static T GetRequiredComponent<T>(this GameObject obj) where T : MonoBehaviour
	{
		T component = obj.GetComponent<T>();
		if (component == null)
		{
			Debug.LogError("Missing component: " + typeof(T), obj);
		}
		return component;
	}

	public static IList<T> FindInterfaceObjects<T>(this GameObject obj, bool firstOnly = false) where T : class
	{
		return InterfaceHelper.FindObjects<T>(firstOnly);
	}

	public static T FindInterfaceObject<T>(this GameObject obj) where T : class
	{
		return InterfaceHelper.FindObject<T>();
	}
}
