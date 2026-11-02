using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
	[SerializeField]
	private bool lockX;

	[SerializeField]
	private bool lockY;

	[SerializeField]
	private bool lockZ;

	private Transform cameraTransform;

	private Vector3 localEulerStart;

	private void Awake()
	{
		localEulerStart = transform.localEulerAngles;
	}

	private void Update()
	{
		if (cameraTransform == null)
		{
			Network_Player value = ComponentManager<Network_Player>.Value;
			if (value != null)
			{
				cameraTransform = value.CameraTransform;
			}
			return;
		}
		Vector3 eulerAngles = Quaternion.LookRotation(cameraTransform.position - transform.position).eulerAngles;
		if (lockX)
		{
			eulerAngles.x = localEulerStart.x;
		}
		if (lockY)
		{
			eulerAngles.y = localEulerStart.y;
		}
		if (lockZ)
		{
			eulerAngles.z = localEulerStart.z;
		}
		transform.eulerAngles = eulerAngles;
	}
}
