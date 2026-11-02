using UnityEngine;

public class QuestInteractable_VarunaBoss_Wall : QuestInteractable
{
	public AxisToTransformRotation particleRotator;

	public Transform particleTransformToRotateWithAxis;

	private void OnValidate()
	{
		if (particleRotator == null)
		{
			particleRotator = GetComponent<AxisToTransformRotation>();
		}
	}

	public void BossInteract(AI_StateMachine_Animal stateMachine)
	{
		bool success = false;
		Vector3 axisRotation = particleRotator.GetAxisRotation(out success, Helper.GetAxisComparedToZOnly(stateMachine.transform, transform));
		if (success)
		{
			particleTransformToRotateWithAxis.localRotation = Quaternion.Euler(axisRotation);
		}
		SendOnInteractNetwork(null, forceSuccess: true);
	}
}
