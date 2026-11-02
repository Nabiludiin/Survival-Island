using UnityEngine;

public class EstimatedTimeToReachPoint : MonoBehaviour
{
	public Transform tracked;

	public float Distance;

	public float TimeBetweenObjects;

	public void RecalculateEstimatedTime(Vector3 toTarget, float movementSpeed)
	{
		UpdateDistanceBetween(toTarget);
		UpdateTimeBetween(movementSpeed);
	}

	private void UpdateDistanceBetween(Vector3 toTarget)
	{
		Distance = Vector3.Distance(tracked.transform.position, toTarget);
	}

	private void UpdateTimeBetween(float movementSpeed)
	{
		TimeBetweenObjects = Distance / movementSpeed;
	}
}
