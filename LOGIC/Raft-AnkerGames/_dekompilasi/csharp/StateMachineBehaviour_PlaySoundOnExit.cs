using FMODUnity;
using UnityEngine;

public class StateMachineBehaviour_PlaySoundOnExit : StateMachineBehaviour
{
	[EventRef]
	[SerializeField]
	private string eventRef_sound;

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		RuntimeManager.PlayOneShot(eventRef_sound, animator.transform.position);
	}
}
