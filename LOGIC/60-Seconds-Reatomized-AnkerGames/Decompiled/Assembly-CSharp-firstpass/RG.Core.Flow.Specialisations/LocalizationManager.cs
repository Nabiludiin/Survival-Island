using System;
using RG.Core.Base;
using RG.Core.Data.Abstractions;
using RG.Core.Data.Specialisations;
using RG.Core.Flow.Abstractions;

namespace RG.Core.Flow.Specialisations;

[Serializable]
public abstract class LocalizationManager : RGMonoBehaviourSingleton<LocalizationManager>, IManager, IDataOwner<LocalizationData>
{
	protected string CurrentLanguageCode
	{
		get
		{
			return Data.CurrentLanguageCode;
		}
		set
		{
			Data.CurrentLanguageCode = value;
		}
	}

	public string[] SupportedLanguages => Data.SupportedLanguages;

	public abstract LocalizationData Data { protected get; set; }

	public string GetValue(string key)
	{
		return Data.GetValue(key);
	}

	public bool SetLanguage(string code)
	{
		for (int i = 0; i < SupportedLanguages.Length; i++)
		{
			if (SupportedLanguages[i] == code)
			{
				CurrentLanguageCode = code;
				DebugManager.LogFormat("Changed language to {0}", code);
				return true;
			}
		}
		DebugManager.LogErrorFormat("Language {0} is not supported.", this, code);
		return false;
	}

	public static string Localize(string key)
	{
		return RGMonoBehaviourSingleton<LocalizationManager>.TheInstance.GetValue(key);
	}

	public void ResetData()
	{
		Data.ResetData();
	}
}
