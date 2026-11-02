using FMODUnity;
using UnityEngine;

public class AI_State_InteractWithInterest : AI_State
{
	private enum InteractState
	{
		MoveToInteractPoint,
		RotTo,
		Interacting,
		MoveToExitPoint
	}

	[SerializeField]
	private AI_State exitState;

	[SerializeField]
	private AnimatorController animatorController;

	[SerializeField]
	private AI_Dialogue dialogue;

	[SerializeField]
	private float movementSpeed = 5f;

	[SerializeField]
	private float rotationSpeed = 6f;

	[SerializeField]
	private float minValidRotationAngle = 5f;

	[SerializeField]
	private float animationLerpSpeed = 3f;

	private AI_Statemachine_NPC statemachineNPC;

	private InterestPoint aiInterest;

	private InteractState interactState;

	private float interactTimer;

	private Vector3 stateMachineEntryPosition;

	private Transform handItem;

	public void OnValidate()
	{
		if (animatorController == null)
		{
			animatorController = GetComponentInParent<AnimatorController>();
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		statemachineNPC = stateMachine as AI_Statemachine_NPC;
		interactState = InteractState.MoveToInteractPoint;
		if (statemachineNPC.dhIK != null && statemachineNPC.dhIK.headIK != null)
		{
			statemachineNPC.dhIK.headIK.allowSnapRotation = false;
		}
		if (Raft_Network.IsHost)
		{
			stateMachineEntryPosition = stateMachine.transform.position;
			interactTimer = 0f;
			stateMachine.movement.SetMovementType();
			stateMachine.movement.SetMovementSpeed(movementSpeed);
			stateMachine.movement.SetRotationSpeed(rotationSpeed);
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		if (statemachineNPC.dhIK != null && statemachineNPC.dhIK.headIK != null)
		{
			statemachineNPC.dhIK.headIK.allowSnapRotation = true;
		}
		if (Raft_Network.IsHost && aiInterest != null)
		{
			aiInterest.Occupy(value: false);
			if (dialogue != null)
			{
				dialogue.allowTalkingAnimator = true;
			}
		}
	}

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
		if (!Raft_Network.IsHost)
		{
			return;
		}
		aiInterest = (InterestPoint)(object)parameters[0];
		if (aiInterest != null)
		{
			aiInterest.Occupy(value: true);
			if (dialogue != null)
			{
				dialogue.allowTalkingAnimator = aiInterest.interestPointData.allowTalkingAnimator;
			}
			if (aiInterest.interestPointData.handItemName != string.Empty)
			{
				handItem = statemachineNPC.npcModel.handParent.Find(aiInterest.interestPointData.handItemName);
			}
			else
			{
				handItem = null;
			}
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (aiInterest == null)
		{
			stateMachine.ChangeState(exitState);
			return;
		}
		switch (interactState)
		{
		case InteractState.MoveToInteractPoint:
		{
			Vector3 position = aiInterest.entryPoint.position;
			stateMachine.movement.Move_Translate(stateMachine.transform, position, stateMachine.movement.MovementSpeed);
			stateMachine.movement.Look(stateMachine.movement.model, position, stateMachine.movement.RotationSpeed, ignoreZRotation: true);
			if (Vector3.Distance(stateMachine.transform.position, position) < 0.1f)
			{
				stateMachine.transform.position = position;
				interactState = InteractState.RotTo;
			}
			break;
		}
		case InteractState.RotTo:
		{
			animatorController.SetFloat("Speed", Mathf.Lerp(animatorController.GetFloat("Speed"), 0f, Time.deltaTime * animationLerpSpeed));
			Vector3 forward = aiInterest.interactPoint.forward;
			stateMachine.movement.model.rotation = Quaternion.Lerp(stateMachine.movement.model.rotation, Quaternion.LookRotation(forward, Vector3.up), Time.deltaTime * stateMachine.movement.RotationSpeed);
			if (Mathf.Abs(Vector3.Angle(stateMachine.movement.model.forward, forward)) < minValidRotationAngle)
			{
				ApplyAnimationProperties(aiInterest, AIStateEvent.EnterState);
				RuntimeManager.PlayOneShot(aiInterest.interestPointData.er_startInteract, stateMachine.movement.model.position);
				interactState = InteractState.Interacting;
			}
			break;
		}
		case InteractState.Interacting:
			interactTimer += Time.deltaTime;
			stateMachine.movement.Move_Translate(stateMachine.transform, aiInterest.interactPoint.position, stateMachine.movement.MovementSpeed);
			if (handItem != null && !handItem.gameObject.activeSelf && interactTimer >= aiInterest.interestPointData.handItemStartDelay)
			{
				handItem.gameObject.SetActiveSafe(value: true);
			}
			if (interactTimer >= aiInterest.interestPointData.interactDuration)
			{
				ApplyAnimationProperties(aiInterest, AIStateEvent.ExitState);
				if (handItem != null)
				{
					handItem.gameObject.SetActiveSafe(value: false);
				}
				if (interactTimer >= aiInterest.interestPointData.interactDuration + aiInterest.interestPointData.endAnimationDelay)
				{
					interactState = InteractState.MoveToExitPoint;
				}
			}
			break;
		case InteractState.MoveToExitPoint:
			animatorController.SetFloat("Speed", Mathf.Lerp(animatorController.GetFloat("Speed"), 1f, Time.deltaTime * animationLerpSpeed));
			stateMachine.movement.Move_Translate(stateMachine.transform, stateMachineEntryPosition, stateMachine.movement.MovementSpeed);
			stateMachine.movement.Look(stateMachine.movement.model, stateMachineEntryPosition, stateMachine.movement.RotationSpeed, ignoreZRotation: true);
			if (Vector3.Distance(stateMachine.transform.position, stateMachineEntryPosition) < 0.1f)
			{
				stateMachine.transform.position = stateMachineEntryPosition;
				stateMachine.ChangeState(exitState);
			}
			break;
		}
	}

	private void ApplyAnimationProperties(InterestPoint interest, AIStateEvent eventType)
	{
		if (animatorController != null && aiInterest != null)
		{
			aiInterest.interestPointData.ApplyAnimationSettings(animatorController, eventType);
		}
	}
}
