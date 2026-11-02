using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class FontColorSetter : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private Color _colorToSet;

	private void Awake()
	{
		if (_text != null)
		{
			_text.faceColor = _colorToSet;
		}
	}
}
