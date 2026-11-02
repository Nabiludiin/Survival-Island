using UnityEngine;

public class AI_State_Wait_Domestic_Eat : AI_State_Wait
{
	[SerializeField]
	private float eatTime;

	[SerializeField]
	[Range(0f, 100f)]
	private float eatChance;

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost && Random.Range(0f, 100f) < eatChance)
		{
			timeToWait += eatTime;
			startWaitTime += eatTime;
		}
	}
}
