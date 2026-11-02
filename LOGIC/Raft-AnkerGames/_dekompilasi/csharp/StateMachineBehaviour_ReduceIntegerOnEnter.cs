using UnityEngine;

public class StateMachineBehaviour_ReduceIntegerOnEnter : StateMachineBehaviour
{
	public string parameterName;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		int integer = animator.GetInteger(parameterName);
		integer = Mathf.Clamp(integer - 1, 0, int.MaxValue);
		animator.SetInteger(parameterName, integer);
	}
}
