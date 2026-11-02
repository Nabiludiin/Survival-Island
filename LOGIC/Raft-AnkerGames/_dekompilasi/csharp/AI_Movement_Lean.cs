using UnityEngine;

public class AI_Movement_Lean : MonoBehaviour
{
	[SerializeField]
	private float maxLeanAngle = 45f;

	[SerializeField]
	private float maxLeanDelta = 1f;

	[SerializeField]
	private float leanLerpSpeed = 1f;

	[SerializeField]
	private float rotDelta;

	[SerializeField]
	private float targetLeanAngle;

	private float previousRot;

	private void LateUpdate()
	{
		rotDelta = transform.eulerAngles.y - previousRot;
		float num = Mathf.Abs(rotDelta);
		if (num < 0.25f)
		{
			num = (rotDelta = 0f);
		}
		float num2 = Mathf.Clamp01(num / maxLeanDelta);
		targetLeanAngle = Mathf.Clamp(maxLeanAngle * num2, 0f, maxLeanAngle);
		if (rotDelta > 0f)
		{
			targetLeanAngle *= -1f;
		}
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.z = Mathf.LerpAngle(localEulerAngles.z, targetLeanAngle, Time.deltaTime * leanLerpSpeed);
		transform.localEulerAngles = localEulerAngles;
		previousRot = transform.eulerAngles.y;
	}
}
