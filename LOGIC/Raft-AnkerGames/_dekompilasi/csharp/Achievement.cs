using Steamworks;

public class Achievement
{
	public AchievementType type;

	public bool achieved;

	public Achievement(AchievementType type)
	{
		this.type = type;
		SteamUserStats.GetAchievement(type.ToString(), out achieved);
	}
}
