using UnityEngine;

namespace RG.Parsecs.Menu;

[CreateAssetMenu(fileName = "Settings", menuName = "RGCore/LanguageTranslations")]
public class LanguagesTranslations : ScriptableObject
{
	[SerializeField]
	private LanguageCodeTranslates[] langCodeTranslates;

	public string GetLangTranslation(string langCode)
	{
		LanguageCodeTranslates[] array = langCodeTranslates;
		foreach (LanguageCodeTranslates languageCodeTranslates in array)
		{
			if (languageCodeTranslates.LangCode.Equals(langCode))
			{
				return languageCodeTranslates.translation;
			}
		}
		Debug.LogError("Lang code not found for language " + langCode);
		return "";
	}
}
