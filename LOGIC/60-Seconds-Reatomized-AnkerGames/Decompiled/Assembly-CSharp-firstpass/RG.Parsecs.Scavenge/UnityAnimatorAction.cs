using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class UnityAnimatorAction : EventAction
{
	public Animator Animator;

	public string AnimatorBlockToPlay;

	protected override void Act(EventActionArgs args = null)
	{
		if (!(Animator == null))
		{
			Animator.Play(AnimatorBlockToPlay);
		}
	}
}
