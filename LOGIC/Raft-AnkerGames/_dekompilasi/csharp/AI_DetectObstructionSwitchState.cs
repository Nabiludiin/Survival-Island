using System.Collections.Generic;
using UnityEngine;

public class AI_DetectObstructionSwitchState : MonoBehaviour
{
	[SerializeField]
	private AI_StateMachine stateMachine;

	[SerializeField]
	private TimerEventer rayTimer;

	[SerializeField]
	private LayerMask rayMask;

	[SerializeField]
	private float rayLength = 10f;

	[SerializeField]
	private float rayRadius;

	[SerializeField]
	private Vector3 localRayDirection = Vector3.forward;

	[SerializeField]
	private AI_State rayHitState;

	[SerializeField]
	private List<AI_State> castRayStates;

	private void Update()
	{
		if (!Raft_Network.IsHost || stateMachine == null || rayHitState == null || !castRayStates.Contains(stateMachine.currentState) || !rayTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
		{
			return;
		}
		Vector3 direction = transform.TransformDirection(localRayDirection);
		RaycastHit hitInfo;
		if (rayRadius <= 0f)
		{
			if (Physics.Raycast(transform.position, direction, rayLength, rayMask, QueryTriggerInteraction.UseGlobal))
			{
				stateMachine.ChangeState(rayHitState);
			}
		}
		else if (Physics.SphereCast(transform.position, rayRadius, direction, out hitInfo, rayLength, rayMask, QueryTriggerInteraction.UseGlobal))
		{
			stateMachine.ChangeState(rayHitState);
		}
	}
}
