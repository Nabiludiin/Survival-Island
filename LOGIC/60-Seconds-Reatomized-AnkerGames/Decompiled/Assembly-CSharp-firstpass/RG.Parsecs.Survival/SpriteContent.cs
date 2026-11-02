using System;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class SpriteContent : EventContent
{
	[SerializeField]
	private HorizontalLayoutGroup _horizontalLayoutGroup;

	[SerializeField]
	private Image _image;

	public void SetSprite(Sprite sprite, EventContentData.ESpriteAlign align)
	{
		_image.sprite = sprite;
		switch (align)
		{
		case EventContentData.ESpriteAlign.LEFT:
			_horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			break;
		case EventContentData.ESpriteAlign.CENTER:
			_horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
			break;
		case EventContentData.ESpriteAlign.RIGHT:
			_horizontalLayoutGroup.childAlignment = TextAnchor.MiddleRight;
			break;
		default:
			throw new ArgumentOutOfRangeException("align", align, null);
		}
	}
}
