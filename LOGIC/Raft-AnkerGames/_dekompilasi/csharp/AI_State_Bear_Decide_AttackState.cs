using UnityEngine;

public class AI_State_Bear_Decide_AttackState : AI_State_DecideState
{
	[SerializeField]
	private MovementDeltaTracker deltaTracker;

	public AI_State jumpAttack;

	public AI_State randomizeAttackState;

	public float targetMovementDeltaTriggerValue = 0.3f;

	public float minimumApporximatedJumpDistance = 2f;

	private AI_StateMachine_Bear bearStateMachine;

	protected override void Start()
	{
		base.Start();
		bearStateMachine = stateMachine as AI_StateMachine_Bear;
	}

	public override void EnterState()
	{
		base.EnterState();
		Network_Player network_Player = Helper.ClosestPlayerToPoint(bearStateMachine.transform.position, includeDeadPlayers: false);
		if ((network_Player != null && network_Player != bearStateMachine.closestTargetPlayer) || (network_Player != null && bearStateMachine.closestTargetPlayer == null))
		{
			bearStateMachine.closestTargetPlayer = network_Player;
		}
		DecideState();
	}

	public override void DecideState()
	{
		base.DecideState();
		Network_Player closestTargetPlayer = bearStateMachine.closestTargetPlayer;
		Vector3 averageDeltaVector = closestTargetPlayer.MovementDeltaTracker.GetAverageDeltaVector();
		averageDeltaVector.y = 0f;
		Vector3 approximateTrackedTransformPositionWithDeltaMovement = closestTargetPlayer.MovementDeltaTracker.GetApproximateTrackedTransformPositionWithDeltaMovement(3f, normalizeDirection: false);
		if (Vector3.Distance(deltaTracker.trackedTransform.position, approximateTrackedTransformPositionWithDeltaMovement) >= minimumApporximatedJumpDistance && closestTargetPlayer.MovementDeltaTracker.GetAverageDeltaVector().sqrMagnitude >= targetMovementDeltaTriggerValue)
		{
			bearStateMachine.ChangeState(jumpAttack);
		}
		else
		{
			bearStateMachine.ChangeState(randomizeAttackState);
		}
	}
}
