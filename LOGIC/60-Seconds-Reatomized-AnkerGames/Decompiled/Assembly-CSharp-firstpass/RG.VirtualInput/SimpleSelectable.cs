using UnityEngine;
using UnityEngine.UI;

namespace RG.VirtualInput;

public class SimpleSelectable : Selectable
{
	[ContextMenu("Set transition to none")]
	public void HelperSetTransitionToNone()
	{
		transition = Transition.None;
	}
}
