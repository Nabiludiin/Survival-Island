using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ScreenEffect
{
	[HideInInspector]
	public Coroutine coroutine;

	protected float alpha;

	public void Enable(MonoBehaviour coroutineBehaviour, SO_ScreenEffect screenEffectAsset)
	{
		if (coroutine != null)
		{
			coroutineBehaviour.StopCoroutine(coroutine);
			coroutine = null;
		}
		coroutine = coroutineBehaviour.StartCoroutine(EnableEffect(screenEffectAsset));
	}

	private IEnumerator EnableEffect(SO_ScreenEffect screenEffectAsset)
	{
		bool fadeIn = alpha < 0.99f;
		if (!fadeIn)
		{
			yield return new WaitForSeconds(screenEffectAsset.timeOnScreen);
		}
		while (true)
		{
			if (fadeIn)
			{
				if (alpha > 0.99f)
				{
					alpha = 1f;
					yield return new WaitForSeconds(screenEffectAsset.timeOnScreen);
					fadeIn = false;
				}
				else
				{
					alpha += Time.deltaTime * screenEffectAsset.fadeInSpeed;
					alpha = Mathf.Clamp(alpha, 0f, 1f);
				}
			}
			else
			{
				alpha -= Time.deltaTime * screenEffectAsset.fadeOutSpeed;
				alpha = Mathf.Clamp(alpha, 0f, 1f);
				if (alpha < 0.05f)
				{
					break;
				}
			}
			Tick();
			yield return null;
		}
		alpha = 0f;
		Tick();
	}

	protected virtual void Tick()
	{
	}
}
