using System;
using UnityEngine;

public class AI_NetworkBehaviour_Hyena : AI_NetworkBehaviour_Animal
{
	[SerializeField]
	private AttachPlayer attachPlayerScript;

	[SerializeField]
	private ParticleSystem kickedByPlayerParticles;

	[SerializeField]
	private AI_State_MeleeAttackAttach state_meleeAttach;

	protected override void Start()
	{
		base.Start();
		if (attachPlayerScript != null)
		{
			AttachPlayer attachPlayer = attachPlayerScript;
			attachPlayer.OnStartCarryPlayer = (Action<Network_Player>)Delegate.Combine(attachPlayer.OnStartCarryPlayer, new Action<Network_Player>(OnStartCarryPlayer));
			AttachPlayer attachPlayer2 = attachPlayerScript;
			attachPlayer2.OnStopCarryPlayer = (Action<Network_Player>)Delegate.Combine(attachPlayer2.OnStopCarryPlayer, new Action<Network_Player>(OnStopCarryPlayer));
		}
	}

	private void OnStartCarryPlayer(Network_Player player)
	{
		if (player != null)
		{
			PlayerAnimator animator = player.Animator;
			animator.OnAnimationEvent = (Action<string>)Delegate.Combine(animator.OnAnimationEvent, new Action<string>(OnCarriedPlayerAnimationEvent));
		}
	}

	private void OnStopCarryPlayer(Network_Player player)
	{
		if (player != null)
		{
			PlayerAnimator animator = player.Animator;
			animator.OnAnimationEvent = (Action<string>)Delegate.Combine(animator.OnAnimationEvent, new Action<string>(OnCarriedPlayerAnimationEvent));
		}
	}

	private void OnCarriedPlayerAnimationEvent(string eventName)
	{
		if (eventName == "HyenaDragKick" && kickedByPlayerParticles != null && attachPlayerScript.HasPlayerAttached && (!attachPlayerScript.carriedPlayer.IsLocalPlayer || attachPlayerScript.carriedPlayer.currentModel.thirdPersonSettings.ThirdPersonModel))
		{
			kickedByPlayerParticles.Play();
		}
	}

	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		return new RGD_AI_NetworkBehaviour(this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.AttachPlayerMeleeAttack)
		{
			return state_meleeAttach.FakeDeserialize(msg, remoteID);
		}
		return base.Deserialize(msg, remoteID);
	}
}
