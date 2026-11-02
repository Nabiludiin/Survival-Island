using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

public class Weather_Particle : Weather, SceneEventInterface
{
	[SerializeField]
	private Vector3 localOffsetFromPlayer = Vector3.zero;

	[SerializeField]
	private ParticleController particleController;

	[SerializeField]
	private ActivateUnderWater activeUnderWaterScript;

	[SerializeField]
	private StudioEventEmitter eventEmitter;

	private Network_Player localPlayer;

	private bool isActive;

	protected override void Awake()
	{
		base.Awake();
		particleController.StopParticles();
	}

	protected override void Start()
	{
		base.Start();
		localPlayer = ComponentManager<Network_Player>.Value;
		WeatherManager.OnWeatherRegionUpdated = (Action)Delegate.Combine(WeatherManager.OnWeatherRegionUpdated, new Action(OnRegionChanged));
	}

	private void OnDestroy()
	{
		localPlayer = null;
		WeatherManager.OnWeatherRegionUpdated = (Action)Delegate.Remove(WeatherManager.OnWeatherRegionUpdated, new Action(OnRegionChanged));
	}

	private void Update()
	{
		if (isActive && particleController != null && localPlayer != null)
		{
			particleController.transform.position = localPlayer.transform.TransformPoint(localOffsetFromPlayer);
		}
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame && eventEmitter != null)
		{
			eventEmitter.Stop();
		}
	}

	private void OnRegionChanged()
	{
		if (isActive)
		{
			if (WeatherManager.IsInNoWeatherParticleRegion)
			{
				particleController.StopParticles();
			}
			else
			{
				particleController.PlayParticles();
			}
		}
	}

	public override IEnumerator StartWeather()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		isActive = true;
		yield return base.StartWeather();
		if (!particleController.gameObject.activeInHierarchy)
		{
			particleController.gameObject.SetActive(value: true);
		}
		particleController.PlayParticles();
		activeUnderWaterScript.activeScript = true;
		if (eventEmitter != null)
		{
			eventEmitter.Play();
		}
		if (!WeatherManager.SetWeatherInstant)
		{
			yield return new WaitForSeconds(3f);
		}
		particleController.SetPrewarmState(state: true);
	}

	public override IEnumerator StopWeather()
	{
		isActive = false;
		activeUnderWaterScript.activeScript = false;
		particleController.SetPrewarmState(state: false);
		particleController.StopParticles();
		CancelInvoke("HideParticles");
		Invoke("HideParticles", particleController.particleSystems[0].main.startLifetime.constant * 1.5f);
		if (eventEmitter != null)
		{
			eventEmitter.EventInstance.keyOff();
		}
		yield break;
	}

	private void HideParticles()
	{
		particleController.gameObject.SetActive(value: false);
	}
}
