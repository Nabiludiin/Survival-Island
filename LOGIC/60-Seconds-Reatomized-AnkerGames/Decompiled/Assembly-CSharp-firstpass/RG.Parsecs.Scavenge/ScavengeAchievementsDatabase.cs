using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[CreateAssetMenu(fileName = "ScavengeAchievementsDatabase", menuName = "Scavenge/ScavengeAchievementsDatabase")]
public class ScavengeAchievementsDatabase : ScriptableObject
{
	public ScavangeItemsDatabase ScavangeItemsDatabase;

	public Achievement DontPanic;

	public Achievement SoupOdyssey;

	public Achievement HandsFull;

	public Achievement AllSeats;

	public void CheckDontPanic()
	{
		if (!DontPanic.IsAchieved)
		{
			AchievementsSystem.UnlockAchievement(DontPanic);
		}
	}

	public void CheckAllSeatsTaken(int crewNumber)
	{
		if (!AllSeats.IsAchieved && crewNumber >= 4)
		{
			AchievementsSystem.UnlockAchievement(AllSeats);
		}
	}

	public void CheckHandsFull(int itemsCount)
	{
		if (!HandsFull.IsAchieved)
		{
			AchievementsSystem.ProgressAchievement(HandsFull, itemsCount, 12);
		}
	}

	internal void CheckSoupOdyssey(int scavengedSoups)
	{
		if (!SoupOdyssey.IsAchieved)
		{
			AchievementsSystem.ProgressAchievement(SoupOdyssey, scavengedSoups, 111);
		}
	}
}
