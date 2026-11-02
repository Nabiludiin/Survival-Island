using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace UltimateWater;

public static class Utilities
{
	public static int LayerMaskToInt(LayerMask mask)
	{
		for (int i = 0; i < 32; i++)
		{
			if ((mask.value & (1 << i)) != 0)
			{
				return i;
			}
		}
		return -1;
	}

	public static Water GetWaterReference()
	{
		return FindRefenrece<Water>();
	}

	public static bool IsNullReference<T>(this T obj, MonoBehaviour caller = null) where T : Object
	{
		if (obj != null)
		{
			return false;
		}
		string text = "";
		MethodBase method = new StackTrace().GetFrame(1).GetMethod();
		if (method != null && method.ReflectedType != null)
		{
			text = "[" + method.ReflectedType.Name + "]: ";
		}
		UnityEngine.Debug.LogError("[Ultimate Water System]" + text + "reference to the: " + typeof(T)?.ToString() + " not set.");
		if (caller != null)
		{
			caller.enabled = false;
		}
		return true;
	}

	public static void Destroy(this Object obj)
	{
		Object.Destroy(obj);
	}

	private static T FindRefenrece<T>() where T : MonoBehaviour
	{
		Object[] array = Object.FindObjectsOfType(typeof(T));
		if (array.Length == 0 || array.Length > 1)
		{
			return null;
		}
		return array[0] as T;
	}
}
