using UnityEngine;

public class SubregionComponent_EnvironmentProbe : SubregionComponent
{
	public SO_EnvironmentProbe probeSettings;

	public override void AddSubregionComponent()
	{
		GetComponent<Subregion>().subregionComponents.Add(typeof(SubregionComponent_EnvironmentProbe), this);
	}

	public Color GetSkyColor(float currentTime = 0f)
	{
		return probeSettings.skyGradient.Evaluate(currentTime);
	}

	public Color GetEquatorColor(float currentTime)
	{
		return probeSettings.equatorGradient.Evaluate(currentTime);
	}

	public Color GetGroundColor(float currentTime)
	{
		return probeSettings.groundGradient.Evaluate(currentTime);
	}
}
