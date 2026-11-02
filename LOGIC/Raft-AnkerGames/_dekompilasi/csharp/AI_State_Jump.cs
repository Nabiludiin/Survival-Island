using UnityEngine;

public class AI_State_Jump : AI_State
{
	[SerializeField]
	private MovementType movementType;

	[SerializeField]
	private AnimationCurve heightCurve;

	[SerializeField]
	private AnimationCurve timeCurve;

	[SerializeField]
	private float length = 2f;

	[SerializeField]
	private float jumpTime = 2f;

	[SerializeField]
	private AI_State nextState;

	private Vector3 startPosition;

	private Vector3 endPosition;

	private float timer;

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetMovementType(movementType);
		Vector3 forward = stateMachine.transform.forward;
		forward.y = 0f;
		startPosition = stateMachine.transform.position;
		endPosition = startPosition + forward.normalized * length;
		timer = 0f;
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		timer += Time.deltaTime;
		float num = timeCurve.Evaluate(Mathf.Clamp01(timer / jumpTime));
		Vector3 vector = Vector3.Lerp(startPosition, endPosition, num);
		vector.y += heightCurve.Evaluate(num);
		if (movementType == MovementType.Translate_Simple)
		{
			if (vector - stateMachine.transform.position != Vector3.zero)
			{
				stateMachine.transform.rotation = Quaternion.LookRotation(vector - stateMachine.transform.position);
			}
			stateMachine.transform.position = vector;
		}
		if (num >= 1f && nextState != null)
		{
			stateMachine.ChangeState(nextState);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (active)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(startPosition, 0.1f);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(endPosition, 0.1f);
			float num = Mathf.Clamp01(timer / jumpTime);
			Vector3 vector2;
			Vector3 vector = (vector2 = Vector3.Lerp(startPosition, endPosition, num));
			vector2.y += heightCurve.Evaluate(num);
			Gizmos.color = Color.Lerp(Color.green, Color.red, num);
			Gizmos.DrawLine(startPosition, endPosition);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawSphere(vector2, 0.1f);
			Gizmos.DrawSphere(vector, 0.1f);
		}
	}
}
