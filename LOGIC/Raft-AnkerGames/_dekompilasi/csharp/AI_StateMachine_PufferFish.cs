using FMODUnity;
using UnityEngine;

public class AI_StateMachine_PufferFish : AI_StateMachine_Animal
{
	public AI_State_PufferFish_Explode state_explode;

	[SerializeField]
	private ParticleSystem bubbleParticle;

	[SerializeField]
	private Interval_Float particleEmitInterval;

	private float particleEmitDelay;

	private float particleEmitTimer;

	public EventReference er_bubble;

	public override void Start()
	{
		base.Start();
		if (!Raft_Network.IsHost)
		{
			GetComponent<AI_Movement_Lean>().enabled = false;
		}
		animController.SetFloat("SwimSpeed", 0f);
		particleEmitTimer = 0f;
		particleEmitDelay = particleEmitInterval.GetRandomValue();
	}

	protected override void UpdateStateMachine()
	{
		base.UpdateStateMachine();
		if (currentState == decayState || currentState == deadState)
		{
			return;
		}
		particleEmitTimer += Time.deltaTime;
		if (particleEmitTimer >= particleEmitDelay)
		{
			particleEmitTimer = 0f;
			particleEmitDelay = particleEmitInterval.GetRandomValue();
			if (bubbleParticle != null)
			{
				bubbleParticle.Emit(1);
			}
			RuntimeManager.PlayOneShot(er_bubble, transform.position);
		}
	}
}
