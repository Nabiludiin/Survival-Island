using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class CameraSettingsControl : MonoBehaviour
{
	[SerializeField]
	public Camera Camera;

	[SerializeField]
	public CameraFollow Follow;

	public void SetCameraRotation(Vector3 rot)
	{
		transform.rotation = Quaternion.Euler(rot);
	}

	public void SetCameraOffset(Vector3 offset)
	{
		Follow.Offset = offset;
	}

	public void SetCameraFov(float fov)
	{
		Camera.fieldOfView = fov;
	}

	public void SetCameraSpeed(float speed)
	{
		Follow.Speed = speed;
	}
}
