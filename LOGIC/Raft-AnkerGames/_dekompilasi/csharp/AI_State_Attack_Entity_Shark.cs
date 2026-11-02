using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Attack_Entity_Shark : AI_State
{
	private enum SharkAttackState
	{
		AggroToBitePlayer,
		DrivingBy,
		ReturningToAttackLocation
	}

	public AI_StateMachine_Shark stateMachineShark;

	public AI_State circulationState;

	private SharkAttackState sharkAttackState;

	[SerializeField]
	private int _attackPlayerDamage = 30;

	[SerializeField]
	private float startBiteAnimationTimeToDistance = 0.8f;

	[SerializeField]
	private GizmoBox biteBox;

	[SerializeField]
	private float returnToAttackPointSpeedMultiplier = 1.5f;

	[SerializeField]
	private float attackSwimSpeedMultiplier = 2f;

	[SerializeField]
	private float attackRotationalMultiplier = 3f;

	private bool hasStartedAttackAnimation;

	[SerializeField]
	private float totallBiteAnimationDuration = 0.8f;

	public EstimatedTimeToReachPoint reachPointEstimater;

	private float returnToLatestAttackPositionMaxDuration = 5f;

	[SerializeField]
	private float driveByMissMinimumDuration = 1f;

	[SerializeField]
	private float driveByMissMaximumDuration = 2.5f;

	[SerializeField]
	private float driveByHitMinimumDuration = 4f;

	[SerializeField]
	private float driveByHitMaximumDuration = 6f;

	[SerializeField]
	private float dmgTreshhold = 10f;

	private float currentDmgRecieved;

	[SerializeField]
	public AI_State dmgTreshholdState;

	private Vector3 latestBitePosition;

	private Vector3 drivebyDirection;

	private float currentDriveByDuration = 4f;

	private float driveByTimer;

	[SerializeField]
	private float findTargetCooldown = 1f;

	private float findTargetProgress;

	private Vector3 playerCameraOffset = new Vector3(0f, 0.75f, 0f);

	private float currentBiteProgress;

	private Raft_Network network;

	private bool damageTreshHoldReached;

	private bool hasStartedAttack;

	public bool HasBitten => sharkAttackState != SharkAttackState.AggroToBitePlayer;

	public int AttackPlayerDamage => (int)((float)_attackPlayerDamage * GameModeValueManager.GetCurrentGameModeValue().sharkVariables.damageMultiplier);

	private void ResetState()
	{
		sharkAttackState = SharkAttackState.AggroToBitePlayer;
		driveByTimer = 0f;
		latestBitePosition = transform.position;
		hasStartedAttack = false;
		damageTreshHoldReached = false;
	}

	protected override void Start()
	{
		base.Start();
		stateMachineShark = stateMachine as AI_StateMachine_Shark;
		AnimatorController animController = stateMachineShark.animController;
		animController.OnAnimationEvent = (Action<string>)Delegate.Combine(animController.OnAnimationEvent, new Action<string>(AnimationString));
		if (Raft_Network.IsHost)
		{
			AI_StateMachine_Shark aI_StateMachine_Shark = stateMachineShark;
			aI_StateMachine_Shark.OnDamageTakenAction = (Action<float>)Delegate.Combine(aI_StateMachine_Shark.OnDamageTakenAction, new Action<float>(OnDamageTaken));
		}
	}

	protected override void Destroy()
	{
		base.Destroy();
		if (Raft_Network.IsHost)
		{
			AI_StateMachine_Shark aI_StateMachine_Shark = stateMachineShark;
			aI_StateMachine_Shark.OnDamageTakenAction = (Action<float>)Delegate.Remove(aI_StateMachine_Shark.OnDamageTakenAction, new Action<float>(OnDamageTaken));
		}
	}

	public override void UpdateState()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		base.UpdateState();
		findTargetProgress += Time.deltaTime;
		if (hasStartedAttackAnimation)
		{
			currentBiteProgress += Time.deltaTime;
			if (currentBiteProgress > totallBiteAnimationDuration)
			{
				currentBiteProgress = totallBiteAnimationDuration;
			}
		}
		else
		{
			currentBiteProgress = 0f;
		}
		if (findTargetProgress > findTargetCooldown)
		{
			stateMachineShark.FindAndSetTargetToAttack();
			findTargetProgress = 0f;
		}
		Network_Player targetToAttack = stateMachineShark.targetToAttack;
		if (sharkAttackState == SharkAttackState.AggroToBitePlayer)
		{
			if (Helper.IsValidWaterTarget(targetToAttack))
			{
				float movementSpeed = stateMachineShark.movement.MovementSpeed;
				float num = stateMachineShark.movement.RotationSpeed;
				float num2 = 0f;
				num2 = ((currentBiteProgress != 0f) ? (Vector3.Distance(transform.position, targetToAttack.MovementDeltaTracker.GetApproximateTrackedTransformPositionWithDeltaMovement(1.5f, normalizeDirection: false)) / (totallBiteAnimationDuration / currentBiteProgress) / Time.deltaTime) : (Vector3.Distance(transform.position, targetToAttack.transform.position) / totallBiteAnimationDuration / Time.deltaTime));
				if (num2 > 15f)
				{
					num2 = 15f;
				}
				if (hasStartedAttack)
				{
					movementSpeed = num2;
					num *= attackRotationalMultiplier;
				}
				if (stateMachine.movement.MovementType == MovementType.Rigidbody_LookObstruction)
				{
					stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, targetToAttack.transform.position + playerCameraOffset, movementSpeed, num, stateMachineShark.rigid);
				}
				else if (stateMachine.movement.MovementType == MovementType.GoldenRule)
				{
					stateMachine.movement.GoldenRuleMovement(targetToAttack.transform.position + playerCameraOffset, stateMachine.rigid, movementSpeed);
				}
				reachPointEstimater.RecalculateEstimatedTime(targetToAttack.MovementDeltaTracker.GetApproximateTrackedTransformPositionWithDeltaMovement(1.5f, normalizeDirection: false), stateMachineShark.movement.MovementSpeed);
				if (reachPointEstimater.TimeBetweenObjects <= startBiteAnimationTimeToDistance && Helper.HasClearVisionAndWithinRange(transform.position, targetToAttack.transform.position, stateMachineShark.obstructionMask, stateMachineShark.playerVisionRange) && !hasStartedAttack)
				{
					hasStartedAttack = true;
					TriggerAttackAnimation();
				}
			}
			else
			{
				stateMachine.ChangeState(circulationState);
			}
		}
		else if (sharkAttackState == SharkAttackState.DrivingBy)
		{
			if (stateMachine.movement.MovementType == MovementType.Rigidbody_LookObstruction)
			{
				stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, stateMachineShark.transform.position + drivebyDirection, stateMachineShark.movement.MovementSpeed, stateMachineShark.movement.RotationSpeed, stateMachineShark.rigid);
			}
			else if (stateMachine.movement.MovementType == MovementType.GoldenRule)
			{
				stateMachine.movement.GoldenRuleMovement(stateMachineShark.transform.position + drivebyDirection, stateMachine.rigid, stateMachine.movement.MovementSpeed);
			}
			driveByTimer += Time.deltaTime;
			if (driveByTimer >= currentDriveByDuration)
			{
				sharkAttackState = SharkAttackState.ReturningToAttackLocation;
			}
		}
		else
		{
			if (sharkAttackState != SharkAttackState.ReturningToAttackLocation)
			{
				return;
			}
			if (Vector3.Distance(stateMachineShark.transform.position, latestBitePosition) <= 3f)
			{
				stateMachine.ChangeState(circulationState);
				return;
			}
			if (stateMachine.movement.MovementType == MovementType.Rigidbody_LookObstruction)
			{
				stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, latestBitePosition, stateMachineShark.movement.MovementSpeed, stateMachineShark.movement.RotationSpeed, stateMachineShark.rigid);
			}
			else if (stateMachine.movement.MovementType == MovementType.GoldenRule)
			{
				stateMachine.movement.GoldenRuleMovement(latestBitePosition, stateMachine.rigid, stateMachine.movement.MovementSpeed);
			}
			if (!Helper.IsValidWaterTarget(targetToAttack))
			{
				stateMachineShark.ChangeState(stateMachineShark.circulationDeepState);
			}
			else if (Helper.HasClearVisionAndWithinRange(stateMachineShark.transform.position, targetToAttack.transform.position, stateMachineShark.obstructionMask, stateMachineShark.playerVisionRange))
			{
				stateMachineShark.ChangeState(this);
			}
		}
	}

	private float LerpMovementSpeedByDistance(float distance, float timeRemaining)
	{
		return 0f;
	}

	private void OnDamageTaken(float damage)
	{
		if (stateMachineShark.currentState == this && hasStartedAttack)
		{
			currentDmgRecieved += damage;
			if (currentDmgRecieved >= dmgTreshhold)
			{
				damageTreshHoldReached = true;
				ForceDriveBy();
			}
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		ResetState();
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (stateMachineShark == null)
		{
			stateMachineShark = stateMachine as AI_StateMachine_Shark;
		}
		if (Raft_Network.IsHost)
		{
			stateMachineShark.FindAndSetTargetToAttack();
		}
	}

	public override void OnWorldShift(Vector3 shift)
	{
		base.OnWorldShift(shift);
		latestBitePosition -= shift;
	}

	public void AnimationString(string animationName)
	{
		if (animationName == "AttackPlayer")
		{
			AttemptAttack();
		}
	}

	public void TriggerAttackAnimation()
	{
		hasStartedAttackAnimation = true;
		stateMachineShark.animController.SetTrigger("Attack");
	}

	public void ForceDriveBy(bool longDuration = true)
	{
		if (longDuration)
		{
			currentDriveByDuration = UnityEngine.Random.Range(driveByHitMinimumDuration, driveByHitMaximumDuration);
		}
		else
		{
			currentDriveByDuration = UnityEngine.Random.Range(driveByMissMinimumDuration, driveByMissMaximumDuration);
		}
		latestBitePosition = stateMachineShark.transform.position;
		sharkAttackState = SharkAttackState.DrivingBy;
		drivebyDirection = stateMachineShark.transform.forward + Vector3.down;
	}

	public void AttemptAttack()
	{
		hasStartedAttackAnimation = false;
		Network_Player network_Player = Helper.ClosestPlayerInWaterToPoint(stateMachine.transform.position, stateMachineShark.playerVisionRange, includeDeadPlayers: false);
		if (stateMachineShark.soundemitter_bitePlayer != null)
		{
			stateMachineShark.soundemitter_bitePlayer.Play();
		}
		if (network_Player == null || !Raft_Network.IsHost || damageTreshHoldReached)
		{
			return;
		}
		hasStartedAttack = false;
		if (HasBitten || network_Player.PersonController.HasRaftAsParent)
		{
			return;
		}
		Vector3.Distance(transform.position, network_Player.transform.position);
		IEnumerable<Collider> overlaps = biteBox.GetOverlaps(includeInactiveObjects: true, LayerMasks.MASK_Players, 1f);
		bool flag = false;
		foreach (Collider item in (IEnumerable)overlaps)
		{
			Network_Entity componentInChildren = item.GetComponentInChildren<Network_Entity>();
			if (componentInChildren != null)
			{
				flag = true;
				network.hostNetwork.DamageEntity(componentInChildren, null, AttackPlayerDamage, Vector3.zero, Vector3.zero, EntityType.Enemy);
				if (componentInChildren.IsDead)
				{
					AnalyticsHelper.CustomEventString(AnalyticsEvent.ANIMAL_KILLPLAYER, new Dictionary<string, object> { 
					{
						"AnimalType",
						AI_NetworkBehaviourType.Shark
					} });
				}
			}
		}
		if (flag)
		{
			ForceDriveBy();
		}
		else
		{
			ForceDriveBy(longDuration: false);
		}
	}

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
		stateMachineShark.targetToAttack = parameters[0] as Network_Player;
	}
}
