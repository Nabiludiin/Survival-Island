using UnityEngine;

public class AI_State_MoveToPoint_Spawn : AI_State_MoveToPoint
{
	[SerializeField]
	private AI_NetworkBehaviour networkBehaviour;

	[SerializeField]
	private bool matchRotation = true;

	[SerializeField]
	private float rotationSpeed = 3f;

	[SerializeField]
	private float minValidAngle = 10f;

	private Transform spawnTransform;

	public override void EnterState()
	{
		base.EnterState();
		spawnTransform = networkBehaviour.connectedSpawner.transform;
		stateMachine.movement.SetRotationSpeed(rotationSpeed);
	}

	protected override bool MoveToPoint(Vector3 targetPoint)
	{
		base.targetPoint = (targetPoint = spawnTransform.position);
		Vector3 vector = (targetPoint - stateMachine.transform.position).XZOnly();
		if (Mathf.Abs(Vector3.Angle(stateMachine.movement.model.forward.XZOnly(), vector)) > minValidAngle && vector.magnitude > 0.5f)
		{
			stateMachine.movement.LookDirection(vector, stateMachine.movement.RotationSpeed, ignoreZRotation: true);
		}
		else if (base.MoveToPoint(targetPoint))
		{
			if (!matchRotation)
			{
				return true;
			}
			stateMachine.movement.LookDirection(spawnTransform.forward, stateMachine.movement.RotationSpeed, ignoreZRotation: true);
			if (Mathf.Abs(Vector3.Angle(stateMachine.movement.model.forward.XZOnly(), spawnTransform.forward.XZOnly())) <= minValidAngle)
			{
				return true;
			}
		}
		return false;
	}
}
