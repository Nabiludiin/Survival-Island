using UnityEngine;

public class Rudder : MonoBehaviour
{
	[SerializeField]
	private Transform transformToRotate;

	public void SetRotation(float rotation)
	{
		Vector3 localEulerAngles = transformToRotate.localEulerAngles;
		localEulerAngles.y = rotation;
		transformToRotate.localEulerAngles = localEulerAngles;
	}
}
