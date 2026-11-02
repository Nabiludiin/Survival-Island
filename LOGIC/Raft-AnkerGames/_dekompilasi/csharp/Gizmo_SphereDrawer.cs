using UnityEngine;

public class Gizmo_SphereDrawer : MonoBehaviour
{
	[SerializeField]
	private Color color = Color.green;

	[SerializeField]
	private float radius = 1f;

	public void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, radius);
	}
}
