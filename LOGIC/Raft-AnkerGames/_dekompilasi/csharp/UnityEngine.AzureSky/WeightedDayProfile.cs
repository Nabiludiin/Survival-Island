namespace UnityEngine.AzureSky;

public struct WeightedDayProfile(AzureSkyProfile profile, float weight)
{
	public AzureSkyProfile profile = profile;

	public float weight = weight;
}
