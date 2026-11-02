using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.AzureSky;

public class WeatherManager : MonoBehaviour, SceneEventInterface
{
	public static Action<Weather> OnWeatherChanged;

	public static Action OnWeatherRegionUpdated;

	public static bool SetWeatherInstant;

	public static bool IsInNoWeatherParticleRegion;

	[SerializeField]
	private Weather startingWeather;

	[SerializeField]
	private List<WeatherPool> weatherPools = new List<WeatherPool>();

	[SerializeField]
	private Weather currentWeather;

	[SerializeField]
	private bool changingWeather;

	[SerializeField]
	private float nextWeatherTimer;

	private WeatherPoolType currentPoolType;

	private Raft_Network network;

	private Network_Host hostNetwork;

	private SO_GameModeValue currentGameMode;

	private Coroutine weatherLoopRoutine;

	private Coroutine currentWeatherStartRoutine;

	public float WeatherTimer
	{
		get
		{
			return nextWeatherTimer;
		}
		set
		{
			nextWeatherTimer = value;
		}
	}

	private void Awake()
	{
		ComponentManager<WeatherManager>.Value = this;
		RegionManager.OnRegionUpdate = (Action)Delegate.Combine(RegionManager.OnRegionUpdate, new Action(OnRegionChanged));
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		hostNetwork = ComponentManager<Network_Host>.Value;
		currentGameMode = GameModeValueManager.GetCurrentGameModeValue();
		AzureSkyController value = ComponentManager<AzureSkyController>.Value;
		if (weatherPools.ContainsItems())
		{
			foreach (WeatherPool weatherPool in weatherPools)
			{
				Weather[] allItems = weatherPool.randomizer.GetAllItems<Weather>();
				if (!allItems.ContainsItems())
				{
					continue;
				}
				Weather[] array = allItems;
				foreach (Weather weather in array)
				{
					if (value.weatherProfileList.Find((WeatherProfile p) => p.Profile == weather.so_weather.skyProfile) == null)
					{
						value.weatherProfileList.Add(new WeatherProfile
						{
							Profile = weather.so_weather.skyProfile,
							TransitionTime = 1f
						});
					}
				}
			}
		}
		if (startingWeather != null)
		{
			if (!Raft_Network.IsHost)
			{
				return;
			}
			if (currentGameMode.gameMode != GameMode.Creative)
			{
				if (GameManager.IsInNewGame || GameManager.QuickStartGame)
				{
					SetWeather(startingWeather, instant: true);
				}
			}
			else
			{
				SetWeather("Calm", instant: true);
			}
		}
		else
		{
			Debug.LogError("You must assign a starting weather to WeatherManager", gameObject);
		}
	}

	private void OnDestroy()
	{
		network = null;
		hostNetwork = null;
		RegionManager.OnRegionUpdate = (Action)Delegate.Remove(RegionManager.OnRegionUpdate, new Action(OnRegionChanged));
	}

	private void Update()
	{
		if (Raft_Network.IsHost && currentGameMode.gameMode != GameMode.Creative && currentWeather != null)
		{
			nextWeatherTimer -= Time.deltaTime;
			if (nextWeatherTimer <= 0f)
			{
				SetWeather(GetRandomWeather(currentPoolType, excludeCurrentWeather: true));
			}
		}
	}

	private void Button_RandomWeather()
	{
		if (Application.isPlaying)
		{
			ActivateRandomWeather(currentPoolType);
		}
	}

	private void OnRegionChanged()
	{
		bool flag = false;
		IsInNoWeatherParticleRegion = false;
		if (RegionManager.CurrentSubregions.ContainsItems())
		{
			foreach (Subregion currentSubregion in RegionManager.CurrentSubregions)
			{
				if (!flag)
				{
					SubRegionComponent_Weather subRegionComponent = currentSubregion.GetSubRegionComponent<SubRegionComponent_Weather>();
					if (subRegionComponent != null)
					{
						ChangePoolType(subRegionComponent.weatherPoolType);
						flag = true;
					}
				}
				if (!IsInNoWeatherParticleRegion && currentSubregion.GetSubRegionComponent<SubRegionComponent_NoWeatherParticle>() != null)
				{
					IsInNoWeatherParticleRegion = true;
				}
			}
		}
		if (!flag)
		{
			ChangePoolType(WeatherPoolType.Default);
		}
		OnWeatherRegionUpdated.CallSafe();
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			OnWeatherChanged = null;
			OnWeatherRegionUpdated = null;
			IsInNoWeatherParticleRegion = false;
			SetWeatherInstant = false;
		}
	}

	public void SetWeather(Weather weather, bool instant = false)
	{
		if (!(weather == null))
		{
			StopCurrentWeatherRoutine();
			weatherLoopRoutine = StartCoroutine(StartWeatherCoroutine(weather, instant));
		}
	}

	public void SetWeather(string weatherName, bool instant = false)
	{
		foreach (WeatherPool weatherPool in weatherPools)
		{
			Weather[] allItems = weatherPool.randomizer.GetAllItems<Weather>();
			if (allItems == null)
			{
				continue;
			}
			foreach (Weather weather in allItems)
			{
				if (weather != null && weather.name.Contains(weatherName, StringComparison.OrdinalIgnoreCase))
				{
					SetWeather(weather, instant);
					return;
				}
			}
		}
	}

	public void SetWeather(UniqueWeatherType uniqueWeatherType, bool instant = false)
	{
		foreach (WeatherPool weatherPool in weatherPools)
		{
			Weather[] allItems = weatherPool.randomizer.GetAllItems<Weather>();
			foreach (Weather weather in allItems)
			{
				if (weather.so_weather.uniqueWeatherType == uniqueWeatherType)
				{
					SetWeather(weather, instant);
					return;
				}
			}
		}
	}

	public Weather GetCurrentWeather()
	{
		return currentWeather;
	}

	public UniqueWeatherType GetCurrentWeatherType()
	{
		if (currentWeather == null)
		{
			return UniqueWeatherType.Default;
		}
		return currentWeather.so_weather.uniqueWeatherType;
	}

	private void ActivateRandomWeather(WeatherPoolType newType)
	{
		SetWeather(GetRandomWeather(newType, excludeCurrentWeather: true));
	}

	private IEnumerator StartWeatherCoroutine(Weather newWeather, bool instant = false)
	{
		if (!(newWeather == null))
		{
			SetWeatherInstant = instant;
			changingWeather = true;
			nextWeatherTimer = newWeather.so_weather.GetRandomDuration + newWeather.so_weather.GreatestTransitionTime;
			if (currentWeather != null)
			{
				yield return StartCoroutine(currentWeather.StopWeather());
			}
			currentWeather = newWeather;
			if (Raft_Network.IsHost)
			{
				network.RPC(new Message_WeatherManager_SetWeather(Messages.WeatherManager_SetWeather, hostNetwork, newWeather, instant), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			OnWeatherChanged.CallSafe(newWeather);
			currentWeatherStartRoutine = StartCoroutine(currentWeather.StartWeather());
			yield return currentWeatherStartRoutine;
			changingWeather = false;
			SetWeatherInstant = false;
		}
	}

	private Weather GetRandomWeather(WeatherPoolType poolType, bool excludeCurrentWeather)
	{
		if (!Raft_Network.IsHost)
		{
			return null;
		}
		int num = 100;
		Weather weather = null;
		WeatherPool poolFromType = GetPoolFromType(poolType);
		do
		{
			weather = poolFromType.randomizer.GetRandomItem<Weather>();
			num--;
		}
		while (weather == currentWeather && num > 0);
		if (weather == null)
		{
			weather = startingWeather;
		}
		return weather;
	}

	private WeatherPool GetPoolFromType(WeatherPoolType type)
	{
		return weatherPools.Find((WeatherPool p) => p.poolType == type);
	}

	private void StopCurrentWeatherRoutine()
	{
		if (weatherLoopRoutine != null)
		{
			StopCoroutine(weatherLoopRoutine);
			weatherLoopRoutine = null;
		}
		if (currentWeatherStartRoutine != null)
		{
			StopCoroutine(currentWeatherStartRoutine);
			currentWeatherStartRoutine = null;
		}
		changingWeather = false;
	}

	private void ChangePoolType(WeatherPoolType newType)
	{
		if (currentPoolType != newType)
		{
			currentPoolType = newType;
			ActivateRandomWeather(currentPoolType);
		}
	}
}
