using System;
using FMODUnity;
using UnityEngine;

public class AI_StateMachine_Boss_Varuna : AI_StateMachine_Animal
{
	public Network_Player enemyTarget;

	public int outerWaypointMovementMultiplier = 3;

	public float outerWaypointRotationMultiplier = 2.5f;

	public float maxChargeCooldown = 10f;

	public AI_State_VarunaBoss_Waypoint_Circulation waypointCirculation;

	public AI_Component_SeePlayerSwitchState seePlayer_component;

	[SerializeField]
	private SO_CameraShaker cameraShake_Data;

	public AreaZone bossArea_Level_1;

	public AreaZone bossArea_Level_2;

	public AreaZone bossArea_Level_3;

	public AreaZone playerTargeting_areaZone;

	public Collider bodyCollider;

	public AI_State waitForQuestState;

	private ScreenEffectManager screenEffectManager;

	[HideInInspector]
	public VarunaBossHelperScript helperScript;

	public AreaZone PlayerTargetingZone => playerTargeting_areaZone;

	public override void Awake()
	{
		base.Awake();
		AI_State_VarunaBoss_Waypoint_Circulation aI_State_VarunaBoss_Waypoint_Circulation = waypointCirculation;
		aI_State_VarunaBoss_Waypoint_Circulation.OnNewWaypointTarget = (Action<Waypoint>)Delegate.Combine(aI_State_VarunaBoss_Waypoint_Circulation.OnNewWaypointTarget, new Action<Waypoint>(NewWaypointTarget));
		AI_State_VarunaBoss_Waypoint_Circulation aI_State_VarunaBoss_Waypoint_Circulation2 = waypointCirculation;
		aI_State_VarunaBoss_Waypoint_Circulation2.OnWaypointTraversed = (Action<Waypoint, Waypoint>)Delegate.Combine(aI_State_VarunaBoss_Waypoint_Circulation2.OnWaypointTraversed, new Action<Waypoint, Waypoint>(OnTraverseWaypoint));
	}

	public override void Start()
	{
		base.Start();
		if (!Raft_Network.IsHost)
		{
			GetComponent<AI_Movement_Lean>().enabled = false;
		}
		bossArea_Level_1 = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.Varuna_Boss_Floor_1).GetComponentInChildren<AreaZone>(includeInactive: true);
		bossArea_Level_2 = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.Varuna_Boss_Floor_2).GetComponentInChildren<AreaZone>(includeInactive: true);
		bossArea_Level_3 = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.Varuna_Boss_Floor_3).GetComponentInChildren<AreaZone>(includeInactive: true);
		helperScript = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.Varuna_Boss_HelperScript).GetComponent<VarunaBossHelperScript>();
		FetchPlayerTargettingZone();
		screenEffectManager = ComponentManager<ScreenEffectManager>.Value;
	}

	public override void OnDestroy()
	{
		AI_State_VarunaBoss_Waypoint_Circulation aI_State_VarunaBoss_Waypoint_Circulation = waypointCirculation;
		aI_State_VarunaBoss_Waypoint_Circulation.OnNewWaypointTarget = (Action<Waypoint>)Delegate.Remove(aI_State_VarunaBoss_Waypoint_Circulation.OnNewWaypointTarget, new Action<Waypoint>(NewWaypointTarget));
		base.OnDestroy();
	}

	public void SetBossAsGateWay(bool value, Waypoint target)
	{
		if (value)
		{
			movement.LerpMovementSpeedTowards(waypointCirculation.gatewayMovementSpeed, waypointCirculation.movementLerpTime, movement.MovementSpeed);
			movement.LerpRotationSpeedTowards(waypointCirculation.gatewayRotationSpeed, waypointCirculation.rotationLerpTime, movement.RotationSpeed);
			return;
		}
		animController.SetBool("Charging", value: false);
		if (target != null && target.waypointID == Waypoint.WayPointID.VARUNA_BOSS_OUTER)
		{
			movement.LerpMovementSpeedTowards(waypointCirculation.movementCirculationSpeed * (float)outerWaypointMovementMultiplier, waypointCirculation.movementLerpTime, movement.MovementSpeed);
			movement.LerpRotationSpeedTowards(waypointCirculation.movementRotationSpeed * outerWaypointRotationMultiplier, waypointCirculation.rotationLerpTime, movement.RotationSpeed);
		}
		else
		{
			movement.LerpMovementSpeedTowards(waypointCirculation.movementCirculationSpeed, waypointCirculation.movementLerpTime, movement.MovementSpeed);
			movement.LerpRotationSpeedTowards(waypointCirculation.movementRotationSpeed, waypointCirculation.rotationLerpTime, movement.RotationSpeed);
		}
	}

	public void NewWaypointTarget(Waypoint waypoint)
	{
		if (waypoint.waypointID == Waypoint.WayPointID.VARUNA_BOSS_INNER)
		{
			SetBossAsInnerCirculation();
		}
		else if (waypoint.waypointID == Waypoint.WayPointID.VARUNA_BOSS_OUTER)
		{
			SetBossAsOuterCirculation();
		}
	}

	public void OnTraverseWaypoint(Waypoint from, Waypoint to)
	{
		SetBossAsGateWay(value: false, to);
	}

	public void SetBossAsInnerCirculation()
	{
		seePlayer_component.SetEnabledSafe(value: true);
	}

	public void SetBossAsOuterCirculation()
	{
		seePlayer_component.SetEnabledSafe(value: false);
	}

	public MovementDeltaTracker GetEnemyTargetDeltaTracker()
	{
		if (enemyTarget != null)
		{
			return enemyTarget.GetComponent<MovementDeltaTracker>();
		}
		return null;
	}

	public override RGD_StateMachine GetStateMachineData()
	{
		return new RGD_StateMachine_VarunaBoss(this);
	}

	public bool CanChargePlayer(Network_Player player)
	{
		if (player == null)
		{
			return false;
		}
		switch (waypointCirculation.currentCirculationID)
		{
		case ObjectOfInterestType.Varuna_Boss_Floor_1:
			if (bossArea_Level_1.GetNormalizedDistanceToPoint(player.transform.position) > 0f)
			{
				return true;
			}
			break;
		case ObjectOfInterestType.Varuna_Boss_Floor_2:
			if (bossArea_Level_2.GetNormalizedDistanceToPoint(player.transform.position) > 0f)
			{
				return true;
			}
			break;
		case ObjectOfInterestType.Varuna_Boss_Floor_3:
			if (bossArea_Level_3.GetNormalizedDistanceToPoint(player.transform.position) > 0f)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public void FetchPlayerTargettingZone()
	{
		playerTargeting_areaZone = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.Varuna_Boss_AreaZone_AllowedTarget).GetComponentInChildren<AreaZone>(includeInactive: true);
	}

	public bool PlayerIsInBossTargetingArea(Network_Player player)
	{
		if (PlayerTargetingZone == null)
		{
			FetchPlayerTargettingZone();
		}
		if (PlayerTargetingZone == null)
		{
			return false;
		}
		if (PlayerTargetingZone.GetNormalizedDistanceToPoint(player.transform.position) > 0f)
		{
			return true;
		}
		return false;
	}

	protected override void UpdateStateMachine()
	{
		base.UpdateStateMachine();
		if (!(currentState == decayState) && !(currentState == deadState) && !(currentState == waitForQuestState))
		{
			UpdatePlayerPositioningInBoundsOfRegions();
		}
	}

	protected override void OnDeath(EntityType damageType)
	{
		AchievementHandler.AddEntityKillByType(networkBehaviour.behaviourType);
		RuntimeManager.PlayOneShot(er_die, transform.position);
		if (bodyCollider != null)
		{
			bodyCollider.enabled = false;
		}
		if (Raft_Network.IsHost)
		{
			QuestProgressTracker.FinishQuestRPC(QuestType.VarunaBossKilled);
		}
	}

	private void UpdatePlayerPositioningInBoundsOfRegions()
	{
		if (waypointCirculation.previous != null && waypointCirculation.previous.waypointID == Waypoint.WayPointID.VARUNA_BOSS_OUTER && waypointCirculation.closest != null && waypointCirculation.closest.waypointID == Waypoint.WayPointID.VARUNA_BOSS_INNER)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		foreach (QuestEventBase floorOnePillar in helperScript.floorOnePillars)
		{
			if (floorOnePillar.IsAtMaxStateIndex)
			{
				flag = true;
				break;
			}
		}
		foreach (QuestEventBase floorTwoPillar in helperScript.floorTwoPillars)
		{
			if (floorTwoPillar.IsAtMaxStateIndex)
			{
				flag2 = true;
				break;
			}
		}
		if (!flag && !flag2)
		{
			waypointCirculation.currentCirculationID = ObjectOfInterestType.Varuna_Boss_Floor_1;
		}
		else if (flag && !flag2)
		{
			waypointCirculation.currentCirculationID = ObjectOfInterestType.Varuna_Boss_Floor_2;
		}
		else if (flag & flag2)
		{
			waypointCirculation.currentCirculationID = ObjectOfInterestType.Varuna_Boss_Floor_3;
		}
		if (currentState == startState)
		{
			ObjectOfInterestType interestID = waypointCirculation.waypointHandler.InterestID;
			ObjectOfInterestType currentCirculationID = waypointCirculation.currentCirculationID;
			if ((interestID == ObjectOfInterestType.Varuna_Boss_WaypointHandler_3 && (currentCirculationID == ObjectOfInterestType.Varuna_Boss_Floor_1 || currentCirculationID == ObjectOfInterestType.Varuna_Boss_Floor_2)) || (interestID == ObjectOfInterestType.Varuna_Boss_WaypointHandler_2 && currentCirculationID == ObjectOfInterestType.Varuna_Boss_Floor_1))
			{
				waypointCirculation.TeleportToNewFloor(currentCirculationID);
			}
		}
	}

	public void HitSolidWithChargeFeedback()
	{
		screenEffectManager.ShakeCamera(cameraShake_Data, transform.position, 25f);
	}
}
