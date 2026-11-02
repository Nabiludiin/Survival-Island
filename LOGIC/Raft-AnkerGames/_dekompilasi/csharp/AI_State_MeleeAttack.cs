using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

public class AI_State_MeleeAttack : AI_State
{
	[SerializeField]
	protected AI_State noTargetState;

	[SerializeField]
	protected AI_State outOfRangeState;

	[SerializeField]
	protected AI_State noVisionState;

	[SerializeField]
	protected AI_State attackDoneAlternateState;

	[SerializeField]
	protected AI_State attackMissedState;

	[SerializeField]
	private float attackCooldown = 1f;

	[SerializeField]
	private float minAttackCooldownOffset;

	[SerializeField]
	private float maxAttackCooldownOffset;

	[Tooltip("Determines how close to target we need to be start attack animation")]
	[SerializeField]
	protected float startAttackRange = 1.5f;

	[Tooltip("Determines how far into the attack it will reach")]
	[SerializeField]
	protected float effectiveAttackRange = 1.7f;

	[FormerlySerializedAs("finishAttackRange")]
	[Tooltip("If target is more than this distance away during attack animation, switch to outOfRangeState")]
	[SerializeField]
	protected float abortAttackRange = 1.5f;

	[SerializeField]
	protected float attackDamage = 10f;

	[Tooltip("Maximum angle between forward and target that counts as hit")]
	[SerializeField]
	protected float attackAngle = 10f;

	[SerializeField]
	protected bool chaseWhileAttacking;

	[SerializeField]
	protected float chaseMovementSpeed = 1f;

	[SerializeField]
	protected float chaseDistance = 5f;

	[SerializeField]
	private MovementType chaseAttackMovementType = MovementType.NavmeshAgent;

	[SerializeField]
	protected bool rotateTowardsTarget = true;

	[SerializeField]
	private bool lerpRotateSpeed;

	[SerializeField]
	private bool rotateWhileAnimating = true;

	[SerializeField]
	private bool ignoreZRotation;

	[SerializeField]
	private float rotationSpeed = 5f;

	[SerializeField]
	protected float rotationSpeedWhenAttacking = 5f;

	[SerializeField]
	private float rotationLerpSpeed = 1f;

	[SerializeField]
	protected AnimatorController animatorController;

	[SerializeField]
	protected string attackAnimationParameter = string.Empty;

	[SerializeField]
	protected string endAttackAnimationParameter = string.Empty;

	[SerializeField]
	private ParticleSystem attackParticles;

	public EventReference er_attack;

	public EventReference er_successfullAttack;

	[SerializeField]
	private float successfullAttackSoundCooldown;

	[SerializeField]
	private bool ignoreObstructionDuringAttack;

	private float successfullAttackTimeStamp;

	protected bool doingAttackAnimation;

	protected bool doingAnimation;

	protected bool hasAttackAnimationParameter;

	protected bool hasEndAttackAnimationParameter;

	private bool canAttack = true;

	private bool hasAttacked;

	private Vector3 startChasePosition;

	protected Network_Host networkHost;

	protected Network_Player targetPlayer;

	private float currentRotationSpeed;

	private EntityVariables gamemodeVariables;

	private SoundManager soundManager;

	public float AttackDamage
	{
		get
		{
			if (gamemodeVariables == null)
			{
				return attackDamage;
			}
			return attackDamage * gamemodeVariables.damageMultiplier;
		}
	}

	protected override void Start()
	{
		base.Start();
		canAttack = true;
		hasAttackAnimationParameter = !string.IsNullOrEmpty(attackAnimationParameter);
		hasEndAttackAnimationParameter = !string.IsNullOrEmpty(endAttackAnimationParameter);
		if (this.animatorController != null)
		{
			AnimatorController animatorController = this.animatorController;
			animatorController.OnAnimationEvent = (Action<string>)Delegate.Combine(animatorController.OnAnimationEvent, new Action<string>(OnAnimationEvent));
		}
		soundManager = ComponentManager<SoundManager>.Value;
		AI_NetworkBehaviour componentInParent = GetComponentInParent<AI_NetworkBehaviour>();
		if (componentInParent != null)
		{
			gamemodeVariables = GameModeValueManager.GetDifficultyEntityVariableFromAINetworkBehaviourType(componentInParent.behaviourType);
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		if (networkHost == null)
		{
			networkHost = ComponentManager<Network_Host>.Value;
		}
		if (Raft_Network.IsHost)
		{
			hasAttacked = false;
			doingAttackAnimation = (doingAnimation = false);
			startChasePosition = stateMachine.transform.position;
			currentRotationSpeed = 0f;
			stateMachine.movement.LerpMovementSpeedTowards(chaseMovementSpeed, 1f, stateMachine.movement.MovementSpeed);
			stateMachine.movement.LerpRotationSpeedTowards(rotationSpeed, 1f, stateMachine.movement.RotationSpeed);
		}
		if (chaseWhileAttacking && stateMachine.movement.agent != null)
		{
			stateMachine.movement.SetMovementType(MovementType.NavmeshAgent);
		}
		else
		{
			stateMachine.movement.SetMovementType(MovementType.None);
		}
		if (Raft_Network.IsHost && !canAttack && !IsInvoking("ResetAttackCooldown"))
		{
			canAttack = true;
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		if (Raft_Network.IsHost)
		{
			doingAttackAnimation = false;
			doingAnimation = false;
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost)
		{
			Network_Player network_Player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, includeDeadPlayers: false);
			if (network_Player == null)
			{
				SwitchTargetPlayer(null);
				return;
			}
			SwitchTargetPlayer(network_Player);
			if (!(targetPlayer != null))
			{
				return;
			}
			float num = Vector3.Distance(transform.position, targetPlayer.FeetPosition);
			bool flag = chaseWhileAttacking && num > startAttackRange;
			if (flag)
			{
				if (Vector3.Distance(stateMachine.transform.position, startChasePosition) >= chaseDistance)
				{
					AbortAttack();
					if (stateMachine.movement.agent != null)
					{
						stateMachine.movement.agent.StopSafe();
					}
					stateMachine.ChangeState(outOfRangeState);
					return;
				}
				if (animatorController != null && animatorController.animator != null)
				{
					animatorController.SetFloat("Speed", Mathf.MoveTowards(animatorController.animator.GetFloat("Speed"), 2f, Time.deltaTime * 4f));
				}
				HandleChaseAttackMovement(targetPlayer.FeetPosition, chaseAttackMovementType);
			}
			else
			{
				if (animatorController != null && animatorController.animator != null)
				{
					animatorController.SetFloat("Speed", Mathf.MoveTowards(animatorController.animator.GetFloat("Speed"), 0f, Time.deltaTime * 4f));
				}
				if (stateMachine.movement.agent != null)
				{
					stateMachine.movement.agent.StopSafe();
				}
			}
			if (outOfRangeState != null && !flag)
			{
				if (num > startAttackRange && !doingAttackAnimation && !doingAnimation)
				{
					stateMachine.ChangeState(outOfRangeState);
					return;
				}
				if (num > abortAttackRange && doingAttackAnimation && !hasEndAttackAnimationParameter)
				{
					AbortAttack();
					stateMachine.ChangeState(outOfRangeState);
					return;
				}
			}
			if (noVisionState != null && !Helper.HasVisionToPlayer(targetPlayer, transform.position, useFeetPosition: false, ignoreObstructionDuringAttack))
			{
				stateMachine.ChangeState(noVisionState);
				return;
			}
			if (!flag && !doingAnimation && !doingAttackAnimation && canAttack)
			{
				if (hasAttacked && attackDoneAlternateState != null)
				{
					stateMachine.ChangeState(attackDoneAlternateState);
					return;
				}
				if (WithinAttackAngle(targetPlayer))
				{
					StartAttack(targetPlayer);
				}
			}
			if (!rotateTowardsTarget || flag)
			{
				return;
			}
			float targetRotationSpeed = GetTargetRotationSpeed();
			float num2 = 0f;
			if (lerpRotateSpeed)
			{
				currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, targetRotationSpeed, Time.deltaTime * rotationLerpSpeed);
				num2 = currentRotationSpeed;
			}
			else
			{
				num2 = targetRotationSpeed;
			}
			if (targetRotationSpeed != 0f)
			{
				_ = stateMachine.movement.model;
				Vector3 position = stateMachine.transform.position;
				Vector3 feetPosition = targetPlayer.FeetPosition;
				feetPosition.y = position.y;
				Vector3 vector = feetPosition - position;
				switch (chaseAttackMovementType)
				{
				case MovementType.Translate:
				case MovementType.NavmeshAgent:
				case MovementType.CharacterController:
				case MovementType.None:
				case MovementType.GoldenRule:
				case MovementType.Translate_Simple:
				case MovementType.Rigidbody:
					stateMachine.movement.LookDirection(stateMachine.movement.model, vector, num2, ignoreZRotation);
					break;
				case MovementType.Rigidbody_LookObstruction:
					stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(vector, Vector3.up), Time.deltaTime * num2);
					break;
				}
			}
		}
		else
		{
			if (!chaseWhileAttacking)
			{
				return;
			}
			Network_Player network_Player2 = Helper.ClosestPlayerToPoint(stateMachine.transform.position, includeDeadPlayers: false);
			if (!(network_Player2 != null))
			{
				return;
			}
			if (Vector3.Distance(transform.position, network_Player2.FeetPosition) > startAttackRange)
			{
				if (animatorController != null && animatorController.animator != null)
				{
					animatorController.SetFloat("Speed", Mathf.MoveTowards(animatorController.animator.GetFloat("Speed"), 2f, Time.deltaTime * 6f));
				}
			}
			else if (animatorController != null && animatorController.animator != null)
			{
				animatorController.SetFloat("Speed", Mathf.MoveTowards(animatorController.animator.GetFloat("Speed"), 0f, Time.deltaTime * 6f));
			}
		}
	}

	protected virtual void OnAnimationEvent(string animationEventName)
	{
		if (!active)
		{
			return;
		}
		if (Raft_Network.IsHost)
		{
			if (animationEventName == attackAnimationParameter)
			{
				Attack(targetPlayer);
			}
			else if (animationEventName == endAttackAnimationParameter)
			{
				doingAnimation = false;
				currentRotationSpeed = 0f;
			}
		}
		else if (animationEventName == attackAnimationParameter)
		{
			Attack(targetPlayer);
		}
	}

	protected virtual void StartAttack(Network_Player player)
	{
		if (player == null)
		{
			return;
		}
		if (animatorController != null && animatorController.animator != null)
		{
			animatorController.animator.ResetTrigger("Abort");
		}
		if (!Raft_Network.IsHost)
		{
			return;
		}
		canAttack = false;
		Invoke("ResetAttackCooldown", GetNewAttackCooldown());
		if (!hasAttackAnimationParameter)
		{
			if (animatorController != null)
			{
				animatorController.SetTrigger(attackAnimationParameter);
			}
			OnAnimationEvent(attackAnimationParameter);
			return;
		}
		doingAttackAnimation = true;
		doingAnimation = true;
		currentRotationSpeed = 0f;
		if (animatorController != null)
		{
			animatorController.SetTrigger(attackAnimationParameter);
		}
	}

	protected virtual void Attack(Network_Player player)
	{
		if (attackParticles != null)
		{
			attackParticles.Play();
		}
		RuntimeManager.PlayOneShot(er_attack, transform.position);
		if (player == null || !Raft_Network.IsHost)
		{
			return;
		}
		bool flag = false;
		if (Vector3.Distance(stateMachine.transform.position, player.FeetPosition) <= effectiveAttackRange && WithinAttackAngle(player))
		{
			if (Time.time - successfullAttackTimeStamp > successfullAttackSoundCooldown && !er_successfullAttack.IsNull)
			{
				soundManager.PlayOneShotNetworked(er_successfullAttack, stateMachine.transform.position);
				successfullAttackTimeStamp = Time.time;
			}
			networkHost.DamageEntity(player.Stats, null, AttackDamage, player.transform.position, Vector3.up, EntityType.Enemy);
			flag = true;
			if (player.Stats.IsDead)
			{
				AI_NetworkBehaviour componentInParent = GetComponentInParent<AI_NetworkBehaviour>();
				if (componentInParent != null)
				{
					AnalyticsHelper.CustomEventString(AnalyticsEvent.ANIMAL_KILLPLAYER, new Dictionary<string, object> { { "AnimalType", componentInParent.behaviourType } });
				}
			}
		}
		hasAttacked = true;
		doingAttackAnimation = false;
		if (!hasEndAttackAnimationParameter)
		{
			doingAnimation = false;
		}
		if (flag)
		{
			SuccessFullAttackCall(player);
		}
		else
		{
			MissedAttackCall();
		}
	}

	protected virtual void MissedAttackCall()
	{
		if (attackMissedState != null)
		{
			stateMachine.ChangeState(attackMissedState);
		}
	}

	protected virtual void SuccessFullAttackCall(Network_Player player)
	{
	}

	private void ResetAttackCooldown()
	{
		canAttack = true;
	}

	protected float GetTargetRotationSpeed()
	{
		float result = (doingAttackAnimation ? rotationSpeedWhenAttacking : rotationSpeed);
		if (!rotateWhileAnimating && doingAnimation && !doingAttackAnimation)
		{
			result = 0f;
		}
		return result;
	}

	private void SwitchTargetPlayer(Network_Player player)
	{
		if (player == null)
		{
			if (doingAttackAnimation)
			{
				AbortAttack();
			}
			stateMachine.ChangeState(noTargetState);
		}
		else if (targetPlayer != player && !doingAttackAnimation)
		{
			targetPlayer = player;
		}
	}

	private bool WithinAttackAngle(Network_Player player)
	{
		if (attackAngle == 0f)
		{
			return true;
		}
		return Helper.GetAngleBetweenObjects(stateMachine.movement.model, player.transform, resetYPosition: true) < attackAngle;
	}

	private float GetNewAttackCooldown()
	{
		return attackCooldown + UnityEngine.Random.Range(minAttackCooldownOffset, maxAttackCooldownOffset);
	}

	private void HandleChaseAttackMovement(Vector3 targetPoint, MovementType chaseAttackMovement)
	{
		switch (chaseAttackMovement)
		{
		case MovementType.Translate:
			stateMachine.movement.MoveLookObstruction_Translate(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
			break;
		case MovementType.NavmeshAgent:
			stateMachine.movement.MoveLook_Navmesh(targetPlayer.FeetPosition, stateMachine.movement.MovementSpeed);
			break;
		case MovementType.Rigidbody_LookObstruction:
			stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed, stateMachine.rigid);
			break;
		case MovementType.GoldenRule:
			stateMachine.movement.GoldenRuleMovement(targetPoint, stateMachine.rigid, stateMachine.movement.MovementSpeed);
			break;
		case MovementType.Translate_Simple:
			stateMachine.movement.MoveLook_Translate(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
			break;
		case MovementType.CharacterController:
		case MovementType.None:
		case MovementType.Rigidbody:
			break;
		}
	}

	protected virtual void AbortAttack()
	{
		if (animatorController != null)
		{
			animatorController.SetTrigger("Abort");
		}
		doingAttackAnimation = false;
		doingAnimation = false;
	}
}
