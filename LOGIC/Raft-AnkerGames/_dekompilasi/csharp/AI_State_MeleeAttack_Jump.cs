using System.Collections;
using FMODUnity;
using UnityEngine;

public class AI_State_MeleeAttack_Jump : AI_State_MeleeAttack
{
	[Tooltip("Switches to this state if distance to target is less then minJumpDistance")]
	[SerializeField]
	private AI_State toCloseToJumpState;

	[SerializeField]
	private float minJumpDistance = 1f;

	[SerializeField]
	private float jumpHeight = 1f;

	[SerializeField]
	private float jumpTime = 1f;

	[SerializeField]
	private float maxYDiff = 1.5f;

	[SerializeField]
	private float stunDelay = 0.5f;

	[SerializeField]
	private float extraJumpDistance;

	[SerializeField]
	private DamageBox damageBox;

	[SerializeField]
	private ParticleSystem landParticles;

	[SerializeField]
	private SO_CameraShaker cameraShakeData;

	[SerializeField]
	private float shakeRange = 10f;

	[SerializeField]
	private bool isPredictive;

	[SerializeField]
	private float predictedJumpLenght = 5f;

	public EventReference er_landJump;

	private bool chargingJump;

	private ScreenEffectManager screenEffectManager;

	protected override void Start()
	{
		base.Start();
		screenEffectManager = ComponentManager<ScreenEffectManager>.Value;
		damageBox.SetDamageAmount(Mathf.CeilToInt(AttackDamage));
	}

	protected override void OnAnimationEvent(string animationEventName)
	{
		base.OnAnimationEvent(animationEventName);
		if (active && animationEventName == endAttackAnimationParameter)
		{
			if (landParticles != null)
			{
				landParticles.Play();
			}
			if (screenEffectManager != null && cameraShakeData != null)
			{
				screenEffectManager.ShakeCamera(cameraShakeData, transform.position, shakeRange);
			}
			RuntimeManager.PlayOneShotAttached(er_landJump, gameObject);
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		damageBox.SetDamageBoxActive(value: false);
		if (Raft_Network.IsHost && HasToCloseToJumpState())
		{
			Network_Player network_Player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, startAttackRange, includeDeadPlayers: false);
			if (network_Player != null)
			{
				HandleTargetCloseCheck(network_Player);
			}
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		damageBox.SetDamageBoxActive(value: false);
		StopAllCoroutines();
		chargingJump = false;
	}

	protected override void StartAttack(Network_Player player)
	{
		base.StartAttack(player);
		damageBox.SetDamageBoxActive(value: false);
		if (Raft_Network.IsHost)
		{
			chargingJump = true;
			StartCoroutine(JumpAttack(player));
		}
	}

	protected override void Attack(Network_Player player)
	{
		if (Raft_Network.IsHost && !Helper.HasVisionToPlayer(player, transform.position))
		{
			AbortAttack();
			stateMachine.ChangeState(outOfRangeState);
			return;
		}
		chargingJump = false;
		damageBox.SetDamageBoxActive(value: true, checkCollisionImmediatly: true);
		CancelInvoke("DisableDamageBox");
		Invoke("DisableDamageBox", jumpTime);
	}

	protected override void AbortAttack()
	{
		base.AbortAttack();
		StopAllCoroutines();
		chargingJump = false;
		rotateTowardsTarget = true;
	}

	private void DisableDamageBox()
	{
		damageBox.SetDamageBoxActive(value: false);
	}

	private IEnumerator JumpAttack(Network_Player player)
	{
		if (player == null)
		{
			yield break;
		}
		while (chargingJump)
		{
			yield return null;
		}
		Vector3 startPosition = stateMachine.transform.position;
		_ = Vector3.zero;
		Vector3 targetPosition = (isPredictive ? Helper.GetClosestPointOnNavMesh(player.MovementDeltaTracker.GetApproximateTrackedTransformPositionWithDeltaMovement(predictedJumpLenght, normalizeDirection: false), 3f) : Helper.GetClosestPointOnNavMesh(player.FeetPosition, 2f));
		if (extraJumpDistance > 0f)
		{
			targetPosition = Helper.GetClosestPointOnNavMeshInDirectionTo(startPosition, targetPosition, extraJumpDistance, 0.2f, 3f);
		}
		float num = Mathf.Abs(targetPosition.y - player.FeetPosition.y);
		float num2 = targetPosition.y - stateMachine.transform.position.y;
		if (num > 2f || (num2 > 0f && num2 > maxYDiff))
		{
			AbortAttack();
			stateMachine.ChangeState(outOfRangeState);
			yield break;
		}
		float startTime = Time.time;
		float normalizedTime = 0f;
		float num3 = Vector3.Distance(startPosition, targetPosition);
		float currentJumpHeight = num3 / startAttackRange * jumpHeight;
		rotateTowardsTarget = false;
		while (normalizedTime < 1f)
		{
			normalizedTime = Mathf.Clamp01((Time.time - startTime) / jumpTime);
			stateMachine.transform.position = Helper.GetBezierCurveValue(startPosition, targetPosition, currentJumpHeight, normalizedTime);
			Transform model = stateMachine.movement.model;
			model.rotation = Quaternion.Lerp(b: Quaternion.LookRotation(targetPosition.XZOnly() - startPosition.XZOnly(), Vector3.up), a: model.rotation, t: Time.deltaTime * rotationSpeedWhenAttacking);
			yield return null;
		}
		yield return new WaitForSeconds(stunDelay);
		if (!hasEndAttackAnimationParameter)
		{
			doingAnimation = false;
		}
		doingAttackAnimation = false;
		rotateTowardsTarget = true;
		HandleTargetCloseCheck(player);
	}

	private bool HandleTargetCloseCheck(Network_Player player)
	{
		if (player == null)
		{
			return false;
		}
		if (HasToCloseToJumpState() && Vector3.Distance(stateMachine.transform.position, player.FeetPosition) < minJumpDistance)
		{
			stateMachine.ChangeState(toCloseToJumpState);
			return true;
		}
		return false;
	}

	private bool HasToCloseToJumpState()
	{
		return toCloseToJumpState != null;
	}
}
