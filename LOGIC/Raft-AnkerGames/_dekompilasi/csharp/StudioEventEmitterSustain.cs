using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

[Serializable]
public class StudioEventEmitterSustain
{
	public StudioEventEmitter eventEmitter;

	public float sustainStopDelay = 3f;

	[HideInInspector]
	public bool sustaining;

	private Coroutine stopDelayedCoroutine;

	public void Play()
	{
		if (stopDelayedCoroutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(stopDelayedCoroutine);
			stopDelayedCoroutine = null;
		}
		if (sustaining)
		{
			sustaining = false;
			eventEmitter.Stop();
		}
		eventEmitter.PlaySafe();
	}

	public void PlaySafe()
	{
		if (!eventEmitter.IsPlaying())
		{
			Play();
		}
	}

	public void Stop()
	{
		if (!sustaining)
		{
			sustaining = true;
			if (eventEmitter.EventInstance.isValid())
			{
				eventEmitter.EventInstance.keyOff();
			}
			if (sustainStopDelay > 0f && CoroutineManager.Singleton != null)
			{
				stopDelayedCoroutine = CoroutineManager.Singleton.AddCoroutine(StopDelayed());
			}
		}
	}

	public void StopHard()
	{
		eventEmitter.Stop();
	}

	private IEnumerator StopDelayed()
	{
		yield return new WaitForSeconds(sustainStopDelay);
		eventEmitter.Stop();
		stopDelayedCoroutine = null;
	}
}
