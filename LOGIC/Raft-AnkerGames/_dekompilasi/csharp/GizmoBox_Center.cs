using UnityEngine;

public class GizmoBox_Center : GizmoBox
{
	public Vector3 centerOffset = Vector3.zero;

	public override Vector3 Center => transform.TransformPoint(centerOffset);

	protected override void OnDrawGizmosSelected()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(centerOffset, size);
	}
}
