using System.Text.RegularExpressions;

namespace RG.Core.GUID;

public static class GuidManager
{
	private const string CONFIRM_WINDOW_TITLE = "Force reassigning GUIDs?";

	private const string CONFIRM_WINDOW_TEXT = "Replacing GUIDs may result in unexpected behaviour in save system and some other systems. Are you sure you want to do that?";

	private const string CONFIRM_WINDOW_CONFIRM_BUTTON_NAME = "Yes";

	private const string CONFIRM_WINDOW_DECLINE_BUTTON_NAME = "No";

	public static bool VerifyGuidName(string guid)
	{
		if (!string.IsNullOrEmpty(guid))
		{
			if (!GuidManagerSettings.Settings.AllowAllCharacters)
			{
				return Regex.IsMatch(guid, GuidManagerSettings.Settings.AllowedPattern);
			}
			return true;
		}
		return false;
	}
}
