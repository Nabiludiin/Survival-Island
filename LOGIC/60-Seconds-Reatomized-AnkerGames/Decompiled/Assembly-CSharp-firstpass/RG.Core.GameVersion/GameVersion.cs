using UnityEngine;

namespace RG.Core.GameVersion;

public class GameVersion : ScriptableObject
{
	public int Major;

	public int Minor;

	public int Patch;

	public int Build;

	public string BuildName;

	public string GetReadableVersion()
	{
		return $"{BuildName}\n{Major}.{Minor}.{Patch} Build {Build}";
	}

	public string GetBundleVersion()
	{
		return $"{Major}.{Minor}.{Patch}";
	}

	public string GetBuildNumber()
	{
		return Build.ToString();
	}
}
