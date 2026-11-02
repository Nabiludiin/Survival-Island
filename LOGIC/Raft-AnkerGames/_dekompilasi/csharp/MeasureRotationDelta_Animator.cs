using UnityEngine;

public class MeasureRotationDelta_Animator : MeasureRotationDelta
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float lerpAnimatorSpeed;

	[SerializeField]
	private string parameter = "RotationSpeed";

	[SerializeField]
	private bool xRotation;

	[SerializeField]
	private bool yRotation;

	[SerializeField]
	private bool zRotation;

	protected override void LateUpdate()
	{
		base.LateUpdate();
		float current = animator.GetFloat(parameter);
		float target = 0f;
		if (xRotation)
		{
			target = NormalizedRotationDelta.x;
		}
		else if (yRotation)
		{
			target = NormalizedRotationDelta.y;
		}
		else if (zRotation)
		{
			target = NormalizedRotationDelta.z;
		}
		current = Mathf.MoveTowards(current, target, Time.deltaTime * lerpAnimatorSpeed);
		animator.SetFloat(parameter, current);
	}
}
