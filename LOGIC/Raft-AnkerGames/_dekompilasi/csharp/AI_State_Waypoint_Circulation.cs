using System;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Waypoint_Circulation : AI_State
{
	public WaypointHandler waypointHandler;

	public Waypoint closest;

	public Waypoint previous;

	public Unstuck_Tracker unstuckTracker;

	public MovementType unstuckMovementType = MovementType.Translate_Simple;

	public Action<Waypoint> OnWayPointReached;

	public Action<Waypoint> OnNewWaypointTarget;

	public Action<Waypoint, Waypoint> OnWaypointTraversed;

	public Action<Waypoint, Waypoint> OnGatewayUsed;

	public float distanceToReach = 3f;

	public float movementLerpTime = 1.5f;

	public float rotationLerpTime = 0.8f;

	public float movementCirculationSpeed = 10f;

	public float movementRotationSpeed = 10f;

	public bool reverseWaypointMovementDirection;

	public ObjectOfInterestType currentCirculationID;

	public bool randomizeStartCirculationID;

	public bool randomizeCirculationIDEveryTime;

	public List<ObjectOfInterestType> cirulationIDInterestTypes = new List<ObjectOfInterestType>();

	public TimerEventer timer_cirulationSwitcher;

	[SerializeField]
	private MovementType movementType = MovementType.Rigidbody_LookObstruction;

	private bool isChasingInitialWaypoint;

	private int currentCirculationIndex;

	private Vector3 targetPoint;

	public bool IsConsideredStuck
	{
		get
		{
			if (unstuckTracker != null)
			{
				return unstuckTracker.IsConsideredStuck;
			}
			return false;
		}
	}

	public bool HasReachedWayPoint
	{
		get
		{
			if (closest != null)
			{
				return Vector3.Distance(closest.transform.position, stateMachine.movement.model.position) <= distanceToReach;
			}
			return false;
		}
	}

	protected override void Start()
	{
		base.Start();
		if (Raft_Network.IsHost && cirulationIDInterestTypes.Count > 0)
		{
			if (randomizeStartCirculationID)
			{
				UpdateCirculationID(cirulationIDInterestTypes[UnityEngine.Random.Range(0, cirulationIDInterestTypes.Count)]);
			}
			TimerEventer timerEventer = timer_cirulationSwitcher;
			timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(OnSwitchCirulationID));
		}
	}

	protected override void Destroy()
	{
		base.Destroy();
		TimerEventer timerEventer = timer_cirulationSwitcher;
		timerEventer.OnTimerReached = (Action)Delegate.Remove(timerEventer.OnTimerReached, new Action(OnSwitchCirulationID));
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, targetPoint);
		Gizmos.DrawCube(targetPoint, Vector3.one);
	}

	public void CheckForClosesWaypoint()
	{
		SetNewClosestWaypoint(waypointHandler.GetClosestWaypoint(stateMachine.transform.position));
	}

	public void CheckForClosesWaypointInVision()
	{
		SetNewClosestWaypoint(waypointHandler.GetClosestWaypoint(stateMachine.transform.position, requiresVision: true));
	}

	public override void EnterState()
	{
		if (Raft_Network.IsHost && unstuckTracker != null)
		{
			Unstuck_Tracker unstuck_Tracker = unstuckTracker;
			unstuck_Tracker.OnStuckValueChanged = (Action<bool, Unstuck_Tracker>)Delegate.Combine(unstuck_Tracker.OnStuckValueChanged, new Action<bool, Unstuck_Tracker>(OnIsStuck));
		}
		ResetState();
		stateMachine.movement.SetMovementType(movementType);
		reverseWaypointMovementDirection = Helper.IntToBool(UnityEngine.Random.Range(0, 2));
		if (movementLerpTime <= 0f)
		{
			stateMachine.movement.SetMovementSpeed(movementCirculationSpeed);
		}
		else
		{
			stateMachine.movement.LerpMovementSpeedTowards(movementCirculationSpeed, movementLerpTime, 0f);
		}
		if (rotationLerpTime <= 0f)
		{
			stateMachine.movement.SetRotationSpeed(movementRotationSpeed);
		}
		else
		{
			stateMachine.movement.LerpRotationSpeedTowards(movementRotationSpeed, rotationLerpTime, 0f);
		}
		if (waypointHandler == null)
		{
			UpdateWaypointHandler();
			if (waypointHandler == null)
			{
				AI_NetworkBehaviour component = stateMachine.GetComponent<AI_NetworkBehaviour>();
				if (component != null && component.connectedSpawner != null && component.connectedSpawner.waypointHandler != null)
				{
					waypointHandler = component.connectedSpawner.waypointHandler;
				}
			}
		}
		if (waypointHandler == null)
		{
			Debug.Log("Unable to find proper waypoint manager");
		}
		else
		{
			OnStateEnterWaypointAssign();
		}
		base.EnterState();
	}

	public override void ExitState()
	{
		if (Raft_Network.IsHost && unstuckTracker != null)
		{
			Unstuck_Tracker unstuck_Tracker = unstuckTracker;
			unstuck_Tracker.OnStuckValueChanged = (Action<bool, Unstuck_Tracker>)Delegate.Remove(unstuck_Tracker.OnStuckValueChanged, new Action<bool, Unstuck_Tracker>(OnIsStuck));
			OnIsStuck(value: false, unstuckTracker);
		}
		stateMachine.movement.SetMovementType(MovementType.None);
		base.ExitState();
	}

	public override void UpdateState()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		base.UpdateState();
		if (cirulationIDInterestTypes.Count > 0)
		{
			timer_cirulationSwitcher.DoUpdate(Time.deltaTime, autoResetTimer: true);
		}
		if (waypointHandler == null)
		{
			waypointHandler = ObjectManager.GetObjectOfInterest(currentCirculationID).GetComponentInChildren<WaypointHandler>(includeInactive: true);
		}
		if (closest == null)
		{
			SetNewClosestWaypoint(waypointHandler.GetClosestWaypoint(stateMachine.transform.position));
		}
		if (closest != null)
		{
			if (IsConsideredStuck)
			{
				HandleStuckMovement(closest.transform.position, unstuckMovementType);
			}
			else
			{
				HandleMovement(closest.transform.position, movementType);
			}
			if (HasReachedWayPoint)
			{
				HandleOnReachWaypoint(closest);
				OnWayPointReached?.Invoke(closest);
			}
		}
	}

	public virtual void OnStateEnterWaypointAssign()
	{
		SetNewClosestWaypoint(waypointHandler.GetClosestWaypoint(stateMachine.transform.position));
		isChasingInitialWaypoint = closest != null;
	}

	public virtual void HandleOnReachWaypoint(Waypoint waypoint)
	{
		isChasingInitialWaypoint = false;
		Waypoint waypoint2 = (reverseWaypointMovementDirection ? waypoint.previous : waypoint.next);
		if (waypoint2 == null)
		{
			reverseWaypointMovementDirection = !reverseWaypointMovementDirection;
			waypoint2 = (reverseWaypointMovementDirection ? waypoint.previous : waypoint.next);
		}
		SetNewClosestWaypoint(waypoint2);
	}

	public void UpdateCirculationID(ObjectOfInterestType ID)
	{
		currentCirculationID = ID;
		waypointHandler = ObjectManager.GetObjectOfInterest(currentCirculationID).GetComponentInChildren<WaypointHandler>(includeInactive: true);
		SetNewClosestWaypoint(waypointHandler.GetClosestWaypoint(stateMachine.movement.model.transform.position));
	}

	public void SetNewClosestWaypoint(Waypoint waypoint)
	{
		if (waypoint != closest)
		{
			OnNewWaypointTarget?.Invoke(waypoint);
			previous = closest;
			if (closest != null && closest.gateway != null && closest.gateway == waypoint)
			{
				OnLocalGateWayUse(closest, waypoint);
			}
			else
			{
				OnLocalWaypointUsed(closest, waypoint);
			}
			closest = waypoint;
		}
	}

	public void UpdateWaypointHandler()
	{
		Transform objectOfInterest = ObjectManager.GetObjectOfInterest(currentCirculationID);
		if (objectOfInterest != null)
		{
			waypointHandler = objectOfInterest.GetComponentInChildren<WaypointHandler>(includeInactive: true);
		}
	}

	protected virtual void OnLocalWaypointUsed(Waypoint from, Waypoint to)
	{
		OnWaypointTraversed?.Invoke(from, to);
	}

	protected virtual void OnLocalGateWayUse(Waypoint from, Waypoint to)
	{
		OnGatewayUsed?.Invoke(from, to);
	}

	private void HandleMovement(Vector3 targetPoint, MovementType movementType)
	{
		this.targetPoint = targetPoint;
		switch (movementType)
		{
		case MovementType.Rigidbody:
			if (isChasingInitialWaypoint)
			{
				stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed, stateMachine.rigid);
			}
			else
			{
				stateMachine.movement.MoveLook_Rigid(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed, stateMachine.rigid);
			}
			break;
		case MovementType.Rigidbody_LookObstruction:
			stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed, stateMachine.rigid);
			break;
		case MovementType.GoldenRule:
			stateMachine.movement.GoldenRuleMovement(targetPoint, stateMachine.rigid, stateMachine.movement.MovementSpeed);
			break;
		case MovementType.NavmeshAgent:
			if (stateMachine.movement.agent != null && !stateMachine.movement.agent.enabled)
			{
				stateMachine.movement.agent.enabled = true;
			}
			stateMachine.movement.MoveLook_Navmesh(targetPoint, stateMachine.movement.MovementSpeed);
			break;
		case MovementType.CharacterController:
		case MovementType.None:
		case MovementType.Translate_Simple:
			break;
		}
	}

	private void HandleStuckMovement(Vector3 targetPoint, MovementType unstuckMovementType)
	{
		switch (unstuckMovementType)
		{
		case MovementType.Translate:
			stateMachine.movement.MoveLookObstruction_Translate(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
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
		case MovementType.NavmeshAgent:
		case MovementType.CharacterController:
		case MovementType.None:
		case MovementType.Rigidbody:
			break;
		}
	}

	private void OnSwitchCirulationID()
	{
		if (randomizeCirculationIDEveryTime)
		{
			ObjectOfInterestType objectOfInterestType = currentCirculationID;
			do
			{
				currentCirculationID = cirulationIDInterestTypes[UnityEngine.Random.Range(0, cirulationIDInterestTypes.Count)];
			}
			while (currentCirculationID == objectOfInterestType && cirulationIDInterestTypes.Count > 1);
		}
		else
		{
			currentCirculationIndex++;
			if (currentCirculationIndex > cirulationIDInterestTypes.Count - 1)
			{
				currentCirculationIndex = 0;
			}
		}
		UpdateCirculationID(currentCirculationID);
	}

	private void OnIsStuck(bool value, Unstuck_Tracker unstuckTracker)
	{
		if (stateMachine != null && stateMachine.rigid != null)
		{
			stateMachine.rigid.isKinematic = value;
		}
	}

	private void ResetState()
	{
		previous = null;
		closest = null;
	}

	public override RGD_AI_State SaveState()
	{
		return base.SaveState();
	}
}
