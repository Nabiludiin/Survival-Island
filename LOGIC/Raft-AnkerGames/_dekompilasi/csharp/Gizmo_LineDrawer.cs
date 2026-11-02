using UnityEngine;

public class Gizmo_LineDrawer : MonoBehaviour
{
	[SerializeField]
	private Transform target;

	[SerializeField]
	private Color lineColor = Color.green;

	public void OnDrawGizmos()
	{
		if (!(target == null))
		{
			Gizmos.color = lineColor;
			Gizmos.DrawLine(transform.position, target.transform.position);
		}
	}
}
