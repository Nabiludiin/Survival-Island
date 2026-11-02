using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class EndGameTextController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _textMesh;

	public EndGameTextId textId;

	public void SetText(string text)
	{
		_textMesh.text = text;
	}
}
