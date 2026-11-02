using UnityEngine;

public class RaycastInteractable_Redirect_JusticeScaleContainer : RaycastInteractable_Redirect
{
	[SerializeField]
	private float maxPlayerYDiff = 0.25f;

	[SerializeField]
	private JusticeScale scale;

	private Network_Player localPlayer;

	public override RaycastInteractable RaycastInteractable
	{
		get
		{
			if (localPlayer == null)
			{
				localPlayer = ComponentManager<Network_Player>.Value;
			}
			if (scale.CanAddWeight && localPlayer != null && localPlayer.PersonController.IsConsideredGrounded && Mathf.Abs(transform.position.y - localPlayer.transform.position.y) <= maxPlayerYDiff)
			{
				return raycastInteractable;
			}
			return null;
		}
	}
}
