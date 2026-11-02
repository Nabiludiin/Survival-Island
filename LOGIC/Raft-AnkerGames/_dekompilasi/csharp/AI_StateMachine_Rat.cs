using System;
using FMODUnity;
using UnityEngine;

public class AI_StateMachine_Rat : AI_StateMachine_Animal
{
	public ParticleSystem landJumpParticles;

	public ParticleSystem takeDamageParticles;

	public EventReference er_landJump;

	public override void Awake()
	{
		base.Awake();
		AnimatorController animatorController = animController;
		animatorController.OnAnimationEvent = (Action<string>)Delegate.Combine(animatorController.OnAnimationEvent, new Action<string>(OnAnimationEvent));
	}

	protected override void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		if (damageType != EntityType.None)
		{
			takeDamageParticles.Play();
		}
		if (currentState != deadState && currentState != decayState)
		{
			base.OnDamageTaken(damage, hitPoint, hitNormal, damageType, isLocal);
		}
	}

	private void OnAnimationEvent(string animationEventName)
	{
		if (animationEventName == "LandJump")
		{
			landJumpParticles.Play();
			RuntimeManager.PlayOneShotAttached(er_landJump, gameObject);
		}
	}
}
