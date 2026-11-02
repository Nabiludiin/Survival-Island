using System;
using RG.Core.Flow.Specialisations;
using UnityEngine;

namespace RG.Core.Base;

public class RGMonoBehaviourSingleton<T> : RGMonoBehaviour where T : RGMonoBehaviour
{
	private static T Instance;

	public static T TheInstance => GetInstance(warnIfNotFound: true);

	protected override void CustomOnDestroy()
	{
		base.CustomOnDestroy();
		Instance = null;
	}

	public static void RequireInstance(Action<T> callback)
	{
		T instance = GetInstance(warnIfNotFound: false);
		if (instance != null)
		{
			callback?.Invoke(instance);
		}
	}

	public static T GetInstance(bool warnIfNotFound)
	{
		if (Instance == null)
		{
			Instance = UnityEngine.Object.FindObjectOfType<T>();
			if ((Instance == null) & warnIfNotFound)
			{
				OnInstanceNotFound();
			}
		}
		return Instance;
	}

	protected void ForceSingletonInstance()
	{
		Instance = this as T;
	}

	private static void OnInstanceNotFound()
	{
		DebugManager.LogWarningFormat("Didn't find the required singleton instance!");
	}
}
