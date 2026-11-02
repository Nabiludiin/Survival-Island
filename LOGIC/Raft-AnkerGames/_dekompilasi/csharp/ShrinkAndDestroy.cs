using System;
using UnityEngine;

public class ShrinkAndDestroy : MonoBehaviour
{
	public Action OnShrinkComplete;

	public bool destroyAtEnd = true;

	[HideInInspector]
	public float waitTimer;

	[HideInInspector]
	public float totalWaitTime;

	[HideInInspector]
	public float shrinkTimer;

	[HideInInspector]
	public float totalShrinkTime;

	[HideInInspector]
	public Vector3 startScale;

	[HideInInspector]
	public Vector3 endScale;

	private bool initialized;

	public float LifeTime => totalShrinkTime - shrinkTimer;

	private void Update()
	{
		if (!initialized || !gameObject.activeInHierarchy)
		{
			return;
		}
		if (waitTimer < totalWaitTime)
		{
			waitTimer += Time.deltaTime;
			return;
		}
		if (shrinkTimer < totalShrinkTime)
		{
			shrinkTimer += Time.deltaTime;
		}
		float num = Mathf.Clamp01(shrinkTimer / totalShrinkTime);
		transform.localScale = Vector3.Lerp(startScale, endScale, num);
		if (num == 1f)
		{
			enabled = false;
			OnShrinkComplete?.Invoke();
			if (destroyAtEnd)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	public void Initialize(Vector3 startScale, Vector3 endScale, float totalWaitTime, float totalShrinkTime)
	{
		initialized = true;
		this.startScale = startScale;
		this.endScale = endScale;
		this.totalShrinkTime = totalShrinkTime;
		this.totalWaitTime = totalWaitTime;
		transform.localScale = startScale;
	}
}
