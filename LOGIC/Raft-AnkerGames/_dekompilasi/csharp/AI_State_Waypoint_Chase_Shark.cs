using UnityEngine;

public class AI_State_Waypoint_Chase_Shark : AI_State_Waypoint_Chase
{
	private AI_StateMachine_Shark stateMachineShark;

	protected override void Start()
	{
		stateMachineShark = stateMachine as AI_StateMachine_Shark;
	}

	protected override void Destroy()
	{
		base.Destroy();
	}

	public override void EnterState()
	{
		if (Raft_Network.IsHost)
		{
			base.EnterState();
			if (stateMachineShark == null)
			{
				stateMachineShark = stateMachine as AI_StateMachine_Shark;
			}
			if (stateMachineShark.targetToAttack == null)
			{
				stateMachineShark.FindAndSetTargetToAttack();
			}
			if (stateMachineShark.targetToAttack == null)
			{
				stateMachineShark.ChangeState(stateMachineShark.circulationDeepState);
				return;
			}
			chaseTarget = stateMachineShark.targetToAttack;
			lastKnownPlayerPosition = chaseTarget.transform.position;
		}
	}

	public override void OnWorldShift(Vector3 shift)
	{
		base.OnWorldShift(shift);
		lastKnownPlayerPosition -= shift;
	}
}
