using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class ChoiceSummaryEventContent : EventContent
{
	[SerializeField]
	[Tooltip("Reference to images that will be used to display the choice.")]
	private Image[] _choices;

	public void SetImages(Sprite[] choices, int choice)
	{
		for (int i = 0; i < _choices.Length; i++)
		{
			if (choices[i] == null)
			{
				_choices[i].gameObject.SetActive(value: false);
				continue;
			}
			_choices[i].sprite = choices[i];
			if (i == choice)
			{
				_choices[i].color = new Color(_choices[i].color.r, _choices[i].color.g, _choices[i].color.b, 1f);
			}
			else
			{
				_choices[i].color = new Color(_choices[i].color.r, _choices[i].color.g, _choices[i].color.b, _choices[i].color.a / 2f);
			}
		}
	}
}
