using UnityEngine;

public class AI_State_Captured : AI_State
{
	[SerializeField]
	private GameObject capturedNetModel;

	private AI_NetworkBehaviour_Domestic domesticBehaviour;

	protected override void Awake()
	{
		base.Awake();
		domesticBehaviour = GetComponentInParent<AI_NetworkBehaviour_Domestic>();
	}

	public override void EnterState()
	{
		base.EnterState();
		if (domesticBehaviour.carryScript != null)
		{
			domesticBehaviour.carryScript.EnableCollider();
		}
		if (capturedNetModel != null)
		{
			capturedNetModel.SetActive(value: true);
		}
		stateMachine.movement.SetMovementType(MovementType.None);
	}

	public override void ExitState()
	{
		base.ExitState();
		if (capturedNetModel != null)
		{
			capturedNetModel.SetActive(value: false);
		}
	}
}
