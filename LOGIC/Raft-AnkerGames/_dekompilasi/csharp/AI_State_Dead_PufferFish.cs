using FMODUnity;
using UnityEngine;

public class AI_State_Dead_PufferFish : AI_State_Dead
{
	[SerializeField]
	private ParticleSystem bubbleParticles;

	public EventReference er_die;

	private AI_StateMachine_PufferFish stateMachinePufferFish;

	protected override void Awake()
	{
		base.Awake();
		stateMachinePufferFish = stateMachine as AI_StateMachine_PufferFish;
	}

	public override void EnterState()
	{
		base.EnterState();
		RuntimeManager.PlayOneShot(er_die, stateMachinePufferFish.transform.position);
		stateMachinePufferFish.animController.SetBool("Dead", value: true);
		stateMachinePufferFish.GetComponent<AI_Movement_Lean>().enabled = false;
		bubbleParticles.transform.SetParent(null);
		bubbleParticles.Stop();
		Object.Destroy(bubbleParticles.gameObject, 5f);
	}
}
