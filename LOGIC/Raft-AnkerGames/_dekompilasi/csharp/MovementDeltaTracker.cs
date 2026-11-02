using System.Collections.Generic;
using UnityEngine;

public class MovementDeltaTracker : MonoBehaviour
{
	public Transform trackedTransform;

	[SerializeField]
	private int savedPositionCount = 3;

	[SerializeField]
	private float trackingUpdateRate = 0.3f;

	private Vector3 previousPosition;

	private Vector3 previousLocalPosition;

	private float trackingUpdateProgress;

	private List<Vector3> previousDeltas = new List<Vector3>();

	private List<Vector3> previousLocalDeltas = new List<Vector3>();

	public bool drawGizmoEstimatedPosition;

	public float gizmoEstimatedPositionDistance = 1f;

	public void OnDrawGizmos()
	{
		if (drawGizmoEstimatedPosition)
		{
			Gizmos.color = Color.yellow;
			Vector3 approximateTrackedTransformPositionWithDeltaMovement = GetApproximateTrackedTransformPositionWithDeltaMovement(gizmoEstimatedPositionDistance, normalizeDirection: false);
			Gizmos.DrawLine(trackedTransform.position, approximateTrackedTransformPositionWithDeltaMovement);
			Gizmos.DrawSphere(approximateTrackedTransformPositionWithDeltaMovement, 0.4f);
		}
	}

	private void Awake()
	{
		previousDeltas.Add(Vector3.zero);
	}

	private void LateUpdate()
	{
		trackingUpdateProgress += Time.deltaTime;
		if (trackingUpdateProgress >= trackingUpdateRate)
		{
			trackingUpdateProgress = 0f;
			HandleDeltaCalculations();
		}
	}

	public Vector3 GetApproximateTrackedTransformPositionWithDeltaMovement(float approximatedDistanceMultiplier = 3f, bool normalizeDirection = true)
	{
		if (normalizeDirection)
		{
			return trackedTransform.position + GetAverageDeltaVector().normalized * approximatedDistanceMultiplier;
		}
		return trackedTransform.position + GetAverageDeltaVector() * approximatedDistanceMultiplier;
	}

	public Vector3 GetAverageDeltaVector()
	{
		Vector3 zero = Vector3.zero;
		if (previousDeltas.Count == 0)
		{
			return Vector3.zero;
		}
		if (previousDeltas.Count == 1)
		{
			return previousDeltas[0];
		}
		for (int num = previousDeltas.Count - 1; num >= 0; num--)
		{
			zero += previousDeltas[num];
		}
		return zero / previousDeltas.Count;
	}

	public Vector3 GetAverageLocalDeltaVector()
	{
		Vector3 zero = Vector3.zero;
		if (previousLocalDeltas.Count == 0)
		{
			return Vector3.zero;
		}
		if (previousLocalDeltas.Count == 1)
		{
			return previousLocalDeltas[0];
		}
		for (int num = previousLocalDeltas.Count - 1; num >= 0; num--)
		{
			zero += previousLocalDeltas[num];
		}
		return zero / previousLocalDeltas.Count;
	}

	private void HandleDeltaCalculations()
	{
		Vector3 position = trackedTransform.position;
		Vector3 localPosition = trackedTransform.localPosition;
		Vector3 item = ((previousDeltas.Count > 0) ? (position - previousPosition) : Vector3.zero);
		Vector3 item2 = ((previousDeltas.Count > 0) ? (localPosition - previousLocalPosition) : Vector3.zero);
		previousDeltas.Add(item);
		if (previousDeltas.Count > savedPositionCount)
		{
			int count = previousDeltas.Count - savedPositionCount;
			previousDeltas.RemoveRange(0, count);
		}
		previousLocalDeltas.Add(item2);
		if (previousLocalDeltas.Count > savedPositionCount)
		{
			int count2 = previousLocalDeltas.Count - savedPositionCount;
			previousLocalDeltas.RemoveRange(0, count2);
		}
		previousPosition = position;
		previousLocalPosition = localPosition;
	}
}
