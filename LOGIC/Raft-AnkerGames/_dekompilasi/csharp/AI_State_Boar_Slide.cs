using FMODUnity;
using UnityEngine;

public class AI_State_Boar_Slide : AI_State
{
	public AI_State boarRunState;

	public AI_State stunState;

	[HideInInspector]
	public Vector3 targetSlidePoint = Vector3.zero;

	public AI_Movement movement;

	public float minimumSpeed = 2f;

	public float slideMovementSpeed;

	public float slideDeaccelerationMultiplier = 2f;

	[SerializeField]
	private float distanceToPointMin = 0.5f;

	[SerializeField]
	private StudioEventEmitter slideEmitter;

	private AI_StateMachine_Boar boarSM;

	protected override void Awake()
	{
		base.Awake();
		boarSM = stateMachine as AI_StateMachine_Boar;
		movement = boarSM.movement;
	}

	public override void EnterState()
	{
		base.EnterState();
		movement.SetMovementType(MovementType.NavmeshAgent);
		movement.SetMovementSpeed(slideMovementSpeed);
		slideEmitter.PlaySafe();
	}

	public override void ExitState()
	{
		slideEmitter.EventInstance.keyOff();
		base.ExitState();
	}

	public override void UpdateState()
	{
		if (Raft_Network.IsHost)
		{
			base.UpdateState();
			if (movement.MovementSpeed > minimumSpeed)
			{
				movement.SetMovementSpeed(movement.MovementSpeed - Time.deltaTime * slideDeaccelerationMultiplier);
			}
			movement.MoveLook_Navmesh(targetSlidePoint, movement.MovementSpeed);
			if (Vector3.Distance(boarSM.transform.position, targetSlidePoint) < distanceToPointMin)
			{
				stateMachine.ChangeState(stunState);
			}
		}
	}

	private void StopSlideSound()
	{
		slideEmitter.StopSafe();
	}
}
