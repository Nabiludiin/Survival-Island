using UnityEngine;

public class Handheld_Compass : MonoBehaviour
{
	public Transform pointer;

	public Transform compass;

	[SerializeField]
	private bool invert;

	[SerializeField]
	private float accelerationSpeed = 5f;

	[SerializeField]
	private float maxAcceleration = 5f;

	[SerializeField]
	private float acceleration;

	[SerializeField]
	private float currentAngle;

	[SerializeField]
	private float targetAngle;

	private void Update()
	{
		Vector3 vector = Vector3.forward;
		if (invert)
		{
			vector = -vector;
		}
		pointer.rotation = Quaternion.LookRotation(vector, compass.up);
		targetAngle = pointer.localEulerAngles.y;
		int num = ((Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime) > currentAngle) ? 1 : (-1));
		acceleration = Mathf.Lerp(acceleration, maxAcceleration * (float)num, Time.deltaTime * accelerationSpeed);
		currentAngle += acceleration;
		if (currentAngle < 0f)
		{
			currentAngle += 360f;
		}
		else if (currentAngle > 360f)
		{
			currentAngle -= 360f;
		}
		pointer.localEulerAngles = new Vector3(0f, currentAngle, 0f);
	}
}
