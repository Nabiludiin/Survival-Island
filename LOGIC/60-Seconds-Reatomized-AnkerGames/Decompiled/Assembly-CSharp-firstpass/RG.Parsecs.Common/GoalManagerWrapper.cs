using System;
using System.Collections.Generic;

namespace RG.Parsecs.Common;

[Serializable]
internal struct GoalManagerWrapper
{
	public List<GoalWrapper> ActiveGoals;

	public List<GoalWrapper> AchievedGoals;

	public List<GoalWrapper> FailedGoals;

	public int Counter;

	public bool ShouldGlow;
}
