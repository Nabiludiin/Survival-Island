using System.Collections.Generic;
using UltimateWater;
using UnityEngine;

public class WaterRoughnessManager : MonoBehaviour
{
	private static float roughness;

	private static float maxRoughness = 14f;

	public WaterProfile antiBobProfile;

	private WaterProfileBlend waterProfileBlend;

	private SoundManager soundManager;

	private List<float> weightMessage;

	private List<float> weightsCache = new List<float>();

	public static bool RoughForAnimals => Roughness > 0.6f;

	public static float Roughness
	{
		get
		{
			return roughness;
		}
		set
		{
			roughness = Mathf.Clamp01(value / maxRoughness);
		}
	}

	public List<float> WaterProfileWeights => waterProfileBlend._Weights;

	private void Awake()
	{
		if (waterProfileBlend == null)
		{
			waterProfileBlend = Object.FindObjectOfType<WaterProfileBlend>();
		}
	}

	private void Start()
	{
		soundManager = ComponentManager<SoundManager>.Value;
	}

	private void Update()
	{
		if (!Raft_Network.IsHost)
		{
			LerpWater(weightMessage);
		}
		else
		{
			LerpWater(GetWaterWeightsFromWeather());
		}
	}

	private List<float> GetWaterWeightsFromWeather()
	{
		if (weightsCache.Count > waterProfileBlend._Profiles.Count)
		{
			weightsCache.RemoveRange(waterProfileBlend._Profiles.Count, weightsCache.Count - waterProfileBlend._Profiles.Count);
		}
		for (int i = 0; i < waterProfileBlend._Profiles.Count; i++)
		{
			if (i < weightsCache.Count)
			{
				weightsCache[i] = 0f;
			}
			else
			{
				weightsCache.Add(0f);
			}
		}
		Weather currentWeather = ComponentManager<WeatherManager>.Value.GetCurrentWeather();
		if (currentWeather == null)
		{
			return null;
		}
		for (int j = 0; j < waterProfileBlend._Profiles.Count; j++)
		{
			WaterProfile waterProfile = waterProfileBlend._Profiles[j];
			if (GeneralSettingsBox.MotionSickness)
			{
				if (waterProfile == antiBobProfile)
				{
					weightsCache[j] = 1f;
				}
				else
				{
					weightsCache[j] = 0f;
				}
			}
			else if (waterProfile == currentWeather.so_weather.waterProfile)
			{
				if (WeatherManager.SetWeatherInstant)
				{
					weightsCache[j] = 1f;
				}
				else
				{
					weightsCache[j] = currentWeather.GetNormalizedTime(currentWeather.so_weather.waterTransitionDuration);
				}
			}
			else if (WeatherManager.SetWeatherInstant)
			{
				weightsCache[j] = 0f;
			}
			else if (waterProfile == antiBobProfile)
			{
				weightsCache[j] = 0f;
			}
			else if (waterProfileBlend._Weights[j] > 0f)
			{
				weightsCache[j] = 1f - currentWeather.GetNormalizedTime(currentWeather.so_weather.waterTransitionDuration);
			}
			else
			{
				weightsCache[j] = 0f;
			}
		}
		return weightsCache;
	}

	private void LerpWater(List<float> weights)
	{
		if (weights == null)
		{
			return;
		}
		if (WeatherManager.SetWeatherInstant)
		{
			float num = 0f;
			List<float> weights2 = waterProfileBlend._Weights;
			for (int i = 0; i < weights2.Count; i++)
			{
				num += waterProfileBlend._Profiles[i].Data.WindSpeed * weights[i];
				weights2[i] = weights[i];
			}
			Roughness = num;
		}
		else
		{
			float num2 = 0f;
			List<float> weights3 = waterProfileBlend._Weights;
			for (int j = 0; j < weights3.Count; j++)
			{
				num2 += waterProfileBlend._Profiles[j].Data.WindSpeed * weights[j];
				weights3[j] = Mathf.MoveTowards(weights3[j], weights[j], Time.deltaTime);
			}
			Roughness = num2;
		}
		waterProfileBlend.OnValidate();
		if (soundManager != null)
		{
			soundManager.SetOceanRoughness(Roughness);
		}
	}

	public void SetWeightMessage(List<float> weightMessage)
	{
		this.weightMessage = weightMessage;
	}
}
