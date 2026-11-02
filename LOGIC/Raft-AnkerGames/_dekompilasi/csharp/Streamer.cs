using UnityEngine;

public class Streamer : MonoBehaviour
{
	[SerializeField]
	private float rotationSpeed;

	[SerializeField]
	private Transform streamerTransform;

	[SerializeField]
	private Vector3 worldRotationTargetDir = -Vector3.forward;

	private void Update()
	{
		Quaternion b = Quaternion.LookRotation(worldRotationTargetDir, streamerTransform.up);
		streamerTransform.rotation = Quaternion.Lerp(streamerTransform.rotation, b, Time.deltaTime * rotationSpeed);
	}
}
