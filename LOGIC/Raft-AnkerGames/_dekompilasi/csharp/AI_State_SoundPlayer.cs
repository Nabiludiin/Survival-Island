using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AI_State_SoundPlayer : MonoBehaviour
{
	[SerializeField]
	private StateSoundPlaySetting[] soundpropertySetting;

	private AI_StateMachine stateMachine;

	private AI_State state;

	private bool initialized;

	private void OnEnterState(AI_State newState)
	{
		if (newState == state)
		{
			PlaySoundViaSetting(AIStateEvent.EnterState);
		}
	}

	private void OnExitState(AI_State newState)
	{
		if (newState == state)
		{
			PlaySoundViaSetting(AIStateEvent.ExitState);
		}
	}

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
			initialized = true;
		}
	}

	private void OnDestroy()
	{
		if (initialized && stateMachine != null)
		{
			AI_StateMachine aI_StateMachine = stateMachine;
			aI_StateMachine.OnEnterState = (Action<AI_State>)Delegate.Remove(aI_StateMachine.OnEnterState, new Action<AI_State>(OnEnterState));
			AI_StateMachine aI_StateMachine2 = stateMachine;
			aI_StateMachine2.OnExitState = (Action<AI_State>)Delegate.Remove(aI_StateMachine2.OnExitState, new Action<AI_State>(OnExitState));
		}
	}

	private void PlaySoundViaSetting(AIStateEvent stateEvent)
	{
		StateSoundPlaySetting[] array = soundpropertySetting;
		foreach (StateSoundPlaySetting stateSoundPlaySetting in array)
		{
			if (stateSoundPlaySetting.stateEvent != stateEvent)
			{
				continue;
			}
			switch (stateSoundPlaySetting.soundSetting)
			{
			case SoundSetting.Play:
				if (stateSoundPlaySetting.eventEmitter != null)
				{
					if (stateSoundPlaySetting.soundCooldown != 0f && !(Time.time - stateSoundPlaySetting.latestSoundPlayTimeStamp >= stateSoundPlaySetting.soundCooldown))
					{
						return;
					}
					stateSoundPlaySetting.latestSoundPlayTimeStamp = Time.time;
					stateSoundPlaySetting.eventEmitter.Play();
				}
				else if (stateSoundPlaySetting.eventRef != string.Empty && (stateSoundPlaySetting.soundCooldown == 0f || Time.time - stateSoundPlaySetting.latestSoundPlayTimeStamp >= stateSoundPlaySetting.soundCooldown))
				{
					stateSoundPlaySetting.latestSoundPlayTimeStamp = Time.time;
					RuntimeManager.PlayOneShotAttached(stateSoundPlaySetting.eventRef, gameObject);
				}
				break;
			case SoundSetting.Stop:
				if (stateSoundPlaySetting.eventEmitter != null)
				{
					stateSoundPlaySetting.eventEmitter.EventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
					stateSoundPlaySetting.eventEmitter.EventInstance.release();
					stateSoundPlaySetting.eventEmitter.EventInstance.clearHandle();
				}
				break;
			case SoundSetting.KeyInFadeIn:
				if (stateSoundPlaySetting.eventEmitter != null)
				{
					stateSoundPlaySetting.eventEmitter.Stop();
					stateSoundPlaySetting.eventEmitter.Play();
				}
				break;
			case SoundSetting.KeyoffFadeOut:
				if (stateSoundPlaySetting.eventEmitter != null)
				{
					stateSoundPlaySetting.eventEmitter.EventInstance.keyOff();
				}
				break;
			}
		}
	}
}
