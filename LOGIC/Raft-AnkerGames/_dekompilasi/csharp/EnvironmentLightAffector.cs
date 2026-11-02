using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentLightAffector : MonoBehaviour
{
	[Serializable]
	public class LightAffectorData
	{
		public AnimationCurve lightingCurve;

		public AffectorID ID;
	}

	private class LightData
	{
		protected float range;

		protected float intensity;

		protected bool enabled;

		protected internal LightData(Light light)
		{
			range = light.range;
			intensity = light.intensity;
			enabled = light.enabled;
		}

		public void RestoreLight(Light light)
		{
			if (!(light == null))
			{
				light.range = range;
				light.intensity = intensity;
				light.enabled = enabled;
			}
		}

		public IEnumerator RestoreLightSoft(Light light, float time)
		{
			light.enabled = enabled;
			while (light.range < range || light.intensity < intensity)
			{
				light.range = Mathf.MoveTowards(light.range, range, 1f / (light.range / range));
				light.intensity = Mathf.MoveTowards(light.intensity, intensity, 1f / (light.intensity / intensity));
				yield return new WaitForEndOfFrame();
			}
			light.range = range;
			light.intensity = intensity;
		}

		public IEnumerator TurnOffLightSoft(Light light, float time)
		{
			while (light.range > 0f || light.intensity > 0f)
			{
				light.range = Mathf.MoveTowards(light.range, 0f, 0.01f);
				light.intensity = Mathf.MoveTowards(light.intensity, 0f, 0.01f);
				yield return new WaitForEndOfFrame();
			}
			light.range = 0f;
			light.intensity = 0f;
			light.enabled = false;
		}

		public void TurnOffLightHard(Light light, float time)
		{
			light.range = 0f;
			light.intensity = 0f;
			light.enabled = false;
		}
	}

	public enum AffectorID
	{
		ElectricityWater
	}

	private Coroutine softLightRestore;

	private Coroutine softTurnOff;

	[SerializeField]
	private float intensityAmplitude = 5f;

	public Light targetLight;

	public List<LightAffectorData> affectorData = new List<LightAffectorData>();

	[SerializeField]
	private MeshRenderer lightCone;

	[SerializeField]
	private bool playAtStart;

	[SerializeField]
	private bool playOnEnable;

	private LightAffectorData currentPlaying;

	private Network_Player localPlayer;

	private LightData originLightData;

	private LightData OriginLightData
	{
		get
		{
			if (originLightData == null)
			{
				originLightData = new LightData(targetLight);
			}
			return originLightData;
		}
		set
		{
			originLightData = value;
		}
	}

	public void Start()
	{
		localPlayer = ComponentManager<Network_Player>.Value;
		if (targetLight == null)
		{
			Debug.LogError("light is not assigned in environmentlightaffector, this is not currently valid.");
			return;
		}
		OriginLightData = new LightData(targetLight);
		if (playAtStart)
		{
			PlayAffector(AffectorID.ElectricityWater);
		}
	}

	public void Update()
	{
		if (currentPlaying != null)
		{
			HandleAffector(currentPlaying);
		}
	}

	private void OnEnable()
	{
		if (playOnEnable)
		{
			PlayAffector(AffectorID.ElectricityWater);
			Invoke("InvokeStop", 1f);
		}
	}

	private void InvokeStop()
	{
		StopAffectors();
	}

	public void HandleAffector(LightAffectorData current)
	{
		float time = Mathf.PingPong(Time.time, 1f);
		float num = current.lightingCurve.Evaluate(time);
		targetLight.intensity = num * intensityAmplitude;
		if (lightCone != null)
		{
			lightCone.enabled = targetLight.intensity > 0f;
		}
	}

	public void PlayAffector(AffectorID id)
	{
		targetLight.SetEnabledSafe(value: true);
		currentPlaying = GetAffectorData(id);
	}

	public void StopAffectors()
	{
		currentPlaying = null;
		OriginLightData.RestoreLight(targetLight);
	}

	public void StopAffectorsSoft(float time)
	{
		currentPlaying = null;
		if (softLightRestore != null)
		{
			StopCoroutine(softLightRestore);
		}
		softLightRestore = StartCoroutine(OriginLightData.RestoreLightSoft(targetLight, time));
	}

	public void TurnOffAffectorsSoft(float time)
	{
		currentPlaying = null;
		if (softTurnOff != null)
		{
			StopCoroutine(softTurnOff);
		}
		if (!gameObject.activeInHierarchy)
		{
			OriginLightData.TurnOffLightHard(targetLight, time);
		}
		else
		{
			softTurnOff = StartCoroutine(OriginLightData.TurnOffLightSoft(targetLight, time));
		}
	}

	public LightAffectorData GetAffectorData(AffectorID id)
	{
		foreach (LightAffectorData affectorDatum in affectorData)
		{
			if (affectorDatum != null && affectorDatum.ID == id)
			{
				return affectorDatum;
			}
		}
		return null;
	}
}
