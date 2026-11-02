using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]
public class SetFontMaterial : MonoBehaviour
{
	[SerializeField]
	private FontMaterialsForLocalization _fontData;

	private TextMeshProUGUI _textMeshPro;

	private TMP_FontAsset _lastFont;

	private void Awake()
	{
		_textMeshPro = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		SetMaterial();
	}

	private void Update()
	{
		if (_textMeshPro.font != _lastFont)
		{
			SetMaterial();
		}
	}

	private void SetMaterial()
	{
		for (int i = 0; i < _fontData.OriginalFont.Count; i++)
		{
			if (!(_fontData.OriginalFont[i] == null) && _textMeshPro.font == _fontData.OriginalFont[i])
			{
				_textMeshPro.fontMaterial = _fontData.FontMaterial[i];
				_lastFont = _textMeshPro.font;
			}
		}
	}
}
