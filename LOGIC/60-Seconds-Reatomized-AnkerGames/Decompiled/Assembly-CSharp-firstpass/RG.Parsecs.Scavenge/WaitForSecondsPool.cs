using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public static class WaitForSecondsPool
{
	private static Dictionary<float, Stack<WaitForSeconds>> _pools = new Dictionary<float, Stack<WaitForSeconds>>();

	public static WaitForSeconds New(float duration)
	{
		float num = Mathf.Round(duration * 100f) / 100f;
		Stack<WaitForSeconds> value = null;
		if (!_pools.TryGetValue(num, out value))
		{
			value = new Stack<WaitForSeconds>();
			_pools.Add(num, value);
			return new WaitForSeconds(num);
		}
		if (value.Count > 0)
		{
			return value.Pop();
		}
		return new WaitForSeconds(num);
	}

	public static void Store(WaitForSeconds obj, float duration)
	{
		float key = Mathf.Round(duration * 100f) / 100f;
		Stack<WaitForSeconds> value = null;
		if (!_pools.TryGetValue(key, out value))
		{
			value = new Stack<WaitForSeconds>();
			_pools.Add(key, value);
		}
		value.Push(obj);
	}
}
