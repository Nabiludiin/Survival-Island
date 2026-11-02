using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

[RequireComponent(typeof(Image))]
public class UseAtlasReference : MonoBehaviour
{
	private string _spriteName;

	private Image _image;

	private void Awake()
	{
		_image = GetComponent<Image>();
		_spriteName = _image.sprite.name;
		Sprite sprite = ContentManager.GetSprite(_spriteName);
		if (sprite != null)
		{
			_image.sprite = sprite;
		}
	}
}
