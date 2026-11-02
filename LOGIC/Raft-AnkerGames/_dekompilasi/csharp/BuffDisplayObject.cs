using System;
using UnityEngine;
using UnityEngine.UI;

public class BuffDisplayObject : MonoBehaviour
{
	public Action OnTimerExpire;

	public BuffDisplayType type;

	[HideInInspector]
	public bool isExpired;

	[SerializeField]
	private Image maskRadialImage;

	[SerializeField]
	private Image radialImage;

	[SerializeField]
	private Image centerImage;

	private float duration;

	private float progress;

	private void Update()
	{
		if (!isExpired)
		{
			progress += Time.deltaTime;
			maskRadialImage.fillAmount = 1f - Mathf.Clamp01(progress / duration);
			if (progress >= duration)
			{
				Expire();
			}
		}
	}

	public void StartTimer(float duration, float progress, BuffDisplayType type, Sprite icon, Color color)
	{
		this.duration = duration;
		this.progress = progress;
		this.type = type;
		centerImage.sprite = icon;
		radialImage.color = color;
		maskRadialImage.fillAmount = 1f - Mathf.Clamp01(progress / duration);
		isExpired = false;
	}

	public void SetNewDurationAndProgress(float duration, float progress)
	{
		if (!isExpired)
		{
			this.duration = duration;
			this.progress = progress;
			maskRadialImage.fillAmount = 1f - Mathf.Clamp01(progress / duration);
		}
	}

	public void Expire()
	{
		if (!isExpired)
		{
			maskRadialImage.fillAmount = 0f;
			OnTimerExpire.CallSafe();
			isExpired = true;
		}
	}
}
