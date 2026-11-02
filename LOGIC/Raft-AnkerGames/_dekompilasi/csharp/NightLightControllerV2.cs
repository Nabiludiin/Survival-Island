using UnityEngine;
using UnityEngine.AzureSky;

public class NightLightControllerV2 : MonoBehaviour, SceneEventInterface
{
	[SerializeField]
	private int fullIntensityDistance = 5;

	[SerializeField]
	private int zeroIntensityDistance = 8;

	[SerializeField]
	private float lerpSpeed = 0.25f;

	private float nightTimeStart = 19.5f;

	private float nightTimeEnd = 4.5f;

	private float startIntensity = 1f;

	private static AzureSkyController skyController;

	private static Transform mainCameraTransform;

	private Light light;

	private void Start()
	{
		light = GetComponent<Light>();
		startIntensity = light.intensity;
		if (skyController == null)
		{
			skyController = ComponentManager<AzureSkyController>.Value;
		}
		if (mainCameraTransform == null)
		{
			mainCameraTransform = Camera.main.transform;
		}
		BedManager value = ComponentManager<BedManager>.Value;
		if (value != null)
		{
			nightTimeStart = value.canSleepAfterTime - 1f;
			nightTimeEnd = value.wakeUpTime;
		}
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			mainCameraTransform = null;
			skyController = null;
		}
	}

	private void Update()
	{
		if (skyController == null)
		{
			skyController = ComponentManager<AzureSkyController>.Value;
		}
		else if (skyController.timeOfDay.hour > nightTimeStart || skyController.timeOfDay.hour < nightTimeEnd)
		{
			SetLightIntensity(startIntensity);
		}
		else
		{
			SetLightIntensity(0f);
		}
	}

	private void SetLightIntensity(float targetLightValue)
	{
		if (mainCameraTransform == null)
		{
			Network_Player value = ComponentManager<Network_Player>.Value;
			if (value != null)
			{
				mainCameraTransform = value.Camera.transform;
			}
			return;
		}
		float intensity = targetLightValue;
		if (targetLightValue > 0f)
		{
			float num = Vector3.Distance(transform.position, mainCameraTransform.position);
			if (num > (float)fullIntensityDistance)
			{
				float num2 = (num - (float)fullIntensityDistance) / (float)(zeroIntensityDistance - fullIntensityDistance);
				intensity = targetLightValue * (1f - num2);
			}
		}
		if (light != null)
		{
			light.intensity = intensity;
			light.SetEnabledSafe(light.intensity > 0.05f);
		}
	}
}
