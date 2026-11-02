using UnityEngine;

namespace RG.SecondsRemaster.Scavenge;

public class ScavengeCameraResizer : MonoBehaviour
{
	[SerializeField]
	private Camera scavengeCamera;

	[SerializeField]
	private float regularAspectRatio = 1.8f;

	private float regularCameraSize = 50f;

	[SerializeField]
	private Transform cameraHolder;

	private Vector3 cameraHolderResized = new Vector3(0f, 0f, -0.085f);

	private Vector3 cameraHolderRegular = new Vector3(0f, 0f, -0.085f);

	private Vector3 rot = new Vector3(0f, 0f, 9f);

	private float irregularCameraSize = 55f;

	private void Start()
	{
		int height = Screen.height;
		float num = (float)Screen.width / (float)height;
		scavengeCamera.fieldOfView = ((num <= regularAspectRatio) ? regularCameraSize : irregularCameraSize);
		transform.parent.localPosition = (((float)Screen.height / (float)Screen.width == 0.75f) ? cameraHolderResized : cameraHolderRegular);
		transform.parent.Rotate(rot);
	}
}
