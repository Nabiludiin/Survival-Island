using UnityEngine;

public class GuidManagerSettings : ScriptableObject
{
	private const string GUID_EDITOR_SETTINGS_PATH = "/Settings/GuidManagerSettings.asset";

	private static GuidManagerSettings _settings;

	[SerializeField]
	private string _allowedPattern = "^[\\w\\d_-]+$";

	[SerializeField]
	private string _forbiddenSignsPatter = "[^\\w\\d_-]";

	[SerializeField]
	private char _replacementCharacterForForbiddenSigns = '_';

	[SerializeField]
	private bool _allowAllCharacters;

	public static GuidManagerSettings Settings
	{
		get
		{
			if (_settings == null)
			{
				_settings = LoadOrCreate();
			}
			return _settings;
		}
	}

	public string AllowedPattern => _allowedPattern;

	public string ForbiddenSignsPatter => _forbiddenSignsPatter;

	public bool AllowAllCharacters => _allowAllCharacters;

	public char ReplacementCharacterForForbiddenSigns => _replacementCharacterForForbiddenSigns;

	public static GuidManagerSettings LoadOrCreate()
	{
		return null;
	}
}
