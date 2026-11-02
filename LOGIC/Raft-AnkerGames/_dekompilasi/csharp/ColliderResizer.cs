using UnityEngine;

public class ColliderResizer : MonoBehaviour
{
	public BoxCollider colli;

	public GizmoBox_Center gizmoBox;

	public Vector3 targetColliderCenter = Vector3.zero;

	public Vector3 targetColliderSize = Vector3.one;

	private Vector3 originColliderCenter = Vector3.zero;

	private Vector3 originColliderSize = Vector3.one;

	private void OnValidate()
	{
		if (gizmoBox == null)
		{
			gizmoBox = GetComponent<GizmoBox_Center>();
		}
		if (colli == null)
		{
			colli = GetComponent<BoxCollider>();
		}
		originColliderCenter = colli.bounds.center;
		originColliderSize = colli.bounds.extents;
		if (gizmoBox != null)
		{
			gizmoBox.size = targetColliderSize;
			gizmoBox.centerOffset = targetColliderCenter;
		}
	}

	private void Awake()
	{
		originColliderSize = colli.bounds.extents;
	}

	public void SetColliderSizeToOrigin()
	{
		colli.size = originColliderSize;
	}

	public void SetColliderCenterToOrigin()
	{
		colli.center = originColliderCenter;
	}

	public void SetOriginColliSettings()
	{
		SetColliderSizeToOrigin();
		SetColliderCenterToOrigin();
	}

	public void SetTargetColliSettings()
	{
		SetColliderSizeToTargetSize();
		SetColliderCenterToTarget();
	}

	public void SetColliderCenterToTarget()
	{
		colli.center = targetColliderCenter;
	}

	public void SetColliderSizeToTargetSize()
	{
		colli.size = targetColliderSize;
	}
}
