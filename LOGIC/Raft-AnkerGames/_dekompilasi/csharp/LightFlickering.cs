using UnityEngine;

public class LightFlickering : MonoBehaviour
{
	public float maxReduction;

	public float maxIncrease;

	public Interval_Float rateDamping;

	public float strength;

	public float overallMultiplier = 1f;

	private LightSingularity lightSingularity;

	private float baseIntensity = 1f;

	private float waitTimer = 1f;

	private float intensity = 1f;

	public void Start()
	{
		lightSingularity = GetComponent<LightSingularity>();
		if (lightSingularity == null)
		{
			Debug.LogError("Flicker script must have a Light Singularity on the same GameObject.");
			return;
		}
		baseIntensity = lightSingularity.currentIntensity;
		waitTimer = rateDamping.GetRandomValue();
		intensity = Random.Range(baseIntensity - maxReduction, baseIntensity + maxIncrease);
	}

	private void Update()
	{
		if (!(lightSingularity == null) && !(lightSingularity.LightComponent == null))
		{
			float num = Mathf.Lerp(lightSingularity.LightComponent.intensity, intensity * overallMultiplier, strength * Time.deltaTime);
			bool flag = num > 0.01f;
			lightSingularity.LightComponent.SetEnabledSafe(flag);
			if (flag)
			{
				lightSingularity.LightComponent.intensity = num;
			}
			waitTimer -= Time.deltaTime;
			if (waitTimer <= 0f)
			{
				intensity = Random.Range(baseIntensity - maxReduction, baseIntensity + maxIncrease);
				waitTimer = rateDamping.GetRandomValue();
			}
		}
	}
}
