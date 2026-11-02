using RG.Parsecs.EventEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

public class IsCaptainAvailable : MonoBehaviour
{
	[SerializeField]
	private GlobalBoolVariable _variable;

	[SerializeField]
	private Toggle _toggle;

	private void Start()
	{
		_toggle.interactable = _variable.Value;
	}
}
