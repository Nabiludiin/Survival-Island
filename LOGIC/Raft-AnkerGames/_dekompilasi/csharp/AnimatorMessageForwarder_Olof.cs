using FMODUnity;
using UnityEngine;

public class AnimatorMessageForwarder_Olof : AnimatorMessageForwarder
{
	[SerializeField]
	private StudioEventEmitter emitter_footstep;

	public override void AnimatorMessage(string message)
	{
		base.AnimatorMessage(message);
		if (message == "Footstep" && emitter_footstep != null)
		{
			emitter_footstep.Play();
		}
	}
}
