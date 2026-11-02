using UnityEngine;

public class AI_State_VarunaBoss_Intro : AI_State
{
	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private float reverseSpeed = 1f;

	[SerializeField]
	private float reverseTime = 1f;

	private float reverseTimer;

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost)
		{
			reverseTimer = 0f;
			QuestProgressTracker.FinishQuestRPC(QuestType.Varuna_Boss_Started);
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost)
		{
			if (reverseTimer < reverseTime)
			{
				reverseTimer += Time.deltaTime;
				stateMachine.transform.position += -stateMachine.transform.forward * Time.deltaTime * reverseSpeed;
			}
			else
			{
				stateMachine.ChangeState(nextState);
			}
		}
	}
}
