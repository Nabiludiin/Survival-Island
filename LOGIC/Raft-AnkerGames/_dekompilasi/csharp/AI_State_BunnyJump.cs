using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class AI_State_BunnyJump : AI_State
{
	public enum JumpType
	{
		Random,
		TowardPlayer,
		StraightLine
	}

	private Raft_Network _network;

	[SerializeField]
	private JumpType jumpType;

	[SerializeField]
	private AI_StateMachine_HyenaBoss bossSM;

	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private AnimatorController animatorController;

	[SerializeField]
	private Interval_Float jumpDistance;

	[SerializeField]
	private float predictionDistance = 3.5f;

	[SerializeField]
	private int numberOfJumps = 3;

	[SerializeField]
	private float jumpHeight = 1f;

	[SerializeField]
	private float jumpTime = 1f;

	[SerializeField]
	private float stunDelay = 1f;

	[SerializeField]
	private float rotationSpeed = 1f;

	[SerializeField]
	private float rotateLerpSpeed = 1f;

	[SerializeField]
	private float damageRange = 1f;

	[SerializeField]
	private float damage = 1f;

	[SerializeField]
	private float knockbackForce = 1f;

	[SerializeField]
	private string attackStartAnimationParameter;

	[SerializeField]
	private string attackAnimationParameter;

	[SerializeField]
	private string attackEndAnimationParameter;

	[SerializeField]
	private ParticleSystem landParticles;

	[SerializeField]
	private SO_CameraShaker cameraShakeData;

	[SerializeField]
	private float shakeRange = 25f;

	public EventReference er_jumpStart;

	public EventReference er_jump;

	public EventReference er_jumpEnd;

	public EventReference er_jumpDamagePlayer;

	[SerializeField]
	private int currentJumpCount;

	[SerializeField]
	private bool chargingJump;

	private Network_Host networkHost;

	private AI_NetworkBehaviour messageReciever;

	private Coroutine coroutine;

	private ScreenEffectManager screenEffectManager;

	private Raft_Network Network
	{
		get
		{
			if (_network == null)
			{
				_network = ComponentManager<Raft_Network>.Value;
			}
			return _network;
		}
	}

	protected override void Start()
	{
		base.Start();
		messageReciever = stateMachine.GetComponent<AI_NetworkBehaviour>();
		if (this.animatorController != null)
		{
			AnimatorController animatorController = this.animatorController;
			animatorController.OnAnimationEvent = (Action<string>)Delegate.Combine(animatorController.OnAnimationEvent, new Action<string>(OnAnimationEvent));
		}
		screenEffectManager = ComponentManager<ScreenEffectManager>.Value;
	}

	private void OnAnimationEvent(string animationEventName)
	{
		if (active)
		{
			if (animationEventName == attackAnimationParameter)
			{
				RuntimeManager.PlayOneShot(er_jumpStart, stateMachine.transform.position);
				chargingJump = false;
			}
			else if (animationEventName == attackStartAnimationParameter)
			{
				RuntimeManager.PlayOneShot(er_jumpStart, stateMachine.transform.position);
			}
			else if (animationEventName == attackEndAnimationParameter)
			{
				RuntimeManager.PlayOneShot(er_jumpEnd, stateMachine.transform.position);
			}
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetMovementType(MovementType.None);
		stateMachine.movement.SetMovementSpeed(0f);
		stateMachine.movement.SetRotationSpeed(0f);
		if (Raft_Network.IsHost)
		{
			coroutine = StartCoroutine(JumpAttack());
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}
	}

	private void DamageLocalPlayerInRange(Vector3 centerPoint)
	{
		if (landParticles != null)
		{
			landParticles.Play();
		}
		if (screenEffectManager != null && cameraShakeData != null)
		{
			screenEffectManager.ShakeCamera(cameraShakeData, centerPoint, shakeRange);
		}
		Network_Player value = ComponentManager<Network_Player>.Value;
		if (value != null && Helper.LocalPlayerIsWithinDistance(centerPoint, damageRange))
		{
			if (networkHost == null)
			{
				networkHost = ComponentManager<Network_Host>.Value;
			}
			float num = damage * GameModeValueManager.GetCurrentGameModeValue().utopiaBossVariables.damageMultiplier;
			networkHost.DamageEntity(value.Stats, null, num, value.transform.position, Vector3.up, EntityType.Enemy);
			value.PersonController.AddExternalVelocity((value.transform.position - stateMachine.transform.position).XZOnly().normalized * knockbackForce, 2f);
			RuntimeManager.PlayOneShotAttached(er_jumpDamagePlayer, value.gameObject);
		}
	}

	private IEnumerator JumpAttack()
	{
		Vector3 targetPosition = stateMachine.transform.position;
		if (!GetNewTargetPosition(ref targetPosition))
		{
			stateMachine.ChangeState(nextState);
			yield break;
		}
		currentJumpCount = numberOfJumps;
		while (currentJumpCount > 0)
		{
			float angle = float.MaxValue;
			while (angle > 15f)
			{
				if (!GetNewTargetPosition(ref targetPosition))
				{
					stateMachine.ChangeState(nextState);
					yield break;
				}
				stateMachine.movement.ChangeRotationSpeedTowards(rotationSpeed, rotateLerpSpeed);
				stateMachine.movement.Look(stateMachine.movement.model, targetPosition, stateMachine.movement.RotationSpeed, ignoreZRotation: true);
				Vector3 vector = targetPosition - stateMachine.transform.position;
				angle = Mathf.Abs(Vector3.Angle(vector.XZOnly(), stateMachine.movement.model.forward.XZOnly()));
				yield return null;
			}
			chargingJump = true;
			animatorController.SetTrigger("Attack_BossJump");
			while (chargingJump)
			{
				if (!GetNewTargetPosition(ref targetPosition))
				{
					stateMachine.ChangeState(nextState);
					yield break;
				}
				stateMachine.movement.ChangeRotationSpeedTowards(rotationSpeed, rotateLerpSpeed);
				stateMachine.movement.Look(stateMachine.movement.model, targetPosition, stateMachine.movement.RotationSpeed * 3f, ignoreZRotation: true);
				yield return null;
			}
			Vector3 startPosition = stateMachine.transform.position;
			float startTime = Time.time;
			float normalizedTime = 0f;
			float num = Vector3.Distance(startPosition, targetPosition);
			float currentJumpHeight = num / jumpDistance.maxValue * jumpHeight;
			while (normalizedTime < 1f)
			{
				normalizedTime = Mathf.Clamp01((Time.time - startTime) / jumpTime);
				stateMachine.transform.position = Helper.GetBezierCurveValue(startPosition, targetPosition, currentJumpHeight, normalizedTime);
				yield return null;
			}
			currentJumpCount--;
			Network.RPC(new Message_NetworkBehaviour_ID_Vector3(Messages.HyenaBoss_LandJump, Network.NetworkIDManager, messageReciever.ObjectIndex, targetPosition), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			DamageLocalPlayerInRange(targetPosition);
			if (bossSM != null)
			{
				bossSM.CreateAcidPool(targetPosition, 4f);
			}
			yield return new WaitForSeconds(stunDelay);
			stateMachine.movement.SetRotationSpeed(0f);
		}
		stateMachine.ChangeState(nextState);
	}

	private bool GetNewTargetPosition(ref Vector3 targetPosition)
	{
		if (jumpType == JumpType.TowardPlayer)
		{
			Network_Player network_Player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, 25f, includeDeadPlayers: false);
			if (bossSM.IsValidTarget(network_Player))
			{
				Vector3 approximateTrackedTransformPositionWithDeltaMovement = network_Player.MovementDeltaTracker.GetApproximateTrackedTransformPositionWithDeltaMovement(predictionDistance, normalizeDirection: false);
				approximateTrackedTransformPositionWithDeltaMovement.y = stateMachine.transform.position.y;
				Vector3 vector = Vector3.ClampMagnitude(approximateTrackedTransformPositionWithDeltaMovement - stateMachine.transform.position, jumpDistance.maxValue);
				Vector3 to = stateMachine.transform.position + vector;
				targetPosition = Helper.GetClosestPointOnNavMeshInDirectionTo(stateMachine.transform.position, to, 0f, 1f, 2f);
				return true;
			}
			return false;
		}
		if (jumpType == JumpType.Random)
		{
			Vector3 to2 = stateMachine.transform.position + UnityEngine.Random.insideUnitSphere.XZOnly().normalized * jumpDistance.GetRandomValue();
			targetPosition = Helper.GetClosestPointOnNavMeshInDirectionTo(stateMachine.transform.position, to2, 0f, 1f, 2f);
			return true;
		}
		if (jumpType == JumpType.StraightLine)
		{
			Vector3 to3 = stateMachine.transform.position + stateMachine.movement.model.forward.XZOnly().normalized * jumpDistance.GetRandomValue();
			targetPosition = Helper.GetClosestPointOnNavMeshInDirectionTo(stateMachine.transform.position, to3, 0f, 1f, 2f);
			return true;
		}
		return false;
	}

	public bool FakeDeserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.HyenaBoss_LandJump)
		{
			Message_NetworkBehaviour_ID_Vector3 message_NetworkBehaviour_ID_Vector = msg as Message_NetworkBehaviour_ID_Vector3;
			DamageLocalPlayerInRange(message_NetworkBehaviour_ID_Vector.Vector);
			if (bossSM != null)
			{
				bossSM.CreateAcidPool(message_NetworkBehaviour_ID_Vector.Vector, 4f);
			}
			return false;
		}
		return false;
	}
}
