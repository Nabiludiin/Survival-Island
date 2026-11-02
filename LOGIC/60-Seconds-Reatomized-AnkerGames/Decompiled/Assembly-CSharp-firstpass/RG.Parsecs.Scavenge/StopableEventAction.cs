using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class StopableEventAction : EventAction
{
	[HideInInspector]
	[SerializeField]
	public float DelayStop = -1f;

	public FloatOptionalRange NewDelayStop;

	public override void OnAfterDeserialize()
	{
		if (!float.IsNaN(DelayStop))
		{
			NewDelayStop = DelayStop;
		}
		DelayStop = float.NaN;
		base.OnAfterDeserialize();
	}

	public override void OnBeforeSerialize()
	{
		base.OnBeforeSerialize();
	}

	public void InvokeStop(EventActionArgs args = null)
	{
		if ((float)NewDelayStop > 0f)
		{
			StartCoroutine(DelayedInvokeStop(args, NewDelayStop));
		}
		else
		{
			Stop(args);
		}
	}

	private IEnumerator DelayedInvokeStop(EventActionArgs args, float delay)
	{
		WaitForSeconds w = WaitForSecondsPool.New(delay);
		yield return w;
		WaitForSecondsPool.Store(w, delay);
		Stop(args);
	}

	protected abstract void Stop(EventActionArgs args = null);
}
