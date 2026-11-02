using UnityEngine;

public class AI_Component_Delta_Animation : AI_Component
{
	[SerializeField]
	private AI_Movement movement;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private string paramName = "Speed";

	[SerializeField]
	private Interval_Float minMaxAnimValue;

	[SerializeField]
	private float stateMaxMovementSpeed = 8f;

	protected override void OnValidate()
	{
		base.OnValidate();
		if (animator == null)
		{
			animator = GetComponentInParent<Animator>();
		}
		if (animator == null)
		{
			animator = GetComponentInParent<Animator>();
		}
	}

	protected override void OnUpdateComponent()
	{
		base.OnUpdateComponent();
		if (Raft_Network.IsHost && movement.agent != null)
		{
			float value = Helper.MapValue(0f, stateMaxMovementSpeed, movement.agent.velocity.magnitude, minMaxAnimValue.minValue, minMaxAnimValue.maxValue);
			animator.SetFloat(paramName, value);
		}
	}
}
