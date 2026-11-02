using System;
using I2.Loc;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class I2LocGlobalVariables : MonoBehaviour, ILocalizationParamsManager
{
	private const string RANDOMIZE_PARAMETER = "RANDOMIZE";

	private void Awake()
	{
		if (!LocalizationManager.ParamManagers.Contains(this))
		{
			LocalizationManager.ParamManagers.Add(this);
			LocalizationManager.LocalizeAll(Force: true);
		}
	}

	private void OnDestroy()
	{
		if (LocalizationManager.ParamManagers.Contains(this))
		{
			LocalizationManager.ParamManagers.Remove(this);
		}
	}

	public string GetParameterValue(string param)
	{
		if (param.StartsWith("RANDOMIZE"))
		{
			string[] array = param.Substring(param.IndexOf("|", StringComparison.Ordinal) + 1).Split('/');
			int num = UnityEngine.Random.Range(0, array.Length);
			return array[num];
		}
		return null;
	}
}
