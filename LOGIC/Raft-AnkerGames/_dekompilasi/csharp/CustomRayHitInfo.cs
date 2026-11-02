using UnityEngine;

public class CustomRayHitInfo
{
	public Transform self;

	public float rayLenght;

	public CustomRay left;

	public CustomRay right;

	public CustomRay forward;

	public CustomRay up;

	public CustomRay down;

	public float lastUpdateTimeStamp;

	public float rayUpdateCooldown;

	public bool hitLeft;

	public bool hitRight;

	public bool hitUp;

	public bool hitDown;

	public bool hitFront;

	public CustomRayHitInfo(Transform from, float p_rayLenght, float p_rayUpdateCooldown = 0f)
	{
		rayLenght = p_rayLenght;
		self = from;
		rayUpdateCooldown = p_rayUpdateCooldown;
		left = new CustomRay(rayLenght);
		right = new CustomRay(rayLenght);
		forward = new CustomRay(rayLenght);
		up = new CustomRay(rayLenght);
		down = new CustomRay(rayLenght);
	}

	public void ResetBoolHits(bool p_value)
	{
		hitLeft = (hitRight = (hitUp = (hitDown = (hitFront = p_value))));
	}
}
