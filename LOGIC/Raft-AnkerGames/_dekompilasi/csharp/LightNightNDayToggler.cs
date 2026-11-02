using UnityEngine;
using UnityEngine.AzureSky;

public class LightNightNDayToggler : MonoBehaviour
{
	[SerializeField]
	private Light lightComponent;

	[SerializeField]
	private bool modifyIntesityByDistance;

	[SerializeField]
	private int fullIntensityDistance = 5;

	[SerializeField]
	private int zeroIntensityDistance = 8;

	[SerializeField]
	private float lerpSpeed = 0.25f;

	private float originIntensity = 1f;

	private float distanceModifier;

	private static float nightTimeStart = 19.5f;

	private static float nightTimeEnd = 4.5f;

	private static AzureSkyController skyController;

	private static Transform mainCameraTransform;

	public void OnValidate()
	{
		if (lightComponent == null)
		{
			lightComponent = GetComponent<Light>();
		}
	}

	public void Start()
	{
		if (skyController == null)
		{
			skyController = ComponentManager<AzureSkyController>.Value;
		}
		BedManager value = ComponentManager<BedManager>.Value;
		if (value != null)
		{
			nightTimeStart = value.canSleepAfterTime - 1f;
			nightTimeEnd = value.wakeUpTime;
		}
		originIntensity = lightComponent.intensity;
	}

	public void Update()
	{
		if (skyController == null)
		{
			skyController = ComponentManager<AzureSkyController>.Value;
		}
		else if (IsValidForIllumination())
		{
			if (lightComponent.intensity < originIntensity)
			{
				SetLightIntensity(originIntensity);
			}
		}
		else if (lightComponent.intensity > 0f)
		{
			SetLightIntensity(0f);
		}
	}

	private void SetLightIntensity(float targetValue)
	{
		if (mainCameraTransform == null)
		{
			Network_Player value = ComponentManager<Network_Player>.Value;
			if (value != null)
			{
				mainCameraTransform = value.Camera.transform;
			}
		}
		else if (modifyIntesityByDistance)
		{
			float num = Vector3.Distance(transform.position, mainCameraTransform.position);
			if (num <= (float)fullIntensityDistance)
			{
				distanceModifier = 1f;
			}
			else
			{
				float num2 = (num - (float)fullIntensityDistance) / (float)(zeroIntensityDistance - fullIntensityDistance);
				distanceModifier = Mathf.Clamp(1f - num2, 0f, 1f);
			}
			lightComponent.intensity = Mathf.Clamp(Mathf.MoveTowards(lightComponent.intensity, targetValue * distanceModifier, Time.deltaTime * lerpSpeed), 0f, originIntensity);
		}
		else
		{
			lightComponent.intensity = Mathf.Clamp(Mathf.MoveTowards(lightComponent.intensity, targetValue, Time.deltaTime * lerpSpeed), 0f, originIntensity);
		}
	}

	protected virtual bool IsValidForIllumination()
	{
		if (skyController.timeOfDay.hour > nightTimeStart || skyController.timeOfDay.hour < nightTimeEnd)
		{
			return true;
		}
		return false;
	}
}
