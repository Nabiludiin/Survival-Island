using System;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.AI;

public class AI_State_Boar_ChargeTowards : AI_State
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
	private bool exitStateOnDamageBoxHit;

	[Space(10f)]
	[SerializeField]
	private float navMeshCheckDistance = 0.5f;

	[SerializeField]
	private float minimumInterupptChargePlayerToTargetDistance = 2f;

	private Vector3 chargeTargetPoint = Vector3.zero;

	private NavMeshAgent agent;

	private AI_StateMachine_Boar boarSM;

	private AI_NetworkBehaviour_Boar boarNet;

	private AI_Movement movement;

	private float chargeChannel;

	private Network_Host network_Host;

	protected override void Awake()
	{
		base.Awake();
		boarSM = stateMachine as AI_StateMachine_Boar;
		boarNet = boarSM.networkBehaviour as AI_NetworkBehaviour_Boar;
		movement = boarSM.movement;
		agent = movement.agent;
	}

	protected override void Start()
	{
		base.Start();
		network_Host = ComponentManager<Network_Host>.Value;
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
		if (isFearlessInState)
		{
			boarSM.SetIsCurrentlyFearless(value: true, 1);
		}
		movement.SetMovementType(chargeMovementType);
		if (Raft_Network.IsHost)
		{
			movement.SetMovementSpeed(stateChargeSpeed);
			movement.SetRotationSpeed(stateChargeModelTurnRate);
			agent.angularSpeed = stateNavAgentAngularSpeed;
			agent.acceleration = stateNavAgentAcceleration;
			if (boarSM.enemyTarget == null)
			{
				boarSM.ChangeState(chargeInterruptedState);
				return;
			}
			Vector3.Distance(boarSM.transform.position, boarSM.enemyTarget.transform.position);
			float extraLength = UnityEngine.Random.Range(minChargeDistance, maxChargeDistance);
			chargeTargetPoint = Helper.GetClosestPointOnNavMeshInDirectionTo(boarSM.transform.position, boarSM.enemyTarget.FeetPosition, extraLength, navMeshCheckDistance, 2f, NavCheckType.AFTER);
			if (!movement.IsValidNavmeshPath(chargeTargetPoint))
			{
				chargeTargetPoint = Helper.GetClosestPointOnNavMeshInDirectionTo(boarSM.transform.position, boarSM.enemyTarget.FeetPosition, extraLength, navMeshCheckDistance, 2f, NavCheckType.BEFORE);
			}
		}
		RuntimeManager.PlayOneShotAttached(boarSM.er_Charge, stateMachine.gameObject);
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
		if (boarSM != null && isFearlessInState)
		{
			boarSM.SetIsCurrentlyFearless(value: false);
		}
		UnHookPlayer();
	}

	public override void UpdateState()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (boarSM.enemyTarget == null)
		{
			boarSM.ChangeState(boarSM.defaultState);
			return;
		}
		base.UpdateState();
		if (isAutoUpdatingChargeTargetting && boarSM.enemyTarget != null)
		{
			Vector3 closestPointOnNavMesh = Helper.GetClosestPointOnNavMesh(boarSM.enemyTarget.FeetPosition, autoUpdateChargeTargettingDistance);
			if (Vector3.Distance(boarSM.transform.position, closestPointOnNavMesh) <= autoUpdateChargeTargettingDistance)
			{
				chargeTargetPoint = closestPointOnNavMesh;
			}
		}
		switch (chargeMovementType)
		{
		case MovementType.NavmeshAgent:
			movement.MoveLook_Navmesh(chargeTargetPoint, movement.MovementSpeed);
			break;
		case MovementType.Translate:
			movement.MoveLookObstruction_Translate(boarSM.transform, chargeTargetPoint, movement.MovementSpeed, movement.RotationSpeed);
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
		if (Vector3.Distance(boarSM.transform.position, chargeTargetPoint) <= distanceToChargePointMinimum)
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
		if (keepChargingWhileCloseToTarget && !boarSM.enemyTarget.PlayerScript.IsDead && Vector3.Distance(boarSM.transform.position, boarSM.enemyTarget.FeetPosition) < distanceToKeepCharging && movement.IsValidNavmeshPath(boarSM.enemyTarget, snapToClosestNavmesh: true))
		{
			return true;
		}
		return false;
	}

	private bool IsEnemyTargetCloseToChargePoint()
	{
		if (Vector3.Distance(boarSM.enemyTarget.transform.position, chargeTargetPoint) < minimumInterupptChargePlayerToTargetDistance)
		{
			return true;
		}
		return false;
	}

	private bool IsBoarWithinSlideRange()
	{
		if (Vector3.Distance(boarSM.transform.position, chargeTargetPoint) <= distanceToPointStartSliding)
		{
			return true;
		}
		return false;
	}

	private void HitPlayerLocally(Network_Player player)
	{
		RuntimeManager.PlayOneShot(boarSM.er_ChargeHit, stateMachine.transform.position);
		if (boarSM.attachPlayer != null && player.IsLocalPlayer)
		{
			Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.Boar_Impale_Player, boarSM.networkBehaviour.Network.NetworkIDManager, boarSM.networkBehaviour.ObjectIndex, player.steamID);
			if (Raft_Network.IsHost)
			{
				boarSM.attachPlayer.StartCarryingPlayer(player);
				boarSM.networkBehaviour.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				boarSM.networkBehaviour.Network.SendP2P(boarSM.networkBehaviour.Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		if (Raft_Network.IsHost && exitStateOnDamageBoxHit)
		{
			stateMachine.ChangeState(chargeDoneState);
		}
	}

	private void UnHookPlayer()
	{
		if (Raft_Network.IsHost && boarSM.attachPlayer != null)
		{
			Network_Player carriedPlayer = boarSM.attachPlayer.carriedPlayer;
			if (carriedPlayer != null)
			{
				Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.Boar_Depale_Player, boarSM.networkBehaviour.Network.NetworkIDManager, boarSM.networkBehaviour.ObjectIndex, carriedPlayer.steamID);
				boarSM.attachPlayer.StopCarryingPlayer(manipulatePosition: true);
				boarSM.networkBehaviour.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}
}
