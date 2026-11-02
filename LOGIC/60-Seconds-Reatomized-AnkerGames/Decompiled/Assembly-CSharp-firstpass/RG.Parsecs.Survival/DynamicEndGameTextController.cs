using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class DynamicEndGameTextController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _textMesh;

	public DynamicEndGameText DynamicEndGameText;

	public void Awake()
	{
		RefreshText();
	}

	public void RefreshText()
	{
		if (DynamicEndGameText != null && (string)DynamicEndGameText.RuntimeData.RuntimeValue != null && !DynamicEndGameText.RuntimeData.RuntimeValue.IsNullOrEmpty())
		{
			_textMesh.text = DynamicEndGameText.RuntimeData.RuntimeValue.ToString();
			return;
		}
		Debug.LogWarningFormat("Runtime term is null/empty in dynamic text {0}", gameObject.name);
		_textMesh.text = string.Empty;
	}

	public void HideText()
	{
		_textMesh.text = string.Empty;
	}
}
