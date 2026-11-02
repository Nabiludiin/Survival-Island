using UnityEngine;

public class AI_State_RunAway_DraggingPlayer : AI_State
{
	[SerializeField]
	private MovementType movementType = MovementType.NavmeshAgent;

	[SerializeField]
	private TimerEventer dragTimer;

	[SerializeField]
	private AttachPlayer attachPlayer;

	[SerializeField]
	private AI_State stopDraggingState;

	[SerializeField]
	private float movementSpeed;

	private AI_NetworkBehaviour networkBehaviour;

	protected override void Start()
	{
		base.Start();
		networkBehaviour = stateMachine.GetComponent<AI_NetworkBehaviour>();
		dragTimer.Initialize();
	}

	public override void EnterState()
	{
		base.EnterState();
		dragTimer.ResetTimer();
		stateMachine.movement.SetMovementType(movementType);
		stateMachine.movement.SetRotationSpeed(0f);
	}

	public override void ExitState()
	{
		base.ExitState();
		stateMachine.movement.SetRotationSpeed(stateMachine.movement.defaultRotationSpeed);
		if (attachPlayer != null && attachPlayer.HasPlayerAttached)
		{
			attachPlayer.StopCarryingPlayer(manipulatePosition: true, manipulateEuler: false);
		}
	}

	public override void UpdateState()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (attachPlayer != null && attachPlayer.HasPlayerAttached)
		{
			if (dragTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
			{
				stateMachine.ChangeState(stopDraggingState);
			}
			else
			{
				MoveBackwards();
			}
		}
		else
		{
			stateMachine.ChangeState(stopDraggingState);
		}
	}

	private void MoveBackwards()
	{
		Vector3 vector = -stateMachine.movement.model.forward;
		vector.y = 0f;
		switch (movementType)
		{
		case MovementType.NavmeshAgent:
			stateMachine.movement.agent.Move(movementSpeed * Time.deltaTime * vector);
			break;
		case MovementType.Translate:
		case MovementType.CharacterController:
		case MovementType.None:
		case MovementType.Rigidbody_LookObstruction:
		case MovementType.GoldenRule:
		case MovementType.Translate_Simple:
		case MovementType.Rigidbody:
			break;
		}
	}
}
