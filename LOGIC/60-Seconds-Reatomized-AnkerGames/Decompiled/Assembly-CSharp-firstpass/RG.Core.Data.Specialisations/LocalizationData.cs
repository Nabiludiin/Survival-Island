using System;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
[CreateAssetMenu(fileName = "LocalizationData", menuName = "RG/Data/Core/Localization/LocalizationData", order = 1)]
public class LocalizationData : DataObject, ISerializable
{
	[SerializeField]
	private string[] _supportedLanguages;

	public virtual string CurrentLanguageCode { get; set; }

	public string[] SupportedLanguages => _supportedLanguages;

	public override void ResetData()
	{
		CurrentLanguageCode = string.Empty;
	}

	public virtual string GetValue(string key)
	{
		return string.Empty;
	}

	public void Load()
	{
		throw new NotImplementedException();
	}

	public void Save()
	{
		throw new NotImplementedException();
	}
}
