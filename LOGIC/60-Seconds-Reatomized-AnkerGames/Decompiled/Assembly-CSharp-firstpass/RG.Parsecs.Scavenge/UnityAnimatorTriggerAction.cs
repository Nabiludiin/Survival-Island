using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class UnityAnimatorTriggerAction : EventAction
{
	public Animator Animator;

	public string TriggerPlay;

	protected override void Act(EventActionArgs args = null)
	{
		if (!(Animator == null))
		{
			Animator.SetTrigger(TriggerPlay);
		}
	}
}
