using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AlphaFadeController : MonoBehaviour
{
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private float LerpSpeed = 1f;

	private bool _fadeIn;

	private bool _fadeOut;

	private WaitForEndOfFrame frameWait = new WaitForEndOfFrame();

	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	public IEnumerator FadeIn()
	{
		_canvasGroup.alpha = 0f;
		while (true)
		{
			_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1f, LerpSpeed * Time.deltaTime);
			if (_canvasGroup.alpha >= 0.99f)
			{
				break;
			}
			yield return frameWait;
		}
		_canvasGroup.alpha = 1f;
	}

	public IEnumerator FadeOut()
	{
		_canvasGroup.alpha = 1f;
		while (true)
		{
			_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 0f, LerpSpeed * Time.deltaTime);
			if (_canvasGroup.alpha < 0.01f)
			{
				break;
			}
			yield return frameWait;
		}
		_canvasGroup.alpha = 0f;
	}
}
