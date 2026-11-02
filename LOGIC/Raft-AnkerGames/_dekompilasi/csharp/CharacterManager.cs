using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
	public static List<SO_Character> SO_Characters = new List<SO_Character>();

	private void Awake()
	{
		SO_Characters = (from c in Resources.LoadAll<SO_Character>("SO_Character")
			orderby c.index
			select c).ToList();
	}

	public static bool UnlockCharacterByIndex(int characterIndex)
	{
		bool flag = SetCharacterStat(characterIndex, 1);
		if (flag)
		{
			SO_Character characterData = SO_Characters[characterIndex];
			(ComponentManager<NotificationManager>.Value.ShowNotification("Character") as Notification_CharacterUnlock).infoQue.Enqueue(new Notification_CharacterUnlock_Info(characterData));
		}
		if (GetUnlockedCharacterIndexes().Count >= 4)
		{
			AchievementHandler.UnlockAchievement(AchievementType.ach_allCharactersUnlocked);
		}
		return flag;
	}

	public static void UnlockAllCharacters()
	{
		foreach (SO_Character sO_Character in SO_Characters)
		{
			SetCharacterStat(sO_Character.index, 1);
		}
	}

	public static bool ResetCharacterUnlock(int characterIndex)
	{
		return SetCharacterStat(characterIndex, 0);
	}

	public static void ResetAllCharacterUnlocks()
	{
		foreach (SO_Character sO_Character in SO_Characters)
		{
			SetCharacterStat(sO_Character.index, 0);
		}
	}

	public static List<int> GetUnlockedCharacterIndexes()
	{
		List<int> list = new List<int>();
		if (AchievementHandler.Stat_test_2 > 0)
		{
			list.Add(2);
		}
		if (AchievementHandler.Stat_test_3 > 0)
		{
			list.Add(3);
		}
		if (AchievementHandler.Stat_test_4 > 0)
		{
			list.Add(4);
		}
		if (AchievementHandler.Stat_test_5 > 0)
		{
			list.Add(5);
		}
		return list;
	}

	public static SO_Character GetCharacterFromIndex(int characterIndex)
	{
		return SO_Characters.Find((SO_Character c) => c.index == characterIndex);
	}

	private static bool SetCharacterStat(int characterIndex, int statValue)
	{
		switch (characterIndex)
		{
		case 2:
			if (AchievementHandler.Stat_test_2 != statValue)
			{
				AchievementHandler.Stat_test_2 = statValue;
				AchievementHandler.SetStat(StatType.stat_test_2, AchievementHandler.Stat_test_2);
				return true;
			}
			break;
		case 3:
			if (AchievementHandler.Stat_test_3 != statValue)
			{
				AchievementHandler.Stat_test_3 = statValue;
				AchievementHandler.SetStat(StatType.stat_test_3, AchievementHandler.Stat_test_3);
				return true;
			}
			break;
		case 4:
			if (AchievementHandler.Stat_test_4 != statValue)
			{
				AchievementHandler.Stat_test_4 = statValue;
				AchievementHandler.SetStat(StatType.stat_test_4, AchievementHandler.Stat_test_4);
				return true;
			}
			break;
		case 5:
			if (AchievementHandler.Stat_test_5 != statValue)
			{
				AchievementHandler.Stat_test_5 = statValue;
				AchievementHandler.SetStat(StatType.stat_test_5, AchievementHandler.Stat_test_5);
				return true;
			}
			break;
		}
		return false;
	}
}
