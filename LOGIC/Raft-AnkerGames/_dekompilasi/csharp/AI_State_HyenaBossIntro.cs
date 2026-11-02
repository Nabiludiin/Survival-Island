using System;
using UnityEngine;

public class AI_State_HyenaBossIntro : AI_State
{
	[SerializeField]
	private SO_VoiceLine lastIntroVoiceLine;

	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private float startJumpDelay = 0.2f;

	[SerializeField]
	private float jumpHeight = 4f;

	[SerializeField]
	private float jumpDuration = 1f;

	private Boss_OlofPart2 olof;

	private AnimatorController animatorController;

	private bool hasJumped;

	private float jumpTime;

	private float fixTimer;

	private bool fixApplied;

	private bool shouldHaveJumpedFromLoad;

	protected override void Start()
	{
		base.Start();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		Transform objectOfInterest = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.Utopia_BossOlofPart2);
		if (objectOfInterest != null)
		{
			olof = objectOfInterest.GetComponent<Boss_OlofPart2>();
			if (olof != null)
			{
				Dialogue dialogue = olof.dialogue;
				dialogue.OnVoiceLinePlayed = (Action<SO_VoiceLine>)Delegate.Combine(dialogue.OnVoiceLinePlayed, new Action<SO_VoiceLine>(OnVoiceLinePlayed));
			}
		}
		AI_StateMachine_Animal aI_StateMachine_Animal = stateMachine as AI_StateMachine_Animal;
		animatorController = aI_StateMachine_Animal.animController;
	}

	public override void EnterState()
	{
		base.EnterState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (olof == null)
		{
			Transform objectOfInterest = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.Utopia_BossOlofPart2);
			if (objectOfInterest != null)
			{
				olof = objectOfInterest.GetComponent<Boss_OlofPart2>();
			}
		}
		if (olof != null)
		{
			stateMachine.transform.position = olof.leashPoint.position;
			stateMachine.transform.forward = olof.leashPoint.forward;
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (hasJumped)
		{
			float num = Mathf.Clamp01((Time.time - jumpTime) / jumpDuration);
			stateMachine.transform.position = Helper.GetBezierCurveValue(olof.leashPoint.position, olof.bossLandPoint.position, jumpHeight, num);
			if (num >= 1f)
			{
				stateMachine.ChangeState(nextState);
			}
		}
		else if (shouldHaveJumpedFromLoad && !fixApplied)
		{
			QuickFix();
		}
		else if (!fixApplied && olof.questEvent_LarsIntro.IsAtMaxStateIndex)
		{
			fixTimer += Time.deltaTime;
			if (fixTimer >= olof.dialogue.voiceDatas[0].totalVoiceLineDuration || (fixTimer > 3f && !olof.dialogue.IsVoiceActive))
			{
				QuickFix();
			}
		}
	}

	protected override void Destroy()
	{
		base.Destroy();
		if (olof != null)
		{
			Dialogue dialogue = olof.dialogue;
			dialogue.OnVoiceLinePlayed = (Action<SO_VoiceLine>)Delegate.Remove(dialogue.OnVoiceLinePlayed, new Action<SO_VoiceLine>(OnVoiceLinePlayed));
		}
		CancelInvoke();
	}

	private void OnVoiceLinePlayed(SO_VoiceLine voiceLine)
	{
		if (voiceLine == lastIntroVoiceLine)
		{
			animatorController.SetTrigger("Attack_BossJump");
		}
	}

	private void Jump()
	{
		QuestProgressTracker.FinishQuestRPC(QuestType.Utopia_Boss_Started);
		hasJumped = true;
		jumpTime = Time.time;
	}

	private void QuickFix()
	{
		fixApplied = true;
		Jump();
	}
}
