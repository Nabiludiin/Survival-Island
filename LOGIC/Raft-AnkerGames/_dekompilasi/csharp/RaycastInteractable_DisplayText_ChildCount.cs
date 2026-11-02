using UnityEngine;

public class RaycastInteractable_DisplayText_ChildCount : RaycastInteractable_DisplayText
{
	[SerializeField]
	private Transform parent;

	[SerializeField]
	private int childCount;

	protected override void OnIsRayed()
	{
		if (parent != null && parent.childCount == childCount)
		{
			base.OnIsRayed();
		}
		else if (isDisplayingText)
		{
			HideDisplayText();
		}
	}
}
