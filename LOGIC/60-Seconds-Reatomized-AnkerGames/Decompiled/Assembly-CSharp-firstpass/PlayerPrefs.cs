using UnityEngine;

public class PlayerPrefs
{
	private const string PLAYER_PREFS_USED = "PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!";

	public static int GetInt(string key, int def = 0)
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
		return def;
	}

	public static string GetString(string key, string def = "")
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
		return def;
	}

	public static float GetFloat(string key, float def = 0f)
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
		return def;
	}

	public static void SetString(string key, string val)
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
	}

	public static void SetInt(string key, int val)
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
	}

	public static void SetFloat(string key, float val)
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
	}

	public static bool HasKey(string key)
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
		return false;
	}

	public static void DeleteKey(string key)
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
	}

	public static void DeleteAll()
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
	}

	public static void Save()
	{
		Debug.LogWarning("PlayerPrefs are being used in a project. They won't work on Nintendo Switch console and should not be used in project!");
	}
}
