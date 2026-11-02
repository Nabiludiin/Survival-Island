using UnityEngine;

public class MeshPath_Zipline_Landmark : MeshPath_Zipline
{
	[SerializeField]
	private Transform pointA;

	[SerializeField]
	private Transform pointB;

	[SerializeField]
	private int minPoints = 8;

	[SerializeField]
	private int maxPoints = 20;

	[SerializeField]
	private float slack = -0.5f;

	public Vector3 LocalPointA => transform.InverseTransformPoint(pointA.transform.position);

	public Vector3 LocalPointB => transform.InverseTransformPoint(pointB.transform.position);

	protected override void OnDrawGizmos()
	{
		if (drawGizmoPoints && pointA != null && pointB != null)
		{
			points = MeshPath.CreatePointsForPath(LocalPointA, LocalPointB, slack, minPoints, maxPoints);
		}
		else
		{
			points = null;
		}
		base.OnDrawGizmos();
	}

	private void OnEnable()
	{
		if (meshFilter.sharedMesh == null && pointA != null && pointB != null)
		{
			CreatePath(pointA.position, pointB.position, slack, replicating: false, minPoints, maxPoints);
		}
	}

	private void ForceCreatePath()
	{
		CreatePath(pointA.position, pointB.position, slack, replicating: false);
	}
}
