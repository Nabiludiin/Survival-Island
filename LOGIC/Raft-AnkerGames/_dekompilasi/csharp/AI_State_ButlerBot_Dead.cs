using UnityEngine;

public class AI_State_ButlerBot_Dead : AI_State_Dead
{
	public Collider pickupColliderToReset;

	public override void EnterState()
	{
		base.EnterState();
		if (pickupColliderToReset != null)
		{
			pickupColliderToReset.enabled = true;
			pickupColliderToReset.gameObject.SetActiveSafe(value: true);
		}
	}
}
