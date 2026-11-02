using UnityEngine;

public class AreaZone : MonoBehaviour
{
	[SerializeField]
	private Color gizmoColor;

	[SerializeField]
	private EnvironmentProbeBoundsType boundsType;

	[SerializeField]
	private Collider collider;

	public Vector3 BoxCenterWorld => transform.position + transform.up * (transform.localScale.y / 2f) + transform.right * (transform.localScale.x / 2f) + transform.forward * (transform.localScale.z / 2f);

	private float MinDistance => transform.localScale.x;

	public float GetNormalizedDistanceToPoint(Vector3 point)
	{
		float minDistance = MinDistance;
		switch (boundsType)
		{
		case EnvironmentProbeBoundsType.Sphere:
			if (!(Vector3.Distance(transform.position, point) <= minDistance))
			{
				return 0f;
			}
			return 1f;
		case EnvironmentProbeBoundsType.Cylinder:
		{
			float num = point.y - transform.position.y;
			if (num < 0f || num > transform.localScale.y)
			{
				return 0f;
			}
			if (!(transform.position.DistanceXZ(point) <= minDistance))
			{
				return 0f;
			}
			return 1f;
		}
		case EnvironmentProbeBoundsType.Box:
			if (!Helper.IsPointInOABB(point, transform, Vector3.one / 2f))
			{
				return 0f;
			}
			return 1f;
		case EnvironmentProbeBoundsType.Collider:
			if (!Helper.IsPointInCollider(point, collider))
			{
				return 0f;
			}
			return 1f;
		case EnvironmentProbeBoundsType.Hemisphere:
		{
			float num = point.y - transform.position.y;
			if (num < 0f || num > transform.localScale.y)
			{
				return 0f;
			}
			if (!(Vector3.Distance(transform.position, point) <= minDistance))
			{
				return 0f;
			}
			return 1f;
		}
		default:
			return 0f;
		}
	}

	public Vector3 GetCenterOfArea()
	{
		switch (boundsType)
		{
		case EnvironmentProbeBoundsType.Sphere:
		case EnvironmentProbeBoundsType.Hemisphere:
			return transform.position;
		case EnvironmentProbeBoundsType.Cylinder:
			return transform.position + transform.up * (transform.localScale.y / 2f);
		case EnvironmentProbeBoundsType.Box:
			return transform.position + transform.up * (transform.localScale.y / 2f) + transform.right * (transform.localScale.x / 2f) + transform.forward * (transform.localScale.z / 2f);
		case EnvironmentProbeBoundsType.Collider:
			if (collider == null)
			{
				return transform.position;
			}
			return collider.bounds.center;
		default:
			return transform.position;
		}
	}

	public Vector3 GetTopCenterOfArea()
	{
		switch (boundsType)
		{
		case EnvironmentProbeBoundsType.Sphere:
			return transform.position + transform.up * MinDistance;
		case EnvironmentProbeBoundsType.Cylinder:
			return transform.position + transform.up * transform.localScale.y;
		case EnvironmentProbeBoundsType.Box:
			return transform.position + transform.up * transform.localScale.y + transform.right * (transform.localScale.x / 2f) + transform.forward * (transform.localScale.z / 2f);
		case EnvironmentProbeBoundsType.Collider:
			if (collider == null)
			{
				return transform.position;
			}
			return collider.bounds.center;
		default:
			return transform.position;
		}
	}

	private bool IsBoundsTypeCollider()
	{
		return boundsType == EnvironmentProbeBoundsType.Collider;
	}
}
