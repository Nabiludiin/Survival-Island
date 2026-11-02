using UnityEngine;

public class AI_State_Chase_MamaBear : AI_State_Chase
{
	[SerializeField]
	private float distanceFromOriginLure = 20f;

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost && distanceFromOrigin != distanceFromOriginLure && QuestProgressTracker.HasFinishedQuest(QuestType.MamaBearLurePlaced))
		{
			distanceFromOrigin = distanceFromOriginLure;
		}
	}
}
