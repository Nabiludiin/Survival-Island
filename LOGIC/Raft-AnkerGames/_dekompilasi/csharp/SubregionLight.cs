using UnityEngine;

public class SubregionLight : MonoBehaviour
{
	[SerializeField]
	private bool disableIntensityAtStart;

	[SerializeField]
	private float lerpSpeed = 1f;

	[SerializeField]
	private Light light;

	[SerializeField]
	private Subregion[] subregions;

	[SerializeField]
	private float initialIntensity = 1f;

	private void Awake()
	{
		if (light != null)
		{
			initialIntensity = light.intensity;
			if (disableIntensityAtStart)
			{
				light.intensity = 0f;
			}
		}
		if (subregions == null || subregions.Length == 0)
		{
			Debug.Log("SubregionLight has no subregion assigned", gameObject);
		}
	}

	private void Update()
	{
		if (light == null)
		{
			return;
		}
		bool flag = false;
		Subregion[] array = subregions;
		foreach (Subregion subregion in array)
		{
			if (!(subregion == null) && subregion.playerIsWithinSubregion)
			{
				flag = true;
				break;
			}
		}
		if (light.enabled)
		{
			float num = (flag ? initialIntensity : 0f);
			if (light.intensity != num)
			{
				light.intensity = Mathf.MoveTowards(light.intensity, num, Time.deltaTime * lerpSpeed);
			}
		}
	}

	private void OnDrawGizmos()
	{
		float num = 0f;
		if (subregions == null || subregions.Length == 0 || subregions[0] == null)
		{
			Gizmos.color = Color.red;
			num = 0.5f;
		}
		else
		{
			Gizmos.color = Color.yellow;
			num = 0.25f;
			if (subregions[0] != null)
			{
				Gizmos.DrawLine(transform.position, subregions[0].transform.position);
			}
		}
		Gizmos.DrawSphere(transform.position, num);
	}

	private bool IsEditorPlaying()
	{
		return Application.isPlaying;
	}
}
