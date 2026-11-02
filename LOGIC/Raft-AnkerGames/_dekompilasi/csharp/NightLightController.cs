using UnityEngine;
using UnityEngine.AzureSky;

public class NightLightController : MonoBehaviour, SceneEventInterface
{
	public LightFlickering flickeringScript;

	[SerializeField]
	private int fullIntensityDistance = 5;

	[SerializeField]
	private int zeroIntensityDistance = 8;

	[SerializeField]
	private float lerpSpeed = 0.25f;

	private float nightTimeStart = 19.5f;

	private float nightTimeEnd = 4.5f;

	private float distanceModifier;

	private static AzureSkyController skyController;

	private static Transform mainCameraTransform;

	private void Start()
	{
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
			nightTimeEnd = value.wakeUpTime + 1f;
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
			SetLightIntensity(1f);
		}
		else
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
			return;
		}
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
		flickeringScript.overallMultiplier = Mathf.Clamp01(Mathf.MoveTowards(flickeringScript.overallMultiplier, targetValue * distanceModifier, Time.deltaTime * lerpSpeed));
	}
}
