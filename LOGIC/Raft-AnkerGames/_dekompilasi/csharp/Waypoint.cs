using UnityEngine;

public class Waypoint : MonoBehaviour
{
	public enum WayPointID
	{
		UNDEFINED,
		VARUNA_BOSS_INNER,
		VARUNA_BOSS_OUTER
	}

	public Waypoint previous;

	public Waypoint next;

	public Waypoint gateway;

	public WaypointHandler waypointHandler;

	public WayPointID waypointID;

	public void ConnectOtherWaypointAsGateway()
	{
		if (gateway != null)
		{
			gateway.gateway = this;
		}
	}

	public bool IsValidGateway(WayPointID ID)
	{
		if (gateway != null && gateway.waypointID == ID)
		{
			return true;
		}
		return false;
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
		if (next != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, next.transform.position);
		}
		if (previous != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, previous.transform.position);
		}
		if (gateway != null)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(transform.position, gateway.transform.position);
		}
	}
}
