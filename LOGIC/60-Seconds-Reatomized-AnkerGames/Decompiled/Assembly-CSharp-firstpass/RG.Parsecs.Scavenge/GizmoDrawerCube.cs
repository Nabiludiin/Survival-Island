using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class GizmoDrawerCube : MonoBehaviour
{
	public Color GizmoColor;

	public string GizmoIconName;

	public Vector3 Size = Vector3.one;

	public Vector3 IconOffset = new Vector3(0f, 1f, 0f);

	public virtual void OnDrawGizmos()
	{
	}
}
