using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class ValueIconController : MonoBehaviour
{
	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private Image _valueImage;

	public void SetSprite(Sprite sprite)
	{
		_backgroundImage.sprite = sprite;
		_valueImage.sprite = sprite;
	}

	public void SetValue(float value)
	{
		if (value < 0f)
		{
			value = 0f;
		}
		else if (value > 1f)
		{
			value = 1f;
		}
		_valueImage.fillAmount = value;
	}
}
