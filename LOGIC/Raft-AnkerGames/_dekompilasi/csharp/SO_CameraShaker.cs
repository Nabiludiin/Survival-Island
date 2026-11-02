using EZCameraShake;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShake", menuName = "Raft/Camera/CameraShake", order = 1)]
public class SO_CameraShaker : ScriptableObject
{
	public float magnitude;

	public float roughness;

	public float fadeInTime;

	public float fadeOutTime;

	public Vector3 positionInfluence;

	public Vector3 rotationInfluence;

	public int hapticEvent;

	public CameraShakeInstance ShakerInstance => new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime)
	{
		PositionInfluence = positionInfluence,
		RotationInfluence = rotationInfluence
	};
}
