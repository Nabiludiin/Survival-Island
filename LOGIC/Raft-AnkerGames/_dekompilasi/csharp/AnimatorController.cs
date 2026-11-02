using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	public Action<string> OnAnimationEvent;

	public Animator animator;

	public Animator[] redirectedAnimators;

	public bool hasSpeedParameter;

	[HideInInspector]
	private List<string> triggeredTriggers = new List<string>();

	public bool IsValid => animator != null;

	public void AnimationString(string animationEventName)
	{
		if (OnAnimationEvent != null)
		{
			OnAnimationEvent(animationEventName);
		}
	}

	public void PlaySound(string eventRefName)
	{
		RuntimeManager.PlayOneShotAttached(eventRefName, gameObject);
	}

	public void SetAnimatorState(bool enabled)
	{
		if (!IsValid)
		{
			return;
		}
		this.animator.enabled = enabled;
		if (redirectedAnimators == null)
		{
			return;
		}
		Animator[] array = redirectedAnimators;
		foreach (Animator animator in array)
		{
			if (animator != null)
			{
				animator.enabled = enabled;
			}
		}
	}

	public void SetTrigger(string parameter)
	{
		if (!IsValid)
		{
			return;
		}
		this.animator.SetTrigger(parameter);
		if (Raft_Network.IsHost && !triggeredTriggers.Contains(parameter))
		{
			triggeredTriggers.Add(parameter);
		}
		if (redirectedAnimators == null)
		{
			return;
		}
		Animator[] array = redirectedAnimators;
		foreach (Animator animator in array)
		{
			if (animator != null)
			{
				animator.SetTrigger(parameter);
			}
		}
	}

	public string[] GetTriggers()
	{
		if (triggeredTriggers == null || triggeredTriggers.Count == 0)
		{
			return null;
		}
		string[] result = triggeredTriggers.ToArray();
		triggeredTriggers.Clear();
		return result;
	}

	public void SetBool(string parameter, bool value)
	{
		if (!IsValid)
		{
			return;
		}
		this.animator.SetBool(parameter, value);
		if (redirectedAnimators == null)
		{
			return;
		}
		Animator[] array = redirectedAnimators;
		foreach (Animator animator in array)
		{
			if (animator != null)
			{
				animator.SetBool(parameter, value);
			}
		}
	}

	public bool GetBool(string parameter)
	{
		if (!IsValid)
		{
			return false;
		}
		return animator.GetBool(parameter);
	}

	public void SetInt(string parameter, int value)
	{
		if (!IsValid)
		{
			return;
		}
		this.animator.SetInteger(parameter, value);
		if (redirectedAnimators == null)
		{
			return;
		}
		Animator[] array = redirectedAnimators;
		foreach (Animator animator in array)
		{
			if (animator != null)
			{
				animator.SetInteger(parameter, value);
			}
		}
	}

	public int GetInt(string paramter)
	{
		if (!IsValid)
		{
			return 0;
		}
		return animator.GetInteger(paramter);
	}

	public void SetFloat(string parameter, float value)
	{
		if (!IsValid)
		{
			return;
		}
		this.animator.SetFloat(parameter, value);
		if (redirectedAnimators == null)
		{
			return;
		}
		Animator[] array = redirectedAnimators;
		foreach (Animator animator in array)
		{
			if (animator != null)
			{
				animator.SetFloat(parameter, value);
			}
		}
	}

	public float GetFloat(string parameter)
	{
		if (!IsValid)
		{
			return 0f;
		}
		return animator.GetFloat(parameter);
	}
}
