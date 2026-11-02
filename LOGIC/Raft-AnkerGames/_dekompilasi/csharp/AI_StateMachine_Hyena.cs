using UnityEngine;

public class AI_StateMachine_Hyena : AI_StateMachine_Animal
{
	public ParticleSystem dustJumpLand;

	public ParticleSystem dustFoot;

	public ParticleSystem dustJumpLeap;

	public ParticleSystem dustHeavyAttack;

	protected override void Animation_Footstep()
	{
		base.Animation_Footstep();
		if (dustFoot != null)
		{
			dustFoot.Play();
		}
	}

	public void Animation_JumpLand()
	{
		if (dustJumpLand != null)
		{
			dustJumpLand.Play();
		}
	}

	public void Animation_Leap()
	{
		if (dustJumpLeap != null)
		{
			dustJumpLeap.Play();
		}
	}

	public void Animation_HeavyAttack()
	{
		if (dustHeavyAttack != null)
		{
			dustHeavyAttack.Play();
		}
	}
}
