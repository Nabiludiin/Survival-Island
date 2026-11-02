using System;
using System.Text;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class LocalizeMultiTexts : MonoBehaviour
{
	public LocalizedString[] content;

	private TextMeshProUGUI label;

	private void Awake()
	{
		label = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		label.text = ConvertStringArrayToString(content);
	}

	private static string ConvertStringArrayToString(LocalizedString[] array)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string value in array)
		{
			stringBuilder.Append(value);
			stringBuilder.Append(Environment.NewLine);
		}
		return stringBuilder.ToString();
	}
}
