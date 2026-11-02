using UnityEngine;

[CreateAssetMenu(fileName = "Raft/Miscallaneous/BoidSetting", menuName = "Raft/Miscallaneous/BoidSetting")]
public class BoidSettings : ScriptableObject
{
	public float rotationSpeed = 5f;

	public bool lerpRotation;

	public float maxSteerForce = 3f;

	public float targetWeight = 1f;

	[Header("Collisions")]
	public LayerMask obstacleMask;

	public float boundsRadius = 0.27f;

	public float avoidCollisionWeight = 10f;

	public float collisionAvoidDst = 5f;

	public float surfaceHeightLevel = -0.8f;

	public float avoidSurfaceDistance = 2.5f;

	public float rayOffset;
}
