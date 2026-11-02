using System;
using TMPro;

namespace RG.Parsecs.Survival;

public class TextContent : EventContent
{
	private TextMeshProUGUI _textMeshPro;

	public string CurrentText => _textMeshPro.text;

	private void Awake()
	{
		_textMeshPro = GetComponent<TextMeshProUGUI>();
	}

	public void SetText(string localizedString)
	{
		_textMeshPro.text = localizedString;
	}

	public void AppendText(string localizedString)
	{
		if (_textMeshPro == null)
		{
			_textMeshPro = GetComponent<TextMeshProUGUI>();
		}
		if (!string.IsNullOrEmpty(_textMeshPro.text))
		{
			TextMeshProUGUI textMeshPro = _textMeshPro;
			textMeshPro.text = textMeshPro.text + Environment.NewLine + Environment.NewLine;
		}
		_textMeshPro.text += localizedString;
	}
}
