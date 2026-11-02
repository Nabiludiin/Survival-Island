using TMPro;
using UnityEngine;

namespace RG.Core.GameVersion;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SetGameVersion : MonoBehaviour
{
	[SerializeField]
	private GameVersion GameVersion;

	private TextMeshProUGUI _text;

	private void Awake()
	{
		gameObject.SetActive(Debug.isDebugBuild);
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		_text.text = GameVersion.ToString();
	}
}
