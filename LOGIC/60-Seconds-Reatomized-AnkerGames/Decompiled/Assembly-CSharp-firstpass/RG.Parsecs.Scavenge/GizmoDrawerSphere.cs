using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class GizmoDrawerSphere : MonoBehaviour
{
	public Color GizmoColor;

	public string GizmoIconName;

	public float Radius = 0.5f;

	public Vector3 IconOffset = new Vector3(0f, 1f, 0f);

	public virtual void OnDrawGizmos()
	{
	}
}
