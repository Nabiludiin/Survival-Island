using UnityEngine;

public class AI_State_Domestic_Water : AI_State
{
	public override void EnterState()
	{
		base.EnterState();
		if (!stateMachine.movement.waterFloat.IsActive())
		{
			stateMachine.movement.SetMovementType(MovementType.CharacterController);
			stateMachine.movement.waterFloat.Activate();
			stateMachine.movement.waterFloat.setToSurface = true;
			if (stateMachine.movement.characterController.isGrounded)
			{
				stateMachine.movement.characterController.Move(Vector3.forward * 0.05f);
			}
		}
	}
}
