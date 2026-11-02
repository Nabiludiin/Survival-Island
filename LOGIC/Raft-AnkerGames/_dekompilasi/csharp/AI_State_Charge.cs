using System;
using FMODUnity;
using UnityEngine;

public class AI_State_Charge : AI_State
{
	[SerializeField]
	private float minChargeDistance = 20f;

	[SerializeField]
	private float maxChargeDistance = 35f;

	[SerializeField]
	private float maxChargeDuration = 6f;

	[Space(10f)]
	[SerializeField]
	private AI_State chargeInterruptedState;

	[SerializeField]
	private AI_State chargeStunState;

	[SerializeField]
	private float rayForwardOffsetDistance;

	[SerializeField]
	private float rayRadius = 0.4f;

	[Space(10f)]
	[SerializeField]
	private bool isAutoUpdatingChargeTargetting;

	[SerializeField]
	private float autoUpdateChargeTargettingDistance = 10f;

	[SerializeField]
	private bool keepChargingWhileCloseToTarget;

	[SerializeField]
	private float distanceToKeepCharging = 5f;

	[Space(10f)]
	[SerializeField]
	private bool isFearlessInState = true;

	[SerializeField]
	private float stateChargeSpeed = 20f;

	[SerializeField]
	private float stateChargeModelTurnRate = 10f;

	[SerializeField]
	private float chargeLerpTime = 2.5f;

	[SerializeField]
	private bool knocksPlayersBackOnDamageBoxHit = true;

	[SerializeField]
	private float knockbackSpeedInheritanceMultipler = 3f;

	[SerializeField]
	private float distanceToReachPoint = 8f;

	[SerializeField]
	private float distanceToChargePointMinimum = 2f;

	[SerializeField]
	private MovementType chargeMovementType = MovementType.NavmeshAgent;

	[Space(10f)]
	[SerializeField]
	private DamageBox chargeDamageBox;

	[SerializeField]
	private bool exitStateOnDamageBoxHit;

	[Space(10f)]
	[SerializeField]
	private float navMeshCheckDistance = 0.5f;

	[SerializeField]
	private float minimumInterupptChargePlayerToTargetDistance = 2f;

	public EventReference er_charge;

	public EventReference er_hitPlayer;

	protected RaycastHit latestHitRay;

	private Vector3 chargeTargetPoint = Vector3.zero;

	private AI_Movement movement;

	private float chargeChannel;

	private Network_Host network_Host;

	private Network_Player enemyTarget;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		network_Host = ComponentManager<Network_Host>.Value;
		movement = stateMachine.movement;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(chargeTargetPoint, 2f);
	}

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
		Network_Player player = (Network_Player)(object)parameters[0];
		HandleEnemyTargetting(player);
	}

	protected virtual void HitPlayerLocally(Network_Player player)
	{
		if (player != null)
		{
			if (player.IsLocalPlayer)
			{
				player.PersonController.SetExternalVelocity(movement.model.forward * knockbackSpeedInheritanceMultipler);
			}
			RuntimeManager.PlayOneShotAttached(er_hitPlayer, player.gameObject);
			if (Raft_Network.IsHost && active && exitStateOnDamageBoxHit)
			{
				stateMachine.ChangeState(chargeStunState);
			}
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		DamageBox damageBox = chargeDamageBox;
		damageBox.OnDamage = (Action<Network_Player>)Delegate.Combine(damageBox.OnDamage, new Action<Network_Player>(HitPlayerLocally));
		if (chargeDamageBox != null)
		{
			chargeDamageBox.SetDamageBoxActive(value: true);
		}
		stateMachine.movement.SetMovementType(chargeMovementType);
		RuntimeManager.PlayOneShotAttached(er_charge, stateMachine.gameObject);
		if (Raft_Network.IsHost && movement != null)
		{
			movement.LerpMovementSpeedTowards(stateChargeSpeed, chargeLerpTime, movement.MovementSpeed);
			movement.SetRotationSpeed(stateChargeModelTurnRate);
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		DamageBox damageBox = chargeDamageBox;
		damageBox.OnDamage = (Action<Network_Player>)Delegate.Remove(damageBox.OnDamage, new Action<Network_Player>(HitPlayerLocally));
		chargeChannel = 0f;
		if (movement != null)
		{
			movement.SetMovementType(MovementType.None);
			movement.LerpMovementSpeedTowards(0f, 1f, movement.MovementSpeed);
		}
		if (chargeDamageBox != null)
		{
			chargeDamageBox.SetDamageBoxActive(value: false);
		}
	}

	public override void UpdateState()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (enemyTarget == null)
		{
			stateMachine.ChangeState(chargeInterruptedState);
			return;
		}
		base.UpdateState();
		if (isAutoUpdatingChargeTargetting && enemyTarget != null)
		{
			Vector3 playerTargettingVector = GetPlayerTargettingVector();
			if (Vector3.Distance(stateMachine.transform.position, playerTargettingVector) <= autoUpdateChargeTargettingDistance)
			{
				chargeTargetPoint = playerTargettingVector;
			}
		}
		switch (chargeMovementType)
		{
		case MovementType.NavmeshAgent:
			movement.MoveLook_Navmesh(chargeTargetPoint, movement.MovementSpeed);
			break;
		case MovementType.Rigidbody:
			movement.MoveLook_Rigid(stateMachine.transform, chargeTargetPoint, movement.MovementSpeed, movement.RotationSpeed, stateMachine.rigid);
			break;
		}
		chargeChannel += Time.deltaTime;
		if (IsWithinTargetPointRange())
		{
			if (ChargeCompleted(latestHitRay))
			{
				if (latestHitRay.collider != null)
				{
					stateMachine.ChangeState(chargeStunState);
				}
				else
				{
					stateMachine.ChangeState(chargeInterruptedState);
				}
			}
		}
		else if (!KeepCharging())
		{
			stateMachine.ChangeState(chargeInterruptedState);
		}
	}

	protected virtual void HandleEnemyTargetting(Network_Player player)
	{
		if (player != null)
		{
			enemyTarget = player;
			chargeTargetPoint = GetExtraDistanceInDirectionTo(stateMachine.transform.position, GetPlayerTargettingVector(), minChargeDistance, maxChargeDistance);
			float maxDistance = Vector3.Distance(chargeTargetPoint, stateMachine.movement.model.position);
			Vector3 normalized = (chargeTargetPoint - stateMachine.transform.position).normalized;
			Physics.SphereCast(stateMachine.transform.position + normalized * rayForwardOffsetDistance, rayRadius, normalized, out latestHitRay, maxDistance, LayerMasks.MASK_Obstruction, QueryTriggerInteraction.Collide);
			if (latestHitRay.collider != null)
			{
				chargeTargetPoint = latestHitRay.point;
			}
		}
		else
		{
			Debug.Log("Player should be assigned when entering this state, but was unable to do so :(");
			stateMachine.ChangeState(chargeInterruptedState);
		}
	}

	protected virtual Vector3 GetPlayerTargettingVector()
	{
		return enemyTarget.transform.position;
	}

	protected virtual bool ChargeCompleted(RaycastHit hitTowards)
	{
		return true;
	}

	public Vector3 GetExtraDistanceInDirectionTo(Vector3 from, Vector3 to, float minRange, float maxRange)
	{
		return to + (to - from).normalized * UnityEngine.Random.Range(minRange, maxRange);
	}

	private bool IsTooCloseToChargePoint()
	{
		if (Vector3.Distance(stateMachine.transform.position, chargeTargetPoint) <= distanceToChargePointMinimum)
		{
			return true;
		}
		return false;
	}

	private bool IsChargeTimeLimitPassed()
	{
		if (chargeChannel >= maxChargeDuration)
		{
			return true;
		}
		return false;
	}

	private bool KeepCharging()
	{
		if (keepChargingWhileCloseToTarget)
		{
			if (!enemyTarget.PlayerScript.IsDead && Vector3.Distance(stateMachine.transform.position, enemyTarget.FeetPosition) < distanceToKeepCharging)
			{
				return true;
			}
			return false;
		}
		if (IsChargeTimeLimitPassed() || IsTooCloseToChargePoint())
		{
			return false;
		}
		return true;
	}

	private bool IsEnemyTargetCloseToChargePoint()
	{
		if (Vector3.Distance(enemyTarget.transform.position, chargeTargetPoint) < minimumInterupptChargePlayerToTargetDistance)
		{
			return true;
		}
		return false;
	}

	private bool IsWithinTargetPointRange()
	{
		if (Vector3.Distance(stateMachine.transform.position, chargeTargetPoint) <= distanceToReachPoint)
		{
			return true;
		}
		return false;
	}
}
