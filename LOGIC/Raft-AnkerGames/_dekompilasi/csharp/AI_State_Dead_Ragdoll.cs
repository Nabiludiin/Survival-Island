using UnityEngine;

public class AI_State_Dead_Ragdoll : AI_State_Dead
{
	[SerializeField]
	private float force;

	[SerializeField]
	private bool createRigidBodyAtTop = true;

	protected RagdollManager ragdollManager;

	private AnimatorController animController;

	protected override void Awake()
	{
		base.Awake();
		animController = GetComponentInParent<AnimatorController>();
		ragdollManager = GetComponentInParent<RagdollManager>();
		if (ragdollManager == null)
		{
			Debug.Log("Can't find any RagdollManager.cs on " + transform.name);
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		if (animController != null)
		{
			animController.SetAnimatorState(enabled: false);
		}
		stateMachine.movement.SetMovementType(MovementType.None);
		stateMachine.movement.useGravity = false;
		WaterFloatSemih_Rigidbody waterFloat = stateMachine.movement.waterFloat;
		if (waterFloat != null)
		{
			waterFloat.surfaceOffset = -0.25f;
			waterFloat.DeActivate();
			ragdollManager.AttachWaterFloat(waterFloat);
		}
		if (force <= 0f)
		{
			ragdollManager.SetRagdollState(state: true);
		}
		else
		{
			Vector3 worldVelocity = stateMachine.movement.GetWorldVelocity();
			OnApplyRagdollForce(worldVelocity * force);
		}
		if (!(ragdollManager != null))
		{
			return;
		}
		if (createRigidBodyAtTop)
		{
			Rigidbody mainRigidbody = ragdollManager.mainRigidbody;
			Rigidbody rigidbody = null;
			Rigidbody rigidbody2 = ragdollManager.gameObject.AddComponent<Rigidbody>();
			if (rigidbody2 != null)
			{
				rigidbody = rigidbody2;
			}
			else if (rigidbody2 == null)
			{
				rigidbody = ragdollManager.gameObject.GetComponent<Rigidbody>();
			}
			rigidbody.mass = 10f;
			mainRigidbody.GetComponent<CharacterJoint>().connectedBody = rigidbody;
			ragdollManager.mainRigidbody.isKinematic = false;
			ragdollManager.mainRigidbody = rigidbody;
		}
		if (waterFloat != null)
		{
			waterFloat.SetBody(ragdollManager.GetMainBody());
		}
		if (stateMachine.movement.agent != null)
		{
			Object.Destroy(stateMachine.movement.agent);
		}
		if (stateMachine.movement.characterController != null)
		{
			Object.Destroy(stateMachine.movement.characterController);
		}
		if (!Raft_Network.IsHost)
		{
			ragdollManager.mainRigidbody.isKinematic = true;
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		HandleRagdollFloating();
	}

	protected virtual void OnApplyRagdollForce(Vector3 force)
	{
		ragdollManager.SetRagdollState(state: true, force);
	}

	private void HandleRagdollFloating()
	{
		WaterFloatSemih_Rigidbody waterFloat = stateMachine.movement.waterFloat;
		if (waterFloat != null && !waterFloat.setToSurface)
		{
			if (!Raft_Network.IsHost && ragdollManager != null && !ragdollManager.mainRigidbody.isKinematic)
			{
				ragdollManager.mainRigidbody.isKinematic = true;
			}
			if (stateMachine.movement.groundHit.transform != null && stateMachine.transform.ParentedToRaft())
			{
				waterFloat.DeActivate();
			}
			else if (!waterFloat.IsActive())
			{
				waterFloat.Activate();
			}
		}
	}
}
