using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Scavenge;

public class HintAnnouncer : MonoBehaviour
{
	public TextMeshProUGUI _hintText;

	public Image _hintImage;

	public Image _hintImageOnScreen;

	private Coroutine _timeoutCoroutine;

	public void Announce(ScavengeTutorialHint hint)
	{
		if (_timeoutCoroutine != null)
		{
			StopCoroutine(_timeoutCoroutine);
		}
		_hintText.text = hint.TextLoc;
		_hintText.enabled = true;
		_hintText.alignment = (TextAlignmentOptions)(hint.alignment | (VerticalAlignmentOptions)2);
		if (hint.ScreenSprite != null)
		{
			_hintImageOnScreen.sprite = hint.ScreenSprite;
			_hintImageOnScreen.enabled = true;
		}
		else
		{
			_hintImageOnScreen.enabled = false;
		}
		if (hint.Sprite != null)
		{
			_hintImage.sprite = hint.Sprite;
			_hintImage.enabled = true;
		}
		else
		{
			_hintImage.enabled = false;
		}
		_timeoutCoroutine = StartCoroutine(_timeout(hint.Duration));
	}

	private IEnumerator _timeout(float duration)
	{
		yield return new WaitForSeconds(duration);
		_hintText.enabled = false;
		_timeoutCoroutine = null;
	}
}
