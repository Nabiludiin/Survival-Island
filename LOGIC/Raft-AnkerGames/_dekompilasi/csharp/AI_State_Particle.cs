using System;
using UnityEngine;

public class AI_State_Particle : MonoBehaviour
{
	public enum ParticleType
	{
		OneShot
	}

	[Serializable]
	public class ParticleSettings
	{
		public ParticleSystem particleSystem;

		public ParticleType particleType;

		public bool enable;

		public AIStateEvent eventType;
	}

	public ParticleSettings[] particleSettings;

	private AI_StateMachine stateMachine;

	private AI_State state;

	private void Awake()
	{
		state = GetComponent<AI_State>();
		stateMachine = GetComponentInParent<AI_StateMachine>();
		if (state != null && stateMachine != null)
		{
			AI_StateMachine aI_StateMachine = stateMachine;
			aI_StateMachine.OnEnterState = (Action<AI_State>)Delegate.Combine(aI_StateMachine.OnEnterState, new Action<AI_State>(OnEnterState));
			AI_StateMachine aI_StateMachine2 = stateMachine;
			aI_StateMachine2.OnExitState = (Action<AI_State>)Delegate.Combine(aI_StateMachine2.OnExitState, new Action<AI_State>(OnExitState));
		}
		if (particleSettings != null && particleSettings.Length != 0)
		{
			ParticleSettings[] array = particleSettings;
			for (int i = 0; i < array.Length; i++)
			{
				ParticleSystem.EmissionModule emission = array[i].particleSystem.emission;
				emission.enabled = false;
			}
		}
	}

	private void OnEnterState(AI_State newState)
	{
		if (newState == state)
		{
			ApplyParticleSettings(AIStateEvent.EnterState);
		}
	}

	private void OnExitState(AI_State newState)
	{
		if (newState == state)
		{
			ApplyParticleSettings(AIStateEvent.ExitState);
		}
	}

	private void ApplyParticleSettings(AIStateEvent eventType)
	{
		if (this.particleSettings == null || this.particleSettings.Length == 0)
		{
			return;
		}
		ParticleSettings[] array = this.particleSettings;
		foreach (ParticleSettings particleSettings in array)
		{
			if (particleSettings.eventType != eventType)
			{
				continue;
			}
			ParticleSystem.EmissionModule emission = particleSettings.particleSystem.emission;
			emission.enabled = particleSettings.enable;
			if (particleSettings.particleType == ParticleType.OneShot)
			{
				if (particleSettings.enable)
				{
					particleSettings.particleSystem.Play();
				}
				else
				{
					particleSettings.particleSystem.Stop();
				}
			}
		}
	}
}
