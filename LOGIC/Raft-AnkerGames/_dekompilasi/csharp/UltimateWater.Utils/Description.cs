using UnityEngine;

namespace UltimateWater.Utils;

[AddComponentMenu("Ultimate Water/Utils/Description")]
public class Description : MonoBehaviour
{
	[Multiline]
	public string Text;

	public static bool Editable;

	[ContextMenu("Toggle Edit")]
	private void ToggleEdit()
	{
		Editable = !Editable;
	}
}
