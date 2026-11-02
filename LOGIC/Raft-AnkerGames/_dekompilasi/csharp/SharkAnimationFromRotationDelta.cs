using UnityEngine;

public class SharkAnimationFromRotationDelta : MonoBehaviour
{
	[SerializeField]
	private MeasureRotationDelta rotationDeltaScript;

	[SerializeField]
	private AnimatorController animatorController;

	[SerializeField]
	private float lerpAnimatorSpeed;

	[SerializeField]
	private string parameterUpDown = "TurnUpDown";

	[SerializeField]
	private string parameterLeftRight = "TurnRightLeft";

	private void OnValidate()
	{
		if (rotationDeltaScript == null)
		{
			rotationDeltaScript = GetComponent<MeasureRotationDelta>();
		}
	}

	private void LateUpdate()
	{
		float current = animatorController.GetFloat(parameterLeftRight);
		float current2 = animatorController.GetFloat(parameterUpDown);
		current = Mathf.MoveTowards(current, rotationDeltaScript.NormalizedRotationDelta.y, Time.deltaTime * lerpAnimatorSpeed);
		current2 = Mathf.MoveTowards(current2, rotationDeltaScript.NormalizedRotationDelta.x, Time.deltaTime * lerpAnimatorSpeed);
		animatorController.SetFloat(parameterLeftRight, current);
		animatorController.SetFloat(parameterUpDown, current2);
	}
}
