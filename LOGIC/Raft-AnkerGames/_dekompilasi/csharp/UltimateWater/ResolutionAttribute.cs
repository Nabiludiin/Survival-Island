using UnityEngine;

namespace UltimateWater;

public class ResolutionAttribute : PropertyAttribute
{
	private readonly int _RecommendedResolution;

	private readonly int[] _Resolutions;

	public int RecommendedResolution => _RecommendedResolution;

	public int[] Resolutions => _Resolutions;

	public ResolutionAttribute(int recommendedResolution, params int[] resolutions)
	{
		_RecommendedResolution = recommendedResolution;
		_Resolutions = resolutions;
	}
}
