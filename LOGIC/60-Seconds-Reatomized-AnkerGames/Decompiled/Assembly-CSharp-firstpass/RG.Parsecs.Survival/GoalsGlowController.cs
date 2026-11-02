using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class GoalsGlowController : MonoBehaviour
{
	[SerializeField]
	private Image _checkmark;

	[SerializeField]
	private Image _highlight;

	[SerializeField]
	private float _lerpSpeed = 0.1f;

	private bool _shouldHide;

	private bool _visible;

	private bool _decrease;

	private float _fadeLerp = 1f;

	private const float MINIMUM_ALPHA = 0.05f;

	private const float MAXIMUM_ALPHA = 0.95f;

	public bool Visible => _visible;

	public void Show()
	{
		_checkmark.gameObject.SetActive(value: true);
		_highlight.gameObject.SetActive(value: true);
		_shouldHide = false;
		_visible = true;
		_fadeLerp = 1f;
		_decrease = false;
	}

	public void Hide()
	{
		_shouldHide = true;
	}

	private void Update()
	{
		if (!_visible)
		{
			return;
		}
		if (!_shouldHide)
		{
			if (_decrease)
			{
				_fadeLerp = Mathf.Lerp(_fadeLerp, 0f, _lerpSpeed * Time.deltaTime);
				if (_fadeLerp <= 0.05f)
				{
					_decrease = false;
				}
			}
			else
			{
				_fadeLerp = Mathf.Lerp(_fadeLerp, 1f, _lerpSpeed * Time.deltaTime);
				if (_fadeLerp >= 0.95f)
				{
					_decrease = true;
				}
			}
			_checkmark.color = new Color(_checkmark.color.r, _checkmark.color.g, _checkmark.color.b, _fadeLerp);
			_highlight.color = new Color(_highlight.color.r, _highlight.color.g, _highlight.color.b, _fadeLerp);
		}
		else
		{
			_checkmark.gameObject.SetActive(value: false);
			_highlight.gameObject.SetActive(value: false);
			_visible = false;
		}
	}
}
