using UnityEngine;

[CreateAssetMenu(fileName = "SteamUser", menuName = "Raft/Miscellaneous/SteamUser")]
public class SO_SteamUser : ScriptableObject
{
	public enum UserLevel
	{
		NONE,
		TESTER,
		DEVELOPER
	}

	public ulong ID;

	public UserLevel userLevel;
}
