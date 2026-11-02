using UnityEngine;

public class Unstuck_Point : MonoBehaviour
{
	private void OnEnable()
	{
		Unstuck_PointManager.AddPointToList(transform);
	}

	private void OnDisable()
	{
		Unstuck_PointManager.RemovePointFromList(transform);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(transform.position, 3f);
	}
}
