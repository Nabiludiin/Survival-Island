using System;
using UnityEngine;

public class AI_State_Wait_Dialogue : AI_State_Wait
{
	public Dialogue dialogue;

	private Network_Player dialogueTarget;

	protected override void Start()
	{
		base.Start();
		Dialogue dialogue = this.dialogue;
		dialogue.OnVoiceDataPlayed = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(dialogue.OnVoiceDataPlayed, new Action<SO_VoiceData.Instance_VoiceData>(OnDialogueStart));
		Dialogue dialogue2 = this.dialogue;
		dialogue2.OnVoiceDataCancelled = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(dialogue2.OnVoiceDataCancelled, new Action<SO_VoiceData.Instance_VoiceData>(OnDialogueDone));
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost && dialogueTarget != null)
		{
			Vector3 direction = dialogueTarget.transform.position - transform.position;
			direction.y = 0f;
			stateMachine.movement.LookDirection(direction, 20f);
		}
	}

	private void OnDialogueStart(SO_VoiceData.Instance_VoiceData dialogueData)
	{
		dialogueTarget = dialogueData.startedPlayer;
	}

	private void OnDialogueDone(SO_VoiceData.Instance_VoiceData instance)
	{
		if (active)
		{
			stateMachine.ChangeState(nextState);
			dialogueTarget = null;
		}
	}
}
