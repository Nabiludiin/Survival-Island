using I2.Loc;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Missions/New End Game Stat Set")]
public class EndGameStatSet : RGScriptableObject
{
	public static int STAT_COUNT = 9;

	public static int SECTION_COUNT = 3;

	[SerializeField]
	[Tooltip("These are the 9 slots for stats in the end game scene, divided into 3 sections. Do NOT change the count of this list. The slots are ordered in columns of three. For example, index 0 is the first slot of the first column and index 5 is the last slot of the second column.")]
	private EndGameStat[] _endGameStats = new EndGameStat[STAT_COUNT];

	[SerializeField]
	[Tooltip("These are the section names displayed in the Starlog stats page between the groups of stats. Do NOT change the count of this list.")]
	private LocalizedString[] _sectionNames = new LocalizedString[SECTION_COUNT];

	public EndGameStat[] EndGameStats => _endGameStats;

	public LocalizedString[] SectionNames => _sectionNames;
}
