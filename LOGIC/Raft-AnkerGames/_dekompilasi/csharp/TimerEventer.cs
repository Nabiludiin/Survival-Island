using System;
using UnityEngine;

[Serializable]
public class TimerEventer
{
	public Action OnTimerReached;

	public Action OnTimerReset;

	[SerializeField]
	private float cooldown;

	[SerializeField]
	private float cooldownOffsetMin;

	[SerializeField]
	private float cooldownOffsetMax;

	private float progress;

	private float originCooldown;

	private bool initialized;

	public TimerEventer()
	{
	}

	public TimerEventer(float cooldown, float startProgress = 0f)
	{
		this.cooldown = cooldown;
		Initialize();
		if (cooldownOffsetMin != cooldownOffsetMax)
		{
			RandomizeCooldown();
		}
		progress = startProgress;
	}

	public void Initialize()
	{
		initialized = true;
		originCooldown = cooldown;
		cooldown = originCooldown;
	}

	public bool DoUpdate(float time, bool autoResetTimer = false)
	{
		if (!initialized)
		{
			Initialize();
		}
		progress += time;
		if (IsTimeBeyondCooldown())
		{
			SendOnTimerReached();
			if (autoResetTimer)
			{
				ResetTimer();
			}
			return true;
		}
		return false;
	}

	public void ResetTimer()
	{
		if (!initialized)
		{
			Initialize();
		}
		progress = 0f;
		if (cooldownOffsetMin != cooldownOffsetMax)
		{
			RandomizeCooldown();
		}
		SendOnTimerReset();
	}

	public void RandomizeCooldown()
	{
		cooldown = originCooldown + UnityEngine.Random.Range(cooldownOffsetMin, cooldownOffsetMax);
	}

	public bool IsTimeBeyondCooldown()
	{
		if (progress >= cooldown)
		{
			return true;
		}
		return false;
	}

	public float GetProgress()
	{
		return progress;
	}

	public float GetNormalizedProgress()
	{
		return Mathf.Clamp01(progress / cooldown);
	}

	public void SetProgress(float progress)
	{
		this.progress = progress;
	}

	public float GetCooldown()
	{
		return cooldown;
	}

	private void SendOnTimerReached()
	{
		OnTimerReached?.Invoke();
	}

	private void SendOnTimerReset()
	{
		OnTimerReset?.Invoke();
	}
}
