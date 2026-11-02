using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class UnityAnimationAction : EventAction
{
	public Animation Animation;

	[Tooltip("Leave empty to play default")]
	public string AnimatonName;

	protected override void Act(EventActionArgs args = null)
	{
		if (!(Animation == null))
		{
			if (string.IsNullOrEmpty(AnimatonName))
			{
				Animation.Play();
			}
			else
			{
				Animation.Play(AnimatonName);
			}
		}
	}
}
