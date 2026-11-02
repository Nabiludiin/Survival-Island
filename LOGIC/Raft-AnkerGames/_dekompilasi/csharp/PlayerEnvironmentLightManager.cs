using System.Collections;
using UnityEngine;
using UnityEngine.AzureSky;

public class PlayerEnvironmentLightManager : MonoBehaviour
{
	[SerializeField]
	private float lerpTime = 1f;

	public Light pointLight;

	[SerializeField]
	private float pointLightIntensity = 1f;

	private AzureSkyController skyController;

	public LayerMask defaultSkyCullMask;

	public LayerMask environmentSkyCullMask;

	private bool lastFrameUsingCustomEnvironmentLight;

	private void Start()
	{
		skyController = ComponentManager<AzureSkyController>.Value;
	}

	private void Update()
	{
		bool usingCustomEnvironmentLight = SkyManager.UsingCustomEnvironmentLight;
		if (usingCustomEnvironmentLight)
		{
			skyController.m_lightComponent.cullingMask = environmentSkyCullMask;
		}
		else
		{
			skyController.m_lightComponent.cullingMask = defaultSkyCullMask;
		}
		if (lastFrameUsingCustomEnvironmentLight != usingCustomEnvironmentLight)
		{
			StopAllCoroutines();
			StartCoroutine(LerpLights(usingCustomEnvironmentLight));
		}
		lastFrameUsingCustomEnvironmentLight = usingCustomEnvironmentLight;
	}

	private IEnumerator LerpLights(bool inCustomLightZone)
	{
		float startTime = Time.time;
		float pointLightStartIntensity = pointLight.intensity;
		float pointLightEndIntensity = (inCustomLightZone ? pointLightIntensity : 0f);
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			normalizedTime = Mathf.Clamp01((Time.time - startTime) / lerpTime);
			pointLight.intensity = Mathf.Lerp(pointLightStartIntensity, pointLightEndIntensity, normalizedTime);
			pointLight.SetEnabledSafe(pointLight.intensity > 0f);
			yield return null;
		}
	}
}
