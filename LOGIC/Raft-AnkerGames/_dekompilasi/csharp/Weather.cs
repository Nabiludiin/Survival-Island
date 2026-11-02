using System.Collections;
using UnityEngine;
using UnityEngine.AzureSky;

public class Weather : MonoBehaviour
{
	public SO_Weather so_weather;

	private float startTime;

	private static AzureSkyController azureSky;

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		if (azureSky == null)
		{
			azureSky = ComponentManager<AzureSkyController>.Value;
		}
	}

	public float GetNormalizedTime(float transitionDuration)
	{
		return Mathf.Clamp01((Time.time - startTime) / transitionDuration);
	}

	public virtual IEnumerator StartWeather()
	{
		startTime = Time.time;
		if (azureSky == null)
		{
			azureSky = ComponentManager<AzureSkyController>.Value;
		}
		azureSky.SetNewWeatherProfile(so_weather.skyProfile, WeatherManager.SetWeatherInstant ? 0.5f : so_weather.skyTransitionDuration);
		if (!WeatherManager.SetWeatherInstant)
		{
			yield return new WaitForSeconds(so_weather.skyTransitionDuration);
			yield return new WaitForSeconds(so_weather.waterTransitionDuration);
		}
	}

	public virtual IEnumerator StopWeather()
	{
		yield break;
	}
}
