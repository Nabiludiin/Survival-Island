using System.Collections;
using UnityEngine;

public class FlickerMaterial : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer renderer;

	[SerializeField]
	private Material onMaterial;

	[SerializeField]
	private Material offMaterial;

	[SerializeField]
	private Interval_Float delay;

	[SerializeField]
	private Interval_Float onOffSpan;

	[SerializeField]
	private Interval_Int numberOfTimes;

	private float timer;

	private float currentDelay;

	private Coroutine flickerRoutine;

	private void Start()
	{
		currentDelay = delay.GetRandomValue();
	}

	private void Update()
	{
		if (renderer == null || onMaterial == null || offMaterial == null)
		{
			return;
		}
		timer += Time.deltaTime;
		if (timer >= currentDelay)
		{
			currentDelay = delay.GetRandomValue();
			timer = 0f;
			if (flickerRoutine != null)
			{
				CoroutineManager.Singleton.StopCoroutine(flickerRoutine);
			}
			flickerRoutine = CoroutineManager.Singleton.AddCoroutine(Flicker());
		}
	}

	private IEnumerator Flicker()
	{
		if (renderer == null)
		{
			yield break;
		}
		Material startMaterial = ((renderer == null) ? onMaterial : renderer.sharedMaterial);
		int count = numberOfTimes.GetRandomValue();
		for (int i = 0; i < count; i++)
		{
			if (renderer == null)
			{
				yield break;
			}
			if (renderer != null)
			{
				Material sharedMaterial = ((renderer.sharedMaterial == onMaterial) ? offMaterial : onMaterial);
				renderer.sharedMaterial = sharedMaterial;
			}
			yield return new WaitForSeconds(onOffSpan.GetRandomValue());
		}
		if (renderer != null)
		{
			renderer.sharedMaterial = startMaterial;
		}
	}
}
