public class AI_State_Dead_Shark : AI_State_Dead
{
	private AI_StateMachine_Shark sharkStateMachine;

	protected override void Start()
	{
		base.Start();
		sharkStateMachine = stateMachine as AI_StateMachine_Shark;
	}

	public override void EnterState()
	{
		base.EnterState();
		if (sharkStateMachine == null)
		{
			sharkStateMachine = stateMachine as AI_StateMachine_Shark;
		}
		if (sharkStateMachine.animController != null)
		{
			sharkStateMachine.animController.SetTrigger("Dead");
		}
		sharkStateMachine.newBiteSound.Stop();
		if (sharkStateMachine.sharkHapticEvent != -1)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.SharkPool.ReleaseIndex(sharkStateMachine.sharkHapticEvent);
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.StopAtennuated((HapticEvent)sharkStateMachine.sharkHapticEvent);
			sharkStateMachine.sharkHapticEvent = -1;
		}
		sharkStateMachine.StopParticleWaterSplash();
		sharkStateMachine.StopParticleWood();
		sharkStateMachine.soundemitter_takeDamage.Stop();
		sharkStateMachine.soundemitter_idle.Stop();
		sharkStateMachine.soundemitter_splash.Stop();
		sharkStateMachine.networkBehaviour.pickupItemChanneling.SetPickupColliderState(state: true);
	}
}
