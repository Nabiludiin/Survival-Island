using RG.Parsecs.EventEditor;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class GetStatValue : MonoBehaviour
{
	public string key;

	public GlobalIntVariable variable;

	public string outOfNumber = "";

	private void Start()
	{
		TextMeshProUGUI component = GetComponent<TextMeshProUGUI>();
		int num = -1;
		string text = ((!(variable == null)) ? variable.Value : StatsManager.Instance.GetGlobalData(key)).ToString();
		if (!string.IsNullOrEmpty(outOfNumber))
		{
			text += outOfNumber;
		}
		component.text = text;
	}
}
