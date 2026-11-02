using UltimateWater;
using UnityEngine;
using UnityEngine.AzureSky;

[CreateAssetMenu(fileName = "Weather", menuName = "Raft/Miscellaneous/Weather")]
public class SO_Weather : ScriptableObject
{
	public UniqueWeatherType uniqueWeatherType;

	[SerializeField]
	private Interval_Float weatherDuration;

	public WaterProfile waterProfile;

	public float waterTransitionDuration;

	public AzureSkyProfile skyProfile;

	public float skyTransitionDuration;

	public float GetRandomDuration => weatherDuration.GetRandomValue() * 60f;

	public float GreatestTransitionTime => Mathf.Max(skyTransitionDuration, waterTransitionDuration);
}
