using UnityEngine;

public class StateMachineBehaviour_RandomIndex : StateMachineBehaviour
{
	public string indexParameterName;

	public Interval_Int indexRange;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		animator.SetInteger(indexParameterName, indexRange.GetRandomValue());
	}
}
