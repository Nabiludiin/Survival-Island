using UnityEngine;

public class SteeringWheelRudderRotator : MonoBehaviour
{
	public Transform rudderRotationTransform;

	public void RotateRudder(Vector3 euler)
	{
		rudderRotationTransform.localEulerAngles = new Vector3(0f, 0f - euler.z, 0f);
	}
}
