using UnityEngine;

public class AI_State_Decay_Domestic : AI_State_Decay
{
	public GameObject gameObjectToDisable;

	public override void EnterState()
	{
		base.EnterState();
		if (gameObjectToDisable != null)
		{
			gameObjectToDisable.SetActiveSafe(value: false);
		}
	}
}
