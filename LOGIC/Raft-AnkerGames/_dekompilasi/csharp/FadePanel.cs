using System.Collections;
using UnityEngine;

public class FadePanel : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public IEnumerator FadeToAlpha(float targetAlpha, float speed)
	{
		while (canvasGroup.alpha != targetAlpha)
		{
			canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.unscaledDeltaTime * speed);
			yield return null;
		}
	}

	public void SetAlpha(float alpha)
	{
		canvasGroup.alpha = alpha;
	}
}
