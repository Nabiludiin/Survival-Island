using System;
using Steamworks;
using UnityEngine;
using UnityEngine.AI;

public class AI_ChargeTowards : AI_State
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
	private AI_State chargeDoneState;

	[SerializeField]
	private AI_State_Boar_Slide boarSlideState;

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
	private float stateNavAgentAngularSpeed = 120f;

	[SerializeField]
	private float stateNavAgentAcceleration = 50f;

	[SerializeField]
	private float distanceToPointStartSliding = 8f;

	[SerializeField]
	private float distanceToChargePointMinimum = 2f;

	[SerializeField]
	private MovementType chargeMovementType = MovementType.NavmeshAgent;

	[Space(10f)]
	[SerializeField]
	private DamageBox hornDamageBox;

	[SerializeField]
	private AttachPlayer attachPlayer;

	[SerializeField]
	private bool exitStateOnDamageBoxHit;

	[Space(10f)]
	[SerializeField]
	private float navMeshCheckDistance = 0.5f;

	[SerializeField]
	private float minimumInterupptChargePlayerToTargetDistance = 2f;

	private Vector3 chargeTargetPoint = Vector3.zero;

	private NavMeshAgent agent;

	private AI_Movement movement;

	private float chargeChannel;

	private Raft_Network network;

	private Network_Player enemyTarget;

	private AI_StateMachine_Animal stateMachine_Animal;

	protected override void Awake()
	{
		base.Awake();
		movement = stateMachine.movement;
		agent = movement.agent;
	}

	protected override void Start()
	{
		base.Start();
		network = ComponentManager<Raft_Network>.Value;
		stateMachine_Animal = stateMachine as AI_StateMachine_Animal;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(chargeTargetPoint, 2f);
	}

	public override void EnterState()
	{
		base.EnterState();
		DamageBox damageBox = hornDamageBox;
		damageBox.OnDamage = (Action<Network_Player>)Delegate.Combine(damageBox.OnDamage, new Action<Network_Player>(HitPlayerLocally));
		hornDamageBox.SetDamageBoxActive(value: true);
		movement.SetMovementType(chargeMovementType);
		if (!Raft_Network.IsHost)
		{
			return;
		}
		movement.SetMovementSpeed(stateChargeSpeed);
		movement.SetRotationSpeed(stateChargeModelTurnRate);
		agent.angularSpeed = stateNavAgentAngularSpeed;
		agent.acceleration = stateNavAgentAcceleration;
		if (enemyTarget == null)
		{
			enemyTarget = Helper.ClosestPlayerToPoint(stateMachine_Animal.transform.position, 35f, includeDeadPlayers: false);
			if (enemyTarget == null)
			{
				stateMachine.ChangeState(chargeInterruptedState);
				return;
			}
		}
		Vector3.Distance(stateMachine.transform.position, enemyTarget.transform.position);
		float extraLength = UnityEngine.Random.Range(minChargeDistance, maxChargeDistance);
		chargeTargetPoint = Helper.GetClosestPointOnNavMeshInDirectionTo(stateMachine.transform.position, enemyTarget.FeetPosition, extraLength, navMeshCheckDistance, 2f, NavCheckType.AFTER);
		if (!movement.IsValidNavmeshPath(chargeTargetPoint))
		{
			chargeTargetPoint = Helper.GetClosestPointOnNavMeshInDirectionTo(stateMachine.transform.position, enemyTarget.FeetPosition, extraLength, navMeshCheckDistance, 2f, NavCheckType.BEFORE);
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		DamageBox damageBox = hornDamageBox;
		damageBox.OnDamage = (Action<Network_Player>)Delegate.Remove(damageBox.OnDamage, new Action<Network_Player>(HitPlayerLocally));
		chargeChannel = 0f;
		if (movement != null)
		{
			movement.SetMovementType(MovementType.None);
			movement.SetMovementSpeed(movement.defaultMovementSpeed);
		}
		if (hornDamageBox != null)
		{
			hornDamageBox.SetDamageBoxActive(value: false);
		}
		UnHookPlayer();
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
		switch (chargeMovementType)
		{
		case MovementType.NavmeshAgent:
			movement.MoveLook_Navmesh(chargeTargetPoint, movement.MovementSpeed);
			break;
		case MovementType.Translate:
			movement.MoveLookObstruction_Translate(stateMachine.transform, chargeTargetPoint, movement.MovementSpeed, movement.RotationSpeed);
			break;
		}
		chargeChannel += Time.deltaTime;
		if (!IsEnemyTargetCloseToChargePoint() && IsBoarWithinSlideRange())
		{
			boarSlideState.targetSlidePoint = chargeTargetPoint;
			stateMachine.ChangeState(boarSlideState);
		}
		else if ((IsChargeTimeLimitPassed() || IsTooCloseToChargePoint()) && !KeepCharging())
		{
			stateMachine.ChangeState(chargeDoneState);
		}
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
			if (!enemyTarget.PlayerScript.IsDead && Vector3.Distance(stateMachine.transform.position, enemyTarget.FeetPosition) < distanceToKeepCharging && movement.IsValidNavmeshPath(enemyTarget, snapToClosestNavmesh: true))
			{
				return true;
			}
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

	private bool IsBoarWithinSlideRange()
	{
		if (Vector3.Distance(stateMachine.transform.position, chargeTargetPoint) <= distanceToPointStartSliding)
		{
			return true;
		}
		return false;
	}

	private void HitPlayerLocally(Network_Player player)
	{
		if (attachPlayer != null && player.IsLocalPlayer)
		{
			Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.Boar_Impale_Player, network.NetworkIDManager, stateMachine_Animal.networkBehaviour.ObjectIndex, player.steamID);
			if (Raft_Network.IsHost)
			{
				attachPlayer.StartCarryingPlayer(player);
				stateMachine_Animal.networkBehaviour.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				stateMachine_Animal.networkBehaviour.Network.SendP2P(stateMachine_Animal.networkBehaviour.Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		if (Raft_Network.IsHost && exitStateOnDamageBoxHit)
		{
			stateMachine.ChangeState(chargeDoneState);
		}
	}

	private void UnHookPlayer()
	{
		if (Raft_Network.IsHost && attachPlayer != null)
		{
			Network_Player carriedPlayer = attachPlayer.carriedPlayer;
			if (carriedPlayer != null)
			{
				Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.Boar_Depale_Player, stateMachine_Animal.networkBehaviour.Network.NetworkIDManager, stateMachine_Animal.networkBehaviour.ObjectIndex, carriedPlayer.steamID);
				attachPlayer.StopCarryingPlayer(manipulatePosition: true);
				stateMachine_Animal.networkBehaviour.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}
}
