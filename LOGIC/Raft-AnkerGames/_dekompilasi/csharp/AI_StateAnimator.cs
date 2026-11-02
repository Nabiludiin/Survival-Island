using System;
using System.Collections;
using UnityEngine;

public class AI_StateAnimator : MonoBehaviour
{
	[SerializeField]
	private AnimatorPropertySetting[] animatorPropertySettings;

	private AI_StateMachine stateMachine;

	private AI_State state;

	private AnimatorController animatorController;

	private bool initialized;

	private void Awake()
	{
		state = GetComponent<AI_State>();
		stateMachine = GetComponentInParent<AI_StateMachine>();
		animatorController = stateMachine.GetComponentInChildren<AnimatorController>();
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

	private void OnEnterState(AI_State newState)
	{
		if (newState == state)
		{
			ApplyAnimationSettings(AIStateEvent.EnterState);
		}
	}

	private void OnExitState(AI_State newState)
	{
		if (newState == state)
		{
			ApplyAnimationSettings(AIStateEvent.ExitState);
		}
	}

	private void ApplyAnimationSettings(AIStateEvent eventType)
	{
		if (animatorController == null || animatorPropertySettings == null || animatorPropertySettings.Length == 0)
		{
			return;
		}
		StopAllCoroutines();
		AnimatorPropertySetting[] array = animatorPropertySettings;
		foreach (AnimatorPropertySetting animatorPropertySetting in array)
		{
			if (animatorPropertySetting.eventType != eventType)
			{
				continue;
			}
			switch (animatorPropertySetting.propertyType)
			{
			case AnimatorPropertyType.Int:
				animatorController.SetInt(animatorPropertySetting.parameterName, animatorPropertySetting.intValue);
				break;
			case AnimatorPropertyType.Float:
				if (animatorPropertySetting.lerp && gameObject.activeInHierarchy)
				{
					StartCoroutine(LerpFloat(animatorPropertySetting.parameterName, animatorPropertySetting.floatValue, animatorPropertySetting.lerpSpeed));
				}
				else
				{
					animatorController.SetFloat(animatorPropertySetting.parameterName, animatorPropertySetting.floatValue);
				}
				break;
			case AnimatorPropertyType.Bool:
				animatorController.SetBool(animatorPropertySetting.parameterName, animatorPropertySetting.boolValue);
				break;
			case AnimatorPropertyType.Trigger:
				animatorController.SetTrigger(animatorPropertySetting.parameterName);
				break;
			}
		}
	}

	private IEnumerator LerpFloat(string parameterName, float newValue, float lerpSpeed)
	{
		float currentValue = animatorController.GetFloat(parameterName);
		float diff = 1f;
		while (diff > 0.05f)
		{
			currentValue = Mathf.Lerp(currentValue, newValue, Time.deltaTime * lerpSpeed);
			animatorController.SetFloat(parameterName, currentValue);
			diff = Mathf.Abs(currentValue - newValue);
			yield return null;
		}
		animatorController.SetFloat(parameterName, newValue);
	}
}
