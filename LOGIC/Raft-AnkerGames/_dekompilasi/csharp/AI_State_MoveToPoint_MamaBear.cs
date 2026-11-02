using UnityEngine;

public class AI_State_MoveToPoint_MamaBear : AI_State_MoveToPoint
{
	[SerializeField]
	private AI_State reachCaveState;

	[SerializeField]
	private AI_State reachLureState;

	private AI_StateMachine_MamaBear stateMachine_MamaBear;

	protected override void Start()
	{
		base.Start();
		stateMachine_MamaBear = stateMachine as AI_StateMachine_MamaBear;
	}

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost)
		{
			if (stateMachine_MamaBear == null)
			{
				stateMachine_MamaBear = stateMachine as AI_StateMachine_MamaBear;
			}
			if (QuestProgressTracker.HasFinishedQuest(QuestType.MamaBearLurePlaced))
			{
				reachPointState = reachLureState;
				targetPoint = stateMachine_MamaBear.lureTransform.position;
			}
			else
			{
				reachPointState = reachCaveState;
				targetPoint = stateMachine_MamaBear.caveTransform.position;
			}
		}
	}
}
