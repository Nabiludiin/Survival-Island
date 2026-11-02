using UnityEngine;

public class InterestPoint : MonoBehaviour, IInterestPoint
{
	public SO_NPC_InterestData interestPointData;

	public Transform entryPoint;

	public Transform interactPoint;

	private bool occupied;

	private void OnEnable()
	{
		occupied = false;
	}

	public Vector3 GetPoint()
	{
		if (entryPoint != null)
		{
			return entryPoint.position;
		}
		return transform.position;
	}

	public bool IsReachable()
	{
		return true;
	}

	public bool Occupied()
	{
		return occupied;
	}

	public bool Occupy(bool value)
	{
		if (occupied == !value)
		{
			occupied = value;
			return true;
		}
		return false;
	}
}
