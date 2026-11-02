using UnityEngine;

public class GroundRaycaster : MonoBehaviour
{
	public Transform carrier;

	public Vector3 carrierRayOffset;

	public LayerMask groundLayer;

	public float rayDistance;

	public CustomRay customGroundRay;

	[HideInInspector]
	public float distanceToGround;

	public Vector3 OffsetedRayPosition => carrier.transform.position + carrierRayOffset;

	public bool HasObstructionUnderneath => customGroundRay.hit.collider != null;

	public float DistanceToObstructionUnderneath
	{
		get
		{
			if (HasObstructionUnderneath)
			{
				return Vector3.Distance(OffsetedRayPosition, customGroundRay.hit.point);
			}
			return 9999f;
		}
	}

	private void Awake()
	{
		customGroundRay = new CustomRay(rayDistance, new Ray(OffsetedRayPosition, Vector3.down));
	}

	public void DoRay()
	{
		customGroundRay.DoRay(groundLayer, new Ray(OffsetedRayPosition, Vector3.down));
		if (HasObstructionUnderneath)
		{
			RayHitGround(customGroundRay.hit);
		}
		else
		{
			Reset();
		}
	}

	protected virtual void RayHitGround(RaycastHit hit)
	{
		distanceToGround = DistanceToObstructionUnderneath;
	}

	public virtual void Reset()
	{
	}
}
