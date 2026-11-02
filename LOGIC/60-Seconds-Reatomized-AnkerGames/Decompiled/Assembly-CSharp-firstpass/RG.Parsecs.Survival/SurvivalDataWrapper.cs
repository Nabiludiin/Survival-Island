using System;

namespace RG.Parsecs.Survival;

[Serializable]
internal struct SurvivalDataWrapper
{
	public int CurrentDay;

	public bool DailyEventResolved;

	public bool LandedOnPlanet;
}
