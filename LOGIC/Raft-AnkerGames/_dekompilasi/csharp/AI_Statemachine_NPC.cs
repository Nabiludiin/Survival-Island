using System;
using UnityEngine;

public class AI_Statemachine_NPC : AI_StateMachine_Animal
{
	[SerializeField]
	private AI_State_Waypoint_Circulation state_circulation;

	[SerializeField]
	private AI_State_MoveToInterest state_moveToInterest;

	[SerializeField]
	private AI_State_Wait_Dialogue state_waitDialogue;

	[SerializeField]
	private AI_Dialogue aiDialogue;

	[SerializeField]
	private AI_Interest aiInterest;

	[SerializeField]
	private TimerEventer interestTimer;

	[SerializeField]
	private Transform modelParent;

	[HideInInspector]
	public NPC_Model npcModel;

	[HideInInspector]
	public DialogueHeadtrackingIKConnection dhIK;

	public override void Start()
	{
		base.Start();
		SetupNPC();
		if (Raft_Network.IsHost)
		{
			TimerEventer timerEventer = interestTimer;
			timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(OnHandleInterest));
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (Raft_Network.IsHost)
		{
			TimerEventer timerEventer = interestTimer;
			timerEventer.OnTimerReached = (Action)Delegate.Remove(timerEventer.OnTimerReached, new Action(OnHandleInterest));
		}
	}

	private void OnAnimatorMessage(string message)
	{
		if (message == "Footstep")
		{
			Animation_Footstep();
		}
	}

	private void OnHandleInterest()
	{
		if (aiInterest.HasInterest && currentState == state_circulation)
		{
			InterestPoint validInterest = aiInterest.GetValidInterest();
			if (validInterest != null)
			{
				validInterest.Occupy(value: true);
				ChangeState<InterestPoint>(state_moveToInterest, validInterest);
			}
		}
	}

	protected override void UpdateStateMachine()
	{
		base.UpdateStateMachine();
		if (Raft_Network.IsHost && currentState == state_circulation)
		{
			interestTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
			if (aiDialogue.IsVoiceActive)
			{
				ChangeState(state_waitDialogue);
			}
		}
	}

	private void SetupNPC()
	{
		Transform child = modelParent.GetChild(0);
		if (child == null)
		{
			Debug.Log("Can't find model for " + transform.name, gameObject);
		}
		movement.model = child;
		npcModel = child.GetComponentInChildren<NPC_Model>();
		animController.animator = GetComponentInChildren<Animator>();
		aiDialogue.animatorController = animController;
		AnimatorMessageForwarder component = animController.animator.GetComponent<AnimatorMessageForwarder>();
		if (component != null)
		{
			component.OnAnimatorMessage = (Action<string>)Delegate.Combine(component.OnAnimatorMessage, new Action<string>(OnAnimatorMessage));
		}
		dhIK = child.GetComponentInChildren<DialogueHeadtrackingIKConnection>();
		if (dhIK != null)
		{
			dhIK.headIK = child.GetComponentInChildren<HeadTrackingIK>();
			if (aiDialogue != null)
			{
				dhIK.ManuallStart(aiDialogue);
			}
		}
	}
}
