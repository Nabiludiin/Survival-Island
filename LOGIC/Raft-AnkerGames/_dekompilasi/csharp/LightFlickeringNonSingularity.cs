using UnityEngine;

public class LightFlickeringNonSingularity : MonoBehaviour, SceneEventInterface
{
	[SerializeField]
	private float maxReduction;

	[SerializeField]
	private float maxIncrease;

	[SerializeField]
	private Interval_Float rateDamping;

	[SerializeField]
	private float strength;

	[SerializeField]
	private float overallMultiplier = 1f;

	[SerializeField]
	private int fullIntensityDistance = 15;

	[SerializeField]
	private int zeroIntensityDistance = 30;

	private static Transform mainCameraTransform;

	private Light m_light;

	private float baseIntensity;

	private float waitTimer;

	private float intensity;

	public void Start()
	{
		m_light = GetComponent<Light>();
		baseIntensity = m_light.intensity;
		waitTimer = rateDamping.GetRandomValue();
		intensity = Random.Range(baseIntensity - maxReduction, baseIntensity + maxIncrease);
		FindCamera();
	}

	private void Update()
	{
		if (FindCamera())
		{
			float num = Vector3.Distance(transform.position, mainCameraTransform.position);
			float num2 = 1f;
			if (num > (float)fullIntensityDistance)
			{
				float num3 = (num - (float)fullIntensityDistance) / (float)(zeroIntensityDistance - fullIntensityDistance);
				num2 = Mathf.Clamp(1f - num3, 0f, 1f);
			}
			float b = intensity * overallMultiplier * num2;
			m_light.intensity = Mathf.Lerp(m_light.intensity, b, strength * Time.deltaTime);
			m_light.SetEnabledSafe(m_light.intensity > 0.01f);
			waitTimer -= Time.deltaTime;
			if (waitTimer <= 0f)
			{
				intensity = Random.Range(baseIntensity - maxReduction, baseIntensity + maxIncrease);
				waitTimer = rateDamping.GetRandomValue();
			}
		}
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			mainCameraTransform = null;
		}
	}

	private bool FindCamera()
	{
		if (mainCameraTransform != null)
		{
			return false;
		}
		Network_Player value = ComponentManager<Network_Player>.Value;
		if (value == null || value.CameraTransform == null)
		{
			return false;
		}
		mainCameraTransform = value.CameraTransform;
		return true;
	}
}
