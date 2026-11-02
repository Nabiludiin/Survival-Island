using System.Collections.Generic;
using UnityEngine;

public class WaypointHandler : MonoBehaviour
{
	public bool autoHandleRegistrationInAwake = true;

	public ObjectOfInterestType InterestID;

	public List<Waypoint> waypoints = new List<Waypoint>();

	private List<Waypoint> calculationWayPoints;

	public void SetWaypointsHandler()
	{
		foreach (Waypoint waypoint in waypoints)
		{
			waypoint.waypointHandler = this;
		}
	}

	private void Awake()
	{
		if (autoHandleRegistrationInAwake)
		{
			calculationWayPoints = new List<Waypoint>(waypoints);
			if (InterestID != ObjectOfInterestType.None && ObjectManager.GetObjectOfInterest(InterestID) == null)
			{
				ObjectManager.AddObjectOfInterest(InterestID, transform);
			}
		}
	}

	private void OnDestroy()
	{
		if (autoHandleRegistrationInAwake)
		{
			ObjectManager.RemoveObjectOfInterest(InterestID);
		}
	}

	public Waypoint GetClosestWaypoint(Vector3 worldPoint)
	{
		SortWaypointsByDistanceToPoint(worldPoint);
		return calculationWayPoints[0];
	}

	public Waypoint GetClosestWaypoint(Vector3 worldPoint, bool requiresVision)
	{
		SortWaypointsByDistanceToPoint(worldPoint);
		if (requiresVision)
		{
			for (int i = 0; i < calculationWayPoints.Count; i++)
			{
				if (!(calculationWayPoints[i] == null) && Helper.HasClearVision(worldPoint, calculationWayPoints[i].transform.position, LayerMasks.MASK_Obstruction))
				{
					return calculationWayPoints[i];
				}
			}
		}
		return calculationWayPoints[0];
	}

	public Waypoint GetClosestWaypoint(Vector3 worldPoint, Waypoint.WayPointID IDType)
	{
		SortWaypointsByDistanceToPoint(worldPoint);
		for (int i = 0; i < calculationWayPoints.Count; i++)
		{
			if (calculationWayPoints[i] != null && calculationWayPoints[i].waypointID == IDType)
			{
				return calculationWayPoints[i];
			}
		}
		return null;
	}

	private void ConnectListOfWaypoints(List<Waypoint> waypoints)
	{
		for (int i = 0; i < waypoints.Count - 1; i++)
		{
			Waypoint waypoint = waypoints[i];
			Waypoint waypoint2 = waypoints[i + 1];
			waypoint.waypointHandler = this;
			waypoint2.waypointHandler = this;
			if (i - 1 > 0)
			{
				waypoint.previous = waypoints[i - 1];
			}
			waypoint.next = waypoint2;
			waypoint2.previous = waypoint;
		}
		waypoints[0].previous = waypoints[waypoints.Count - 1];
		waypoints[waypoints.Count - 1].next = waypoints[0];
		waypoints[0].waypointHandler = this;
		waypoints[waypoints.Count - 1].waypointHandler = this;
	}

	private void SortWaypointsByDistanceToPoint(Vector3 worldPoint)
	{
		if (calculationWayPoints.Count == 0)
		{
			calculationWayPoints = waypoints;
		}
		calculationWayPoints.Sort((Waypoint a, Waypoint b) => (worldPoint - a.transform.position).sqrMagnitude.CompareTo((worldPoint - b.transform.position).sqrMagnitude));
	}

	public void OnDrawGizmosSelected()
	{
		foreach (Waypoint waypoint in waypoints)
		{
			Gizmos.DrawCube(waypoint.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
			if (waypoint.next != null)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(waypoint.transform.position, waypoint.next.transform.position);
			}
			if (waypoint.previous != null)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(waypoint.transform.position, waypoint.previous.transform.position);
			}
			if (waypoint.gateway != null)
			{
				Gizmos.color = Color.magenta;
				Gizmos.DrawLine(waypoint.transform.position, waypoint.gateway.transform.position);
			}
		}
	}
}
