using System;
using UnityEngine;

public class Intervaller : MonoBehaviour
{
	public Action OnIntervalActivate;

	public Action OnIntervalDeactivate;

	[SerializeField]
	protected float activityDuration = 1f;

	[SerializeField]
	protected float downTimeDuration = 0.5f;

	private float currentTime;

	private bool currentlyActive;

	public void DoUpdate(float time)
	{
		float time2 = time % (activityDuration + downTimeDuration);
		if (IsTimeInActiveIntervall(time2))
		{
			if (!currentlyActive)
			{
				ActivateInterval();
			}
		}
		else if (currentlyActive)
		{
			DeActivateInterval();
		}
	}

	protected virtual void ActivateInterval()
	{
		currentlyActive = true;
		OnIntervalActivate?.Invoke();
	}

	protected virtual void DeActivateInterval()
	{
		currentlyActive = false;
		OnIntervalDeactivate?.Invoke();
	}

	private bool IsTimeInActiveIntervall(float time)
	{
		if (time >= activityDuration)
		{
			return false;
		}
		return true;
	}
}
