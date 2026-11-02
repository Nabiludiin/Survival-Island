using System.Collections;
using UnityEngine;

public class AI_State_VarunaBoss_Waypoint_Circulation : AI_State_Waypoint_Circulation
{
	public float gatewayMovementSpeed;

	public float gatewayRotationSpeed;

	[SerializeField]
	private int minimumOuterCiculationWaypointCount = 2;

	[SerializeField]
	private int maximumOuterCiculationWaypointCount = 7;

	[SerializeField]
	private float gateWayDestuctionDelay = 0.35f;

	[SerializeField]
	private float seePlayerCooldownOnDestroyWall = 3f;

	public int outerCirculationWaypointsLeftBeforeGoingIn;

	public ObjectOfInterestType confirmedCirculationID = ObjectOfInterestType.Varuna_Boss_Floor_1;

	private AI_StateMachine_Boss_Varuna bossStateMachine;

	protected override void Start()
	{
		base.Start();
		bossStateMachine = stateMachine as AI_StateMachine_Boss_Varuna;
	}

	public override void EnterState()
	{
		base.EnterState();
	}

	public override void UpdateState()
	{
		base.UpdateState();
	}

	public override void OnStateEnterWaypointAssign()
	{
		SetNewClosestWaypoint(waypointHandler.GetClosestWaypoint(stateMachine.transform.position, requiresVision: true));
	}

	public void RandomNewOuterCirculationCount()
	{
		outerCirculationWaypointsLeftBeforeGoingIn = Random.Range(minimumOuterCiculationWaypointCount, maximumOuterCiculationWaypointCount);
	}

	public override void HandleOnReachWaypoint(Waypoint waypoint)
	{
		bool flag = false;
		bool flag2 = bossStateMachine.bossArea_Level_2.GetNormalizedDistanceToPoint(transform.position) > 0f;
		bool flag3 = bossStateMachine.bossArea_Level_3.GetNormalizedDistanceToPoint(transform.position) > 0f;
		bool flag4 = waypoint.waypointID == Waypoint.WayPointID.VARUNA_BOSS_INNER;
		bool flag5 = waypoint.waypointID == Waypoint.WayPointID.VARUNA_BOSS_OUTER;
		if (flag2 && currentCirculationID == ObjectOfInterestType.Varuna_Boss_Floor_2)
		{
			confirmedCirculationID = ObjectOfInterestType.Varuna_Boss_Floor_2;
		}
		else if (flag3 && currentCirculationID == ObjectOfInterestType.Varuna_Boss_Floor_3)
		{
			confirmedCirculationID = ObjectOfInterestType.Varuna_Boss_Floor_3;
		}
		bool num = confirmedCirculationID != currentCirculationID && currentCirculationID == ObjectOfInterestType.Varuna_Boss_Floor_2;
		bool flag6 = confirmedCirculationID != currentCirculationID && currentCirculationID == ObjectOfInterestType.Varuna_Boss_Floor_3;
		if (num | flag6)
		{
			outerCirculationWaypointsLeftBeforeGoingIn = 0;
			if (flag5)
			{
				if (waypoint.IsValidGateway(Waypoint.WayPointID.VARUNA_BOSS_OUTER) && waypoint.gateway.waypointHandler.InterestID > waypointHandler.InterestID)
				{
					SetNewClosestWaypoint(waypoint.gateway);
					waypointHandler = closest.waypointHandler;
					flag = true;
				}
				else if (waypoint.IsValidGateway(Waypoint.WayPointID.VARUNA_BOSS_INNER) && WaypointHandlerToFloor(waypointHandler.InterestID) == currentCirculationID)
				{
					SetNewClosestWaypoint(waypoint.gateway);
					waypointHandler = closest.waypointHandler;
					flag = true;
				}
			}
			else if (waypoint.IsValidGateway(Waypoint.WayPointID.VARUNA_BOSS_OUTER))
			{
				SetNewClosestWaypoint(waypoint.gateway);
				waypointHandler = closest.waypointHandler;
				flag = true;
			}
		}
		else
		{
			if (outerCirculationWaypointsLeftBeforeGoingIn > 0)
			{
				if (flag4)
				{
					if (waypoint.IsValidGateway(Waypoint.WayPointID.VARUNA_BOSS_OUTER))
					{
						SetNewClosestWaypoint(waypoint.gateway);
						waypointHandler = closest.waypointHandler;
						flag = true;
					}
				}
				else
				{
					outerCirculationWaypointsLeftBeforeGoingIn--;
				}
			}
			if (flag5 && outerCirculationWaypointsLeftBeforeGoingIn <= 0 && waypoint.IsValidGateway(Waypoint.WayPointID.VARUNA_BOSS_INNER))
			{
				SetNewClosestWaypoint(waypoint.gateway);
				waypointHandler = closest.waypointHandler;
				flag = true;
			}
		}
		if (!flag)
		{
			if (reverseWaypointMovementDirection)
			{
				SetNewClosestWaypoint(waypoint.previous);
			}
			else
			{
				SetNewClosestWaypoint(waypoint.next);
			}
		}
	}

	public void TeleportToNewFloor(ObjectOfInterestType floor)
	{
		WaypointHandler waypointHandler = bossStateMachine.helperScript.waypointHandlerFloorOne;
		if (floor == ObjectOfInterestType.Varuna_Boss_Floor_2)
		{
			waypointHandler = bossStateMachine.helperScript.waypointHandlerFloorTwo;
		}
		if (floor == ObjectOfInterestType.Varuna_Boss_Floor_3)
		{
			waypointHandler = bossStateMachine.helperScript.waypointHandlerFloorThree;
		}
		SetNewClosestWaypoint(waypointHandler.waypoints[0]);
		base.waypointHandler = waypointHandler;
		bossStateMachine.transform.position = waypointHandler.waypoints[0].transform.position;
		Debug.Log("Teleported boss to floor " + floor);
	}

	private static ObjectOfInterestType WaypointHandlerToFloor(ObjectOfInterestType wayPointID)
	{
		return wayPointID switch
		{
			ObjectOfInterestType.Varuna_Boss_WaypointHandler_1 => ObjectOfInterestType.Varuna_Boss_Floor_1, 
			ObjectOfInterestType.Varuna_Boss_WaypointHandler_2 => ObjectOfInterestType.Varuna_Boss_Floor_2, 
			ObjectOfInterestType.Varuna_Boss_WaypointHandler_3 => ObjectOfInterestType.Varuna_Boss_Floor_3, 
			_ => ObjectOfInterestType.Varuna_Boss_Floor_1, 
		};
	}

	protected override void OnLocalGateWayUse(Waypoint from, Waypoint to)
	{
		base.OnLocalGateWayUse(from, to);
		bossStateMachine.SetBossAsGateWay(value: true, to);
		if (!Raft_Network.IsHost)
		{
			return;
		}
		Vector3 normalized = (to.transform.position - from.transform.position).normalized;
		float radius = 1.2f;
		RaycastHit[] array = Physics.SphereCastAll(from.transform.position, radius, normalized, Vector3.Distance(from.transform.position, to.transform.position), LayerMasks.MASK_Obstruction, QueryTriggerInteraction.Ignore);
		bool value = false;
		if (array.Length != 0)
		{
			RaycastHit[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				RaycastHit raycastHit = array2[i];
				if (raycastHit.collider != null && raycastHit.transform.tag == "Varuna_Boss_Wall")
				{
					value = true;
					CoroutineManager.Singleton.StartCoroutine(DestroyWall(raycastHit.collider.GetComponent<QuestInteractable_VarunaBoss_Wall>(), gateWayDestuctionDelay));
					break;
				}
			}
		}
		bossStateMachine.animController.SetBool("Charging", value);
	}

	private IEnumerator DestroyWall(QuestInteractable_VarunaBoss_Wall wall, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		bossStateMachine.seePlayer_component.SetCooldownTimeStamp(seePlayerCooldownOnDestroyWall);
		wall.BossInteract(bossStateMachine);
	}
}
