using System;

namespace RG.Parsecs.Common;

[Serializable]
public class TestStatsEntry : StatsEntry
{
	public bool IsPassed;

	public bool IsCrewTest;

	public bool IsCaptain;

	public string TestedAttributeId;

	public string TestedCharacterId;

	public override string ToString()
	{
		return string.Format("Test entry. Day: {5}, Passed: {0}, CrewTest: {1}, Captain: {2}, Tested attribute: {3}, Tested character: {4}\n", IsPassed, IsCrewTest, IsCaptain, TestedAttributeId, TestedCharacterId, Day);
	}
}
