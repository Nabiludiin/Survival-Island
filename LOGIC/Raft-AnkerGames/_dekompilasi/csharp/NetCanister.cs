using System;
using System.Collections;
using UnityEngine;

public class NetCanister : Throwable_Object
{
	[SerializeField]
	private Rigidbody[] netEnds;

	public Vector3 startScale = Vector3.zero;

	public Vector3 targetScale = Vector3.zero;

	public float scaleTime = 1f;

	public override void Initialize(ThrowableComponent throwableComponent, uint throwableIndex, uint throwablePickupIndex, Vector3 force, bool colliderEnabled)
	{
		base.Initialize(throwableComponent, throwableIndex, throwablePickupIndex, force, colliderEnabled);
		if (netEnds != null)
		{
			for (int i = 0; i < netEnds.Length; i++)
			{
				netEnds[i].AddForce(force);
				OnHitWater = (Action)Delegate.Combine(OnHitWater, new Action(OnWaterHit));
			}
		}
		transform.localScale = startScale;
		StartCoroutine(ScaleOverTime(scaleTime));
	}

	private void OnWaterHit()
	{
		for (int i = 0; i < netEnds.Length; i++)
		{
			netEnds[i].drag = waterDrag * 2f;
		}
	}

	private IEnumerator ScaleOverTime(float time)
	{
		Vector3 originalScale = startScale;
		Vector3 destinationScale = targetScale;
		float currentTime = 0f;
		do
		{
			transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
			currentTime += Time.deltaTime;
			yield return null;
		}
		while (currentTime <= time);
		yield return null;
	}
}
