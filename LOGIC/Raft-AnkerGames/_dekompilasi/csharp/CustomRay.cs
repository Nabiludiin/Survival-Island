using UnityEngine;

public class CustomRay
{
	public Ray ray;

	public RaycastHit hit;

	public float rayLenght;

	public float HitNormalInfo
	{
		get
		{
			if (!hit.collider)
			{
				return 1f;
			}
			return 1f - Mathf.Clamp01(hit.distance / rayLenght);
		}
	}

	public CustomRay(float p_raylenght)
	{
		rayLenght = p_raylenght;
	}

	public CustomRay(float p_raylenght, Ray ray)
	{
		rayLenght = p_raylenght;
		this.ray = ray;
	}

	public bool DoRay(LayerMask obstructionLayer)
	{
		return Physics.Raycast(ray, out hit, rayLenght, obstructionLayer);
	}

	public bool DoRay(LayerMask obstructionLayer, Ray ray)
	{
		return Physics.Raycast(ray, out hit, rayLenght, obstructionLayer);
	}

	public void DebugRay()
	{
		Debug.DrawRay(ray.origin, ray.direction * rayLenght, hit.collider ? Color.red : Color.green);
	}

	public float GetDistanceToHitPointFrom(Vector3 point)
	{
		return Vector3.Distance(point, hit.point);
	}

	public float HitNormalDistance()
	{
		return 1f - Mathf.Clamp01(hit.distance / rayLenght);
	}

	public float HitNormalDistance(float overriddenRayLenght)
	{
		return 1f - Mathf.Clamp01(hit.distance / overriddenRayLenght);
	}
}
